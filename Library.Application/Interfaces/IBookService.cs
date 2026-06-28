using Library.Application.DTOs.Books;

namespace Library.Application.Interfaces;

public interface IBookService
{
    Task<IEnumerable<BookDto>> GetAllAsync(BookFilterQuery filter, CancellationToken ct = default);
    Task<BookDetailsDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(CreateBookDto request, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateBookDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
