using NodaTime;

namespace Server.Models;

public class SalePosition : IIdentifiable<int>
{
    public int Id { get; set; }

    public int SaleId { get; set; }
    
    public int ConfigurationId { get; set; }

    public decimal Cost { get; set; }
    public short DiscountPercentage { get; set; }
    public required Period WarrantyPeriod { get; set; }
}