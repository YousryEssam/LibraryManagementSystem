namespace Library.Application.DTOs.Publishers;

public sealed class PublisherDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Website { get; set; }
}
