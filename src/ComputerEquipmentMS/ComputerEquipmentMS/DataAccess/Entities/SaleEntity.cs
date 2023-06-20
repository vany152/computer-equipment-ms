using NodaTime;

namespace ComputerEquipmentMS.DataAccess.Entities;

public class SaleEntity
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    
    public Instant SaleMoment  { get; set; }

    public short DiscountPercentage { get; set; }

    public ICollection<int>? SalePositionIds { get; set; }
}
