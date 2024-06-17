using System.Data;

namespace DapperHttps.DataAccess;

public class AppDbContext : IAppDbContext
{
    private readonly IConfiguration _config;
    private readonly IDbConnection _dbConnection;
    protected IDbTransaction _dbTransaction;

    public IDbTransaction Transaction
    {
        get 
        { 
            return _dbTransaction; 
        } 
    }


    public ICustomerRepository Customer { get; set; }
    public IExternalApiLogRepository ExternalApiLog { get; set; }


    public AppDbContext (IConfiguration configuration, IDbConnection dbConnection,
                            ICustomerRepository customer, IExternalApiLogRepository externalApiLog)
    {
        _config = configuration;
        _dbConnection = dbConnection;
        _dbConnection.Open();
        Customer = customer;
        ExternalApiLog = externalApiLog;
    }

    public void BeginTransaction()
    {
        _dbTransaction = _dbConnection.BeginTransaction();
    }

    public void Commit()
    { 
        _dbTransaction.Commit();
        Dispose();
    }

    public void Rollback()
    {
        _dbTransaction.Rollback();
        Dispose();
    }

    public void Dispose() => _dbTransaction.Dispose();

    ~AppDbContext() 
    {
        _dbConnection.Close();
        _dbConnection.Dispose();
    }
}