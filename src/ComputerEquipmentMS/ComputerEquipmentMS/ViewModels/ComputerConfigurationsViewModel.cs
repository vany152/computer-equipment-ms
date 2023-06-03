using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class ComputerConfigurationsViewModel : Collection<ComputerConfigurationViewModel>
{
    public ComputerConfigurationsViewModel(IEnumerable<ComputerConfigurationViewModel> configurations)
        : base (configurations.ToList())
    {
        
    }
}