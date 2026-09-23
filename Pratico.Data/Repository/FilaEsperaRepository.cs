using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class FilaEsperaRepository : Repository<FilaEspera>, IFilaEsperaRepository
    {
        public FilaEsperaRepository(PraticoContext context) : base(context) { }
    }
}
