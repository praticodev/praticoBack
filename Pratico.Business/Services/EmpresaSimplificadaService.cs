using Pratico.Business.Utils;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class EmpresaSimplificadaService : IEmpresaSimplificadaService
    {
        private readonly IEmpresaSimplificadaRepository _empresaSimplificadaRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly ITipoDocumentoEntregaRepository _tipoDocumentoEntregaRepository;
        private readonly ISyncLogRepository _syncLogRepository;

        public EmpresaSimplificadaService(IEmpresaSimplificadaRepository empresaSimplificadaRepository, 
            ICondominioRepository condominioRepository,
            ISyncLogRepository syncLogRepository,
            ITipoDocumentoEntregaRepository tipoDocumentoEntregaRepository)
        {
            _empresaSimplificadaRepository = empresaSimplificadaRepository;
            _condominioRepository = condominioRepository;
            _tipoDocumentoEntregaRepository = tipoDocumentoEntregaRepository;
            _syncLogRepository = syncLogRepository;
        }

        public async Task<EmpresaSimplificada> Adicionar(EmpresaSimplificada empresa, int codigo)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codigo);
            empresa.Cnpj = empresa.Cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            empresa.CondominioId = condominio.Id;
            var result = await _empresaSimplificadaRepository.Adicionar(empresa);
            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = empresa.CondominioId,
                Entidade = "Transportadora",
                RegistroId = empresa.Id,
                Operacao = "INSERT"
            });
            return result;
        }

        public async Task<EmpresaSimplificada> Atualizar(EmpresaSimplificada empresa, int codigo)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codigo);
            if (condominio == null)
                return null;

            var empresaCadastrada = await _empresaSimplificadaRepository.Buscar(x => x.Id == empresa.Id && x.CondominioId == condominio.Id);
            if (!empresaCadastrada.Any())
                return null;

            empresa.Cnpj = empresa.Cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            empresa.CondominioId = condominio.Id;

            await _empresaSimplificadaRepository.Atualizar(empresa);
            await _syncLogRepository.Adicionar(new SyncLog()
            {
                CondominioId = empresa.CondominioId,
                Entidade = "Transportadora",
                RegistroId = empresa.Id,
                Operacao = "UPDATE"
            });

            return empresa;
        }

        public async Task<bool> VerificaCpnj(string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            var result = await _empresaSimplificadaRepository.Buscar(x => x.Cnpj == cnpj);
            if (result.Any())
                return true;
            return false;
        }

        public async Task<IEnumerable<EmpresaSimplificada>> ListarEntregadoras(int codigo)
        {
            try
            {
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codigo);
                return await _empresaSimplificadaRepository.Buscar(x => x.Tipo == 2 && x.CondominioId == condominio.Id);
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task<IEnumerable<TipoDocumentoEntrega>> ObterTipoDocumentos()
        {
            return await _tipoDocumentoEntregaRepository.ObterTodos();
        }

        public async Task<bool> Remover(Guid id, int codCondominio)
        {
            try
            {
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
                if (condominio == null)
                    return false;

                if (condominio != null)
                {
                    var empresa = await _empresaSimplificadaRepository.Buscar(x => x.Id == id && x.CondominioId == condominio.Id);
                    if (empresa.Any())
                    {
                        await _empresaSimplificadaRepository.RemoverTodos(empresa);
                        await _syncLogRepository.Adicionar(new SyncLog()
                        {
                            CondominioId = condominio.Id,
                            Entidade = "Transportadora",
                            RegistroId = id,
                            Operacao = "DELETE"
                        });
                    }
                    return true;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _empresaSimplificadaRepository.Dispose(); 
        }
    }
}
