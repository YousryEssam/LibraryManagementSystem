using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Auth;

public class CreateSystemUserDto
{
    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    /// <summary>One or more of: Administrator, Librarian, Staff.</summary>
    [Required, MinLength(1)]
    public string[] Roles { get; set; } = Array.Empty<string>();
}
