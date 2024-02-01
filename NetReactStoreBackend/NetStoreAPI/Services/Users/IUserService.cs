using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Utils;

namespace NetStoreAPI.Services.Users;

public interface IUserService
{
    Task<OperationResult<User>> CreateUser(UserSignUpDto request);
    Task<OperationResult<UserLoginResponseDto>> LoginUser(UserLoginDto request);
    Task<UserLoginResponseDto> ConvertUserToUserLoginResponseDto(User user);
    Task<User> FetchUser(FetchUserDto request);
    Task<Address> GetSavedUserAddress(string username);
}