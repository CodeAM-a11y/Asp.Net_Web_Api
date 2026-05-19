namespace Zählerstände.Models;

public class PatchMeterReading {
    public int? MeterId { get; set; }
    public DateTime? Date { get; set; }
    public decimal? Reading { get; set; }
}