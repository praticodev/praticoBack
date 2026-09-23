using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Validations;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class AndarService : BaseService, IAndarService
    {
        private readonly IAndarRepository _andarRepository;
        private readonly IImovelRepository _imovelRepository;

        public AndarService(IAndarRepository andarRepository,
                              IImovelRepository imovelRepository,
                              INotificador notificador) : base(notificador)
        {
            _andarRepository = andarRepository;
            _imovelRepository = imovelRepository;
        }

        public async Task<string> Imove(Guid id)
        {
            var imovel = await _imovelRepository.ImovelCondominio(id);
            var andar = await _andarRepository.ObterPorId(imovel.AndarId);
            return imovel.Andar.Conjunto.Condominio.NomeFantasia;

        }
        public async Task<Pratico.Dominio.Model.Andar> Adicionar(Pratico.Dominio.Model.Andar andar)
        {
            if (!ExecutarValidacao(new AndarValidation(), andar))
                return null;

            foreach (var item in andar.Imoveis)
            {
                GerenciamentoDocImovel doc = new GerenciamentoDocImovel()
                {
                    DocEnviado = false,
                    EmailEnviado = false,
                    DocValidado = false,
                    EmailLido = false
                };
                item.DocImovel = doc;
            }

            var result = await _andarRepository.Adicionar(andar);

            return result;
        }

        public async Task<bool> Atualizar(Pratico.Dominio.Model.Andar andar)
        {
            if (!ExecutarValidacao(new AndarValidation(), andar)) return false;

            var list = await _imovelRepository.ObterImoveisPorAndar(andar.Id);

            if (list.ToList().Count > 0)
                await RemoverImoveisPorAndar(list);

            await _andarRepository.Atualizar(andar);
            return true;
        }

        private async Task RemoverImoveisPorAndar(IEnumerable<Imovel> imoveis)
        {
            await _imovelRepository.RemoverImoveisPorAndar(imoveis);
        }

        public async Task Remover(Guid id)
        {
            await _andarRepository.Remover(id);
        }
        public void Dispose()
        {
            _andarRepository?.Dispose();
        }
    }
}
