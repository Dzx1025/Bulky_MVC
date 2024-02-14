﻿using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository;

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
