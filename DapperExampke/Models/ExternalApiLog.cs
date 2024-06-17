namespace DapperHttps.Models;

public class ExternalApiLog
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public string Request { get; set; } = string.Empty;
    public string Url {  get; set; } = string.Empty;
    public int? StatusCode { get; set; }
    public string Response { get; set; } = string.Empty;
    public DateTimeOffset CreatedOn { get; set; }
}
