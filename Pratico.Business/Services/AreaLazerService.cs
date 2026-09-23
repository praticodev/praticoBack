using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using Pratico.Dominio.Notificacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class AreaLazerService : BaseService, IAreaLazerService
    {
        private readonly IAreaLazerRepository _areaLazerRepository;
        private readonly ITipoAreaLazerRepository _tipoAreaLazerRepository;
        private readonly IAgendaAreaLazerRepository _agendaAreaLazerRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICondominioRepository _condominioRepository;

        public AreaLazerService(IAreaLazerRepository areaLazerRepository,
            ITipoAreaLazerRepository tipoAreaLazerRepository,
            IAgendaAreaLazerRepository agendaAreaLazerRepository,
            IPessoaRepository pessoaRepository,
            ICondominioRepository condominioRepository,
            INotificador notificador) : base(notificador)
        {
            _areaLazerRepository = areaLazerRepository;
            _tipoAreaLazerRepository = tipoAreaLazerRepository;
            _agendaAreaLazerRepository = agendaAreaLazerRepository;
            _pessoaRepository = pessoaRepository;
            _condominioRepository = condominioRepository;
        }

        public async Task<AreaLazer> Adicionar(AreaLazer areaLazer)
        {
            return await _areaLazerRepository.Adicionar(areaLazer);
        }

        public async Task<IEnumerable<TipoAreaLazer>> ObterTiposAreas(Guid condiminio)
        {
            var result = await _tipoAreaLazerRepository.Buscar(x => x.CondominioId == condiminio);
            return result;
        }

        public async Task<IEnumerable<AgendaAreaLazerCompleta>> ObterResevasPorPeriodo(Guid area, DateTime inicio, DateTime fim)
        {
            var listAgenda = new List<AgendaAreaLazerCompleta>();
            var result = await _agendaAreaLazerRepository.Buscar(x => x.AreaLazerId == area && inicio >= x.DataInicio && fim <= x.DataFim);
            foreach (var item in result)
            {
                Pessoa reservador = await _pessoaRepository.ObterPorId(item.PessoaId);
                AreaLazer areaReservada = await _areaLazerRepository.ObterPorId(item.AreaLazerId);
                AgendaAreaLazerCompleta reserva = new AgendaAreaLazerCompleta
                {
                    DonoDaReserva = reservador.Nome,
                    NomeArea = areaReservada.Nome,
                    FimReserva = item.DataFim,
                    InicioReserva = item.DataInicio
                };
                listAgenda.Add(reserva);
            }
            return listAgenda;
        }

        public async Task AtualizaAreaLazer(AreaLazer area)
        {
            var areaLazer = await _areaLazerRepository.ObterPorId(area.Id);
            areaLazer.Nome = area.Nome;
            areaLazer.Descricao = area.Descricao;
            await _areaLazerRepository.Atualizar(area);
        }

        public async Task RemoveAreaLazer(Guid id)
        {
            await _areaLazerRepository.Remover(id);
        }

        public void Dispose()
        {
            _areaLazerRepository.Dispose();
            _tipoAreaLazerRepository.Dispose();
            _agendaAreaLazerRepository.Dispose();
        }

        public async Task<IEnumerable<AreaLazer>> ObterAreasPorCondominio(int condiminio)
        {
            var condominioBanco = await _condominioRepository.ObterCondominioPorCodigo(condiminio);
            return await _areaLazerRepository.Buscar(x => x.CondominioId == condominioBanco.Id);
        }
    }
}
