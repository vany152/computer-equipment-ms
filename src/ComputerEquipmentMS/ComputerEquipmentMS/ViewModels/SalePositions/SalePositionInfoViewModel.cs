﻿using NodaTime;

namespace ComputerEquipmentMS.ViewModels.SalePositions;

public class SalePositionInfoViewModel : SalePositionViewModelBase
{
    public Instant SaleMoment { get; set; }
    public decimal StartingCost { get; set; }
    public decimal FinalCost { get; set; }
    public required Period WarrantyPeriod { get; set; }
}