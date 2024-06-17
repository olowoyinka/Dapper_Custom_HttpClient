using System.Data;

namespace DapperHttps.DataAccess;

public interface IAppDbContext
{
    public IDbTransaction Transaction { get; }

    ICustomerRepository Customer { get; set; }

    IExternalApiLogRepository ExternalApiLog { get; set; }

    void Commit();

    void BeginTransaction();

    void Rollback();
}