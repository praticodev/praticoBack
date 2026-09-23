using Microsoft.Extensions.Configuration;
using Pratico.Business.Configuration;
using Pratico.Business.Utils;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Pratico.Business.Services
{
    public class EmailService : BaseService, IEmailService
    {
        private IConfiguration _configuration;
        private IDocImovelRepository _docRepository;
        private readonly IImovelRepository _imovelRepository;
        private readonly IContatoRepository _contatoRepository;

        public EmailService(INotificador notificador,
            IDocImovelRepository docRepository,
            IImovelRepository imovelRepository,
            IContatoRepository contatoRepository,
            IConfiguration configuration) : base(notificador)
        {
            _configuration = configuration;
            _docRepository = docRepository;
            _imovelRepository = imovelRepository;
            _contatoRepository = contatoRepository;
        }

        //public async Task<string> EnviaLinkProprietario(Guid idImovel, string nome, string email)
        //{
        //    try
        //    {
        //        // valida o email
        //        bool bValidaEmail = ValidaEnderecoEmail(email);

        //        // Se o email não é validao retorna uma mensagem
        //        if (bValidaEmail == false)
        //            return "Email do destinatário inválido: " + email;                

        //        using (var client = new SmtpClient())
        //        {
        //            var appSettingsSection = _configuration.GetSection("Email");
        //            var emailSettings = appSettingsSection.Get<EmailConfig>();

        //            client.Port = emailSettings.Porta;
        //            client.Host = emailSettings.Host;

        //            // cria uma mensagem
        //            var imovel = await _imovelRepository.ImovelCondominio(idImovel);
        //            MailMessage mensagemEmail = new MailMessage(
        //                    emailSettings.Usuario,
        //                    email,
        //                    "Envio dos documentos do seu imóvel no condomínio " + imovel.Andar.Conjunto.Condominio.NomeFantasia,
        //                    PreencheDadosProprietarioMensagem(nome, "Parque Nova Suiça"));

        //            client.EnableSsl = true;
        //            NetworkCredential cred = new NetworkCredential(emailSettings.Usuario, emailSettings.Senha);
        //            client.Credentials = cred;

        //            // inclui as credenciais
        //            client.UseDefaultCredentials = false;
        //            mensagemEmail.IsBodyHtml = true;

        //            // envia a mensagem
        //            client.Send(mensagemEmail);
        //            return "Mensagem enviada para " + email + " às " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string erro = ex.InnerException.Message.ToString();
        //        return erro;
        //    }
        //}

        public async Task<string> EnviaEmailConviteApp(Guid idMorador, int CodCondominio)
        {
            try
            {
                string mascara = Criptografia.GerarHashCodCondominio(CodCondominio);
                var contato = await _contatoRepository.ObterContatoPorEntidade(idMorador);
                var mensagem = String.Format("http://localhost:3000/#/register/{0}", mascara);
                Enviar(contato.Email, "Envio de convite", mensagem);
                return "E-mail enviado!";
            }
            catch (Exception e)
            {
                return "Erro!";
            }
        }

        public async Task<string> EnviaLinkProprietario(Guid idImovel, string nome, string email)
        {
            try
            {
                // valida o email
                bool bValidaEmail = ValidaEnderecoEmail(email);//praticodocs

                // Se o email não é validao retorna uma mensagem
                if (bValidaEmail == false)
                    return "Email do destinatário inválido: " + email;

                
                using (var client = new SmtpClient())
                {
                    var appSettingsSection = _configuration.GetSection("Email");
                    var emailSettings = appSettingsSection.Get<EmailConfig>();

                    client.Port = emailSettings.Porta;

                    client.Host = emailSettings.Host;

                    // cria uma mensagem
                    var imovel = await _imovelRepository.ImovelCondominio(idImovel);
                    MailMessage mensagemEmail = new MailMessage(
                            emailSettings.Usuario,
                            email,
                            "Envio dos documentos do seu imóvel no condomínio " + imovel.Andar.Conjunto.Condominio.NomeFantasia,
                            PreencheDadosProprietarioMensagem(nome, "Parque Nova Suiça"));

                    client.EnableSsl = false;
                    NetworkCredential cred = new NetworkCredential(emailSettings.Usuario, emailSettings.Senha);
                    client.Credentials = cred;

                    // inclui as credenciais
                    client.UseDefaultCredentials = false;
                    mensagemEmail.IsBodyHtml = true;

                    // envia a mensagem
                    client.Send(mensagemEmail);
                    return "Mensagem enviada para " + email + " às " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".";
                }
            }
            catch (Exception ex)
            {
                string erro = ex.Message.ToString();
                return erro;
            }
        }

        public async Task<string> EnviaCodigoRedefinicaoSenha(string destinatario, string codigo)
        {
            try
            {
                if (ValidaEnderecoEmail(destinatario) == false)
                {
                    return "Email do destinatário inválido: " + destinatario;
                }

                using (var client = new SmtpClient())
                {
                    var appSettingsSection = _configuration.GetSection("Email");
                    var emailSettings = appSettingsSection.Get<EmailConfig>();

                    client.Port = emailSettings.Porta;
                    client.Host = emailSettings.Host;
                    client.EnableSsl = false;
                    client.Credentials = new NetworkCredential(emailSettings.Usuario, emailSettings.Senha);
                    client.UseDefaultCredentials = false;

                    var mensagemEmail = new MailMessage(
                        emailSettings.Usuario,
                        destinatario,
                        "Código para redefinição de senha",
                        $"Seu código para redefinição de senha é: <strong>{codigo}</strong>");

                    mensagemEmail.IsBodyHtml = true;

                    await client.SendMailAsync(mensagemEmail);
                    return "Mensagem enviada para " + destinatario + " às " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public string PreencheDadosProprietarioMensagem(string nome, string condominio)
        {
            string path = @"Template/EmailImovelProprietario/ImovelProprietario.html";
            string bodyMessage = File.ReadAllText(path);
            bodyMessage = bodyMessage.Replace("[#Nome#]", nome);
            bodyMessage = bodyMessage.Replace("[#Condominio#]", condominio);
            return bodyMessage;
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

        public string EnviaMensagemEmail(string Destinatario, string Remetente, string Assunto, string enviaMensagem)
        {
            try
            {
                // valida o email
                bool bValidaEmail = ValidaEnderecoEmail(Destinatario);

                // Se o email não é validao retorna uma mensagem
                if (bValidaEmail == false)
                    return "Email do destinatário inválido: " + Destinatario;

                // cria uma mensagem
                MailMessage mensagemEmail = new MailMessage(Remetente, Destinatario, Assunto, enviaMensagem);

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                NetworkCredential cred = new NetworkCredential("SEU_EMAIL@gmail.com", "SUA_SENHA");
                client.Credentials = cred;

                // inclui as credenciais
                client.UseDefaultCredentials = true;

                // envia a mensagem
                client.Send(mensagemEmail);

                return "Mensagem enviada para  " + Destinatario + " às " + DateTime.Now.ToString() + ".";
            }
            catch (Exception ex)
            {
                string erro = ex.InnerException.ToString();
                return ex.Message.ToString() + erro;
            }
        }

        private void Enviar(string destinatario, string assunto, string corpo)
        {
            using var mensagem = new MailMessage();

            mensagem.From = new MailAddress("sistema@localhost.com");
            mensagem.To.Add(destinatario);
            mensagem.Subject = assunto;
            mensagem.Body = corpo;

            using var smtp = new SmtpClient("localhost", 1025);

            smtp.EnableSsl = false;

            smtp.Send(mensagem);
        }
    }
}
