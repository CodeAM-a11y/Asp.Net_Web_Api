using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zählerstände.Data;
using Zählerstände.Models;

namespace Zählerstände.Controllers;
[Route("meters")]
[ApiController]
public class MetersControllers : ControllerBase {
    private readonly ZählerständeDbContext _ctx;
    public MetersControllers(ZählerständeDbContext ctx) {
        _ctx = ctx;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Meter>>> GetAllCustomer([FromQuery] int? customerId) {
        return await _ctx.Meters
            .Where(m=>customerId == null||m.CustomerId==customerId)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Meter>> GetMeterById(int id) {
        var met = await _ctx.Meters.FindAsync(id);
        if (met is null)
        { return NotFound(); }

        return met;
    }

    [HttpPost]
    public async Task<ActionResult<Meter>> CreateMeter([Bind(
            nameof(Meter.CustomerId), nameof(Meter.MeterNumber))]
        Meter meter) {
        if (ModelState.IsValid)
        { _ctx.Add(meter); 
        await _ctx.SaveChangesAsync(); }

        return CreatedAtAction(nameof(GetMeterById), new { id = meter.Id }, meter);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMeter(int id, [Bind(
            nameof(Meter.CustomerId), nameof(Meter.MeterNumber))]
        Meter meter) {
        var met = await _ctx.Meters.FindAsync(id);
        if (met is null)
        { _ctx.Add(meter); }
        else
        { met.MeterNumber = meter.MeterNumber;
        met.CustomerId = meter.CustomerId; }

        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchMeter(int id, PatchMeter patchMeter) {
        var pMeter = await _ctx.Meters.FindAsync(id);
        if (pMeter is null)
        { return NotFound(); }

        pMeter.MeterNumber =
            string.IsNullOrWhiteSpace(patchMeter.MeterNumber) ? pMeter.MeterNumber : patchMeter.MeterNumber;
        pMeter.CustomerId = 
            patchMeter.CustomerId ?? pMeter.CustomerId;
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeter(int id) {
        var met = await _ctx.Meters.FindAsync(id);
        if (met is null)
        { return NotFound(); }

        _ctx.Meters.Remove(met);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}