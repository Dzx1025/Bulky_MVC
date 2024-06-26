using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories;

public class IndexModel(ApplicationDbContext db) : PageModel
{
    public readonly ApplicationDbContext _db = db;
    public List<Category> CategoryList { get; set; }

    public void OnGet()
    {
        CategoryList = [.. _db.Categories];
    }
}
