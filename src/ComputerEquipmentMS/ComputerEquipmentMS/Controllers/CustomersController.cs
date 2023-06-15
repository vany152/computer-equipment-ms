using ComputerEquipmentMS.Constants;
using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels.Customers;
using ComputerEquipmentMS.ViewModels.Sales;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace ComputerEquipmentMS.Controllers;

public class CustomersController : ControllerBase
{
    private readonly IRepository<Customer, int> _customersRepository;
    private readonly NpgsqlStoredFunctionsExecutor _functionsExecutor;

    public CustomersController(
        IRepository<Customer, int> customersRepository,
        NpgsqlStoredFunctionsExecutor functionsExecutor,
        ILogger<CustomersController> logger) : base(logger)
    {
        _customersRepository = customersRepository;
        _functionsExecutor = functionsExecutor;
    }
    
    public IActionResult Index()
    {
        var customers = _customersRepository.GetAll();

        var customerViewModels = customers.Adapt<List<CustomerDetailsViewModel>>();
        var customersViewModel = new CustomersViewModel(customerViewModels); 
        
        return View(customersViewModel);
    }

    
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(CustomerDetailsViewModel newCustomerVm)
    {
        var newCustomer = newCustomerVm.Adapt<Customer>();
        
        newCustomer.RegistrationDate = LocalDate.FromDateTime(DateTime.Now);
        _customersRepository.Add(newCustomer);
        
        return RedirectToAction(nameof(Index));
    }
    
    
    
    [HttpGet]
    public IActionResult Details(int id)
    {
        var customer = _customersRepository.GetById(id);
        if (customer is null) return HandleError($"cannot find {nameof(Customer)} with id = {id}");

        var purchases = _functionsExecutor.GetCustomersPurchases(id);
        var purchaseVms = purchases.Adapt<List<SaleDetailsViewModel>>();
        
        var customerVm = customer.Adapt<CustomerWithPurchasesViewModel>();
        customerVm.Purchases = purchaseVms;
        
        return View(customerVm);
    }
    
    
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var customer = _customersRepository.GetById(id);
        if (customer is null) return HandleError($"cannot find {nameof(Customer)} with id = {id}");

        var customerVm = customer.Adapt<CustomerDetailsViewModel>();
        return View(customerVm);
    }
    
    [HttpPost]
    public IActionResult Edit(CustomerDetailsViewModel customerViewModel)
    {
        var customer = customerViewModel.Adapt<Customer>();

        var result = _customersRepository.Edit(customer);
        if (result) return RedirectToAction(nameof(Index));

        return result
            ? RedirectToAction(nameof(Index))
            : HandleError($"error while editing {nameof(Customer)}: element with id {customer.Id} was not edited");
    }
    
    
    
    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public IActionResult Remove(int id)
    {
        var result = _customersRepository.Remove(id);
        return result 
            ? RedirectToAction(nameof(Index)) 
            : HandleError($"error while removing {nameof(Customer)}: element with id {id} was not removed");
    }
}