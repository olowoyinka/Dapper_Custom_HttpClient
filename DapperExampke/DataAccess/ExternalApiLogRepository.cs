using Dapper;
using DapperHttps.Models;
using System.Data;

namespace DapperHttps.DataAccess;

public class ExternalApiLogRepository : IExternalApiLogRepository
{
    private readonly IDbConnection _dbConnection;

    public ExternalApiLogRepository (IDbConnection dbConnection)
    {
        this._dbConnection = dbConnection;
    }


    public async Task<int> AddExternalApiLog (ExternalApiLog model)
    {
        const string query = """
                               INSERT INTO Requests (Name, Description, UserId, Request, Url, StatusCode, Response, CreatedOn)
                               VALUES (@Name, @Description, @UserId, @Request, @Url, @StatusCode, @Response, @CreatedOn);
                               SELECT CAST(SCOPE_IDENTITY() as int)
                             """;

        return await _dbConnection.QuerySingleOrDefaultAsync<int>(query, model);
    }
}