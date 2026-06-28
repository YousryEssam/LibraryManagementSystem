namespace Library.Infrastructure.Authentication
{
    public sealed class JwtOptions : ITokenSettings
    {
        public const string SectionName = "Jwt";

        [Required, MinLength(32)]
        public string SecretKey { get; init; } = string.Empty;

        [Required]
        public string Issuer { get; init; } = string.Empty;

        [Required]
        public string Audience { get; init; } = string.Empty;

        public int ExpiryMinutes { get; init; } = 60;
    }
}
