using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels.ComponentManufacturers;

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