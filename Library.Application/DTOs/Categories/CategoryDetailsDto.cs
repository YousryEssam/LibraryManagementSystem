namespace Library.Application.DTOs.Categories;

public sealed class CategoryDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public string? ParentName { get; set; }
    public int BookCount { get; set; }
    public IEnumerable<CategoryDto> SubCategories { get; set; } = [];
}
