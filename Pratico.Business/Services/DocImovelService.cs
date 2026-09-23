using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class DocImovelService : BaseService, IDocImovelService
    {
        private readonly IDocImovelRepository _docRepository;
        public DocImovelService(IDocImovelRepository docRepository,
                                 INotificador notificador) : base(notificador)
        {
            _docRepository = docRepository;
        }

        public async Task<bool> Atualizar(Guid id, int etapa)
        {
            var doc = await _docRepository.ObterPorId(id);
            if (doc != null)
            {
                switch (etapa)
                {
                    case 1:
                        doc.DocEnviado = true;
                        break;
                    case 2:
                        doc.EmailEnviado = true;
                        break;
                    case 3:
                        doc.EmailLido = true;
                        break;
                    case 4:
                        doc.DocValidado = true;
                        break;
                }
                await _docRepository.Atualizar(doc);
                return true;
            }
            else
                return false;
        }

        public void Disposible()
        {
            _docRepository?.Dispose();
        }
    }
}
