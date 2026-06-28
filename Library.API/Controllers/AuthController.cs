namespace Library.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Authenticates a system user and returns a JWT access token.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var response = await _authService.LoginAsync(request, ipAddress, ct);
        return Ok(ApiResponse<LoginResponseDto>.Ok(response, "Login successful."));
    }

    /// <summary>Creates a new system user (Administrator, Librarian, or Staff). Administrator only.</summary>
    [HttpPost("register")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] CreateSystemUserDto request, CancellationToken ct)
    {
        var userId = await _authService.CreateUserAsync(request, ct);
        var payload = new { id = userId };
        return CreatedAtAction(nameof(Login), null, ApiResponse<object>.Ok(payload, "User created successfully."));
    }
}
