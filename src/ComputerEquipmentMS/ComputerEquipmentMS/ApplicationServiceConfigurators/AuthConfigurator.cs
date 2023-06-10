using Microsoft.AspNetCore.Authentication.Cookies;

namespace ComputerEquipmentMS.ApplicationServiceConfigurators;

public static class AuthConfigurator
{
    public static void AddAuth(this IServiceCollection services)
    {
        services.AddAuthorization(
            options => options.AddPolicy(
                "defaultPolicy",
                policy => policy.RequireAuthenticatedUser()
            )
        );
        
        services
            .AddAuthentication(options => options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => options.LoginPath = "/Auth/Login");
    }
}