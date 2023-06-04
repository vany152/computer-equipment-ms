using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels.Sales;

public class SalesViewModel : Collection<SaleDetailsViewModel>
{
    public SalesViewModel(IEnumerable<SaleDetailsViewModel> sales)
        : base(sales.ToList())
    {
    }
}