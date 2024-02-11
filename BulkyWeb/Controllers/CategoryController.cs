using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        //if (obj.Name == obj.DisplayOrder.ToString())
        //{
        //    ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
        //}

        if (ModelState.IsValid)
        {
            db.Categories.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
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
        db.SaveChanges();
        return RedirectToAction("Index");
    }
}
