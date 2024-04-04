﻿using eCinemax.Server.Shared.ValueObjects;
using MimeKit;

namespace eCinemax.Server.Helpers;

public static class EmailHelper
{
    public static async Task SmptSendAsync(GmailConfig config, string toMail, string title, string body)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.Sender = new MailboxAddress(config.DisplayName, config.Mail);
        mimeMessage.From.Add(new MailboxAddress(config.DisplayName, config.Mail));
        mimeMessage.To.Add(MailboxAddress.Parse(toMail));
        mimeMessage.Subject = title;
        mimeMessage.Body = new TextPart("html") { Text = body };
        
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        
        try {
            await smtp.ConnectAsync(config.Host, config.Port, false);
            await smtp.AuthenticateAsync (config.Mail, config.Password);
            await smtp.SendAsync(mimeMessage);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Có lỗi xảy ra", ex);
        }
    }
}