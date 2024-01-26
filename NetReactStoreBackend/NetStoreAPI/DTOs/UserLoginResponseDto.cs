namespace NetStoreAPI.DTOs;

public class UserLoginResponseDto
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
    public BasketDto Basket { get; set; }
}