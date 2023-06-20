using System.Collections.ObjectModel;

namespace ComputerEquipmentMS.ViewModels.ComponentCategories;

public class ComponentCategoriesViewModel : Collection<ComponentCategoryViewModel>
{
    public ComponentCategoriesViewModel(ICollection<ComponentCategoryViewModel> componentCategoryViewModels)
        : base(componentCategoryViewModels.ToList())
    {
    }

    public ComponentCategoriesViewModel()
    {
    }
}