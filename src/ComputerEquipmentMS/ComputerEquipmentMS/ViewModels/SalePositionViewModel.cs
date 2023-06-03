using NodaTime;

namespace ComputerEquipmentMS.ViewModels;

public class SalePositionViewModel
{
    public int Id { get; set; }
    
    public required ComputerConfigurationInfoViewModel Configuration { get; set; }
    
    public decimal StartingCost { get; set; }
    public decimal FinalCost { get; set; }
    public short DiscountPercentage { get; set; }
    
    public required Period WarrantyPeriod { get; set; }
}