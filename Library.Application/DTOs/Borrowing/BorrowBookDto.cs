using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Borrowing;

public sealed class BorrowBookDto
{
    [Required]
    public int BookId { get; set; }

    [Required]
    public int MemberId { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}
