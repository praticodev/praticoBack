using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Pratico.Business.Services
{
    public class PessoaUsuarioService : IPessoaUsuarioService
    {
        private readonly IPessoaUsuarioRepository _pessoaUsuario;
        private readonly ICondominioRepository _condominioRepository;

        public PessoaUsuarioService(IPessoaUsuarioRepository pessoaUsuario, ICondominioRepository condominioRepository)
        {
            _pessoaUsuario = pessoaUsuario;
            _condominioRepository = condominioRepository;
        }

        public Task<PessoaUsuario> Adicionar(PessoaUsuario usuario)
        {
            try
            {
                return _pessoaUsuario.Adicionar(usuario);
            }
            catch (Exception e)
            {

                throw e;
            };
        }

        public void Dispose()
        {
            _pessoaUsuario.Dispose();
        }

        public async Task<PessoaUsuario> ObterPorUsuario(string user, int codCondominio)
        {
            Guid id = new Guid(user);
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            var result = await _pessoaUsuario.Buscar(x => x.UsuarioId == id && x.CondominioId == condominio.Id);
            if (result != null)
                return result.FirstOrDefault();

            return new PessoaUsuario();
        }

        public async Task<PessoaUsuario> ObterPorPessoa(Guid id, Guid condominioId)
        {
            var result = await _pessoaUsuario.Buscar(x => x.PessoaId == id && x.CondominioId == condominioId);
            if (result != null)
                return result.FirstOrDefault();

            return new PessoaUsuario();
        }
    }
}
