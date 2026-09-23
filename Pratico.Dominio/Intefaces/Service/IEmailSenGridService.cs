using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IEmailSenGridService
    {
        Task EnviaEmail(string emialDestinatario, string nome, string codigoRetirada, string numero);
        bool ValidaEnderecoEmail(string email);
        Task EnviaEmailConfirmacaoRetirada(string emialDestinatario, string nome, string numero, string produto, string terceiro, string rg);
        Task EnviaEmailVacanciaReserva(string emialDestinatario, string nome, string local, DateTime dataReserva);
    }
}
