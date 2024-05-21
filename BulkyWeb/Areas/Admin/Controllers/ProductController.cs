using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment) : Controller
{
    public IActionResult Index()
    {
        var objProductList = unitOfWork.Product.GetAll("Category").ToList();
        return View(objProductList);
    }

    public IActionResult Upsert(int? id)
    {
        var productVM = new ProductVM
        {
            Product = new Product(),
            CategoryList = unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            }),
        };
        
        if (id is null or 0)
        {
            // Create
            return View(productVM);
        }
        // Update
        productVM.Product = unitOfWork.Product.Get(u => u.Id == id);
        return View(productVM);
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM productVM, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            var wwwRootPath = webHostEnvironment.WebRootPath;
            if (file != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var productPath = Path.Combine(wwwRootPath, "images/product");

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

                productVM.Product.ImageUrl = "/images/product/" + fileName;
            }

            if (productVM.Product.Id == 0)
            {
                unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                unitOfWork.Product.Update(productVM.Product);
            }

            unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction(nameof(Index));
        }

        productVM.CategoryList = unitOfWork.Category.GetAll().Select(u => new SelectListItem
        {
            Text = u.Name,
            Value = u.Id.ToString(),
        });

        return View(productVM);
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var objProductList = unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return Json(new { data = objProductList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var objToBeDeleted = unitOfWork.Product.Get(u => u.Id == id);
        if (objToBeDeleted == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, objToBeDeleted.ImageUrl.TrimStart('/'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        unitOfWork.Product.Remove(objToBeDeleted);
        unitOfWork.Save();

        return Json(new { success = true, message = "Delete successful" });
    }

    #endregion
}