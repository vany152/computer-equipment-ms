﻿using NodaTime;

namespace ComputerEquipmentMS.Models.Domain;

public class ComputerConfiguration : IIdentifiable<int>
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required Period WarrantyPeriod { get; set; }
    public decimal Margin { get; set; }
    public required ICollection<Component> Components { get; set; }
}
