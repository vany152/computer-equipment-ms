using NodaTime;

namespace ComputerEquipmentMS.ViewModels.SalePositions;

public class SalePositionDetailsViewModel : SalePositionViewModelBase
{
    public required string ComputerConfigurationName { get; set; }
    
    public decimal StartingCost { get; set; }
    public decimal FinalCost { get; set; }
    
    public required Period WarrantyPeriod { get; set; }
}