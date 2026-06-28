using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Publishers;

public sealed class CreatePublisherDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(150), EmailAddress]
    public string? ContactEmail { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(300), Url]
    public string? Website { get; set; }
}
