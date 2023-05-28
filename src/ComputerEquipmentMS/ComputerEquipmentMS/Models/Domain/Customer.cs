using ComputerEquipmentMS.Models.Auxiliary;
using NodaTime;

namespace ComputerEquipmentMS.Models.Domain;

public class Customer : IIdentifiable<int>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Contacts? Contacts { get; set; }
    public LocalDate RegistrationDate { get; set; }
}