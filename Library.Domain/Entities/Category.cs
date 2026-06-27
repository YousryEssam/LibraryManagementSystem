namespace Library.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Self-referencing for hierarchical structure (e.g. Science > Physics > Quantum)
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }

    // Navigation Properties
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
