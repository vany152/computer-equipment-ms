using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels;

public class ComputerConfigurationsViewModel : Collection<ComputerConfigurationDetailsViewModel>
{
    public ComputerConfigurationsViewModel(IEnumerable<ComputerConfigurationDetailsViewModel> configurations)
        : base (configurations.ToList())
    {
        
    }
}