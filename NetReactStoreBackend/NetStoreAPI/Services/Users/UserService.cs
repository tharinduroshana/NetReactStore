using Microsoft.EntityFrameworkCore;
using NetStoreAPI.Data;
using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Services.Tokens;
using NetStoreAPI.Utils;

namespace NetStoreAPI.Services.Users;

public class UserService : IUserService
{
    private readonly StoreContext _storeContext;
    private readonly ITokenService _tokenService;

    public UserService(StoreContext storeContext, ITokenService tokenService)
    {
        _storeContext = storeContext;
        _tokenService = tokenService;
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
            return OperationResult<User>.Failure("Internal Server Error.", 500);
        }
    }

    public async Task<OperationResult<UserLoginResponseDto>> LoginUser(UserLoginDto request)
    {
        try
        {
            var existingUser = await _storeContext.Users.FindAsync(request.Username);
            
            if (existingUser == null)
            {
                return OperationResult<UserLoginResponseDto>.Failure("User not found.", 404);
            }
            
            var isPasswordValid = PasswordUtils.VerifyPassword(request.Password, existingUser.PasswordHash, existingUser.PasswordSalt);

            if (!isPasswordValid)
            {
                return OperationResult<UserLoginResponseDto>.Failure("Invalid Credentials.", 401);
            }

            return OperationResult<UserLoginResponseDto>.SuccessResult(ConvertUserToUserLoginResponseDto(existingUser));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return OperationResult<UserLoginResponseDto>.Failure("Internal Server Error.", 500);
        }
    }

    public UserLoginResponseDto ConvertUserToUserLoginResponseDto(User user)
    {
        return new UserLoginResponseDto
        {
            Username = user.Username,
            Name = user.Name,
            Token = _tokenService.GenerateToken(user)
        };
    }

    public async Task<User> FetchUser(FetchUserDto request)
    {
        return await _storeContext.Users.FindAsync(request.Username);
    }
}