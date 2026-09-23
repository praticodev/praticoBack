using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using Pratico.Dominio.Validations;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class RepresentanteLegalService : BaseService, IRepresentanteLegalService
    {
        private readonly IRepresentanteLegalRepository _representanteLegalRepository;

        public RepresentanteLegalService(IRepresentanteLegalRepository representanteLegalRepository,
                                 INotificador notificador) : base(notificador)
        {
            _representanteLegalRepository = representanteLegalRepository;
        }

        public async Task<RepresentanteLegal> Adicionar(RepresentanteLegal representante)
        {
            if (!representante.RazaoSocialRep.Equals(string.Empty))
            {
                if (!ExecutarValidacao(new RepresentanteLegalJuridicaValidation(), representante))
                    return null;
            }
            else
            {
                if (!ExecutarValidacao(new RepresentanteLegalValidation(), representante))
                    return null;
            }

            return await _representanteLegalRepository.Adicionar(representante);
        }

        public async Task<bool> Atualizar(RepresentanteLegal representante)
        {
            if (!ExecutarValidacao(new RepresentanteLegalValidation(), representante)) return false;

            var existe = await _representanteLegalRepository.ObterRepresentantePorCondominio(representante.CondominioId);
            if (existe != null)
            {
                await _representanteLegalRepository.Atualizar(representante);
                return true;
            }
            else
            {
                var novo = await _representanteLegalRepository.Adicionar(representante);
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            _representanteLegalRepository?.Dispose();
        }
    }
}
