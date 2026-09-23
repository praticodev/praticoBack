using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(PraticoContext context) : base(context) { }

        public async Task<Endereco> ObterEnderecoPorCondominio(Guid condominio)
        {
            return await Db.Endereco.AsNoTracking().FirstOrDefaultAsync(x => x.Entidade == condominio);
        }

        public async Task<Endereco> ObterEnderecoPorProprietario(Guid proprietario)
        {
            return await Db.Endereco.AsNoTracking().FirstOrDefaultAsync(x => x.Entidade == proprietario);
        }
    }
}
