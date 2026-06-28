namespace Library.Infrastructure.Services;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<SystemUser> _userManager;
    private readonly SignInManager<SystemUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IActivityLogService _activityLog;

    public AuthService(
        UserManager<SystemUser> userManager,
        SignInManager<SystemUser> signInManager,
        ITokenService tokenService,
        IActivityLogService activityLog)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _activityLog = activityLog;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, string? ipAddress, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("This account has been deactivated.");

        // lockoutOnFailure: true delegates AccessFailedCount / lockout handling to Identity.
        var signIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

        if (signIn.IsLockedOut)
            throw new UnauthorizedAccessException("Account locked due to multiple failed attempts. Try again later.");

        if (!signIn.Succeeded)
        {
            await _activityLog.LogAsync(user.Id, "LoginFailed", nameof(SystemUser), user.Id, "Invalid password.", ipAddress, ct);
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        user.LastLoginAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        await _activityLog.LogAsync(user.Id, "Login", nameof(SystemUser), user.Id, "Login successful.", ipAddress, ct);

        var token = _tokenService.GenerateToken(user, roles);

        return new LoginResponseDto
        {
            Token = token.AccessToken,
            ExpiresAt = token.ExpiresAt,
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            Roles = roles.ToArray()
        };
    }

    public async Task<int> CreateUserAsync(CreateSystemUserDto request, CancellationToken ct = default)
    {
        var invalidRoles = request.Roles.Except(Roles.All).ToArray();
        if (invalidRoles.Length > 0)
            throw new ArgumentException($"Invalid role(s): {string.Join(", ", invalidRoles)}");

        if (await _userManager.FindByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("A user with this email already exists.");

        var user = new SystemUser
        {
            FullName = request.FullName,
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRolesAsync(user, request.Roles);

        return user.Id;
    }
}
