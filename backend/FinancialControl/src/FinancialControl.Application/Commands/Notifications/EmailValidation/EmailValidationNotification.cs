using FinancialControl.Domain.Entities;
using MediatR;

namespace FinancialControl.Application.Commands.Notifications.EmailValidation;

public class EmailValidationNotification(User user) : INotification {
    public User User { get; set; } = user;
}
