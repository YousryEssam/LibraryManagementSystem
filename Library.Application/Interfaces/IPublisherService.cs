using Library.Application.DTOs.Publishers;

namespace Library.Application.Interfaces;

public interface IPublisherService
{
    Task<IEnumerable<PublisherDto>> GetAllAsync(CancellationToken ct = default);
    Task<PublisherDetailsDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(CreatePublisherDto request, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdatePublisherDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
