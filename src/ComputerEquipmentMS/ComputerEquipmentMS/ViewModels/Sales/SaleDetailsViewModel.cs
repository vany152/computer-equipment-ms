using ComputerEquipmentMS.ViewModels.Customers;
using ComputerEquipmentMS.ViewModels.SalePositions;

namespace ComputerEquipmentMS.ViewModels.Sales;

public class SaleDetailsViewModel : SaleInfoViewModel
{
    public required CustomerInfoViewModel Customer;
    public required ICollection<SalePositionDetailsViewModel> SalePositions { get; set; }
}