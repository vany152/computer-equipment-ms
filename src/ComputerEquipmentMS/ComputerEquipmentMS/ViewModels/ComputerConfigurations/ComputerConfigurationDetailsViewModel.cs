using ComputerEquipmentMS.ViewModels.Components;

namespace ComputerEquipmentMS.ViewModels.ComputerConfigurations;

public class ComputerConfigurationDetailsViewModel : ComputerConfigurationInfoViewModel
{
    public required ICollection<ComponentViewModel> Components { get; set; }
}