using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Categories;

public sealed class CreateCategoryDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int? ParentCategoryId { get; set; }
}
