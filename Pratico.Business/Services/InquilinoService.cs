using Pratico.Dominio.Enums;
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
    public class InquilinoService : BaseService, IInquilinoService
    {
        private readonly IInquilinoRepository _inquilinoRepository;
        private readonly IInquilinoImovelRepository _inquilinoImovelRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IImovelRepository _imovelRepository;
        private readonly IPessoaRepository _pessoaRepository;
        public InquilinoService(IInquilinoRepository inquilinoRepository, 
                                IInquilinoImovelRepository inquilinoImovelRepository,
                                IImovelRepository imovelRepository,
                                IEnderecoRepository enderecoRepository,
                                IPessoaRepository pessoaRepository,
                                INotificador notificador) : base(notificador)
        {
            _inquilinoRepository = inquilinoRepository;
            _inquilinoImovelRepository = inquilinoImovelRepository;
            _enderecoRepository = enderecoRepository;
            _imovelRepository = imovelRepository;
            _pessoaRepository = pessoaRepository;
        }

        public async Task<Inquilino> Adicionar(Inquilino inquilino, Endereco endereco, Guid imovelId)
        {
            if (inquilino.Cnpj != null && inquilino.Cnpj.Length > 10)
            {
                inquilino.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                inquilino.TipoPessoa = TipoPessoa.Inquilino;
            }
            else
            {
                inquilino.Cpf = inquilino.Cpf.Replace(".", "").Replace("-", "");
                inquilino.TipoPessoa = TipoPessoa.Inquilino;
            }
            
            inquilino = await _inquilinoRepository.Adicionar(inquilino);
            var list = await _inquilinoImovelRepository.Buscar(x => x.ImovelId == imovelId);

            foreach (var item in list)
            {
                item.Ativo = false;
                await _inquilinoImovelRepository.Atualizar(item);
            }

            endereco.Entidade = inquilino.Id;
            await _enderecoRepository.Adicionar(endereco);

            InquilinoImovel inquilinoImovel = new InquilinoImovel
            {
                Ativo = true,
                ImovelId = imovelId,
                PessoaId = inquilino.Id,
            };

            await _inquilinoImovelRepository.Adicionar(inquilinoImovel);
            return inquilino;
        }

        public async Task<ProprietarioEndereco> ObterInquilinoEndereco(Guid id)
        {
            ProprietarioEndereco propEndereco = null;
            try
            {
                var imovel = await _imovelRepository.ObterPorId(id);
                if (imovel != null)
                {
                    var pessoas = await _pessoaRepository.Buscar(x => x.Id == imovel.Proprietario.Value && x.TipoPessoa == TipoPessoa.Inquilino);
                    if (pessoas.ToList().Count > 0)
                    {
                        var pessoa = pessoas.ToList()[0];
                        var endereco = await _enderecoRepository.ObterEnderecoPorCondominio(pessoa.Id);
                        if (pessoas.ToList().Count > 0 && endereco != null)
                        {
                            propEndereco = new ProprietarioEndereco
                            {
                                //AssinaDoc = pessoa
                                //Cargo =
                                Cnpj = pessoa.Cnpj,
                                Cpf = pessoa.Cpf,
                                DataNascimento = pessoa.DataNascimento.ToString("yyyy-MM-dd"),
                                EstadoCivil = (int)pessoa.EstadoCivil,
                                //FimPeriodo =
                                Id = pessoa.Id,
                                ImovelId = imovel.Id,
                                InscEstadual = pessoa.InscEstadual,
                                InscMunicipal = pessoa.InscMunicipal,
                                Nome = pessoa.Nome,
                                NomeFantasia = pessoa.NomeFantasia,
                                //NumDoc =
                                RazaoSocial = pessoa.Nome,
                                Sexo = (int)pessoa.Sexo,
                                //TipoDocumento = 
                                //FimPeriodo =
                                //IniPeriodo =
                                Bairro = endereco.Bairro,
                                Cep = endereco.Cep,
                                Cidade = endereco.Cidade,
                                Cobranca = endereco.Cobranca,
                                Complemento = endereco.Complemento,
                                Logradouro = endereco.Logradouro,
                                Entidade = endereco.Entidade,
                                Numero = endereco.Numero,
                                Referencia = endereco.Referencia,
                                UF = endereco.UF
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            return propEndereco;
        }

        public void Dispose()
        {
            _inquilinoRepository?.Dispose();
        }
    }
}
