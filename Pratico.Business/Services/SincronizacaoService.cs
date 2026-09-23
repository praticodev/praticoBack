using Pratico.Dominio.Intefaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class SincronizacaoService
    {
        private readonly IImovelRepository _imovelRepository;
        private readonly IMoradorImovelRepository _moradorImovelRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IPessoaUsuarioRepository _pessoaUsuarioRepository;
        private readonly IUsuarioCondominioRepository _usuarioCondominioRepository;
        private readonly IVeiculoRepository _veiculoRepository;

        public SincronizacaoService(IImovelRepository imovelRepository, 
            IMoradorImovelRepository moradorImovelRepository, 
            IPessoaRepository pessoaRepository, 
            IContatoRepository contatoRepository, 
            ICondominioRepository condominioRepository, 
            IPessoaUsuarioRepository pessoaUsuarioRepository, 
            IUsuarioCondominioRepository usuarioCondominioRepository, 
            IVeiculoRepository veiculoRepository)
        {
            _imovelRepository = imovelRepository;
            _moradorImovelRepository = moradorImovelRepository;
            _pessoaRepository = pessoaRepository;
            _contatoRepository = contatoRepository;
            _condominioRepository = condominioRepository;
            _pessoaUsuarioRepository = pessoaUsuarioRepository;
            _usuarioCondominioRepository = usuarioCondominioRepository;
            _veiculoRepository = veiculoRepository;
        }


    }
}
