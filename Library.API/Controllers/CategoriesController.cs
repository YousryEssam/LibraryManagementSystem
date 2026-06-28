using Library.Application.DTOs.Categories;

namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
        => _categoryService = categoryService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetAll(CancellationToken ct)
    {
        var categories = await _categoryService.GetAllAsync(ct);
        return Ok(ApiResponse<IEnumerable<CategoryDto>>.Ok(categories, "Categories retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CategoryDetailsDto>>> GetById(int id, CancellationToken ct)
    {
        var category = await _categoryService.GetByIdAsync(id, ct);
        return Ok(ApiResponse<CategoryDetailsDto>.Ok(category));
    }

    [HttpPost]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse<object>>> Create(
        [FromBody] CreateCategoryDto request, CancellationToken ct)
    {
        var id = await _categoryService.CreateAsync(request, ct);
        return CreatedAtAction(
            nameof(GetById), new { id },
            ApiResponse<object>.Ok(new { id }, "Category created successfully."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Librarian,Administrator")]
    public async Task<ActionResult<ApiResponse>> Update(
        int id, [FromBody] UpdateCategoryDto request, CancellationToken ct)
    {
        await _categoryService.UpdateAsync(id, request, ct);
        return Ok(ApiResponse.Ok("Category updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult<ApiResponse>> Delete(int id, CancellationToken ct)
    {
        await _categoryService.DeleteAsync(id, ct);
        return Ok(ApiResponse.Ok("Category deleted successfully."));
    }
}
