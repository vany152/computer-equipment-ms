using ComputerEquipmentMS.DataAccess.Entities.Auxiliary;
using NodaTime;

namespace ComputerEquipmentMS.DataAccess.Entities;

public class CustomerEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ContactsEntity? Contacts { get; set; }
    public LocalDate RegistrationDate { get; set; }
}