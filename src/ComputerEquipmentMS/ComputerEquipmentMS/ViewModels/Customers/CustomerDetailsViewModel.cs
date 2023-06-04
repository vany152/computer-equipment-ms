namespace ComputerEquipmentMS.ViewModels.Customers;

public class CustomerDetailsViewModel : CustomerInfoViewModel
{
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public DateOnly RegistrationDate { get; set; } // todo заменить на LocalDate
}