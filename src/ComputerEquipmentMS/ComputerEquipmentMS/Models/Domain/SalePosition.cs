using NodaTime;

namespace ComputerEquipmentMS.Models.Domain;

public class SalePosition : IIdentifiable<int>
{
    public int Id { get; set; }

    public int SaleId { get; set; }
    
    public required ComputerConfiguration Configuration { get; set; }

    public decimal Cost { get; set; }
    public short DiscountPercentage { get; set; }
    public required Period WarrantyPeriod { get; set; }
}