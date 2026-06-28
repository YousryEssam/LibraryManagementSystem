namespace Library.Application.DTOs.Books;

public sealed class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string ISBN { get; set; } = default!;
    public string? Edition { get; set; }
    public int? PublicationYear { get; set; }
    public string? Language { get; set; }
    public string CategoryName { get; set; } = default!;
    public string PublisherName { get; set; } = default!;
    public BookStatus Status { get; set; }
    public IEnumerable<BookAuthorDto> Authors { get; set; } = [];
}
