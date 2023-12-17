using Dominate.Data.ViewModel;
using Dominate.Services.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Dominate.Services.Repositories
{
    public class EmailHelper: IEmailHelper
    {
        public readonly IConfiguration _configuration;
        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> ForgotPasswordEmailAsync(ForgotPasswordEmailViewModel model)
        {
            string mailbody = $@"
        <h1>Reset Password Email</h1>
        <p>Dear {{userName}},</p>
        <p>This is your reset passwod token:</p>
        <p>{{ResetPasswordToken}}</p>
        <p>Regards,</p>
        ";
            mailbody = mailbody.Replace("{userName}", model.UserFullName).
                Replace("{ResetPasswordToken}", model.ResetPasswordToken);

            SendEmailViewModel sendEmailViewModel = new SendEmailViewModel();
            sendEmailViewModel.Subject = "Reset Password";
            sendEmailViewModel.Body = mailbody;
            sendEmailViewModel.Email = model.Email;
            return await SendEmail(sendEmailViewModel);


        }

        public async Task<bool> SendEmail(SendEmailViewModel model)
        {
            bool sendEmail = false;
            try
            {

                // Configure the SMTP client
                var emailConfiguration = new EmailConfigurationViewModel();
                emailConfiguration.SmtpType = _configuration.GetSection("EmailConfiguration:smtpType").Value;
                emailConfiguration.SmtpPort =  Convert.ToInt32(_configuration.GetSection("EmailConfiguration:smtpPort").Value);
                emailConfiguration.EmailSender =  _configuration.GetSection("EmailConfiguration:EmailSender").Value;
                emailConfiguration.Password =  _configuration.GetSection("EmailConfiguration:Password").Value;
                emailConfiguration.IsSsl =  Convert.ToBoolean(_configuration.GetSection("EmailConfiguration:IsSsl").Value);

                MailMessage mailmsg = new MailMessage();
                mailmsg.IsBodyHtml = true;

                mailmsg.From = new MailAddress(emailConfiguration.EmailSender);

                mailmsg.To.Add(model.Email);

                mailmsg.Subject = model.Subject;

                mailmsg.Body = model.Body;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = emailConfiguration.SmtpType;
                smtp.Port = emailConfiguration.SmtpPort;
                smtp.EnableSsl = emailConfiguration.IsSsl;
                NetworkCredential network = new NetworkCredential(emailConfiguration.EmailSender, emailConfiguration.Password);
                smtp.Credentials = network;
                smtp.UseDefaultCredentials = false;
                await smtp.SendMailAsync(mailmsg);
                sendEmail = true;
                return sendEmail;

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
  
}
