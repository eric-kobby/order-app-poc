using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace order_app.entities.Repositories.Implementations
{
    public class Repository<T>(DbContext ctx) : IRepository<T> where T : class
    {
        private readonly DbSet<T> _set = ctx.Set<T>();

        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public void Delete(T entity)
        {
            _set.Remove(entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _set;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await ctx.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _set.Update(entity);
        }
    }
}
