using System.Linq.Expressions;

namespace order_app.entities.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
    }
}
