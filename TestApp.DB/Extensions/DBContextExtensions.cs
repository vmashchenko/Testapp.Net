using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestApp.Domain;

namespace TestApp.DB.Extensions
{
    public static class DBContextExtensions
    {
        public static void SetPropertyConvention(
            this ModelBuilder instance,
            Func<IMutableEntityType, bool> entityTypeFilter,
            Func<IMutableProperty, bool> propertyFilter,
            Action<IMutableProperty> conventionLogic)
        {
            foreach (var property in instance.Model.GetEntityTypes().Where(entityTypeFilter)
               .SelectMany(t => t.GetProperties())
               .Where(propertyFilter).ToArray())
            {
                conventionLogic(property);
            }
        }

        public static void SetPropertyConvention(
            this ModelBuilder instance,
            Func<IMutableProperty, bool> propertyFilter,
            Action<IMutableProperty> conventionLogic)
        {
            instance.SetPropertyConvention(e => true, propertyFilter, conventionLogic);
        }

        public static void SetEntityConvention(
            this ModelBuilder instance,
            Func<IMutableEntityType, bool> entityTypeFilter,
            Action<IMutableEntityType> conventionLogic)
        {
            foreach (var entityType in instance.Model.GetEntityTypes().Where(entityTypeFilter))
            {
                //entityType.own
                conventionLogic(entityType);

            }
        }

        public static ModelBuilder AggregateRoot<TEntity>(this ModelBuilder instance)
            where TEntity : AggregateRoot
        {
            instance.Entity<TEntity>(entity => entity.AggregateRoot());
            return instance;
        }

        public static ModelBuilder AggregateRoot<TEntity>(this ModelBuilder instance, Action<EntityTypeBuilder<TEntity>> buildAction)
            where TEntity : AggregateRoot
        {
            instance.Entity<TEntity>(entity => entity.AggregateRoot());

            instance.Entity(buildAction);

            return instance;
        }

        public static EntityTypeBuilder<TEntity> AggregateRoot<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder)
            where TEntity : AggregateRoot
        {
            //entityTypeBuilder
            //    .HasOne(d => d.CreatedUser)
            //    .WithMany()
            //    .HasForeignKey(d => d.CreatedUserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            //entityTypeBuilder.HasOne(d => d.ModifiedUser)
            //    .WithMany()
            //    .HasForeignKey(d => d.ModifiedUserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            return entityTypeBuilder;
        }
    }
}
