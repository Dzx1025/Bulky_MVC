using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment) : Controller
{
    private readonly IUnitOfWork _unitOfWork = _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment = _webHostEnvironment;

    public IActionResult Index()
    {
        var ObjProductList = _unitOfWork.Product.GetAll("Category").ToList();

        return View(ObjProductList);
    }

    public IActionResult Upsert(int? id)
    {
        var productVM = new ProductVM()
        {
            Product = new Product(),
            CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            }),
        };
        if (id == null || id == 0)
        {
            // Create
            return View(productVM);
        }
        else
        {
            // Update
            productVM.Product = _unitOfWork.Product.Get(u => id == u.Id);
            return View(productVM);
        }
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM productVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images/product");

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    // Delete the old image
                    var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create);
                file.CopyTo(fileStream);

                productVM.Product.ImageUrl = @"/images/product/" + fileName;
            }

            if (productVM.Product.Id == 0)
            {
                _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
            }
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction(nameof(Index));
        }
        else
        {
            productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });

            return View(productVM);
        }
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }

        var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        if (productFromDb == null)
        {
            return NotFound();
        }

        return View(productFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        var productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
        if (productFromDb == null)
        {
            return NotFound();
        }

        _unitOfWork.Product.Remove(productFromDb);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}