using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Domain
{
    /// <summary>
    /// Represents Repository pattern.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entities { get; }
        Task<T> Find(params object[] keyValues);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChanges();

        void Attach(T entity);
        void MarkAsDeleted(object entity);
        void MarkAsModified(T entity);
    }
}
