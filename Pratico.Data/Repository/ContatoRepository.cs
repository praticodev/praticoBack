using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class ContatoRepository : Repository<Contato>, IContatoRepository
    {
        public ContatoRepository(PraticoContext context) : base(context) { }

        public async Task<Contato> ObterContatoPorDados(Contato contato)
        {
            return await Db.Contato.AsNoTracking().FirstOrDefaultAsync(x => x.Email == contato.Email && x.Ddd == contato.Ddd && x.Telefone ==  contato.Telefone && x.Entidade == contato.Entidade);
        }

        public async Task<Contato> ObterContatoPorEntidade(Guid contato)
        {
            return await Db.Contato.AsNoTracking().FirstOrDefaultAsync(x => x.Entidade == contato);
        }
    }
}
