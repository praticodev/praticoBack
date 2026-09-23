using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IImovelRepository : IRepository<Imovel>
    {
        Task<IEnumerable<Imovel>> ObterImoveisPorAndar(Guid andar);
        Task<bool> RemoverImoveisPorAndar(IEnumerable<Imovel> imoveis);
        Task<Imovel> ImovelCondominio(Guid id);
        Task<List<Imovel>> ImoveisProprietario(Guid id, Guid condId);
    }
}
