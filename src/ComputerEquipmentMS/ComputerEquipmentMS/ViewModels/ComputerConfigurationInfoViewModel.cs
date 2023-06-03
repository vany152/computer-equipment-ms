namespace ComputerEquipmentMS.ViewModels;

public class ComputerConfigurationInfoViewModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int WarrantyPeriodMonths { get; set; }
    public decimal Margin { get; set; }
}