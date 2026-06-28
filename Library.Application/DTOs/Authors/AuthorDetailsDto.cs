namespace Library.Application.DTOs.Authors;

public sealed class AuthorDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string? Bio { get; set; }
    public string? Nationality { get; set; }
    public int BookCount { get; set; }
}