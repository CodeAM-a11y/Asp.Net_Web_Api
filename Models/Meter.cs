namespace Zählerstände.Models;

public class Meter {
    public int Id { get; init; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string MeterNumber { get; set; }
    public ICollection<MeterReading> MeterReadings { get; } = new List<MeterReading>();
}