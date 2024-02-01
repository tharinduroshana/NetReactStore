using System.ComponentModel.DataAnnotations;

namespace NetStoreAPI.Entities;

public class User
{
    [Key]
    public string Username { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public UserAddress Address { get; set; }
}