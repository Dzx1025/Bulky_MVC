using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext _db) : Repository<Product>(_db), IProductRepository
{
    private readonly ApplicationDbContext _db = _db;

    public void Update(Product obj)
    {
        var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
        if (objFromDb != null)
        {
            objFromDb.Title = obj.Title;
            objFromDb.ISBN = obj.ISBN;
            objFromDb.Price = obj.Price;
            objFromDb.Price50 = obj.Price50;
            objFromDb.ListPrice = obj.ListPrice;
            objFromDb.Price100 = obj.Price100;
            objFromDb.Description = obj.Description;
            objFromDb.CategoryId = obj.CategoryId;
            objFromDb.Author = obj.Author;
            if (objFromDb.ImageUrl != null)
            {
                objFromDb.ImageUrl = obj.ImageUrl;
            }

        }
    }
}