using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Shared.Commands
{
    public interface ICommand
    {
        void Validate();
    }
}
