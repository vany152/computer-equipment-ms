using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class SalesViewModel : Collection<SaleDetailsViewModel>
{
    public SalesViewModel(IEnumerable<SaleDetailsViewModel> sales)
        : base(sales.ToList())
    {
    }
}