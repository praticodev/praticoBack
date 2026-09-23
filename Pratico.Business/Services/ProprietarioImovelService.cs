using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class ProprietarioImovelService : BaseService, IProprietarioImovelService
    {
        IProprietarioImovelRepository _proprietarioImovelRepository;
        public ProprietarioImovelService(IProprietarioImovelRepository proprietarioImovelRepository, INotificador notificador) : base(notificador)
        {
            _proprietarioImovelRepository = proprietarioImovelRepository;
        }

        public async Task<Guid> ObterImovelValidado(Guid id)
        {
            var result = await _proprietarioImovelRepository.Buscar(x => x.PessoaId == id && x.Validado == true);
            if (result.ToList().Count > 0)
                return result.ToList().FirstOrDefault().ImovelId;
            
            return new Guid("00000000-0000-0000-0000-000000000000");
        }

        public async Task<Guid> ObterImovelNaoValidado(Guid id)
        {
            var result = await _proprietarioImovelRepository.Buscar(x => x.PessoaId == id && x.Validado == false);
            if (result.ToList().Count > 0)
                return result.ToList().FirstOrDefault().ImovelId;

            return new Guid("00000000-0000-0000-0000-000000000000");
        }

        public async Task<bool> ObterProprietarioValidado(Guid id)
        {
            var result =  await _proprietarioImovelRepository.Buscar(x => x.PessoaId == id);
            return result.Any(x => x.Validado == true);
        }
    }
}
