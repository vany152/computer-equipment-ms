namespace Server.Models;

public class ComponentManufacturer : IIdentifiable<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
