namespace Api.Configuration
{
    public class DatabaseOptions
    {
        public const string SectionName = "ConnectionStrings";
        public string Default { get; init; } = "";
    }
}
