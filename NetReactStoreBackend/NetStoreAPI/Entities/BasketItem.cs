namespace NetStoreAPI.Entities;

public class BasketItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    // Navigation Properties
    public int ProductId { get; set; }
    public Product Product { get; set; }
}