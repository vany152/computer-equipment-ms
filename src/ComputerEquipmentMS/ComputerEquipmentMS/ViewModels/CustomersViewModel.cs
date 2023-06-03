using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class CustomersViewModel : Collection<CustomerViewModel>
{
    public CustomersViewModel(IEnumerable<CustomerViewModel> customers)
        : base(customers.ToList())
    {
    }
}