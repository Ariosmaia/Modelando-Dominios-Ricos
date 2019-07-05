using Flunt.Validations;
using PaymentContext.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Domain.ValueObjects
{
    public class Name : ValueObject
    {
        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            AddNotifications(new Contract()
                .HasMinLen(FirstName, 3, "Name.FirstName", "Nome deve conter pelo menos 3 caractesres")
                .HasMinLen(LastName, 3, "Name.LastName", "Sobrenome deve conter pelo menos 3 caractesres")
                .HasMaxLen(FirstName, 40, "Name.FirstName"," Nome deve conter até 40 caractesres"));
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
