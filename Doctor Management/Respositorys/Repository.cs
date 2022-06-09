using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Doctor_Management.IRepository;
using Doctor_Management.Models;
using Microsoft.EntityFrameworkCore;


namespace Doctor_Management.Respositorys
{
    public class Repository<T> : IRepositoryData<T> where T: class
    {
        private readonly DataContext db;

        public Repository(DataContext db)
        {
            this.db = db;
        }
        public void Add(T entity)
        {
            db.Set<T>().Add(entity);
            db.SaveChanges();
        }

        public void Add(IEnumerable<T> entitys)
        {
            db.Set<T>().AddRange(entitys);
            db.SaveChanges();
        }

        public async Task AddAsync(T entity)
        {
            await db.Set<T>().AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<T> entity)
        {
            await db.Set<T>().AddRangeAsync(entity);
            await db.SaveChangesAsync();
        }

        public bool Any(Func<T, bool> func)
        {
            return db.Set<T>().AsNoTracking().Any(func);
        }

        public void Delete(T entity)
        {
            db.Set<T>().Remove(entity);
            db.SaveChanges();
        }
        public void Delete(int? id)
        {
            var model = Find(id);
            db.Set<T>().Remove(model);
            db.SaveChanges();
        }
        public async Task DeleteAsync(T entity)
        {
            db.Set<T>().Remove(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int? id)
        {
            if (id == null)
                return;

            var model = await db.Set<T>().FindAsync(id);
            db.Set<T>().Remove(model);
            await db.SaveChangesAsync();
        }

        public T Find(int? id)
        {
            if (id == null)
                return null;

            return db.Set<T>().Find(id);
        }

        public T Find(Func<T, bool> func)
        {
            return db.Set<T>().AsNoTracking().Where(func).FirstOrDefault();
        }

        public async Task<T> FindAsync(int? id)
        {
            return await db.Set<T>().FindAsync(id);
        }

        public async Task<T> FindAsync(Func<T, bool> func)
        {
            return  db.Set<T>().AsNoTracking().Where(func).FirstOrDefault();
        }

        public IEnumerable<T> Get(Func<T, bool> func)
        {
            return db.Set<T>().AsNoTracking().Where(func).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return db.Set<T>().AsNoTracking().ToList();
        }

   
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await db.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAsync(Func<T, bool> func)
        {
            return  db.Set<T>().AsNoTracking().Where(func).ToList();
        }

        public void Update(T entity)
        {
            db.Set<T>().Update(entity);
            db.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            db.Set<T>().Update(entity);
            await db.SaveChangesAsync();
        }

        public IQueryable<T> GetClude()
        {
            return db.Set<T>().AsNoTracking().AsQueryable();
        }

        public void Delete(IEnumerable<T> entities)
        {
            db.Set<T>().RemoveRange(entities);
            db.SaveChanges();
        }

        public async Task DeleteAsync(IEnumerable<T> entities)
        {
            db.Set<T>().RemoveRange(entities);
            await db.SaveChangesAsync();
        }

        public void Update(IEnumerable<T> entities)
        {
            db.Set<T>().UpdateRange(entities);
            db.SaveChanges();
        }

        public async Task UpdateAsync(IEnumerable<T> entities)
        {
            db.Set<T>().UpdateRange(entities);
            await db.SaveChangesAsync();
        }
    }
}
