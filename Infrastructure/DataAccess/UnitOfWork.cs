using Core.Interfaces;
using Infrastructure.DataModel;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly DataContext _db;


        public UnitOfWork(DataContext db)
        {
            _db = db;
            Entity = new Repository<T>(db);
        }


        public IRepository<T> Entity { get; init; }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
