using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Utils;

namespace NetStoreAPI.Services.Users;

public interface IUserService
{
    Task<OperationResult<User>> CreateUser(UserSignUpDto request);
    Task<OperationResult<UserLoginResponseDto>> LoginUser(UserLoginDto request);
    UserLoginResponseDto ConvertUserToUserLoginResponseDto(User user);
    Task<User> FetchUser(FetchUserDto request);
}