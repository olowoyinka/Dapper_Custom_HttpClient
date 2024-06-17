using DapperHttps.Models;

namespace DapperHttps.DataAccess;

public interface IExternalApiLogRepository
{
    Task<int> AddExternalApiLog(ExternalApiLog model);
}