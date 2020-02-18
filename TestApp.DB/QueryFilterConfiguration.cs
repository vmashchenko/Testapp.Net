using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TestApp.DB
{
    public abstract class QueryFilterConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        private readonly Expression<Func<T, bool>> _queryFilter;

        protected QueryFilterConfiguration(Expression<Func<T, bool>> queryFilter)
        {
            _queryFilter = queryFilter;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasQueryFilter(_queryFilter);
            ConfigureEntity(builder);
        }

        protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
    }
}
