using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Repository
{
    public class TipoAreaLazerRepository : Repository<TipoAreaLazer>, ITipoAreaLazerRepository
    {
        public TipoAreaLazerRepository(PraticoContext context) : base(context) { }
    }
}
