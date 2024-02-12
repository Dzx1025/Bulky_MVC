using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class CategoryController(ApplicationDbContext db) : Controller
{
    public IActionResult Index()
    {
        var ObjCategoryList = db.Categories.ToList();
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
            db.Categories.Add(obj);
            db.SaveChanges();
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
        var categoryFromDb = db.Categories.Find(id);
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
            db.Categories.Update(obj);
            db.SaveChanges();
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
        var categoryFromDb = db.Categories.Find(id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        var categoryFromDb = db.Categories.Find(id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        db.Categories.Remove(categoryFromDb);
        db.SaveChanges();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction(nameof(Index));
    }
}
