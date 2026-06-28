using Library.Application.DTOs.Books;

namespace Library.Application.Interfaces;

public interface IBookAuthorService
{
    Task AddAsync(int bookId, BookAuthorEntryDto request, CancellationToken ct = default);
    Task UpdateRoleAsync(int bookId, int authorId, UpdateBookAuthorRoleDto request, CancellationToken ct = default);
    Task RemoveAsync(int bookId, int authorId, CancellationToken ct = default);
}
