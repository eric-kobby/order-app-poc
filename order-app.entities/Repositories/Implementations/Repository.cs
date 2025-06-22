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

        public IEnumerable<T> GetAll()
        {
            return _set;
        }

        public T? GetById(int id)
        {
            return _set.Find(id);
        }

        public void Update(T entity)
        {
            _set.Update(entity);
        }
    }
}
