using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository user)
        {
            _userRepository = user;
        }

        public async Task<bool> CreateUserAsync(CreateUserRequestDto request)
        {
            var existingUser = await _userRepository
                .GetByEmailAsync(request.Email);

            if (existingUser != null)
                return false;

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "Staff",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(user => new UserResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            }).ToList();
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            return new UserResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserRequestDto request)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            var existingUser = await _userRepository
                .GetByEmailAsync(request.Email);

            if (existingUser != null &&
                existingUser.UserId != id)
            {
                return false;
            }

            user.Name = request.Name;
            user.Email = request.Email;

            await _userRepository.UpdateAsync(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}
