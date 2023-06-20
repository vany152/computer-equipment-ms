using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels.Components;

public class ComponentsViewModel : Collection<ComponentViewModel>
{
    public ComponentsViewModel(IEnumerable<ComponentViewModel> components)
        : base (components.ToList())
    {
        
    }

    
    // ReSharper disable once UnusedMember.Global
    public ComponentsViewModel()
    {
    }
}