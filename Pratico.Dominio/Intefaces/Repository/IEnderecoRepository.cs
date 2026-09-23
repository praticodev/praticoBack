using Pratico.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<Endereco> ObterEnderecoPorCondominio(Guid condominio);
        Task<Endereco> ObterEnderecoPorProprietario(Guid proprietario);
    }
}
