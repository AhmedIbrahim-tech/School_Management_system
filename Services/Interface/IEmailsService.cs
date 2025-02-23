namespace Services.Interface;

public interface IEmailsService
{
    Task<bool> SendEmailAsync(EmailDto emailDto);
}
