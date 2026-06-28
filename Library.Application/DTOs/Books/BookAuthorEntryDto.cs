using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Books
{
    public sealed class BookAuthorEntryDto
    {
        [Required]
        public int AuthorId { get; set; }
        public BookAuthorRole Role { get; set; } = BookAuthorRole.Author;
    }
}
