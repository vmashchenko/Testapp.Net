using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.DB.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using TestApp.Domain;
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected ICurrentUserProvider CurrentUserProvider { get; }
        protected AppDbContext DbContext { get; }

        public RepositoryBase(
            AppDbContext dbContext,
            ICurrentUserProvider currentUserProvider
        )
        {
            CurrentUserProvider = currentUserProvider;
            DbContext = dbContext;
        }

        public IQueryable<T> Entities => DbContext.Set<T>();

        public virtual Task<T> Find(params object[] keyValues)
            => DbContext.FindAsync<T>(keyValues).AsTask();

        public virtual void Add(T entity)
        {
            var now = DateTime.UtcNow;
            SetCreationProperties(entity, now);
            SetLastChangeProperties(entity, now);
            DbContext.Add(entity);
        }
        public virtual void Update(T entity)
        {
            SetLastChangeProperties(entity);
            DbContext.Update(entity);
        }

        public virtual void Delete(T entity)
            => DbContext.Remove(entity);

        public virtual void Attach(T entity)
            => DbContext.Attach(entity);

        public virtual void MarkAsDeleted(object entity)
            => DbContext.Entry(entity).State = EntityState.Deleted;

        public virtual void MarkAsModified(T entity)
            => DbContext.Entry(entity).State = EntityState.Modified;

        public virtual Task SaveChanges()
            => DbContext.SaveChangesAsync();

        private void SetCreationProperties(T entity, DateTime? now = null)
        {
            if (entity is ICreationTrackable ct)
            {
                ct.CreatedUserId = CurrentUserProvider.GetUserId();
                ct.CreatedDateTime = now ?? DateTime.UtcNow;
            }
        }

        private void SetLastChangeProperties(T entity, DateTime? now = null)
        {
            if (entity is ILastChangeTrackable lct)
            {
                lct.ModifiedUserId = CurrentUserProvider.GetUserId();
                lct.ModifiedDateTime = now ?? DateTime.UtcNow;
            }
        }
    }
}
