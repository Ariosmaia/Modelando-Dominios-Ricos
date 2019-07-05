using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Domain.Services
{
    public interface IEmailService
    {
        void Send(String to, string email, string subject, string body);
    }
}
