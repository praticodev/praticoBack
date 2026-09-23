using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IPessoaService
    {
        Task<Pessoa> Adicionar(Pessoa pessoa);
        Task<Pessoa> ObterPorEmail(string email, Guid condominioId);
    }
}
