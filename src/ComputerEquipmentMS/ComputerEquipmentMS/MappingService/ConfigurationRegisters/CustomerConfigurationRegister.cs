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
            .NewConfig<CustomerViewModel, Customer>()
            .Map(customer => customer.RegistrationDate, viewModel => LocalDate.FromDateOnly(viewModel.RegistrationDate))
            .Map(customer => customer.Contacts, viewModel => AdaptCustomerViewModelContactsToCustomerContacts(viewModel));
        
        // cannot use .TwoWays() because of exception, so there is another configuration
        config
            .NewConfig<Customer, CustomerViewModel>()
            .Include<Customer, CustomerWithPurchasesViewModel>()
            .Map(vm => vm.RegistrationDate, customer => customer.RegistrationDate.ToDateOnly())
            .Map(vm => vm.Email, customer => GetEmail(customer.Contacts))
            .Map(vm => vm.Phone, customer => GetPhone(customer.Contacts));
    }
    
    

    private static Contacts? AdaptCustomerViewModelContactsToCustomerContacts(CustomerViewModel customerViewModel)
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

    private static string? GetEmail(Contacts? contacts) => contacts?.GetValueOrDefault(ContactType.Email);
    private static string? GetPhone(Contacts? contacts) => contacts?.GetValueOrDefault(ContactType.Phone);
}