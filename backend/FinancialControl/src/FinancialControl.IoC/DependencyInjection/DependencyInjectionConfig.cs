using FinancialControl.Application.Commands.Users.CreateUser;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Application.Services;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialControl.IoC.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {

            //Mediatr
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly));

            //Validators
            services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();

            //Services
            services.AddScoped<IEncryptionService, EncryptionService>();

            //Repositories
            services.AddScoped<IUserRepository, UserRespository>();

            return services;
        }
    }

}
