﻿using NodaTime;

namespace Server.Models;

public class Sale : IIdentifiable<int>
{
    public int Id { get; set; }

    public int CustomerId { get; set; }
    
    public Instant SaleMoment  { get; set; }

    public short DiscountPercentage { get; set; }
}
