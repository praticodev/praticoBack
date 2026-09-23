using Microsoft.AspNetCore.Http;
using Pratico.Business.Utils;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class VeiculoService : IVeiculoService
    {
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly ISyncLogRepository _syncLogRepository;

        public VeiculoService(IVeiculoRepository veiculoRepository, 
                              ICondominioRepository condominioRepository,
                              ISyncLogRepository syncLogRepository)
        {
            _veiculoRepository = veiculoRepository;
            _condominioRepository = condominioRepository;
            _syncLogRepository = syncLogRepository;
        }

        public async Task<Veiculo> Adicionar(Veiculo veiculo, IFormFile? imagem, int codCondominio)
        {
            try
            {
                if (codCondominio == 0)
                    return null;
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
                veiculo.CondominioId = condominio.Id;
                if (imagem != null)
                {
                    var imagemNuvem = await InsereBlobImagem(imagem);
                    veiculo.Imagem = imagemNuvem;
                }
                veiculo =  await _veiculoRepository.Adicionar(veiculo);
                await _syncLogRepository.Adicionar(new SyncLog()
                {
                    CondominioId = condominio.Id,
                    ImovelId = veiculo.ImovelId,
                    Entidade = "Veiculo",
                    RegistroId = veiculo.Id,
                    Operacao = "INSERT"
                });
                return veiculo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Veiculo> Atualizar(Veiculo veiculo, IFormFile? imagem, int codCondominio)
        {
            try
            {   
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
                if(condominio != null)
                {
                    var veiculoAtual = (await _veiculoRepository.Buscar(x => x.Id == veiculo.Id && x.CondominioId == condominio.Id)).FirstOrDefault();
                    if (veiculoAtual != null)
                    {
                        if (imagem != null)
                        {
                            if (!string.IsNullOrWhiteSpace(veiculoAtual.Imagem))
                                await S3Service.DeleteIfExistsAsync(veiculoAtual.Imagem, "veiculos");

                            veiculo.Imagem = await InsereBlobImagem(imagem);
                        }
                        veiculo.CondominioId = condominio.Id;
                        veiculo.DataCadastro = veiculoAtual.DataCadastro;
                        await _veiculoRepository.Atualizar(veiculo);
                        await _syncLogRepository.Adicionar(new SyncLog()
                        {
                            CondominioId = condominio.Id,
                            ImovelId = veiculo.ImovelId,
                            Entidade = "Veiculo",
                            RegistroId = veiculo.Id,
                            Operacao = "UPDATE"
                        });
                        return veiculo;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> Remover(Guid id, int codCondominio)
        {
            try
            {
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
                if (condominio == null)
                    return false;

                if(condominio != null)
                {
                    var veiculo = await _veiculoRepository.Buscar(x => x.Id == id && x.CondominioId == condominio.Id);
                    if (veiculo.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(veiculo.FirstOrDefault().Imagem))
                            await S3Service.DeleteIfExistsAsync(veiculo.FirstOrDefault().Imagem, "veiculos");

                        await _veiculoRepository.RemoverTodos(veiculo);
                        await _syncLogRepository.Adicionar(new SyncLog()
                        {
                            CondominioId = condominio.Id,
                            ImovelId = veiculo.FirstOrDefault().ImovelId,
                            Entidade = "Veiculo",
                            RegistroId = id,
                            Operacao = "DELETE"
                        });
                    }
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Veiculo>> ObterPorUnidade(Guid imovelId, int codCondominio)
        {
            try
            {
                if (imovelId == Guid.Empty || codCondominio == 0)
                    return Enumerable.Empty<Veiculo>();

                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
                if (condominio == null)
                    return Enumerable.Empty<Veiculo>();

                return await _veiculoRepository.ObterPorUnidade(imovelId, condominio.Id);
            }
            catch (Exception)
            {
                return Enumerable.Empty<Veiculo>();
            }
        }

        private async Task<string> InsereBlobImagem(IFormFile imagem)
        {
            if (imagem == null || imagem.Length == 0)
                return null;

            if (!imagem.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return null;

            try
            {
                using var stream = imagem.OpenReadStream();
                var t = await S3Service.UploadAsync(stream, imagem.FileName, imagem.ContentType, true, "veiculos");
                return t;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _veiculoRepository.Dispose();
        }
    }
}
