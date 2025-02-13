using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Domain.Settings
{
    public class ApiResourceUrls
    {
        public string ResetPin { get; set; }
        public string CreatePin { get; set; }
        public string QueryByUserId { get; set; }
        public string SendEmail { get; set; }
        public string SendEmailAttachment { get; set; }
        public string FindUser { get; set; }
        public string EnableUser { get; set; }
        public string DisableUser { get; set; }
        public string PasswordResetUrl { get; set; }
        public string SignupVerificationUrl { get; set; }
        public string MailBaseURL { get; set; }
        public string MailSenderEmail { get; set; }
        public string MailSender { get; set; }
        public string MailAuthKey { get; set; }
        public string OtpUrl { get; set; }
        public string Login { get; set; }

    }


    public class Rootobject
    {
        public string ResetPin { get; set; }
        public string CreatePin { get; set; }
        public string QueryByUserId { get; set; }
        public string SendEmail { get; set; }
        public string FindUser { get; set; }
        public string EnableUser { get; set; }
        public string DisableUser { get; set; }
    }
}

