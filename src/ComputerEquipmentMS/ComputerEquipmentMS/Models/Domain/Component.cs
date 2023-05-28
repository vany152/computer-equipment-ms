using ComputerEquipmentMS.Models.Auxiliary;
using NodaTime;

namespace ComputerEquipmentMS.Models.Domain;

public class Component : IIdentifiable<int>
{
    public int Id { get; set; }

    public int ComponentCategoryId { get; set; }

    public int ComponentManufacturerId { get; set; }

    public required string Name { get; set; }
    public required ComponentSpecifications Specifications { get; set; }

    public decimal Cost { get; set; }
    public required Period WarrantyPeriod { get; set; }
}