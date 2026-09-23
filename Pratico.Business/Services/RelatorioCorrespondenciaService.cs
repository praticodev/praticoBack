using Microsoft.VisualBasic;
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
    public class RelatorioCorrespondenciaService : IRelatorioCorrespondenciaService
    {
        private readonly IRemessaRepository _remessaRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IRelatorioCorrespondenciaRepository _relatorioCorrespondenciaRepository;
        private readonly IOperadorRepository _operadorRepository;
        public RelatorioCorrespondenciaService(IRemessaRepository remessaRepository, 
                                               ICondominioRepository condominioRepository, 
                                               IRelatorioCorrespondenciaRepository relatorioCorrespondenciaRepository,
                                               IOperadorRepository operadorRepository)
        {
            _remessaRepository = remessaRepository;
            _condominioRepository = condominioRepository;
            _relatorioCorrespondenciaRepository = relatorioCorrespondenciaRepository;
            _operadorRepository = operadorRepository;
        }

        public async Task<List<Operador>> ObterOperadoresRelatorio(int condigo)
        {
            List<Operador> result = new List<Operador>();
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(condigo);
            if (condominio != null)
            {
                var colecao = await _operadorRepository.Buscar(x => x.CondominioId == condominio.Id && x.Ativo == true);
                result = colecao.ToList();
            }   
            return result;
        }

        public async Task<List<RemessaExternaRelatorio>> ObterRemessasExternasAbertasRelatorio(int condigo, DateTime? dataInicio, DateTime? dataFim, Guid? operador)
        {
            List<RemessaExternaRelatorio> lista = new List<RemessaExternaRelatorio>();
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(condigo);
            var result =  await _relatorioCorrespondenciaRepository.ObterRemessasExternasAbertasRelatorio(condominio.Id,dataInicio,dataFim,operador);
            foreach (var item in result) 
            {
                RemessaExternaRelatorio novo = new RemessaExternaRelatorio()
                {
                    Data = item.DataCadastro.ToString("dd/MM/yyyy"),
                    Numero = item.Numero,
                    Operador = item.Operador != null ? item.Operador.Nome : "",  
                    QuantidadeItens = item.QuantidadeItens,
                };
                lista.Add(novo);
            }
            return lista;
        }
    }
}
