namespace NetStoreAPI.Entities;

public class Basket
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public List<BasketItem> Items { get; set; } = new();

    public void AddItem(Product product, int quantity)
    {
        if (Items.All( basketItem => basketItem.ProductId != product.Id ))
        {
            Items.Add(new BasketItem { Product = product, Quantity = quantity });
        }

        var existingItem = Items.FirstOrDefault(basketItem => basketItem.ProductId == product.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
    }

    public void RemoveItem(int productId, int quantity)
    {
        var item = Items.FirstOrDefault(basketItem => basketItem.ProductId == productId);
        
        if (item == null) return;
        
        if (item.Quantity > quantity)
        {
            item.Quantity -= quantity;
        }
        else
        {
            Items.Remove(item);
        }
    }
}