using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

using TestApp.Domain;

namespace TestApp.DB.Extensions
{
    public static class QueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly FieldInfo QueryModelGeneratorField = QueryCompilerTypeInfo.DeclaredFields.First(x => x.Name == "_queryModelGenerator");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        private static readonly IDictionary<string, MethodInfo> Sorters = typeof(Queryable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(mi => mi.Name.StartsWith("OrderBy") && mi.GetParameters().Length == 2)
            .ToDictionary(mi => mi.Name);

        public static IQueryable<T> Page<T>(this IQueryable<T> query, IPaging paging)
        {
            if (paging.PageSize == null || paging.PageNumber == null)
                return query;

            var skip = ((paging.PageNumber - 1) * paging.PageSize).Value;
            var take = paging.PageSize.Value;

            return query
                .Skip(skip)
                .Take(take);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, ISorting sorting)
            => Sort(query, sorting, null);

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, ISorting sorting, IDictionary<string, LambdaExpression> mappedProperties)
        {
            if (sorting.SortBy == null)
                return query;

            var sortPropertyExp = mappedProperties != null && mappedProperties.TryGetValue(sorting.SortBy, out var pe)
                ? pe
                : GetPropertyExpression<T>(sorting.SortBy);

            var methodName = sorting.SortDir == SortDirection.Desc ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy);

            var method = Sorters[methodName].MakeGenericMethod(sortPropertyExp.Type.GetGenericArguments());

            query = (IQueryable<T>)method.Invoke(query, new object[] { query, sortPropertyExp });

            return query;
        }

        /// <returns>expression of type p=>p.PropertyName</returns>
        private static LambdaExpression GetPropertyExpression<T>(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            return GetPropertyExpression(propertyInfo);
        }

        /// <returns>expression of type p=>p.PropertyName</returns>
        private static LambdaExpression GetPropertyExpression(PropertyInfo propertyInfo)
        {
            var parameter = Expression.Parameter(propertyInfo.ReflectedType, "o");

            var type = typeof(Func<,>).MakeGenericType(propertyInfo.ReflectedType, propertyInfo.PropertyType);
            var body = Expression.MakeMemberAccess(parameter, propertyInfo);
            var parameters = new[] { parameter };

            return Expression.Lambda(type, body, parameters);
        }
    }
}
