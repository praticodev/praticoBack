using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IEmailService
    {
        Task<string> EnviaLinkProprietario(Guid IdImovel, string nome, string email);
        Task<string> EnviaCodigoRedefinicaoSenha(string destinatario, string codigo);
        Task<string> EnviaEmailConviteApp(Guid idMorador, int CodCondominio);
        bool ValidaEnderecoEmail(string email);
    }
}
