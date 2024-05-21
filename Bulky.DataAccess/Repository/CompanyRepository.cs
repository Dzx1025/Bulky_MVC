using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;

public class CompanyRepository(ApplicationDbContext _db) : Repository<Company>(_db), ICompanyRepository
{
    public void Update(Company obj)
    {
        _db.Companies.Update(obj);
    }
}