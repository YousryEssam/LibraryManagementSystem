namespace Library.Application.DTOs.Books
{
    public sealed class BookAuthorDto
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; } = default!;
        public BookAuthorRole Role { get; set; }
    }
}
