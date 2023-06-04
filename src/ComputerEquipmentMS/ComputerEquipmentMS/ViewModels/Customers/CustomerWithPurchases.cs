using ComputerEquipmentMS.ViewModels.Sales;

namespace ComputerEquipmentMS.ViewModels.Customers;

public class CustomerWithPurchasesViewModel : CustomerDetailsViewModel
{
    public required ICollection<SaleDetailsViewModel> Purchases;
}