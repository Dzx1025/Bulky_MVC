using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IUnitOfWork unitOfWork) : Controller
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public IActionResult Index()
    {
        var ObjProductList = _unitOfWork.Product.GetAll().ToList();

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
            _unitOfWork.Product.Add(productVM.Product);
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
