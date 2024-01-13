using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.Entities;
using NetStoreAPI.Extensions;
using NetStoreAPI.RequestHelpers;

namespace NetStoreAPI.Controllers;

public class ProductsController : BaseApiController
{
    private readonly StoreContext _context;

    public ProductsController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery]ProductParams productParams)
    {
        var query = _context.Products
            .Sort(productParams.OrderBy)
            .Search(productParams.SearchTerm)
            .Filter(productParams.Brands, productParams.Types)
            .AsQueryable();

        var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);
        
        Response.AddPaginationHeader(products.MetaData);

        return products;
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

    [HttpGet("filters")]
    public async Task<IActionResult> GetFilters()
    {
        var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
        var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

        return Ok(new { brands, types });
    }
}