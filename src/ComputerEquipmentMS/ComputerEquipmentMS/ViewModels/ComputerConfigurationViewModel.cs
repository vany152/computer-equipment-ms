using NodaTime;

namespace ComputerEquipmentMS.ViewModels;

public class ComputerConfigurationViewModel
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required Period WarrantyPeriod { get; set; }
    public decimal Margin { get; set; }
    public required ICollection<ComponentViewModel> Components { get; set; }
}