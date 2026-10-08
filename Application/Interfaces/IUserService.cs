using Application.DTOs.Auth;
using Application.DTOs.Users;

namespace Application.Interfaces;

public interface IUserService
{
    Task<bool> CreateUserAsync(CreateUserRequestDto request);

    Task<List<UserResponseDto>> GetAllUsersAsync();

    Task<UserResponseDto?> GetUserByIdAsync(int id);

    Task<bool> UpdateUserAsync(int id, UpdateUserRequestDto request);

    Task<bool> DeleteUserAsync(int id);
}