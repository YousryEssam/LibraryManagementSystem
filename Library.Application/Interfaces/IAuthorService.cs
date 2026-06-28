using Library.Application.DTOs.Authors;

namespace Library.Application.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<AuthorDto>> GetAllAsync(CancellationToken ct = default);
    Task<AuthorDetailsDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(CreateAuthorDto request, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateAuthorDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
