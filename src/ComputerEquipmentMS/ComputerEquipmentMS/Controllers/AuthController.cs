using System.Security.Claims;
using System.Text.RegularExpressions;
using ComputerEquipmentMS.DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ComputerEquipmentMS.Controllers;

[AllowAnonymous]
public class AuthController : Controller
{
    private readonly IDbConnectionString _mainDbConnectionString;

    public AuthController(IDbConnectionString mainDbConnectionString)
    {
        _mainDbConnectionString = mainDbConnectionString;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }    
    
    [HttpPost]
    public async Task<IActionResult> Login(string userName, string password, string returnUrl = "/")
    {
        var connectionSuccessful = await TryConnectToDatabase(userName, password);
        if (!connectionSuccessful) return View(model: "Неверные учетные данные");

        await AddUser(userName);

        return LocalRedirect(returnUrl);
    }
    
    
    
    [HttpPost]
    public async Task<IActionResult> Logout(string returnUrl = "/")
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return LocalRedirect(returnUrl);
    }



    private async Task AddUser(string userName)
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, userName),
            new (ClaimsIdentity.DefaultRoleClaimType, userName),
        };
        var id = new ClaimsIdentity(
            claims, 
            "ApplicationCookie", 
            ClaimsIdentity.DefaultNameClaimType, 
            ClaimsIdentity.DefaultRoleClaimType
        );
            
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(id));
    }
    
    private async Task<bool> TryConnectToDatabase(string username, string password)
    {
        bool connectionResult;
        
        var connectionString = ConstructTestConnectionString(username, password);
        await using var connection = new NpgsqlConnection(connectionString);
        try
        {
            connection.Open();
            connectionResult = true;
        }
        catch (NpgsqlException)
        {
            connectionResult = false;
        }

        return await Task.FromResult(connectionResult);
    }

    private string ConstructTestConnectionString(string username, string password)
    {
        var result = Regex.Replace(
            _mainDbConnectionString.Value, 
            @"Username\s*=\s*\w*;", 
            $"Username={username};", 
            RegexOptions.IgnoreCase);
        
        result = Regex.Replace(
            result, 
            @"Password\s*=\s*\w*;", 
            $"Password={password};",
            RegexOptions.IgnoreCase);
        
        return result;
    }
}
