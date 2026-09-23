using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IEnderecoService
    {
        Task<Endereco> Adicionar(Endereco condominio);
        Task<Endereco> ObterEnderecoPorCondominio(Guid condominio);
        Task<bool> Atualizar(Endereco endereco);
    }
}
