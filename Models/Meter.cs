using System.Text.Json.Serialization;

namespace Zählerstände.Models;

public class Meter {
    public int Id { get; init; }
    public int CustomerId { get; set; }
    [Newtonsoft.Json.JsonIgnoreAttribute]
    public Customer? Customer { get; set; }
    public string MeterNumber { get; set; }
    [Newtonsoft.Json.JsonIgnoreAttribute]
    public ICollection<MeterReading> MeterReadings { get; } = new List<MeterReading>();
}