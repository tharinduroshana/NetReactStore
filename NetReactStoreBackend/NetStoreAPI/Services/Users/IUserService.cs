using NetStoreAPI.DTOs;
using NetStoreAPI.Entities;
using NetStoreAPI.Utils;

namespace NetStoreAPI.Services.Users;

public interface IUserService
{
    Task<OperationResult<User>> CreateUser(UserSignUpDto request);
    Task<OperationResult<User>> LoginUser(UserLoginDto request);
}