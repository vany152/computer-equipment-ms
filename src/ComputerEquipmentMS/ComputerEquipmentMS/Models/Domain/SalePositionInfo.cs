using NodaTime;

namespace ComputerEquipmentMS.Models.Domain;

public class SalePositionInfo
{
    public int SalePositionId { get; set; }
    public Instant SaleMoment { get; set; }
    public decimal StartingCost { get; set; }
    public decimal FinalCost { get; set; }
    public short DiscountPercentage { get; set; }
    public required Period WarrantyPeriod { get; set; }
}