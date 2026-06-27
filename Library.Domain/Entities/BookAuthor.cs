namespace Library.Domain.Entities;

/// <summary>
/// Join table for the many-to-many relationship between Book and Author.
/// Inherits BaseEntity for the composite key convenience via EF config.
/// </summary>
public class BookAuthor : BaseEntity
{
    public int BookId { get; set; }
    public int AuthorId { get; set; }

    public BookAuthorRole Role { get; set; } = BookAuthorRole.Author;
    // Navigation Properties
    public Book Book { get; set; } = null!;
    public Author Author { get; set; } = null!;
}
