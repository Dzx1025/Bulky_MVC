using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

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

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Product obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    public IActionResult Edit(int? id)
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
    [HttpPost]
    public IActionResult Edit(Product obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product updated successfully";
            return RedirectToAction(nameof(Index));
        }
        return View();
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
