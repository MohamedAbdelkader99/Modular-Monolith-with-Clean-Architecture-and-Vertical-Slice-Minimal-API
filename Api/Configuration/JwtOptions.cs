namespace Api.Configuration
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public bool Enabled { get; init; } = false;
        public string Authority { get; init; } = "";
        public string Audience { get; init; } = "";
    }
}
