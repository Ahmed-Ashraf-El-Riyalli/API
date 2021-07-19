using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetByID(object id);

        Task<IEnumerable<TOut>> GetAllWithOptions<TOut>(
            Expression<Func<T, TOut>> selector,
            Expression<Func<T, bool>> filter = null) where TOut : class;

        Task<TOut> GetOneWithOptions<TOut>(
            Expression<Func<T, TOut>> selector,
            Expression<Func<T, bool>> filter = null) where TOut : class;

        Task<T> DeleteByID(object id);

        void UpdateByID(object id, T entity);

        Task Create(T entity);
    }
}
