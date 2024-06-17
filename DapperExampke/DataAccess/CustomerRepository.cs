using Dapper;
using DapperHttps.Models;
using System.Data;

namespace DapperHttps.DataAccess;

public class CustomerRepository : ICustomerRepository
{
    private readonly IDbConnection _dbConnection;

    public CustomerRepository(IDbConnection dbConnection)
    {
        this._dbConnection = dbConnection;
    }


    public async Task<IEnumerable<Customer>> GetAllCustomer()
    {
        var query = "SELECT * FROM Customers";

        return await _dbConnection.QueryAsync<Customer>(query);
    }


    public async Task<int> AddCustomer (Customer model)
    {
        const string query = """
                               INSERT INTO Customers (FirstName, LastName, Email, DataOfBirth)
                               VALUES (@FirstName, @LastName, @Email, @DataOfBirth);
                               SELECT CAST(SCOPE_IDENTITY() as int)
                             """;

        return await _dbConnection.QuerySingleOrDefaultAsync<int>(query, model);
    }


    public async Task<int?> AddCustomerTransaction (Customer model)
    {
        _dbConnection.Open();
        var transaction = _dbConnection.BeginTransaction();

        int? id = null;

        try
        {
            const string sql = """
                                  INSERT INTO Customers (FirstName, LastName, Email, DataOfBirth)
                                  VALUES (@FirstName, @LastName, @Email, @DataOfBirth);
                                  SELECT CAST(SCOPE_IDENTITY() as int)
                               """;

            id = await _dbConnection.QuerySingleOrDefaultAsync<int>(sql, model, transaction);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            try
            {
                transaction.Rollback();
            }
            catch (Exception ex2)
            {

            }
        }

        return id;
    }


    public async Task<Customer?> GetCustomerById (int id)
    {
        const string sql = """
                              SELECT * FROM Customers
                              WHERE Id = @CustomerId
                           """;

        return await _dbConnection.QueryFirstOrDefaultAsync<Customer>(sql, new { CustomerId = id });
    }

    
    public async Task<int> UpdateCustomer (int id, Customer model)
    {
        model.Id = id;

        const string sql = """
                              UPDATE Customers
                              SET FirstName = @FirstName,
                                  LastName = @LastName,
                                  Email = @Email,
                                  DataOfBirth = @DataOfBirth
                              WHERE Id = @Id
                           """;

        return await _dbConnection.ExecuteAsync(sql, model);
    }


    public async Task<int> DeleteCustomer (int id)
    {
        const string sql = """
                              DELETE FROM Customers
                              WHERE Id = @CustomerId
                           """;

        return await _dbConnection.ExecuteAsync(sql, new { CustomerId = id });
    }
}