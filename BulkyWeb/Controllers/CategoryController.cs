using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

public class CategoryController(ICategoryRepository db) : Controller
{
    private readonly ICategoryRepository _categoryRepo = db;

    public IActionResult Index()
    {
        var ObjCategoryList = _categoryRepo.GetAll().ToList();
        return View(ObjCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (ModelState.IsValid)
        {
            _categoryRepo.Add(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category created successfully";
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
        var categoryFromDb = _categoryRepo.Get(u => u.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            _categoryRepo.Update(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category updated successfully";
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
        var categoryFromDb = _categoryRepo.Get(u => u.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        var categoryFromDb = _categoryRepo.Get(u => u.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        _categoryRepo.Remove(categoryFromDb);
        _categoryRepo.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
