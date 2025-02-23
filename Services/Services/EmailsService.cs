using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Services.Services;

public class EmailsService : IEmailsService
{
    #region Fields
    private readonly EmailSettings _emailSettings;
    #endregion

    #region Constructors
    public EmailsService(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    #endregion

    #region Handle Functions
    public async Task<bool> SendEmailAsync(EmailDto emailDto)
    {
        try
        {
            var builder = new BodyBuilder();
            var email = new MimeMessage();


            email.Sender = MailboxAddress.Parse(_emailSettings.Email);
            email.Subject = emailDto.Subject;
            builder.HtmlBody = emailDto.Body;
            email.Body = builder.ToMessageBody();

            email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.Email));


            if (emailDto.MailToList.Any())
                foreach (var mailAddress in emailDto.MailToList)
                    email.To.Add(MailboxAddress.Parse(mailAddress));
            else
                email.To.Add(MailboxAddress.Parse(emailDto.MailTo));



            if (emailDto.EmailCC.Any())
            {
                InternetAddressList internetAddresses = new InternetAddressList();

                foreach (var cc in emailDto.EmailCC)
                    internetAddresses.Add(InternetAddress.Parse(cc));

                email.Cc.AddRange(internetAddresses);
            }


            if (emailDto.EmailBCC.Any())
            {
                InternetAddressList internetAddresses = new InternetAddressList();

                foreach (var cc in emailDto.EmailBCC)
                    internetAddresses.Add(InternetAddress.Parse(cc));

                email.Bcc.AddRange(internetAddresses);
            }



            if (emailDto.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in emailDto.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            if (emailDto.Priority == "High")
                email.Priority = MessagePriority.Urgent;

            else if (emailDto.Priority == "Low")
                email.Priority = MessagePriority.NonUrgent;

            else
                email.Priority = MessagePriority.Normal;





            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);
            var result = await smtp.SendAsync(email);

            smtp.Disconnect(true);

            return result.Contains("OK");
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    #endregion
}
