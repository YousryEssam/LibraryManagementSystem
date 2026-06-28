using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Books;

public sealed class UpdateBookAuthorRoleDto
{
    [Required]
    public BookAuthorRole Role { get; set; }
}
