using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MyAdmin.Admin.Widgets;
using MyAdmin.Fields;

namespace MyAdmin.Admin;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAdmin<TContext>(this IServiceCollection services, Action<AdminOptions> setup)
        where TContext : DbContext
    {
        // dependency
        services.AddRazorPages();
        services.AddRazorTemplating();
        services.AddAntiforgery();
        services.AddHttpContextAccessor();

        // core services
        services.AddScoped<ContentTypeInitializer<TContext>>();
        var options = new AdminOptions();
        if (setup != null)
        {
            setup.Invoke(options);

            // Adds registered admin model types from selectOptions to ServiceCollection
            foreach (string key in options.Admins.Keys)
            {
                ModelAdminTypePair pair = options.Admins.GetValueOrDefault(key)!;
                Type adminType = pair.Admin;
                if (adminType != null)
                {
                    services.TryAddScoped(adminType);
                }
            }
        }

        services.AddScoped<IFieldService, FieldService<TContext>>();

        services.AddTransient<Form>();
        services.AddScoped<FormFactory>();
        services.AddTransient<FormBuilder>();
        
        services.AddScoped<FormRendererFactory>();
        services.AddScoped<DefaultFormRenderer>();
        services.AddScoped<TabularInlineFormRenderer>();
        services.AddScoped<StackedInlineFormRenderer>();

        services.AddScoped<AdminServiceProvider>();
        services.AddSingleton<RouteHelper>();
        services.AddSingleton(typeof(IOptions<AdminOptions>), Options.Create(options));
        services.AddSingleton<WidgetFactory>();
        services.AddSingleton<FieldFactory>();

        // Add cookie auth
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(cookieOptions =>
            {
                string root = options.RootPath.EndsWith("/") ? options.RootPath : options.RootPath + "/";
                cookieOptions.Cookie.HttpOnly = true;
                cookieOptions.LoginPath = $"/account/login";
                cookieOptions.LogoutPath = $"/account/logout";
                cookieOptions.AccessDeniedPath = $"/account/AccessDenied";
            });

        return services;
    }

    public static IServiceCollection RegisterAdmin<TAdmin>(this IServiceCollection services)
        where TAdmin : class
    {
        services.TryAddScoped<TAdmin>();
        return services;
    }
}
