using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class ProprietarioImovelRepository : RepositoryN<ProprietarioImovel>, IProprietarioImovelRepository
    {
        public ProprietarioImovelRepository(PraticoContext context) : base(context) { }
    }
}
