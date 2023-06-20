using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels.ComputerConfigurations;

public class ComputerConfigurationsViewModel : Collection<ComputerConfigurationDetailsViewModel>
{
    public ComputerConfigurationsViewModel(IEnumerable<ComputerConfigurationDetailsViewModel> configurations)
        : base (configurations.ToList())
    {
    }

    public ComputerConfigurationsViewModel()
    {
    }
}