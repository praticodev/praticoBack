using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IPessoaUsuarioService : IDisposable
    {
        Task<PessoaUsuario> Adicionar(PessoaUsuario usuario);
        Task<PessoaUsuario> ObterPorUsuario(string user, int codCondominio);
        Task<PessoaUsuario> ObterPorPessoa(Guid id, Guid condominioId);
    }
}
