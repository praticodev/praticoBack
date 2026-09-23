using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IContatoRepository _contatoRepository;

        public PessoaService(IPessoaRepository pessoaRepository, IContatoRepository contatoRepository)
        {
            _pessoaRepository = pessoaRepository;
            _contatoRepository = contatoRepository;
        }

        public async Task<Pessoa> Adicionar(Pessoa pessoa)
        {
            return await _pessoaRepository.Adicionar(pessoa);
        }

        public async Task<Pessoa> ObterPorEmail(string email, Guid condominioId)
        {
            var listaEmailBanco = await _contatoRepository.Buscar(x => x.Email == email && x.CondominioId == condominioId);
            var emailBanco = listaEmailBanco.FirstOrDefault();
            var pessoas = await _pessoaRepository.Buscar(x => x.Id == emailBanco.Entidade);
            return pessoas.FirstOrDefault();
        }
    }
}
