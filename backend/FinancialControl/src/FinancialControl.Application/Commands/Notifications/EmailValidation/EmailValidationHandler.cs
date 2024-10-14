using FinancialControl.Application.Interfaces.Services;
using MediatR;

namespace FinancialControl.Application.Commands.Notifications.EmailValidation;

public class EmailValidationHandler(IValidationCodeService validationCodeService) : INotificationHandler<EmailValidationNotification>
{
    private readonly IValidationCodeService _validationCodeService = validationCodeService;
    public Task Handle(EmailValidationNotification notification, CancellationToken cancellationToken)
    {
        var code = _validationCodeService.GenerateCode();

        // Validar código

        // Inserir código no banco de dados

        // Enfileirar envio de notificação

        throw new NotImplementedException();
    }
}
