namespace Core.Settings
{
    public class SettingsModel
    {
        public string Environment { get; set; }
        public string Host { get; set; }
        public string ConnectionStrings { get; set; }
        public string Database { get; set; }

        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public int JwtExpiresInMinutes { get; set; }
    }
}