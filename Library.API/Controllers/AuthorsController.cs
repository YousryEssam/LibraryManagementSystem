using Library.Application.DTOs.Authors;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public sealed class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
            => _authorService = authorService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<AuthorDto>>>> GetAll(CancellationToken ct)
        {
            var authors = await _authorService.GetAllAsync(ct);
            return Ok(ApiResponse<IEnumerable<AuthorDto>>.Ok(authors, "Authors retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<AuthorDetailsDto>>> GetById(int id, CancellationToken ct)
        {
            var author = await _authorService.GetByIdAsync(id, ct);
            return Ok(ApiResponse<AuthorDetailsDto>.Ok(author));
        }

        [HttpPost]
        [Authorize(Roles = "Librarian,Administrator")]
        public async Task<ActionResult<ApiResponse<object>>> Create(
            [FromBody] CreateAuthorDto request,
            CancellationToken ct)
        {
            var id = await _authorService.CreateAsync(request, ct);
            return CreatedAtAction(
                nameof(GetById),
                new { id },
                ApiResponse<object>.Ok(new { id }, "Author created successfully."));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Librarian,Administrator")]
        public async Task<ActionResult<ApiResponse>> Update(
            int id,
            [FromBody] UpdateAuthorDto request,
            CancellationToken ct)
        {
            await _authorService.UpdateAsync(id, request, ct);
            return Ok(ApiResponse.Ok("Author updated successfully."));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ApiResponse>> Delete(int id, CancellationToken ct)
        {
            await _authorService.DeleteAsync(id, ct);
            return Ok(ApiResponse.Ok("Author deleted successfully."));
        }
    }
}
