using FinancialControl.Application.Commands.Notifications.EmailValidation;
using FinancialControl.Application.Interfaces.Services;
using FinancialControl.Domain.DTOs.Notifications;
using FinancialControl.Domain.Entities;
using FinancialControl.Domain.Interfaces.Repositories;
using Moq;

namespace FinancialControl.Application.Tests.Commands.Notifications.EmailValidation;

public class EmailValidationHandlerTests
{
    private readonly Mock<IValidationCodeService> _validationCodeServiceMock = new();
    private readonly Mock<IValidationCodeRepository> _validationCodeRepositoryMock = new();
    private readonly Mock<INotificationEnqueueService> _notificationEnqueueServiceMock = new();
    private readonly EmailValidationHandler _handler;
    private readonly User _user;
    private readonly EmailValidationNotification _emailValidationNotification;

    public EmailValidationHandlerTests() 
    { 
        _handler = new EmailValidationHandler(_validationCodeServiceMock.Object, _validationCodeRepositoryMock.Object, _notificationEnqueueServiceMock.Object);

        _user = new() { Id = 1, Name = "User", Email = "email@email.com", Password = "123pass" };
        _emailValidationNotification = new(_user);        
    }

    [Fact]
    public async Task Handle_WhenCodeDoesNotExistForUser_ShouldGenerateAndInsertNewCode()
    {
        var code = "1234-5678";
        _validationCodeServiceMock.Setup(mock => mock.GenerateCode()).Returns(code);
        _validationCodeRepositoryMock.Setup(mock => mock.IsValidationCodeExistsAsync(code, _user.Id)).ReturnsAsync(false);

        await _handler.Handle(_emailValidationNotification, CancellationToken.None);

        _validationCodeRepositoryMock.Verify(mock => mock.AddValidationCodeAsync(It.Is<ValidationCode>(vc => vc.UserId == _user.Id && vc.Code == code)));
    }

    [Fact]
    public async Task Handle_WhenCodeInUse_ShouldRegenerateUntilCodeIsUniqueAndInsert()
    {
        var codeA = "1234-5678";
        var codeB = "2345-6789";
        var codeC = "3456-7890";

        _validationCodeServiceMock.SetupSequence(mock => mock.GenerateCode())
            .Returns(codeA)
            .Returns(codeB)
            .Returns(codeC);

        _validationCodeRepositoryMock.Setup(mock => mock.IsValidationCodeExistsAsync(codeA, _user.Id)).ReturnsAsync(true);
        _validationCodeRepositoryMock.Setup(mock => mock.IsValidationCodeExistsAsync(codeB, _user.Id)).ReturnsAsync(true);
        _validationCodeRepositoryMock.Setup(mock => mock.IsValidationCodeExistsAsync(codeC, _user.Id)).ReturnsAsync(false);

        await _handler.Handle(_emailValidationNotification, CancellationToken.None);

        _validationCodeRepositoryMock.Verify(mock => mock.AddValidationCodeAsync(It.Is<ValidationCode>(vc => vc.UserId == _user.Id && vc.Code == codeC)));
    }

    [Fact]
    public async Task Handle_WhenCodeWasGenerated_ShouldEnqueueNotification()
    {
        var code = "1234-5678";
        _validationCodeServiceMock.Setup(mock => mock.GenerateCode()).Returns(code);
        _validationCodeRepositoryMock.Setup(mock => mock.IsValidationCodeExistsAsync(code, _user.Id)).ReturnsAsync(false);

        await _handler.Handle(_emailValidationNotification, CancellationToken.None);

        _notificationEnqueueServiceMock.Verify(mock => mock.EnqueueNotification(It.Is<NotificationDTO>(n => 
            n.Recipient.Name == _user.Name && 
            n.Recipient.Email == _user.Email && 
            n.NotificationType == Domain.Enums.Notifications.NotificationType.ValidationEmail && 
            n.Params.ContainsKey("code"))));
    }
}
