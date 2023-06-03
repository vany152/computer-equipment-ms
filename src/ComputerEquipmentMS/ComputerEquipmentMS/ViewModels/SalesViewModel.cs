using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class SalesViewModel : Collection<SaleViewModel>
{
    public SalesViewModel(IEnumerable<SaleViewModel> sales)
        : base(sales.ToList())
    {
    }
}