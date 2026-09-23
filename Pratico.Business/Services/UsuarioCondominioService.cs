using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class UsuarioCondominioService : BaseService, IUsuarioCondominioService
    {
        private readonly IUsuarioCondominioRepository _moradorCondominioRepository;
        private readonly IPessoaRepository _pessoaRepository;

        public UsuarioCondominioService(IUsuarioCondominioRepository moradorCondominioRepository,
                                        IPessoaRepository pessoaRepository,
                                        INotificador notificador) : base(notificador)
        {
            _moradorCondominioRepository = moradorCondominioRepository;
            _pessoaRepository = pessoaRepository;
        }

        public async Task<UsuarioCondominio> Adicionar(UsuarioCondominio morador)
        {
            return await _moradorCondominioRepository.Adicionar(morador);
        }

        public async Task<bool> AdicionarUsuarioImovel(Pessoa morador, PessoaUsuario usuario)
        {
            try
            {
                var pessoa = await _pessoaRepository.Adicionar(morador);
                if (pessoa != null)
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
