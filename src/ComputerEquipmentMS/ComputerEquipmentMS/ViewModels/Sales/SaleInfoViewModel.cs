using NodaTime;

namespace ComputerEquipmentMS.ViewModels.Sales;

public class SaleInfoViewModel : SaleViewModelBase
{
    public Instant SaleMoment  { get; set; }
    public decimal StartingCost { get; set; }
    public decimal FinalCost { get; set; }
}