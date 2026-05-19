using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zählerstände.Data;
using Zählerstände.Models;

namespace Zählerstände.Controllers;

[Route("customers")]
[ApiController]
public class CustomersController : ControllerBase {
    private readonly ZählerständeDbContext _ctx;

    public CustomersController(ZählerständeDbContext ctx) {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomer() {
        return await _ctx.Customers.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomerById(int id) {
        var cus = await _ctx.Customers.FindAsync(id);
        if (cus is null)
        { return NotFound(); }

        return cus;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateCustomer([Bind(
            nameof(Customer.FirstName),nameof(Customer.LastName),nameof(Customer.Street)
            , nameof(Customer.Zip),nameof(Customer.City) )]
        Customer customer) {
        if (ModelState.IsValid)
        { _ctx.Add(customer);
        await _ctx.SaveChangesAsync(); }

        return CreatedAtAction(nameof(GetCustomerById),new {id=customer.Id},customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(int id, [Bind(
            nameof(Customer.FirstName), nameof(Customer.LastName), nameof(Customer.Street)
            , nameof(Customer.Zip), nameof(Customer.City))]
        Customer customer) {
        var cus = await _ctx.Customers.FindAsync(id);
        if (cus is null)
        { _ctx.Add(customer); }
        else
        { cus.City = customer.City;
        cus.FirstName = customer.FirstName;
        cus.LastName = customer.LastName;
        cus.Street = customer.Street;
        cus.Zip = customer.Zip; }

        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCustomer(int id, [FromBody] JsonPatchDocument<Customer> cus) {
        if (cus == null) return BadRequest();
        var customer = await _ctx.Customers.FindAsync(id);
        if (customer is null) return NotFound();
        cus.ApplyTo(customer);
        if (!TryValidateModel(customer))
        { return ValidationProblem(ModelState);}

        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id) {
        var customer = await _ctx.Customers.FindAsync(id);
        if (customer is null)
        { return NotFound(); }

        _ctx.Customers.Remove(customer);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}