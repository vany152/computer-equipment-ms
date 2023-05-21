namespace Server.Models;

public class ComponentCategory : IIdentifiable<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }
}