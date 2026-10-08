using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Users;

public class UpdateUserRequestDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}