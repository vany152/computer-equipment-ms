using NodaTime;

namespace Server.Models.Domain;

public class SalePositionInfo
{
    public int SalePositionId;
    public Instant SaleMoment;
    public decimal StartingCost;
    public decimal FinalCost;
    public short DiscountPercentage;
    public required Period WarrantyPeriod;
}