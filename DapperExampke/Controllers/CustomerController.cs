using DapperHttps.DataAccess;
using DapperHttps.Models;
using Microsoft.AspNetCore.Mvc;

namespace DapperHttps.Controllers;


[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }


    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _customerRepository.GetAllCustomer();

        return Ok(customers);
    }


    [HttpPost("customers")]
    public async Task<IActionResult> PostCustomers([FromBody] Customer model)
    {
        var customer = await _customerRepository.AddCustomer(model);

        return Ok(customer);
    }


    [HttpPost("customers/transanction")]
    public async Task<IActionResult> PostCustomersTransanction([FromBody] Customer model)
    {
        var customer = await _customerRepository.AddCustomerTransaction(model);

        return Ok(customer);
    }


    [HttpGet("customers/{id}")]
    public async Task<IActionResult> GetCustomersById([FromRoute] int id)
    {
        var customer = await _customerRepository.GetCustomerById(id);

        return Ok(customer);
    }


    [HttpPut("customers/{id}")]
    public async Task<IActionResult> EditCustomersById([FromRoute] int id, [FromBody] Customer model)
    {
        var customer = await _customerRepository.UpdateCustomer(id, model);

        return Ok(customer);
    }


    [HttpDelete("customers/{id}")]
    public async Task<IActionResult> DeleteCustomersById([FromRoute] int id)
    {
        var customer = await _customerRepository.DeleteCustomer(id);

        return Ok(customer);
    }
}