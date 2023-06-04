namespace ComputerEquipmentMS.ViewModels;

public class SaleDetailsViewModel : SaleInfoViewModel
{
    public required CustomerInfoViewModel Customer;
    public required ICollection<SalePositionDetailsViewModel> SalePositions { get; set; }
}