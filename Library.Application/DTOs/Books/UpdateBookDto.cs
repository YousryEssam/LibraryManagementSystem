using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Books;

public sealed class UpdateBookDto
{
    [Required, MaxLength(300)]
    public string Title { get; set; } = default!;

    [Required, MaxLength(20)]
    public string ISBN { get; set; } = default!;

    [MaxLength(50)]
    public string? Edition { get; set; }

    public int? PublicationYear { get; set; }

    [MaxLength(50)]
    public string? Language { get; set; }

    [MaxLength(2000)]
    public string? Summary { get; set; }

    [MaxLength(500), Url]
    public string? CoverImageUrl { get; set; }

    [Required]
    public int PublisherId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required, MinLength(1, ErrorMessage = "At least one author is required.")]
    public IEnumerable<BookAuthorEntryDto> Authors { get; set; } = [];
}
