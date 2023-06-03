namespace ComputerEquipmentMS.ViewModels;

public class CustomerViewModel
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }

    public DateOnly RegistrationDate { get; set; } // todo заменить на LocalDate
}