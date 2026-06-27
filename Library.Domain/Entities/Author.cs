namespace Library.Domain.Entities;

public class Author : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public string? Nationality { get; set; }

    // Many-to-Many
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}
