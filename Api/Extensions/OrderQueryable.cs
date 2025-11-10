using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Api.Extensions;

public static class OrderQueryable
{
    public static IQueryable<Order> IncludeProducts(this IQueryable<Order> queryable) => queryable
        .Include(o => o.OrderProducts)!
            .ThenInclude(op => op.Product)
        .Select(o => new Order
        {
            Id = o.Id,
            OrderProducts = o.OrderProducts!.Select(op => new OrderProduct
            {
                Id = op.Id,
                OrderId = op.OrderId,
                ProductId = op.ProductId,
                Count = op.Count,
                Product = new Product
                {
                    Id = op.Product!.Id,
                    Name = op.Product.Name
                },
            }).ToList()
        });
}