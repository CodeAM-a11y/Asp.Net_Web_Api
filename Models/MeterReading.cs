namespace Zählerstände.Models;

public class MeterReading {
    public int Id { get; init; }
    public int MeterId { get; set; }
    public Meter? Meter { get; init; }
    public DateTime Date { get; set; }
    public decimal Reading { get; set; }
}