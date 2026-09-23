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
    public class PrestadorServicoService : IPrestadorServicoService
    {
        private readonly IPrestadorServicoRepository _prestadorServicoRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly ISyncLogRepository _syncLogRepository;

        public PrestadorServicoService(IPrestadorServicoRepository prestadorServicoRepository,
                                       ICondominioRepository condominioRepository,
                                       ISyncLogRepository syncLogRepository)
        {
            _prestadorServicoRepository = prestadorServicoRepository;
            _condominioRepository = condominioRepository;
            _syncLogRepository = syncLogRepository;
        }

        public async Task<IEnumerable<TipoPrestadorServico>> ListarPrestadores()
        {
            var prestadores = await _prestadorServicoRepository.ObterTodos();

            return prestadores
                .OrderBy(x => x.Descricao == "Outros")
                .ThenBy(x => x.Descricao);
        }

        public async Task<PrestadorServico> Adicionar(PrestadorServico prestador, IFormFile imagem, int codCondominio)
        {
            if (codCondominio == 0)
                return null;

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return null;

            prestador.CondominioId = condominio.Id;

            if (imagem != null)
                prestador.Imagem = await InsereBlobImagem(imagem);

            var prestadorGravado = await _prestadorServicoRepository.AdicionarPrestador(prestador);

            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = condominio.Id,
                ImovelId = prestador.ImovelId,
                Entidade = "PrestadorServico",
                RegistroId = prestador.Id,
                Operacao = "INSERT"
            });

            return prestadorGravado;
        }

        public async Task<PrestadorServico> Atualizar(PrestadorServico prestador, IFormFile imagem, int codCondominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return null;

            var prestadorAtual = await _prestadorServicoRepository.ObterPrestadorPorId(prestador.Id, condominio.Id);
            if (prestadorAtual == null)
                return null;

            if (imagem != null)
            {
                if (!string.IsNullOrWhiteSpace(prestadorAtual.Imagem))
                    await S3Service.DeleteIfExistsAsync(prestadorAtual.Imagem, "prestadores");

                prestador.Imagem = await InsereBlobImagem(imagem);
            }
            else
            {
                prestador.Imagem = prestadorAtual.Imagem;
            }

            prestador.CondominioId = condominio.Id;
            prestador.DataCadastro = prestadorAtual.DataCadastro;

            await _prestadorServicoRepository.AtualizarPrestador(prestador);
            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = condominio.Id,
                ImovelId = prestador.ImovelId,
                Entidade = "PrestadorServico",
                RegistroId = prestador.Id,
                Operacao = "UPDATE"
            });

            return prestador;
        }

        public async Task<bool> Remover(Guid id, int codCondominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return false;

            var prestador = await _prestadorServicoRepository.ObterPrestadorPorId(id, condominio.Id);
            if (prestador == null)
                return false;

            if (!string.IsNullOrWhiteSpace(prestador.Imagem))
                await S3Service.DeleteIfExistsAsync(prestador.Imagem, "prestadores");

            await _prestadorServicoRepository.RemoverPrestador(id);
            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = condominio.Id,
                ImovelId = prestador.ImovelId,
                Entidade = "PrestadorServico",
                RegistroId = id,
                Operacao = "DELETE"
            });

            return true;
        }

        public async Task<IEnumerable<PrestadorServico>> ObterPorUnidade(Guid imovelId, int codCondominio)
        {
            if (imovelId == Guid.Empty || codCondominio == 0)
                return Enumerable.Empty<PrestadorServico>();

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<PrestadorServico>();

            return await _prestadorServicoRepository.ObterPorUnidade(imovelId, condominio.Id);
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
                return await S3Service.UploadAsync(stream, imagem.FileName, imagem.ContentType, true, "prestadores");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void Dispose()
        {
            _prestadorServicoRepository.Dispose();
        }
    }
}
