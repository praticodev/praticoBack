using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Repository
{
    public class TipoDocumentoEntregaRepository : Repository<TipoDocumentoEntrega>, ITipoDocumentoEntregaRepository
    {
        public TipoDocumentoEntregaRepository(PraticoContext context) : base(context) { }
    }
}
