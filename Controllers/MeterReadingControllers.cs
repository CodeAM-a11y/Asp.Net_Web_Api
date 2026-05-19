using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zählerstände.Data;
using Zählerstände.Models;

namespace Zählerstände.Controllers;
[Route(nameof(MeterReading))] 
[ApiController]
public class MeterReadingControllers : ControllerBase {
    private readonly ZählerständeDbContext _ctx;
    public MeterReadingControllers(ZählerständeDbContext ctx) {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MeterReading>>> GetAllMeterReading() {
        return await _ctx.MeterReadings.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MeterReading>> GetMeterReadingById(int id) {
        var MeterRead = await _ctx.MeterReadings.FindAsync(id);
        if (MeterRead is null)
        { return NotFound(); }

        return MeterRead;
    }

    [HttpPost]
    public async Task<ActionResult<MeterReading>> CreateMeterReading(MeterReading meterReading) {
        var meterRead = new MeterReading() {
            Date = meterReading.Date,
            MeterId = meterReading.MeterId,
            Reading = meterReading.Reading
        };
        _ctx.MeterReadings.Add(meterRead);
        await _ctx.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMeterReadingById), new { id = meterRead.Id }, meterRead);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMeterReading(int id, MeterReading meterReading) {
        var meterRead = await _ctx.MeterReadings.FindAsync(id);
        if (meterRead is null)
        { _ctx.Add(meterReading); }
        else
        { meterRead.MeterId = meterReading.MeterId;
        meterRead.Date = meterReading.Date;
          meterRead.Reading = meterReading.Reading; }

        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchMeterReading(int id, PatchMeterReading patchMeterReading) {
        var pMeterRead = await _ctx.MeterReadings.FindAsync(id);
        if (pMeterRead is null)
        { return NotFound(); }

        pMeterRead.MeterId =
            patchMeterReading.MeterId?? pMeterRead.MeterId;
        pMeterRead.Date =
            patchMeterReading.Date ?? pMeterRead.Date;
        pMeterRead.Reading =
            patchMeterReading.Reading ?? pMeterRead.Reading;
        
        await _ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMeterReading(int id) {
        var pMeterRead = await _ctx.MeterReadings.FindAsync(id);
        if (pMeterRead is null)
        { return NotFound(); }

        _ctx.MeterReadings.Remove(pMeterRead);
        await _ctx.SaveChangesAsync();
        return NoContent();
    }
}