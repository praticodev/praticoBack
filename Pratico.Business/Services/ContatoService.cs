using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using Pratico.Dominio.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class ContatoService : BaseService, IContatoService
    {
        private readonly IContatoRepository _contatoRepository;
        public ContatoService(IContatoRepository contatoRepository,
                                 INotificador notificador) : base(notificador)
        {
            _contatoRepository = contatoRepository;
        }
        public async Task<Contato> Adicionar(Contato contato)
        {
            if (!ExecutarValidacao(new ContatoValidation(), contato))
                return null;

            var existe = await ObterContatoPorDados(contato);
            if(existe != null)
                return null;

            contato.Principal = true;
            return await _contatoRepository.Adicionar(contato);
        }

        public async Task<Contato> ObterContatoPorDados(Contato contato)
        {
            return await _contatoRepository.ObterContatoPorDados(contato);
        }

        public async Task<bool> Atualizar(Contato contato)
        {
            if (!ExecutarValidacao(new ContatoValidation(), contato)) return false;

            var contatoExistente = contato.Id != Guid.Empty
                ? await _contatoRepository.ObterPorId(contato.Id)
                : null;

            if (contatoExistente == null && contato.Entidade != Guid.Empty)
                contatoExistente = await _contatoRepository.ObterContatoPorEntidade(contato.Entidade);

            if (contatoExistente == null)
                return false;

            contato.Id = contatoExistente.Id;
            contato.DataCadastro = contatoExistente.DataCadastro;

            if (contato.Entidade == Guid.Empty)
                contato.Entidade = contatoExistente.Entidade;

            if (contato.CondominioId == Guid.Empty)
                contato.CondominioId = contatoExistente.CondominioId;

            await _contatoRepository.Atualizar(contato);
            return true;
        }
    }
}
