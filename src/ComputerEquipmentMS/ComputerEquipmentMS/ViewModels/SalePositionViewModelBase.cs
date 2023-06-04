namespace ComputerEquipmentMS.ViewModels;

public class SalePositionViewModelBase
{
    public int Id { get; set; }
    public int ComputerConfigurationId { get; set; }
    public short DiscountPercentage { get; set; }
}