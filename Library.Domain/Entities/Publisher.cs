namespace Library.Domain.Entities;

public class Publisher : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Address { get; set; }

    public string? ContactEmail { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    // Navigation Properties
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
