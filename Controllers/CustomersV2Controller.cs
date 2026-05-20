using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zählerstände.Data;
using Zählerstände.Models;

namespace Zählerstände.Controllers;
[Route("metersV2")]
[ApiController]
public class CustomersV2Controller : ControllerBase {
    private readonly ZählerständeDbContext _ctx;

    public CustomersV2Controller(ZählerständeDbContext ctx) {
        _ctx = ctx;
    }
    // GET: api/CustomersV2
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerGetModel>>> GetAllCustomers(string? searchTerm) {
        var query = _ctx.Customers.AsQueryable();
        //Filter customers
        if (!string.IsNullOrWhiteSpace(searchTerm))
        { searchTerm = searchTerm.ToUpper();
        query = query.Where(c => c.FirstName.ToUpper().Contains(searchTerm) ||
                                 c.LastName.ToUpper().Contains(searchTerm) ||
                                 c.Street.ToUpper().Contains(searchTerm) ||
                                 c.City.ToUpper().Contains(searchTerm)); }
        return await _ctx.Customers
            .Select(c=> new CustomerGetModel() {
                Data = c,
                Links = new CustomerLinkModel() {
                    Meters = $"http://localhost:5120/metersV2?customerId={c.Id}"
                }
            })
            .ToListAsync();
    }
    
}