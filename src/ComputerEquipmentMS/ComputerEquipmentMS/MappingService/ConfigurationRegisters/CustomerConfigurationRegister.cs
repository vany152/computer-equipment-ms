using ComputerEquipmentMS.Models.Auxiliary;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using NodaTime;

namespace ComputerEquipmentMS.MappingService.ConfigurationRegisters;

// ReSharper disable once UnusedType.Global - will be used on startup while IRegisters scanning
public class CustomerConfigurationRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Customer, CustomerDetailsViewModel>()
            .Include<Customer, CustomerWithPurchasesViewModel>()
            .Map(vm => vm.RegistrationDate, c => c.RegistrationDate.ToDateOnly())
            .Map(vm => vm.Email, c => GetEmail(c.Contacts))
            .Map(vm => vm.Phone, c => GetPhone(c.Contacts));
        
        // cannot use .TwoWays() because of exception, so there is another configuration
        config
            .NewConfig<CustomerDetailsViewModel, Customer>()
            .Map(c => c.RegistrationDate, vm => LocalDate.FromDateOnly(vm.RegistrationDate))
            .Map(c => c.Contacts, vm => AdaptCustomerViewModelContactsToCustomerContacts(vm));   
    }
    
    

    private static Contacts? AdaptCustomerViewModelContactsToCustomerContacts(CustomerDetailsViewModel customerViewModel)
    {
        var email = customerViewModel.Email;
        var phone = customerViewModel.Phone;

        var emailEmpty = string.IsNullOrEmpty(email);
        var phoneEmpty = string.IsNullOrEmpty(phone);

        if (emailEmpty && phoneEmpty)
            return null;
        
        var contacts = new Contacts();
        if (!emailEmpty)
            contacts[ContactType.Email] = email!;

        if (!phoneEmpty)
            contacts[ContactType.Phone] = phone!;

        return contacts;
    }

    private static string? GetEmail(Contacts? contacts) => 
        contacts?.GetValueOrDefault(ContactType.Email);
    private static string? GetPhone(Contacts? contacts) => 
        contacts?.GetValueOrDefault(ContactType.Phone);
}