namespace Library.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ── 1. Infrastructure ─────────────────────────────────────────────────
        builder.Services.AddInfrastructureServices(builder.Configuration);

        // ── 2. Controllers ────────────────────────────────────────────────────
        builder.Services.AddControllers();

        // ── 3. Swagger / OpenAPI ──────────────────────────────────────────────
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            // ── API Info ──────────────────────────────────────────────────────
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Library Management System API",
                Version = "v1",
                Description = """
                    A RESTful API for managing a library system.

                    **Roles & Access:**
                    -------------------------------------------------------------
                    | Role          | Access Level                              |
                    |---------------|-------------------------------------------|
                    | Administrator | Full access to all endpoints              |
                    | Librarian     | Books, Members, Borrowing                 |
                    | Staff         | View books, process borrow/return         |
                    -------------------------------------------------------------
                    **Authentication:** Use `/api/auth/login` to get a JWT token,
                    then click **Authorize** and enter: `Bearer {your_token}`
                    """,
                Contact = new OpenApiContact
                {
                    Name = "Library API Support",
                    Email = "yousry.essam.ayoub@gmail.com"
                }
            });

            // ── JWT Security Definition ───────────────────────────────────────
            const string schemeName = "Bearer";

            options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token. Example: `Bearer eyJhbGci...`"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference(schemeName, document),
                    new List<string>()
                }
            });

            // ── XML Comments ────────────────────────────────────────────────────
            var xmlFiles = new[]
            {
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
                "Library.Application.xml",
                "Library.Domain.xml"
            };

            foreach (var xmlFile in xmlFiles)
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            }

            // ── Show enum names instead of raw integers ───────────────────────
            options.UseInlineDefinitionsForEnums();

            options.DocInclusionPredicate((_, _) => true);
        });

        // ── 4. CORS ───────────────────────────────────────────────────────────
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
        });

        // ─────────────────────────────────────────────────────────────────────
        var app = builder.Build();
        // ─────────────────────────────────────────────────────────────────────

        // ── 5. Seed Database ──────────────────────────────────────────────────
        await DatabaseSeeder.SeedAsync(app.Services, builder.Configuration);

        // ── 6. Middleware Pipeline (ORDER MATTERS) ────────────────────────────
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
                options.RoutePrefix = string.Empty;          // Swagger at root URL
                options.DocExpansion(DocExpansion.List);     // Collapsed by default
                options.DisplayRequestDuration();            // Show ms per request
                options.ConfigObject.AdditionalItems["persistAuthorization"] = true;
            });
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}