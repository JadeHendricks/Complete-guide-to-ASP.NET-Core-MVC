using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    public class EmailSender : IEmailSender
    {
        //we need to register this implementation in the program.cs file
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //logic to send email - in the future
            return Task.CompletedTask;
        }
    }
}
