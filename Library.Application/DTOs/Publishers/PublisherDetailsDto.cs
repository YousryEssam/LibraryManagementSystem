namespace Library.Application.DTOs.Publishers;

public sealed class PublisherDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Address { get; set; }
    public string? ContactEmail { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public int BookCount { get; set; }
}
