﻿namespace ComputerEquipmentMS.ViewModels.Sales;

public class CreateSaleViewModel : SaleViewModelBase
{
    public int CustomerId { get; set; }
    public string ComputerConfigurationIdsWithDiscountsJson { get; set; } = string.Empty;
}