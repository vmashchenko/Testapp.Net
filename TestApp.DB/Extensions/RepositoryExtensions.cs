using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.Domain;

namespace TestApp.DB.Extensions
{
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Compare two collections and call Add/Update/Delete on the repository.
        /// Appropriate for simple entities.
        /// </summary>
        public static void UpdateCollection<TEntity, TKey>(
            this IRepository<TEntity> repository,
            ICollection<TEntity> oldItems,
            ICollection<TEntity> newItems)
            where TEntity : EntityBase<TKey>
            where TKey : IEquatable<TKey>
        {
            var oldItemsDict = oldItems.ToDictionary(i => i.Id, i => i);
            var newItemsIds = new HashSet<TKey>(newItems.Select(i => i.Id));

            var itemsToDelete = oldItems.Where(item => !newItemsIds.Contains(item.Id));
            foreach (var victim in itemsToDelete)
            {
                repository.Delete(victim);
            }

            var itemsToUpdate = newItems.Where(item => oldItemsDict.ContainsKey(item.Id));
            foreach (var shapeShifter in itemsToUpdate)
            {
                repository.Update(shapeShifter);
            }

            var itemsToAdd = newItems.Where(item => !oldItemsDict.ContainsKey(item.Id));
            foreach (var newborn in itemsToAdd)
            {
                repository.Add(newborn);
            }
        }

        /// <summary>
        /// Compare two collections and call Add/Update/Delete on the repository.
        /// Appropriate for more complex entities, e.g. AggregateRoot derived ones
        /// </summary>
        public static void UpdateCollection<TEntity, TKey, TViewModel>(
            this IRepository<TEntity> repository,
            ICollection<TEntity> oldItems,
            ICollection<TViewModel> newItems,
            Func<TViewModel, TKey> keySelector,
            Func<TViewModel, TEntity> entityCreator,
            Func<TEntity, TViewModel, TEntity> updateMerger)
            where TEntity : EntityBase<TKey>
            where TKey : IEquatable<TKey>
        {
            var oldItemsDict = oldItems.ToDictionary(i => i.Id, i => i);
            var newItemsIds = new HashSet<TKey>(newItems.Select(keySelector));

            var itemsToDelete = oldItems.Where(item => !newItemsIds.Contains(item.Id));
            foreach (var victim in itemsToDelete)
            {
                repository.Delete(victim);
            }

            var itemsToUpdate = newItems.Where(item => oldItemsDict.ContainsKey(keySelector(item)));
            foreach (var newItem in itemsToUpdate)
            {
                var updatedItem = updateMerger(oldItemsDict[keySelector(newItem)], newItem);
                repository.Update(updatedItem);
            }

            var itemsToAdd = newItems.Where(item => !oldItemsDict.ContainsKey(keySelector(item)));
            foreach (var newborn in itemsToAdd)
            {
                repository.Add(entityCreator(newborn));
            }
        }
    }
}
