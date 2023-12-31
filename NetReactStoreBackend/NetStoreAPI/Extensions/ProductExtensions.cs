using NetStoreAPI.Entities;

namespace NetStoreAPI.Extensions;

public static class ProductExtensions
{
    public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
    {
        if (string.IsNullOrEmpty(orderBy)) return query.OrderBy(product => product.Name);
        
        query = orderBy switch
        {
            "price" => query.OrderBy(product => product.Price),
            "priceDesc" => query.OrderByDescending(product => product.Price),
            _ => query.OrderBy(product => product.Name)
        };

        return query;
    }
}