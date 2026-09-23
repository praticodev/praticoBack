using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Data.Repository
{
    public class PessoaUsuarioRepository : RepositoryN<PessoaUsuario>, IPessoaUsuarioRepository
    {
        public PessoaUsuarioRepository(PraticoContext context) : base(context) { }
    }
}
