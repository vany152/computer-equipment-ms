using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class ComponentsViewModel : Collection<ComponentViewModel>
{
    public ComponentsViewModel(IEnumerable<ComponentViewModel> components)
        : base (components.ToList())
    {
        
    }
}