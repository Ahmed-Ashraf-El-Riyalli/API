using Core.Interfaces;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _db;
        private readonly DbSet<T> _table;


        public Repository(DataContext db)
        {
            _db = db;
            _table = db.Set<T>();
        }


        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> GetByID(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<IEnumerable<TOut>> GetAllWithOptions<TOut>(
            Expression<Func<T, TOut>> selector,
            Expression<Func<T, bool>> filter = null) where TOut : class
        {
            if(filter != null)
                return await _table.Where(filter).Select(selector).ToListAsync();

            return await _table.Select(selector).ToListAsync();
        }

        public async Task<TOut> GetOneWithOptions<TOut>(
            Expression<Func<T, TOut>> selector,
            Expression<Func<T, bool>> filter = null) where TOut : class
        {
            if(filter != null)
                return await _table.Where(filter).Select(selector).FirstOrDefaultAsync();

            return await _table.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<T> DeleteByID(object id)
        {
            T entity = await _table.FindAsync(id);

            if (entity == null)
                return null;

            _table.Remove(entity);

            return entity;
        }

        public void UpdateByID(object id, T entity)
        {
            _table.Update(entity);
        }

        public async Task Create(T entity)
        {
            if (entity != null)
                await _table.AddAsync(entity);
        }
    }
}
