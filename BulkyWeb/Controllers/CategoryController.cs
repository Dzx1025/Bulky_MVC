using BulkyWeb.Data;
using BulkyWeb.Models;
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

}
