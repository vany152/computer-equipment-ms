using ComputerEquipmentMS.DataAccess.Entities.Auxiliary;
using NodaTime;

namespace ComputerEquipmentMS.DataAccess.Entities;

public class ComponentEntity
{
    public int Id { get; set; }

    public int ComponentCategoryId { get; set; }

    public int ComponentManufacturerId { get; set; }

    public required string Name { get; set; }
    public required ComponentSpecificationsEntity Specifications { get; set; }

    public decimal Cost { get; set; }
    public required Period WarrantyPeriod { get; set; }
}