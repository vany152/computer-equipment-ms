using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class CustomersViewModel : Collection<CustomerDetailsViewModel>
{
    public CustomersViewModel(IEnumerable<CustomerDetailsViewModel> customers)
        : base(customers.ToList())
    {
    }

    public CustomersViewModel()
    {
    }
}