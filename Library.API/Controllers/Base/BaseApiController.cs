namespace Library.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    // ── Success helpers ───────────────────────────────────────────────────────

    protected IActionResult OkResponse<T>(T data, string? message = null)
        => StatusCode(200, ApiResponse<T>.Ok(data, message));

    protected IActionResult CreatedResponse<T>(T data, string? message = null)
        => StatusCode(201, ApiResponse<T>.Created(data, message));

    protected IActionResult PagedResponse<T>(T data, int page, int pageSize, int totalCount)
        => StatusCode(200, ApiResponse<T>.Paged(data, page, pageSize, totalCount));

    protected IActionResult NoContentResponse(string? message = null)
        => StatusCode(200, ApiResponse<object?>.NoContent(message));

    // ── Failure helpers ───────────────────────────────────────────────────────

    protected IActionResult BadRequestResponse(string message, IEnumerable<string>? errors = null)
        => StatusCode(400, ApiResponse<object?>.BadRequest(message, errors));

    protected IActionResult NotFoundResponse(string message = "Resource not found.")
        => StatusCode(404, ApiResponse<object?>.NotFound(message));

    protected IActionResult ForbiddenResponse(string message = "Access denied.")
        => StatusCode(403, ApiResponse<object?>.Forbidden(message));

    protected IActionResult ConflictResponse(string message)
        => StatusCode(409, ApiResponse<object?>.Conflict(message));

    protected IActionResult ServerErrorResponse(string message = "An unexpected error occurred.")
        => StatusCode(500, ApiResponse<object?>.ServerError(message));

    // ── Validation helper (works with FluentValidation result) ────────────────

    protected IActionResult ValidationFailed(IEnumerable<string> errors)
        => StatusCode(422, ApiResponse<object?>.Fail(422, "Validation failed.", errors));
}