namespace Zählerstände.Models;

public class Customer {
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Street { get; set; }
    public int Zip { get; set; }
    public string City { get; set; }
    public ICollection<Meter> Meters { get; } = new List<Meter>();
}