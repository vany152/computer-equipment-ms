using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class ComponentManufacturersViewModel : Collection<ComponentManufacturerViewModel>
{
    public ComponentManufacturersViewModel(ICollection<ComponentManufacturerViewModel> manufacturerViewModels)
        : base(manufacturerViewModels.ToList())
    {
    }
    
    public ComponentManufacturersViewModel()
    {
    }
}