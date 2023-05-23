using NodaTime;

namespace Server.Models.Domain;

public class ComputerConfiguration : IIdentifiable<int>
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required Period WarrantyPeriod { get; set; }
    public decimal Margin { get; set; }
    public required ICollection<int> ComponentIds { get; set; }
}
