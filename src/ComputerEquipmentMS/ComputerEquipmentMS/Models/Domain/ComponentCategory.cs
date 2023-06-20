namespace ComputerEquipmentMS.Models.Domain;

public class ComponentCategory : IIdentifiable<int>
{
    public int Id { get; set; }
    public required string Name { get; set; }
}