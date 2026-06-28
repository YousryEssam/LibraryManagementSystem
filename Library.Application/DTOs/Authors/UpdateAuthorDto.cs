using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Authors;

public sealed class UpdateAuthorDto
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = default!;

    [MaxLength(2000)]
    public string? Bio { get; set; }

    [MaxLength(100)]
    public string? Nationality { get; set; }
}
