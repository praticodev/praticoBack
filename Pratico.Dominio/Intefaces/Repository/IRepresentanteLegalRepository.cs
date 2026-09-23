using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IRepresentanteLegalRepository : IRepository<RepresentanteLegal>
    {
        Task<RepresentanteLegal> ObterRepresentantePorCondominio(Guid id);
        Task<RepresentanteLegal> ObterRepresentantePorCpf(string cpf);
        Task<RepresentanteLegal> ObterRepresentantePorCnpj(string cnpj);
    }
}
