using NetStoreAPI.Entities.OrderAggregate;

namespace NetStoreAPI.DTOs;

public class CreateOrderDto
{
    public bool SaveAddress { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
    public string Username { get; set; }
}