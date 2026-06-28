using Library.Application.DTOs.Categories;

namespace Library.Infrastructure.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IActivityLogService _activityLogService;
    private readonly ICurrentUserService _currentUser;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IActivityLogService activityLogService,
        ICurrentUserService currentUser)
    {
        _categoryRepository = categoryRepository;
        _activityLogService = activityLogService;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken ct = default)
        => await _categoryRepository.Query()
            .Where(c => c.DeletedAt == null)
            .AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ParentCategoryId = c.ParentCategoryId,
                ParentName = c.ParentCategory != null ? c.ParentCategory.Name : null
            })
            .ToListAsync(ct);

    public async Task<CategoryDetailsDto> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetWithDetailsAsync(id, ct)
            ?? throw new KeyNotFoundException("Category not found.");

        return new CategoryDetailsDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId,
            ParentName = category.ParentCategory?.Name,
            BookCount = await _categoryRepository.HasActiveBooksAsync(id, ct)
                                   ? category.Books?.Count(b => b.DeletedAt == null) ?? 0
                                   : 0,
            SubCategories = category.SubCategories.Select(s => new CategoryDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ParentCategoryId = s.ParentCategoryId
            })
        };
    }

    public async Task<int> CreateAsync(CreateCategoryDto request, CancellationToken ct = default)
    {
        if (await _categoryRepository.ExistsByNameAsync(request.Name, ct: ct))
            throw new InvalidOperationException($"Category '{request.Name}' already exists.");

        if (request.ParentCategoryId.HasValue)
            await EnsureParentExistsAsync(request.ParentCategoryId.Value, ct);

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentCategoryId = request.ParentCategoryId,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(category, ct);
        await _categoryRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "CreateCategory", nameof(Category),
            category.Id, $"Category '{category.Name}' created.", ct: ct);

        return category.Id;
    }

    public async Task UpdateAsync(int id, UpdateCategoryDto request, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Category not found.");

        if (category.DeletedAt is not null)
            throw new KeyNotFoundException("Category not found.");

        if (await _categoryRepository.ExistsByNameAsync(request.Name, excludeId: id, ct: ct))
            throw new InvalidOperationException($"Category '{request.Name}' already exists.");

        if (request.ParentCategoryId.HasValue)
        {
            if (request.ParentCategoryId.Value == id)
                throw new InvalidOperationException("A category cannot be its own parent.");

            await EnsureParentExistsAsync(request.ParentCategoryId.Value, ct);
        }

        category.Name = request.Name;
        category.Description = request.Description;
        category.ParentCategoryId = request.ParentCategoryId;
        category.ModifiedAt = DateTime.UtcNow;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "UpdateCategory", nameof(Category),
            category.Id, $"Category '{category.Name}' updated.", ct: ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException("Category not found.");

        if (category.DeletedAt is not null)
            throw new KeyNotFoundException("Category not found.");

        if (await _categoryRepository.HasChildrenAsync(id, ct))
            throw new InvalidOperationException("Cannot delete a category that has sub-categories.");

        if (await _categoryRepository.HasActiveBooksAsync(id, ct))
            throw new InvalidOperationException("Cannot delete a category that has active books.");

        category.DeletedAt = DateTime.UtcNow;
        category.ModifiedAt = DateTime.UtcNow;

        _categoryRepository.Update(category);
        await _categoryRepository.SaveChangesAsync(ct);

        await _activityLogService.LogAsync(
            _currentUser.UserId, "DeleteCategory", nameof(Category),
            category.Id, $"Category '{category.Name}' deleted.", ct: ct);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private async Task EnsureParentExistsAsync(int parentId, CancellationToken ct)
    {
        var parent = await _categoryRepository.GetByIdAsync(parentId, ct);
        if (parent is null || parent.DeletedAt is not null)
            throw new KeyNotFoundException($"Parent category with id {parentId} not found.");
    }
}
