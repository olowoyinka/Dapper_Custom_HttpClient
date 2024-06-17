using DapperHttps.Models;

namespace DapperHttps.DataAccess;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomer();

    Task<int> AddCustomer(Customer model);

    Task<int?> AddCustomerTransaction(Customer model);

    Task<Customer?> GetCustomerById(int id);

    Task<int> UpdateCustomer(int id, Customer model);

    Task<int> DeleteCustomer(int id);
}
