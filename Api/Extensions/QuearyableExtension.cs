using Shared.Queries;

namespace Api.Extensions;

public static class QuearyableExtension
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagerQuery pager)
    {
        if (pager.Number <= 0 || pager.Size <= 0) return query;

        return query.Skip((pager.Number - 1) * pager.Size).Take(pager.Size);
    }
}