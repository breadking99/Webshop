using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Queries;

namespace Api.Extensions;

public static class ProductExtension
{
    public static IQueryable<Product> SelectOrderCounts(this IQueryable<Product> queryable) => queryable
        .Select(o => new Product
        {
            Id = o.Id,
            Name = o.Name,
            Store = o.Store,
            OrderProducts = o.OrderProducts!.Select(op => new OrderProduct
            {
                Count = op.Count
            }).ToList()
        });

    public static IQueryable<Product> FilterProduct(this IQueryable<Product> queryable, ProductFilter? filter)
    {
        if (filter == null) return queryable;

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            queryable = queryable.Where(p => p.Name.Contains(filter.NameContains));
        }

        if (filter.OnlyAvailable)
        {
            queryable = queryable.Where(p => p.Store - (p.OrderProducts!.Sum(op => (int?)op.Count) ?? 0) > 0);
        }

        if (filter.Number > 0 && filter.Size > 0)
        {
            queryable = queryable.ApplyPaging(filter);
        }

        return queryable;
    }
}