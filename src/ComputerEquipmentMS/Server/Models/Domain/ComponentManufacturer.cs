namespace Server.Models.Domain;

public class ComponentManufacturer : IIdentifiable<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }
}
