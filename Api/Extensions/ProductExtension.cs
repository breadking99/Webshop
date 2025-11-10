using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Api.Extensions;

public static class ProductExtension
{
    public static IQueryable<Product> IncludeOrderCounts(this IQueryable<Product> queryable) => queryable
        .Include(p => p.OrderProducts)!
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
}