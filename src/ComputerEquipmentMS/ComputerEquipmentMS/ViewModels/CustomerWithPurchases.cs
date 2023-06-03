namespace ComputerEquipmentMS.ViewModels;

public class CustomerWithPurchasesViewModel : CustomerViewModel
{
    public required ICollection<SaleViewModel> Purchases;
}