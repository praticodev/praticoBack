using Pratico.Dominio.Intefaces;
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
    public class ReversaService : BaseService, IReversaService
    {
        private readonly IAgendaAreaLazerRepository _agendaAreaLazerRepository;
        private readonly IPessoaUsuarioRepository _pessoaUsuarioRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IFilaEsperaRepository _filaEsperaRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly IEmailSenGridService _emailService;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IAreaLazerRepository _areaLazerRepository;
        public ReversaService(IAgendaAreaLazerRepository agendaAreaLazerRepository,
                                IPessoaUsuarioRepository pessoaUsuarioRepository,
                                ICondominioRepository condominioRepository,
                                IFilaEsperaRepository filaEsperaRepository,
                                IContatoRepository contatoRepository,
                                IEmailSenGridService emailService,
                                IPessoaRepository pessoaRepository,
                                IAreaLazerRepository areaLazerRepository,
                                INotificador notificador) : base(notificador)
        {
            _agendaAreaLazerRepository = agendaAreaLazerRepository;
            _pessoaUsuarioRepository = pessoaUsuarioRepository;
            _condominioRepository = condominioRepository;
            _filaEsperaRepository = filaEsperaRepository;
            _contatoRepository = contatoRepository;
            _emailService = emailService;
            _pessoaRepository = pessoaRepository;
            _areaLazerRepository = areaLazerRepository;
        }

        public async Task<AgendaAreaLazer> Adicionar(AgendaAreaLazer agenda, Guid usuario, int codCond)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCond);
            var pessoaUsuario = await _pessoaUsuarioRepository.Buscar(x => x.UsuarioId == usuario);
            agenda.PessoaId = pessoaUsuario.ToList().FirstOrDefault().PessoaId;
            agenda.CondominioId = condominio.Id;
            return await _agendaAreaLazerRepository.Adicionar(agenda);
        }

        public async Task<bool> Atualizar(Guid agenda, bool acao)
        {
            var reserva = await _agendaAreaLazerRepository.ObterPorId(agenda);
            if (reserva != null)
            {
                if (acao)
                    reserva.Autorizado = 1;
                else
                    reserva.Autorizado = 2;

                await _agendaAreaLazerRepository.Atualizar(reserva);

                if (!acao)
                {
                    var fila = await _filaEsperaRepository.Buscar(x => x.PessoaId == reserva.PessoaId && x.AgendaAreaLazerId == reserva.Id);
                    if (fila.Any())
                    {
                        var contato = await _contatoRepository.ObterContatoPorEntidade(fila.FirstOrDefault().PessoaId);
                        if (contato != null)
                        {
                            if (_emailService.ValidaEnderecoEmail(contato.Email))
                            {
                                var pessoa = await _pessoaRepository.ObterPorId(fila.FirstOrDefault().PessoaId);
                                if (pessoa != null)
                                {
                                    var arera = await _areaLazerRepository.ObterPorId(reserva.AreaLazerId);
                                    if (arera != null)
                                        await _emailService.EnviaEmailVacanciaReserva(contato.Email, pessoa.Nome, arera.Nome, reserva.DataInicio);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ListarReservasPendetes(int codCond)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCond);
            var result = await _agendaAreaLazerRepository.ListaCompletaPorMoradorPendente(condominio.Id);
            return result;
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ObterReservas(int condCondominio, Guid areaId, DateTime data)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(condCondominio);
            var result = await _agendaAreaLazerRepository.ObterReservas(condominio.Id, areaId, new DateTime(data.Year, data.Month, data.Day));
            return result;
        }

        public async Task<int> AdicionaFila(Guid pessoaId, Guid agendaId)
        {
            var pessoaUsuario = await _pessoaUsuarioRepository.Buscar(x => x.UsuarioId == pessoaId);
            if (pessoaUsuario.Any())
            {
                var existe = await _filaEsperaRepository.Buscar(x => x.PessoaId == pessoaUsuario.FirstOrDefault().PessoaId && x.AgendaAreaLazerId == agendaId);
                if (existe.ToList().Count > 0)
                    return 2;
                else
                {
                    var filaEspera = new FilaEspera();
                    filaEspera.PessoaId = pessoaUsuario.FirstOrDefault().PessoaId;
                    filaEspera.AgendaAreaLazerId = agendaId;
                    var result = await _filaEsperaRepository.Adicionar(filaEspera);
                    return 1;
                }
            }
            return 3;
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ObterReservasPorMorador(Guid morador)
        {
            var pessoaUsuario = await _pessoaUsuarioRepository.Buscar(x => x.UsuarioId == morador);
            //tpm
            if (pessoaUsuario.ToList().Count > 0)
            {
                var result = await _agendaAreaLazerRepository.ObterTodasMorador(pessoaUsuario.FirstOrDefault().PessoaId);
                return result;
            }
            //var result = await _agendaAreaLazerRepository.ObterTodasMorador(pessoaUsuario.FirstOrDefault().PessoaId);
            return null;
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ObterReservasAprovadasMorador(Guid morador)
        {
            var pessoaUsuario = await _pessoaUsuarioRepository.Buscar(x => x.UsuarioId == morador);
            return await _agendaAreaLazerRepository.ObterReservasAutorizadas(pessoaUsuario.FirstOrDefault().PessoaId);
        }

        public async Task<IEnumerable<DateTime>> ListarReservasPorMes(int codCond, int mes)
        {
            List<DateTime> reservas = new List<DateTime>();
            DateTime mesAtual = new DateTime(DateTime.Now.Year, mes, 1);
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCond);
            var result = await _agendaAreaLazerRepository.ListarReservasPorMes(condominio.Id, mesAtual);
            foreach (var item in result)
                reservas.Add(item.DataInicio);

            return reservas;
        }

        public void Dispose()
        {
            _agendaAreaLazerRepository.Dispose();
            _pessoaUsuarioRepository.Dispose();
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ListarReservasParametros(int codCond, int status, int ano)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCond);
            if (ano == 0)
                ano = DateTime.Now.Year;

            if (condominio != null)
                return await _agendaAreaLazerRepository.ListaCompleta(condominio.Id, status, ano);
            else
                return new List<AgendaAreaLazer>();
        }

        public async Task<IEnumerable<EventoCalendario>> ListarReservasCalendarioParametros(int codCond, int status)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCond);
            if (condominio != null)
            {
                List<EventoCalendario> eventos = new List<EventoCalendario>();
                var reservas = await _agendaAreaLazerRepository.ListaEventosBase(condominio.Id, status, DateTime.Now.Year);
                foreach (var item in reservas)
                {
                    var evento = new EventoCalendario();
                    evento.InsereIniciio(item.DataInicio);
                    evento.InsereFinal(item.DataFim);
                    eventos.Add(evento);
                }
                return eventos;
            }
            return new List<EventoCalendario>();
        }
    }
}
