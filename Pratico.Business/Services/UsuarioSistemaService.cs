using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class UsuarioSistemaService : BaseService, IUsuarioSistemaService
    {
        private readonly IUsuarioSistemaRepository _moradorSistemaRepository;

        public UsuarioSistemaService(IUsuarioSistemaRepository moradorSistemaRepository, INotificador notificador) : base(notificador)
        {
            _moradorSistemaRepository = moradorSistemaRepository;
        }

        public async Task<UsuarioSistema> Adicionar(UsuarioSistema usuario)
        {
            return await _moradorSistemaRepository.Adicionar(usuario);
        }

        public async Task<UsuarioSistema> Buscar(string usuarioId, Guid CondominioId)
        {
            Guid id = new Guid(usuarioId);
            var result = await _moradorSistemaRepository.Buscar(x => x.UsuarioId == id && x.CondominioId == CondominioId);
            return result.FirstOrDefault();
        }
    }
}
