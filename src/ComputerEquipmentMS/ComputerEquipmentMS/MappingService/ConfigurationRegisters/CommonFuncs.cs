﻿using NodaTime;

namespace ComputerEquipmentMS.MappingService.ConfigurationRegisters;

public static class CommonFuncs
{
    public static int WarrantyPeriodToMonths(Period warrantyPeriod) =>
        warrantyPeriod.Years * 12 + warrantyPeriod.Months;

    public static Period WarrantyPeriodFromMonths(int months) =>
        Period.FromMonths(months);
    
    public static decimal CalculateFinalCost(decimal startingCost, short discountPercentage) =>
        startingCost - (startingCost * discountPercentage) / 100;
}