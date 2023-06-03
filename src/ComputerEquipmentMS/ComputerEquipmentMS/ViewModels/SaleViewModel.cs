using NodaTime;

namespace ComputerEquipmentMS.ViewModels;

public class SaleViewModel
{
    public int Id { get; set; }
    
    public Instant SaleMoment  { get; set; }

    public decimal StartingCost { get; set; }
    public short DiscountPercentage { get; set; }
    public decimal FinalCost { get; set; }

    public required ICollection<SalePositionViewModel> SalePositions { get; set; }
}