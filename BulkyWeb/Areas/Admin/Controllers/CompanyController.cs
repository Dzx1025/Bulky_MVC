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
public class CompanyController(IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index()
    {
        var objCompanyList = unitOfWork.Company.GetAll().ToList();
        return View(objCompanyList);
    }

    public IActionResult Upsert(int? id)
    {
        if (id is null or 0)
        {
            // Create
            return View(new Company());
        }

        // Update
        var companyObj = unitOfWork.Company.Get(u => u.Id == id);
        return View(companyObj);
    }

    [HttpPost]
    public IActionResult Upsert(Company companyObj)
    {
        if (ModelState.IsValid)
        {
            if (companyObj.Id == 0)
            {
                unitOfWork.Company.Add(companyObj);
            }
            else
            {
                unitOfWork.Company.Update(companyObj);
            }

            unitOfWork.Save();
            TempData["success"] = "Company created successfully";
            return RedirectToAction(nameof(Index));
        }

        return View(companyObj);
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        var objCompanyList = unitOfWork.Company.GetAll().ToList();
        return Json(new { data = objCompanyList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var objToBeDeleted = unitOfWork.Company.Get(u => u.Id == id);
        if (objToBeDeleted == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        unitOfWork.Company.Remove(objToBeDeleted);
        unitOfWork.Save();

        return Json(new { success = true, message = "Delete successful" });
    }

    #endregion
}