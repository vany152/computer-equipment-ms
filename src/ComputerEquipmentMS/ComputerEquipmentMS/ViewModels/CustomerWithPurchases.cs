namespace ComputerEquipmentMS.ViewModels;

public class CustomerWithPurchasesViewModel : CustomerDetailsViewModel
{
    public required ICollection<SaleDetailsViewModel> Purchases;
}