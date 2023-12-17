using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Data.ViewModel
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

    }
    public class ForgotPasswordEmailViewModel
    {
        public string Email { get; set; }
        public string ResetPasswordToken { get; set; }
        public string UserFullName { get; set; }

    }
    public class EmailConfigurationViewModel
    {
        public string SmtpType { get; set; }
        public int SmtpPort { get; set; }
        public string EmailSender { get; set; }
        public string Password { get; set; }
        public bool IsSsl { get; set; }
    }
}
