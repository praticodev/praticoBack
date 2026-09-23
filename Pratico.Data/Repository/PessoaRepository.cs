using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class PessoaRepository : Repository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<Morador>> ObterMoradoresAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token)
        {
            var moradores = await Db.Pessoa
                .AsNoTracking()
                .Where(pessoa =>
                    pessoa.CondominioId == condominioId &&
                    Db.MoradorImovel
                        .AsNoTracking()
                        .Any(moradorImovel =>
                            moradorImovel.PessoaId == pessoa.Id &&
                            moradorImovel.CondominioId == condominioId &&
                            moradorImovel.ImovelId == imovelId) &&
                    Db.SyncLog
                        .AsNoTracking()
                        .Any(log =>
                            log.CondominioId == condominioId &&
                            log.ImovelId == imovelId &&
                            log.RegistroId == pessoa.Id &&
                            log.Entidade == nameof(Morador) &&
                            log.Operacao != "DELETE" &&
                            log.Token > token))
                .Select(pessoa => new
                {
                    pessoa.Id,
                    pessoa.Nome,
                    pessoa.NomeFantasia,
                    pessoa.Cpf,
                    pessoa.Rg,
                    pessoa.EstadoCivil,
                    pessoa.Sexo,
                    pessoa.Imagem,
                    pessoa.DataNascimento,
                    ImovelId = Db.MoradorImovel
                        .AsNoTracking()
                        .Where(moradorImovel =>
                            moradorImovel.PessoaId == pessoa.Id &&
                            moradorImovel.CondominioId == condominioId &&
                            moradorImovel.ImovelId == imovelId)
                        .Select(moradorImovel => (Guid?)moradorImovel.ImovelId)
                        .FirstOrDefault(),
                    Email = Db.Contato
                        .AsNoTracking()
                        .Where(contato =>
                            contato.Entidade == pessoa.Id &&
                            contato.CondominioId == condominioId)
                        .OrderByDescending(contato => contato.Principal)
                        .Select(contato => contato.Email)
                        .FirstOrDefault(),
                    Ddd = Db.Contato
                        .AsNoTracking()
                        .Where(contato =>
                            contato.Entidade == pessoa.Id &&
                            contato.CondominioId == condominioId)
                        .OrderByDescending(contato => contato.Principal)
                        .Select(contato => (int?)contato.Ddd)
                        .FirstOrDefault(),
                    Telefone = Db.Contato
                        .AsNoTracking()
                        .Where(contato =>
                            contato.Entidade == pessoa.Id &&
                            contato.CondominioId == condominioId)
                        .OrderByDescending(contato => contato.Principal)
                        .Select(contato => contato.Telefone)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return moradores.Select(morador => new Morador
            {
                Ativo = true,
                Nome = morador.Nome,
                NomeFantasia = morador.NomeFantasia,
                Id = morador.Id,
                ImovelId = morador.ImovelId ?? Guid.Empty,
                Email = morador.Email,
                Telefone = morador.Ddd.HasValue ? $"({morador.Ddd}){morador.Telefone}" : null,
                Sexo = morador.Sexo,
                Cpf = morador.Cpf,
                Rg = morador.Rg,
                EstadoCivil = morador.EstadoCivil,
                Imagem = morador.Imagem,
                DataNascimento = morador.DataNascimento,
            });
        }
    }
}
