using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IContatoService
    {
        Task<Contato> Adicionar(Contato contato);
        Task<Contato> ObterContatoPorDados(Contato contato);
        Task<bool> Atualizar(Contato contato);
    }
}
