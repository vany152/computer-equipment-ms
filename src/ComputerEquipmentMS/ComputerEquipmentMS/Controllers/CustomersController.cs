using ComputerEquipmentMS.DataAccess;
using ComputerEquipmentMS.Models.Domain;
using ComputerEquipmentMS.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace ComputerEquipmentMS.Controllers;

public class CustomersController : Controller
{
    private readonly IRepository<Customer, int> _customersRepository;
    private readonly NpgsqlStoredFunctionsExecutor _functionsExecutor;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(
        IRepository<Customer, int> customersRepository,
        NpgsqlStoredFunctionsExecutor functionsExecutor,
        ILogger<CustomersController> logger)
    {
        _customersRepository = customersRepository;
        _functionsExecutor = functionsExecutor;
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        var customers = _customersRepository.GetAll();

        var customerViewModels = customers.Adapt<List<CustomerViewModel>>();
        var customersViewModel = new CustomersViewModel(customerViewModels); 
        
        return View(customersViewModel);
    }

    
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(CustomerViewModel newCustomerVm)
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
        if (customer is null)
            return NotFound();
        
        var purchases = _functionsExecutor.GetCustomersPurchases(id);
        var purchaseVms = purchases.Adapt<List<SaleViewModel>>();
        
        var customerVm = customer.Adapt<CustomerWithPurchasesViewModel>();
        customerVm.Purchases = purchaseVms;
        
        return View(customerVm);
    }
    
    
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var customer = _customersRepository.GetById(id);
        if (customer is null)
            return NotFound();

        var customerVm = customer.Adapt<CustomerViewModel>();
        return View(customerVm);
    }
    
    [HttpPost]
    public IActionResult Edit(CustomerViewModel customerViewModel)
    {
        var customer = customerViewModel.Adapt<Customer>();

        var result = _customersRepository.Edit(customer);
        if (result) return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while editing customer: customer with id {Id} was not edited", customer.Id);
        return NotFound();
    }
    
    
    
    [HttpPost]
    public IActionResult Remove(int id)
    {
        var result = _customersRepository.Remove(id);
        if (result) return RedirectToAction(nameof(Index));
        
        _logger.LogError("error while removing customer: customer with id {Id} was not removed", id);
        return NotFound();
    }
}