using NodaTime;

namespace ComputerEquipmentMS.DataAccess.Entities;

public class ComputerConfigurationEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required Period WarrantyPeriod { get; set; }
    public decimal Margin { get; set; }
    public required ICollection<int> ComponentIds { get; set; }
}
