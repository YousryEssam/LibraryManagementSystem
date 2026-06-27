namespace Library.API.Extensions;

public static class SerilogConfigurationExtensions
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                // ── 1. Read everything from appsettings.json ──────────────────
                .ReadFrom.Configuration(context.Configuration)

                // ── 2. DI-aware enrichers (can't be declared in JSON) ─────────
                .ReadFrom.Services(services)

                // ── 3. Additional programmatic enrichers ──────────────────────
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithEnvironmentName()

                // ── 4. User enricher (needs IHttpContextAccessor from DI) ─────
                .Enrich.With(services.GetRequiredService<UserEnricher>())

                // ── 5. Correlation ID (already in LogContext via middleware,
                //        but default here keeps startup logs clean) ─────────────
                .Enrich.WithProperty("CorrelationId", "startup");
        });

        // Register services required by UserEnricher
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<UserEnricher>();

        return builder;
    }

    public static Serilog.ILogger CreateBootstrapLogger() =>
        new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] BOOTSTRAP » {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(
                path: "Logs/bootstrap-.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .CreateBootstrapLogger();
}