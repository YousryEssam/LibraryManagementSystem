using System.ComponentModel.DataAnnotations;

namespace Library.Application.DTOs.Borrowing;

public sealed class ReturnBookDto
{
    [MaxLength(500)]
    public string? Notes { get; set; }
}
