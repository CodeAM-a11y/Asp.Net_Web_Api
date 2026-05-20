using System.Text.Json.Serialization;

namespace Zählerstände.Models;

public class MeterReading {
    public int Id { get; init; }
    public int MeterId { get; set; }
    [Newtonsoft.Json.JsonIgnoreAttribute]
    public Meter? Meter { get; init; }
    public DateTime Date { get; set; }
    public decimal Reading { get; set; }
}