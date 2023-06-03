namespace ComputerEquipmentMS.ViewModels;

public class ComputerConfigurationDetailsViewModel : ComputerConfigurationInfoViewModel
{
    public required ICollection<ComponentViewModel> Components { get; set; }
}