namespace ComputerEquipmentMS.ViewModels;

public class ComponentViewModel
{
    public int Id { get; set; }

    public int ComponentCategoryId { get; set; }
    public string ComponentCategory { get; set; } = string.Empty;
    
    public int ComponentManufacturerId { get; set; }
    public string ComponentManufacturer { get; set; } = string.Empty;

    public required string Name { get; set; }
    public required string Specifications { get; set; }

    public decimal Cost { get; set; }
    public int WarrantyPeriodMonths { get; set; }
}