namespace NetStoreAPI.Entities.OrderAggregate;

public class Order
{
    public int Id { get; set; }
    public string BuyerId { get; set; }
    public string PaymentIntentId { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> OrderItems { get; set; }
    public long SubTotal { get; set; }
    public long DeliveryFee { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    public long GetTotal()
    {
        return SubTotal + DeliveryFee;
    }
}