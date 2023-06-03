using NodaTime;

namespace ComputerEquipmentMS.Models.Domain;

public class Sale : IIdentifiable<int>
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    
    public Instant SaleMoment  { get; set; }
    
    public decimal StartingCost { get; set; }

    public short DiscountPercentage { get; set; }

    public ICollection<SalePosition> SalePositions { get; set; } = new List<SalePosition>();
}
