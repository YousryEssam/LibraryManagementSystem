using Library.Application.DTOs.Publishers;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublishersController(IPublisherService publisherService)
            => _publisherService = publisherService;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<PublisherDto>>>> GetAll(CancellationToken ct)
        {
            var publishers = await _publisherService.GetAllAsync(ct);
            return Ok(ApiResponse<IEnumerable<PublisherDto>>.Ok(publishers, "Publishers retrieved successfully."));
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<PublisherDetailsDto>>> GetById(int id, CancellationToken ct)
        {
            var publisher = await _publisherService.GetByIdAsync(id, ct);
            return Ok(ApiResponse<PublisherDetailsDto>.Ok(publisher));
        }

        [HttpPost]
        [Authorize(Roles = "Librarian,Administrator")]
        public async Task<ActionResult<ApiResponse<object>>> Create(
            [FromBody] CreatePublisherDto request, CancellationToken ct)
        {
            var id = await _publisherService.CreateAsync(request, ct);
            return CreatedAtAction(
                nameof(GetById), new { id },
                ApiResponse<object>.Ok(new { id }, "Publisher created successfully."));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Librarian,Administrator")]
        public async Task<ActionResult<ApiResponse>> Update(
            int id, [FromBody] UpdatePublisherDto request, CancellationToken ct)
        {
            await _publisherService.UpdateAsync(id, request, ct);
            return Ok(ApiResponse.Ok("Publisher updated successfully."));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<ApiResponse>> Delete(int id, CancellationToken ct)
        {
            await _publisherService.DeleteAsync(id, ct);
            return Ok(ApiResponse.Ok("Publisher deleted successfully."));
        }
    }
}
