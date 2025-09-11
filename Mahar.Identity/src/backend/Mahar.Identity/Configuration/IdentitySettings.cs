public class IdentitySettings
{
    public string JwtIssuer { get; set; }
    public string JwtAudience { get; set; }
    public string JwtSecret { get; set; }
    public int JwtExpireDays { get; set; }
    public string ConnectionString { get; set; }
}