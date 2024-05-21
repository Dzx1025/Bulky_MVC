using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CategoryController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index()
    {
        var objCategoryList = unitOfWork.Category.GetAll().ToList();
        return View(objCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (!ModelState.IsValid) return View();

        unitOfWork.Category.Add(obj);
        unitOfWork.Save();
        TempData["success"] = "Category created successfully";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int? id)
    {
        if (id is null or 0)
        {
            return NotFound();
        }

        var categoryFromDb = unitOfWork.Category.Get(u => u.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }

        return View(categoryFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (!ModelState.IsValid) return View();

        unitOfWork.Category.Update(obj);
        unitOfWork.Save();
        TempData["success"] = "Category updated successfully";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
        if (id is null or 0)
        {
            return NotFound();
        }

        var categoryFromDb = unitOfWork.Category.Get(u => u.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }

        return View(categoryFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        var categoryFromDb = unitOfWork.Category.Get(u => u.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }

        unitOfWork.Category.Remove(categoryFromDb);
        unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}