using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Utils;

namespace NetStoreAPI.Services.Users;

public class UserService : IUserService
{
    private readonly StoreContext _storeContext;

    public UserService(StoreContext storeContext)
    {
        _storeContext = storeContext;
    }

    public async Task<OperationResult<User>> CreateUser(UserSignUpDto request)
    {
        try
        {
            var existingUser = await _storeContext.Users.FindAsync(request.Username);

            if (existingUser != null)
            {
                return OperationResult<User>.Failure("User already exists.", 409);
            }

            var (passwordHash, passwordSalt) = PasswordUtils.CreatePasswordHash(request.Password);

            var user = new User
            {
                Username = request.Username,
                Name = request.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _storeContext.Users.AddAsync(user);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0 ? OperationResult<User>.SuccessResult(user) : OperationResult<User>.Failure("Failed to create user.", 500);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}