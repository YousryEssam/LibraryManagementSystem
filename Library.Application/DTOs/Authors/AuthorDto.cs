namespace Library.Application.DTOs.Authors;

public sealed class AuthorDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = default!;
    public string? Nationality { get; set; }
}
