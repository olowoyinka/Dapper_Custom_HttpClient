using DapperHttps.DataAccess;
using DapperHttps.Models;

namespace DapperHttps.Services;

public class LoggerService
{
    private readonly IAppDbContext _appDbContext;

    public LoggerService (IAppDbContext appDbContext)
    {
        this._appDbContext = appDbContext;
    }

    public async Task<long> AddApiLog(int? userId, string apiName, string description, string url, string requestBody, int statusCode, string response)
    {

        if (string.IsNullOrEmpty(apiName) && string.IsNullOrEmpty(description))
            return 0;

        if (url?.Length > 1000)
            url = url.Substring(0, 1000);

        if (apiName?.Length > 200)
            apiName = apiName.Substring(0, 199);

        if (description?.Length > 500)
            description = description.Substring(0, 500);

        if (requestBody?.Length > 1000)
            requestBody = requestBody.Substring(0, 1000);

        if (response?.Length > 2000)
            response = response.Substring(0, 2000);

        var apiLog = new ExternalApiLog()
        {
            Name = apiName,
            Description = description,
            UserId = userId,
            Url = url,
            Request = requestBody,
            CreatedOn = DateTimeOffset.UtcNow,
            StatusCode = statusCode,
            Response = response
        };

        await _appDbContext.ExternalApiLog.AddExternalApiLog(apiLog);

        return apiLog.Id;
    }

    public async Task<long> AddApiRequestLog(int? userId, string apiName, string description, string url, string requestBody)
    {

        if (string.IsNullOrEmpty(apiName) && string.IsNullOrEmpty(description))
            return 0;

        if (url?.Length > 1000)
            url = url.Substring(0, 1000);

        if (apiName?.Length > 200)
            apiName = apiName.Substring(0, 199);

        if (description?.Length > 500)
            description = description.Substring(0, 500);

        if (requestBody?.Length > 1000)
            requestBody = requestBody.Substring(0, 1000);

        var apiLog = new ExternalApiLog()
        {
            Name = apiName,
            Description = description,
            UserId = userId,
            Url = url,
            Request = requestBody,
            CreatedOn = DateTime.UtcNow,
        };

        await _appDbContext.ExternalApiLog.AddExternalApiLog(apiLog);

        return apiLog.Id;
    }
    public async Task<long> AddApiResponseLog(int? userId, string apiName, string description, string url, string response)
    {
        if (string.IsNullOrEmpty(apiName) && string.IsNullOrEmpty(description))
            return 0;

        if (url?.Length > 1000)
            url = url.Substring(0, 1000);

        if (apiName?.Length > 200)
            apiName = apiName.Substring(0, 199);

        if (description?.Length > 500)
            description = description.Substring(0, 500);

        if (response?.Length > 2000)
            response = response.Substring(0, 2000);

        var apiLog = new ExternalApiLog()
        {
            Name = apiName,
            Description = description,
            UserId = userId,
            Url = url,
            CreatedOn = DateTime.UtcNow,
            Response = response
        };

        await _appDbContext.ExternalApiLog.AddExternalApiLog(apiLog);

        return apiLog.Id;
    }
}