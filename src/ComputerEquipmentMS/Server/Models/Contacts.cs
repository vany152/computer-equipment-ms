namespace Server.Models;

public class Contacts : Dictionary<ContactType, string>
{
    public Contacts(IDictionary<ContactType, string> contacts) : base(contacts)
    {
    }
}