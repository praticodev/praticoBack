using Microsoft.AspNetCore.Http;
using Pratico.Business.Utils;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class VisitanteImovelService : IVisitanteImovelService
    {
        private readonly IVisitanteImovelRepository _visitanteImovelRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly ISyncLogRepository _syncLogRepository;

        public VisitanteImovelService(IVisitanteImovelRepository visitanteImovelRepository,
                                      ICondominioRepository condominioRepository,
                                      ISyncLogRepository syncLogRepository)
        {
            _visitanteImovelRepository = visitanteImovelRepository;
            _condominioRepository = condominioRepository;
            _syncLogRepository = syncLogRepository;
        }

        public async Task<VisitanteImovel> Adicionar(VisitanteImovel visitante, IFormFile imagem, int codCondominio)
        {
            if (codCondominio == 0)
                return null;

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return null;

            visitante.CondominioId = condominio.Id;

            if (imagem != null)
                visitante.Imagem = await InsereBlobImagem(imagem);

            var visitanteGravado = await _visitanteImovelRepository.Adicionar(visitante);

            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = condominio.Id,
                ImovelId = visitante.ImovelId,
                Entidade = "VisitanteImovel",
                RegistroId = visitante.Id,
                Operacao = "INSERT"
            });

            return visitanteGravado;
        }

        public async Task<VisitanteImovel> Atualizar(VisitanteImovel visitante, IFormFile imagem, int codCondominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return null;

            var visitanteAtual = await _visitanteImovelRepository.ObterVisitantePorId(visitante.Id, condominio.Id);
            if (visitanteAtual == null)
                return null;

            if (imagem != null)
            {
                if (!string.IsNullOrWhiteSpace(visitanteAtual.Imagem))
                    await S3Service.DeleteIfExistsAsync(visitanteAtual.Imagem, "visitantes");

                visitante.Imagem = await InsereBlobImagem(imagem);
            }
            else
            {
                visitante.Imagem = visitanteAtual.Imagem;
            }

            visitante.CondominioId = condominio.Id;
            visitante.DataCadastro = visitanteAtual.DataCadastro;

            await _visitanteImovelRepository.Atualizar(visitante);
            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = condominio.Id,
                ImovelId = visitante.ImovelId,
                Entidade = "VisitanteImovel",
                RegistroId = visitante.Id,
                Operacao = "UPDATE"
            });

            return visitante;
        }

        public async Task<bool> Remover(Guid id, int codCondominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return false;

            var visitante = await _visitanteImovelRepository.ObterVisitantePorId(id, condominio.Id);
            if (visitante == null)
                return false;

            if (!string.IsNullOrWhiteSpace(visitante.Imagem))
                await S3Service.DeleteIfExistsAsync(visitante.Imagem, "visitantes");

            await _visitanteImovelRepository.Remover(id);
            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = condominio.Id,
                ImovelId = visitante.ImovelId,
                Entidade = "VisitanteImovel",
                RegistroId = id,
                Operacao = "DELETE"
            });

            return true;
        }

        public async Task<IEnumerable<VisitanteImovel>> ObterPorUnidade(Guid imovelId, int codCondominio)
        {
            if (imovelId == Guid.Empty || codCondominio == 0)
                return Enumerable.Empty<VisitanteImovel>();

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<VisitanteImovel>();

            return await _visitanteImovelRepository.ObterPorUnidade(imovelId, condominio.Id);
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
                return await S3Service.UploadAsync(stream, imagem.FileName, imagem.ContentType, true, "visitantes");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _visitanteImovelRepository.Dispose();
        }
    }
}
