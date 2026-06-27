namespace Library.Domain.Entities;

public class Book : AuditableEntity
{
    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public string? Edition { get; set; }

    public int? PublicationYear { get; set; }

    public string? Language { get; set; }

    public string? Summary { get; set; }

    public string? CoverImageUrl { get; set; }

    public BookStatus Status { get; set; } = BookStatus.Available;

    // Foreign Keys
    public int PublisherId { get; set; }
    public int CategoryId { get; set; }

    // Navigation Properties
    public Publisher Publisher { get; set; } = null!;
    public Category Category { get; set; } = null!;

    // Many-to-Many
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<BorrowingTransaction> BorrowingTransactions { get; set; } = new List<BorrowingTransaction>();
}
