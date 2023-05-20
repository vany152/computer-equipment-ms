namespace Server.Models;

public class Customer : IIdentifiable<int>, ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Contacts? Contacts { get; set; }
    public DateTime RegistrationDate { get; set; }
    
    public object Clone() =>
        new Customer
        {
            Id = Id,
            Name = Name,
            Contacts = Contacts is not null 
                ? new Contacts(Contacts)
                : null,
            RegistrationDate = RegistrationDate
        };
}