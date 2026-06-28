namespace Library.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        // ── 1. Database ───────────────────────────────────────────────────────
        services.AddDbContext<LibraryDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(LibraryDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                }));

        // ── 2. MS Identity ────────────────────────────────────────────────────
        services
            .AddIdentity<SystemUser, IdentityRole<int>>(options =>
            {
                // Password rules
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;

                // Lockout — built-in brute-force protection
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;

                // User
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<LibraryDbContext>()  // Wires UserManager → LibraryDbContext
            .AddDefaultTokenProviders();                    // For password-reset tokens

        // ── 3. JWT Authentication ─────────────────────────────────────────────
        services.AddJwtAuthentication(configuration);

        // ── 4. Repositories & Services 

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IActivityLogService, ActivityLogService>();

        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IMemberRepository, MemberRepository>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IAuthorService, AuthorService>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IPublisherService, PublisherService>();

        services.AddScoped<IBorrowingTransactionRepository, BorrowingTransactionRepository>();
        services.AddScoped<IBorrowingTransactionService, BorrowingTransactionService>();
        services.AddScoped<IBookRepository, BookRepository>();


        return services;
    }

    // ── JWT Helper ────────────────────────────────────────────────────────────
    private static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var secretKey = jwtSection["SecretKey"]
                         ?? throw new InvalidOperationException("JWT SecretKey is not configured.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                                                  Encoding.UTF8.GetBytes(secretKey)),
                    ClockSkew = TimeSpan.Zero  // No grace period on expiry
                };
            });

        // ── Authorization Policies ────────────────────────────────────────────
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", p => p.RequireRole("Administrator"));
            options.AddPolicy("LibrarianUp", p => p.RequireRole("Administrator", "Librarian"));
            options.AddPolicy("StaffUp", p => p.RequireRole("Administrator", "Librarian", "Staff"));
        });

        return services;
    }
}
