namespace Library.Application.DTOs.Books;

public sealed class BookDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string ISBN { get; set; } = default!;
    public string? Edition { get; set; }
    public int? PublicationYear { get; set; }
    public string? Language { get; set; }
    public string? Summary { get; set; }
    public string? CoverImageUrl { get; set; }
    public BookStatus Status { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = default!;
    public int PublisherId { get; set; }
    public string PublisherName { get; set; } = default!;
    public IEnumerable<BookAuthorDto> Authors { get; set; } = [];
}
