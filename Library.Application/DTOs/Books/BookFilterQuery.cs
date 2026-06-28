namespace Library.Application.DTOs.Books;

public sealed class BookFilterQuery
{
    public string? Title { get; set; }
    public string? AuthorName { get; set; }
    public string? Category { get; set; }
    public int? PublisherId { get; set; }
    public BookStatus? Status { get; set; }
}
