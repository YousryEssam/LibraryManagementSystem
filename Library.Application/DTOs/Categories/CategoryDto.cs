namespace Library.Application.DTOs.Categories;

public sealed class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ParentName { get; set; }
}
