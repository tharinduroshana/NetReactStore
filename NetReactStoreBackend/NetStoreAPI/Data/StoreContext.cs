using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NetStoreAPI.Entities;

namespace NetStoreAPI.Data;

public class StoreContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Basket> Baskets { get; set; }
    public StoreContext(DbContextOptions<StoreContext> dbContextOptions) : base(dbContextOptions)
    {
        try
        {
            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator != null)
            {
                if (!databaseCreator.CanConnect()) databaseCreator.Create();
                
                if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
            }
        }
        catch (Exception e)
        {
            Console.Write(e);
        }
    }
}