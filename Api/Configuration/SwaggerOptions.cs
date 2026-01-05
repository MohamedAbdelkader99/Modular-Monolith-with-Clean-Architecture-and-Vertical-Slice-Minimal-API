namespace Api.Configuration
{
    public class SwaggerOptions
    {
        public const string SectionName = "Swagger";

        public bool Enabled { get; init; } = true;
        public string Title { get; init; } = "API";
        public string Version { get; init; } = "v1";
    }
}
