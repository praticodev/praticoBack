using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using Pratico.Dominio.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class EnderecoService : BaseService, IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoService(IEnderecoRepository enderecoRepository,
                                 INotificador notificador) : base(notificador)
        {
            _enderecoRepository = enderecoRepository;
        }

        public async Task<Endereco> Adicionar(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco))
                return null;

            var existe = await ObterEnderecoPorCondominio(endereco.Entidade);

            if(existe != null)
                return null; ;

            return await _enderecoRepository.Adicionar(endereco);
        }

        public async Task<bool> Atualizar(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco)) return false;
            var existe = await ObterEnderecoPorCondominio(endereco.Entidade);
            if (existe != null)
                await _enderecoRepository.Atualizar(endereco);
            else
                await Adicionar(endereco);
            return true;
        }

        public async Task<Endereco> ObterEnderecoPorCondominio(Guid condominio)
        {
            return await _enderecoRepository.ObterEnderecoPorCondominio(condominio);
        }
    }
}
