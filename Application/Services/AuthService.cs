using Application.DTOs.Auth;
using Application.Interfaces;
using BCrypt.Net;
using Domain.Entities;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto request)
    {
        var user = await _userRepository
            .GetByEmailAsync(request.Email);

        if (user == null)
            return null;

        if (!user.IsActive)
            return null;

        bool passwordValid = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash
        );

        if (!passwordValid)
            return null;

        var token = _jwtService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            UserName = user.Name,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30)
        };
    }

    
}