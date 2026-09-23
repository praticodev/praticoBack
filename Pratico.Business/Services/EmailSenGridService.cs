using Microsoft.Extensions.Configuration;
using Pratico.Business.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pratico.Dominio.Intefaces.Service;

namespace Pratico.Business.Services
{
    public class EmailSenGridService : IEmailSenGridService
    {
        private readonly IConfiguration _configuration;

        public EmailSenGridService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviaEmail(string emialDestinatario, string nome, string codigoRetirada, string numero)
        {
            var appSettingsSection = _configuration.GetSection("SendGrid");
            var sendGridSettings = appSettingsSection.Get<SenGridConfiguration>();
            var client = new SendGridClient(sendGridSettings.Key);
            var from = new EmailAddress(sendGridSettings.EmailBase, "Prático Codomínios");
            var subject = "Sua nova encomenda espera por você";
            var to = new EmailAddress(emialDestinatario, nome);
            var plainTextContent = "Prático Codomínios";
            var htmlContent = "<p>Número da sua remessa: <strong>" + numero + "</strong></p><p>Use o código <strong>" + codigoRetirada + "</strong> para retirar sua encomenda.</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            try
            {
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task EnviaEmailConfirmacaoRetirada(string emialDestinatario, string nome, string numero, string produto, string terceiro, string rg)
        {
            var appSettingsSection = _configuration.GetSection("SendGrid");
            var sendGridSettings = appSettingsSection.Get<SenGridConfiguration>();
            var client = new SendGridClient(sendGridSettings.Key);
            var from = new EmailAddress(sendGridSettings.EmailBase, "Prático Codomínios");
            var subject = "Item da sua remessa foi retirado";
            var to = new EmailAddress(emialDestinatario, nome);
            var plainTextContent = "Prático Codomínios";
            StringBuilder sb = new StringBuilder();
            sb.Append("<p>O item: <strong>" + produto + "</strong> da remessa número: " + numero + ".</p>");
            if (!terceiro.Equals(string.Empty))
            {
                sb.Append("<p>Foi retirado por: " + terceiro + ". - RG.: " + rg +".</p>");
            }
            else
            {
                sb.Append("<p>Foi retirado.</p>");
            }
            sb.Append("<p>Data da retirada:" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " H.</p>");
            
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, sb.ToString());
            var response = await client.SendEmailAsync(msg);
        }

        public async Task EnviaEmailVacanciaReserva(string emialDestinatario, string nome, string local, DateTime dataReserva)
        {
            var appSettingsSection = _configuration.GetSection("SendGrid");
            var sendGridSettings = appSettingsSection.Get<SenGridConfiguration>();
            var client = new SendGridClient(sendGridSettings.Key);
            var from = new EmailAddress(sendGridSettings.EmailBase, "Prático Codomínios");
            var subject = "Data para reserva da área de lazer está disponível";
            var to = new EmailAddress(emialDestinatario, nome);
            var plainTextContent = "Prático Codomínios";
            StringBuilder sb = new StringBuilder();
            sb.Append("<p>O local de lazer: " + local + " para a data: " + dataReserva.ToString("dd/MM/yyyy") + " está disponível</p>");
            sb.Append("<p>Acesse o sistema e faça a sua reserva.</p>");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, sb.ToString());
            var response = await client.SendEmailAsync(msg);
        }

        public bool ValidaEnderecoEmail(string email)
        {
            try
            {
                //define a expressão regulara para validar o email
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                // testa o email com a expressão
                if (expressaoRegex.IsMatch(email))
                {
                    // o email é valido
                    return true;
                }
                else
                {
                    // o email é inválido
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
