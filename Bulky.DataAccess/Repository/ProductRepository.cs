using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;

public class ProductRepository(ApplicationDbContext _db) : Repository<Product>(_db), IProductRepository
{
    private readonly ApplicationDbContext _db = _db;

    public void Update(Product obj)
    {
        _db.Products.Update(obj);
    }
}
