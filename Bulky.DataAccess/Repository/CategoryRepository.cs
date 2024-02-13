using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class CategoryRepository(ApplicationDbContext _db) : Repository<Category>(_db), ICategoryRepository
{
    private readonly ApplicationDbContext _db = _db;

    public void Save()
    {
        _db.SaveChanges();
    }

    public void Update(Category obj)
    {
        _db.Update(obj);
    }
}
