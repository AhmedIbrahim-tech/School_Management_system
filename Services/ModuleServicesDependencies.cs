using Microsoft.Extensions.DependencyInjection;
using Services.AuthServices.Implementations;
using Services.AuthServices.Interfaces;

namespace Services;

public static class ModuleServicesDependencies
{
    public static IServiceCollection AddServicesDependencies(this IServiceCollection services)
    {
        services.AddTransient<IStudentServices, StudentServices>();
        services.AddTransient<IApplicationUserService, ApplicationUserService>();
        services.AddTransient<IAuthenticationServiceAsync, AuthenticationServiceAsync>();
        services.AddTransient<IAuthorizationServiceAsync, AuthorizationServiceAsync>();
        services.AddTransient<IEmailsService, EmailsService>();
        services.AddTransient<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IFileService, FileService>();

        return services;
    }
}
