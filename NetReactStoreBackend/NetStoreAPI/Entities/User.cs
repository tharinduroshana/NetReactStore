namespace NetStoreAPI.Entities;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}