using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.Entities;
using NetStoreAPI.Extensions;

namespace NetStoreAPI.Controllers;

public class ProductsController : BaseApiController
{
    private readonly StoreContext _context;
    
    public ProductsController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts(string orderBy)
    {
        var query =  _context.Products.Sort(orderBy).AsQueryable();

        return await query.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public ActionResult<Product> CreateProduct()
    {
        var product = new Product
        {
            Name = "Samsung Galaxy S23",
            Description = "Samsung Galaxy S23",
            Brand = "Samsung",
            PictureUrl = "/images/s23.jpeg",
            Price = 20000,
            QuantityInStock = 25,
            Type = "Type A"
        };
        _context.Products.Add(product);
        _context.SaveChanges();

        return product;
    }
}