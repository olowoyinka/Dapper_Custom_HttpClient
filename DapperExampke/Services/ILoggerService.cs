namespace DapperHttps.Services;

public interface ILoggerService
{
    Task<long> AddApiLog(int? userId, string apiName, string description, string url, string requestBody, int statusCode, string response);
    Task<long> AddApiRequestLog(int? userId, string apiName, string description, string url, string requestBody);
    Task<long> AddApiResponseLog(int? userId, string apiName, string description, string url, string response);
}