using FinancialControl.Application.Commands.Notifications.EmailValidation;
using FinancialControl.Application.Commands.Users.CreateUser;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Application.Services;
using FinancialControl.Application.Services.Notifications;
using FinancialControl.Domain.DTOs.Notifications;
using FinancialControl.Domain.Interfaces.Publishers;
using FinancialControl.Domain.Interfaces.Repositories;
using FinancialControl.Infrastructure.Data;
using FinancialControl.Infrastructure.Messaging;
using FinancialControl.Infrastructure.Messaging.Connections;
using FinancialControl.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialControl.IoC.DependencyInjection;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FinancialControlDbContext>(options =>
            options.UseNpgsql(GetDatabaseConnectionString(configuration),
                  b => b.MigrationsAssembly("FinancialControl.Infrastructure")));


        //Mediatr
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserHandler).Assembly));

        //Validators
        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();

        //Services
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IValidationCodeService, ValidationCodeService>();
        services.AddScoped<INotificationEnqueueService, NotificationEnqueueService>();

        //Repositories
        services.AddScoped<IUserRepository, UserRespository>();
        services.AddScoped<IValidationCodeRepository, ValidationCodeRepository>();

        //Messaging
        services.AddSingleton<IRabbitMQConnection>(new RabbitMQConnection(GetRabbitMQURI()));
        services.AddSingleton<IMessagePublisher<NotificationDTO>, RabbitMQPublisher<NotificationDTO>>();        

        return services;
    }

    private static string? GetDatabaseConnectionString(IConfiguration configuration)
    {
        var envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
        if (string.IsNullOrEmpty(envConnectionString))
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }
        return envConnectionString;
    }

    private static string? GetRabbitMQURI()
        => Environment.GetEnvironmentVariable("RABBITMQ_CONNECTION_STRING");
}
