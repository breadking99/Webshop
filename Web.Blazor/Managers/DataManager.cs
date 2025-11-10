using Newtonsoft.Json.Linq;
using Shared.Models;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Blazor.Managers;

public static class DataManager
{
    private static AuthData authData = new();
    private static string token = string.Empty;

    public static event Action? StateChanged;

    public static bool IsLoggedIn => authData.Success;

    public static string Token => authData.Token ?? string.Empty;

    public static AuthData AuthData
    {
        get => authData;
        set
        {
            authData = value ?? new AuthData();
            NotifyStateChanged();
        }
    }

    public static Order CurrentOrder { get; private set; } = CreateOrder();

    public static bool HasOrderItems => CurrentOrder.OrderProducts?.Any() ?? false;

    public static void ResetAuth()
    {
        authData = new AuthData();
        ResetOrder();
    }

    public static void ResetOrder()
    {
        CurrentOrder = CreateOrder();
        NotifyStateChanged();
    }

    public static void IncrementProduct(Product product, int count)
    {
        if (product == null || count <= 0) return;
        EnsureOrderProducts();
        OrderProduct? existing = CurrentOrder.OrderProducts!
            .FirstOrDefault(x => x.ProductId == product.Id);

        if (existing == null)
        {
            CurrentOrder.OrderProducts!.Add(new OrderProduct
            {
                ProductId = product.Id,
                Product = product,
                Count = count
            });
        }
        else
        {
            existing.Count += count;
            existing.Product ??= product;
        }

        NotifyStateChanged();
    }

    public static void SetProductCount(int productId, int count)
    {
        if (count < 0) count = 0;
        if (CurrentOrder.OrderProducts == null) return;
        OrderProduct? existing = CurrentOrder.OrderProducts
            .FirstOrDefault(x => x.ProductId == productId);
        if (existing == null) return;

        if (count == 0)
        {
            CurrentOrder.OrderProducts.Remove(existing);
        }
        else
        {
            existing.Count = count;
        }

        NotifyStateChanged();
    }

    public static void RemoveProduct(int productId)
    {
        if (CurrentOrder.OrderProducts == null) return;
        OrderProduct? existing = CurrentOrder.OrderProducts
            .FirstOrDefault(x => x.ProductId == productId);
        if (existing == null) return;
        CurrentOrder.OrderProducts.Remove(existing);
        NotifyStateChanged();
    }

    public static Order CreateOrderRequest()
    {
        Order request = new()
        {
            OrderProducts = CurrentOrder.OrderProducts?
                .Select(x => new OrderProduct
                {
                    ProductId = x.ProductId,
                    Count = x.Count
                })
                .Where(x => x.Count > 0)
                .ToList()
        };

        return request;
    }

    private static void EnsureOrderProducts()
    {
        CurrentOrder.OrderProducts ??= new List<OrderProduct>();
    }

    private static Order CreateOrder()
    {
        return new Order
        {
            OrderProducts = new List<OrderProduct>()
        };
    }

    private static void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
