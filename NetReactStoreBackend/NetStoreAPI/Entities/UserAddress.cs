using System.ComponentModel.DataAnnotations.Schema;

namespace NetStoreAPI.Entities;

[Table("UserAddresses")]
public class UserAddress : Address
{
    public string Id { get; set; }
}