using Library.Infrastructure.Repositories;

namespace Library.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        // ── 0. Bootstrap logger ───────────────────────────────────────────────
        Log.Logger = SerilogConfigurationExtensions.CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Library Management System API...");

            var builder = WebApplication.CreateBuilder(args);

            // ── 1. Serilog ────────────────────────────────────────────────────
            builder.AddSerilogLogging();

            // ── 2. Infrastructure ─────────────────────────────────────────────
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // ── 3. Controllers ────────────────────────────────────────────────
            builder.Services.AddControllers();

            // ── 4. Swagger / OpenAPI ──────────────────────────────────────────
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
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

                options.UseInlineDefinitionsForEnums();
                options.DocInclusionPredicate((_, _) => true);
            });

            // ── 5. CORS ───────────────────────────────────────────────────────
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            // ─────────────────────────────────────────────────────────────────
            var app = builder.Build();
            // ─────────────────────────────────────────────────────────────────

            // ── 6. Seed Database ──────────────────────────────────────────────
            try
            {
                await DatabaseSeeder.SeedAsync(app.Services, builder.Configuration);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Database seeding failed. Application will shut down.");
                throw;
            }

            // ── 7. Middleware Pipeline (ORDER MATTERS) ────────────────────────
            app.UseGlobalExceptionHandler();    // 1st — wraps everything
            app.UseCorrelationId();             // 2nd — ID must exist before any logging
            app.UseStructuredRequestLogging();  // 3rd — Serilog HTTP request logs

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
                    options.RoutePrefix = string.Empty;
                    options.DocExpansion(DocExpansion.List);
                    options.DisplayRequestDuration();
                    options.ConfigObject.AdditionalItems["persistAuthorization"] = true;
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            Log.Information("Library Management System API started successfully.");
            await app.RunAsync();
        }
        catch (Exception ex) when (ex is not OperationCanceledException
                                && ex.GetType().Name != "StopTheHostException")
        {
            // Catches fatal startup failures and logs them before the process dies
            Log.Fatal(ex, "Application terminated unexpectedly.");
        }
        finally
        {
            // Ensure all buffered log events are flushed to disk/console
            await Log.CloseAndFlushAsync();
        }
    }
}