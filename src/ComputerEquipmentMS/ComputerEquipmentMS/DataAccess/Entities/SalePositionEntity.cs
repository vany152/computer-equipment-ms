using NodaTime;

namespace ComputerEquipmentMS.DataAccess.Entities;

public class SalePositionEntity
{
    public int Id { get; set; }

    public int SaleId { get; set; }
    
    public int ConfigurationId { get; set; }

    public decimal Cost { get; set; }
    public short DiscountPercentage { get; set; }
    public required Period WarrantyPeriod { get; set; }
}