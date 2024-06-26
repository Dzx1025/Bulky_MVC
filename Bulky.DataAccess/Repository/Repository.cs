﻿using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BulkyBook.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;

    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        dbSet = db.Set<T>();
        _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
    }

    public void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        var query = dbSet.AsQueryable();
        query = query.Where(filter);
        if (string.IsNullOrEmpty(includeProperties)) return query.FirstOrDefault();
        query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProp) => current.Include(includeProp));

        return query.FirstOrDefault();
    }

    public IEnumerable<T> GetAll(string? includeProperties = null)
    {
        var query = dbSet.AsQueryable();
        if (string.IsNullOrEmpty(includeProperties)) return [.. query];
        query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProp) => current.Include(includeProp));

        return [.. query];
    }

    public void Remove(T entity)
    {
        dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }
}