using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHanlder :
        Notifiable,
        IHandler<CreateBoletoSubscriptionCommand>,
        IHandler<CreatePayPalSubscriptionCommand>,
        IHandler<CreateCreditCardSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHanlder(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            // Fail fast validations
            command.Validate();
            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar seu cadsatro");
            }

            // Verificar se Documento já está cadsatrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verificar se E-mail já está cadastro
            if (_repository.DocumentExists(command.Email))
                AddNotification("Document", "Este E-mail já está em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // Gerar as Entidades
            var student = new Student(name, document, email);
            var subscrition = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email);

            // Relacionamentos
            subscrition.AddPayment(payment);
            student.AddSubscription(subscrition);

            // Agrupar as Validações
            AddNotifications(name, document, email, address, student, subscrition, payment);

            // Checar as notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            // Salvar as Informações
            _repository.CreateSubscription(student);

            // Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ", "Sua assinatura foi criada");

            // Retornar informações
            return new CommandResult(true, "Assinatura realizada comsucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {
            // Fail fast validations
            // Incluir validações no command
            //command.Validate();
            //if (command.Invalid)
            //{
            //    AddNotifications(command);
            //    return new CommandResult(false, "Não foi possível realizar seu cadsatro");
            //}

            // Verificar se Documento já está cadsatrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verificar se E-mail já está cadastro
            if (_repository.DocumentExists(command.Email))
                AddNotification("Document", "Este E-mail já está em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // Gerar as Entidades
            var student = new Student(name, document, email);
            var subscrition = new Subscription(DateTime.Now.AddMonths(1));

            // Só muda a implementação do Pagamento
            var payment = new PayPalPayment(
                command.TransctionCode,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email);

            // Relacionamentos
            subscrition.AddPayment(payment);
            student.AddSubscription(subscrition);

            // Agrupar as Validações
            AddNotifications(name, document, email, address, student, subscrition, payment);

            // Checar as notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            // Salvar as Informações
            _repository.CreateSubscription(student);

            // Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ", "Sua assinatura foi criada");

            // Retornar informações
            return new CommandResult(true, "Assinatura realizada comsucesso");
        }

        public ICommandResult Handle(CreateCreditCardSubscriptionCommand command)
        {
            // Fail fast validations
            // Incluir validações no command
            //command.Validate();
            //if (command.Invalid)
            //{
            //    AddNotifications(command);
            //    return new CommandResult(false, "Não foi possível realizar seu cadsatro");
            //}

            // Verificar se Documento já está cadsatrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("Document", "Este CPF já está em uso");

            // Verificar se E-mail já está cadastro
            if (_repository.DocumentExists(command.Email))
                AddNotification("Document", "Este E-mail já está em uso");

            // Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);

            // Gerar as Entidades
            var student = new Student(name, document, email);
            var subscrition = new Subscription(DateTime.Now.AddMonths(1));

            // Só muda a implementação do Pagamento
            var payment = new CreditCardPayment(
                command.CardHolderName,
                command.CardNumber,
                command.LastTransactionNumber,
                command.PaidDate,
                command.ExpireDate,
                command.Total,
                command.TotalPaid,
                command.Payer,
                new Document(command.PayerDocument, command.PayerDocumentType),
                address,
                email);

            // Relacionamentos
            subscrition.AddPayment(payment);
            student.AddSubscription(subscrition);

            // Agrupar as Validações
            AddNotifications(name, document, email, address, student, subscrition, payment);

            // Checar as notificações
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            // Salvar as Informações
            _repository.CreateSubscription(student);

            // Enviar E-mail de boas vindas
            _emailService.Send(student.Name.ToString(), student.Email.Address, "Bem vindo ", "Sua assinatura foi criada");

            // Retornar informações
            return new CommandResult(true, "Assinatura realizada comsucesso");
        }
    }
}
