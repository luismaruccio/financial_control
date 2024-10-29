using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Domain.DTOs.Notifications;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Enums;
using FinancialControl.Domain.Enums.Notifications;
using FinancialControl.Domain.Interfaces.Repositories;
using MediatR;

namespace FinancialControl.Application.Commands.Notifications.EmailValidation;

public class EmailValidationHandler(IValidationCodeService validationCodeService, IValidationCodeRepository validationCodeRepository, INotificationEnqueueService notificationEnqueueService) : INotificationHandler<EmailValidationNotification>
{
    private readonly IValidationCodeService _validationCodeService = validationCodeService;
    private readonly IValidationCodeRepository _validationCodeRepository = validationCodeRepository;
    private readonly INotificationEnqueueService _notificationEnqueueService = notificationEnqueueService;

    public async Task Handle(EmailValidationNotification notification, CancellationToken cancellationToken)
    {
        var code = await GenerateCode(notification.User.Id);
        await InsertValidationCode(code, notification.User.Id);
        await EnqueueNotification(code, notification.User);
    }

    private async Task<string> GenerateCode(int userId)
    {
         var code = _validationCodeService.GenerateCode();

        while (await _validationCodeRepository.IsValidationCodeExistsAsync(code, userId))
        {
            code = _validationCodeService.GenerateCode();
        }

        return code;
    }

    private async Task InsertValidationCode(string code, int userId)
    {
        var validationCode = new ValidationCode()
        {
            UserId = userId,
            Code = code,
            CodePurpose = ValidationCodePurpose.ValidationEmail,
            ValidateDate = DateTime.UtcNow.AddHours(1)
        };

        await _validationCodeRepository.AddValidationCodeAsync(validationCode);
    }

    private async Task EnqueueNotification(string code, User user)
    {
        var notification = new NotificationDTO()
        {
            Recipient = new RecipientDTO()
            {
                Name = user.Name,
                Email = user.Email,
            },
            NotificationType = NotificationType.ValidationEmail,
            Params = new Dictionary<string, object>
            {
                { "code", code }
            }
        };

        await _notificationEnqueueService.EnqueueNotification(notification);

    }
}
