using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Doctor_Management.IRepository
{
   public interface IRepositoryData<Entity> where Entity : class
    {
        void Add(Entity entity);

        void Add(IEnumerable<Entity> entitys);

        Task AddAsync(Entity entity);

        Task AddAsync(IEnumerable<Entity> entity);

        void Update(Entity entity);

        Task UpdateAsync(Entity entity);

        void Delete(Entity entity);

        void Delete(int? id);

        Task DeleteAsync(Entity entity);

        Task DeleteAsync(int? id);

        IEnumerable<Entity> GetAll();

        Task<IEnumerable<Entity>> GetAllAsync();

        Entity Find(int? id);

        Entity Find(Func<Entity , bool> func);

        Task<Entity> FindAsync(int? id);

        Task<Entity> FindAsync(Func<Entity, bool> func);

        IEnumerable<Entity> Get(Func<Entity, bool> func);

        Task<IEnumerable<Entity>> GetAsync(Func<Entity, bool> func);

        bool Any(Func<Entity, bool> func);

        IQueryable<Entity> GetClude();

        void Delete(IEnumerable<Entity> entities);

        Task DeleteAsync(IEnumerable<Entity> entities);

        void Update(IEnumerable<Entity> entities);

        Task UpdateAsync(IEnumerable<Entity> entities);

    }
}
