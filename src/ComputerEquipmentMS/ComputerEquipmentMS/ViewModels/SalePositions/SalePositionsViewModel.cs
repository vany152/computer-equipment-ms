using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels.SalePositions;

public class SalePositionsViewModel : Collection<SalePositionDetailsViewModel>
{
    public SalePositionsViewModel(ICollection<SalePositionDetailsViewModel> salePositionDetailsViewModels)
        : base(salePositionDetailsViewModels.ToList())
    {
    }

    public SalePositionsViewModel()
    {
    }
}