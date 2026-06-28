using Library.Application.DTOs.Categories;

namespace Library.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<CategoryDetailsDto> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(CreateCategoryDto request, CancellationToken ct = default);
    Task UpdateAsync(int id, UpdateCategoryDto request, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
