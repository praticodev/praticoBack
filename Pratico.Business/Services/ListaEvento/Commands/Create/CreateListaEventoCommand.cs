using MediatR;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Business.Services.ListaEvento.Commands.Create
{
    public class CreateListaEventoCommand : IRequest<Dominio.Model.ListaEvento>
    {
        public Guid AgendaAreaLazerId { get; set; }
        public List<ConvidadoEvento> Convidados { get; set; } = new List<ConvidadoEvento>();
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string NomeArea { get; set; }
        public string NomeDonoEvento { get; set; }
        public Pratico.Dominio.Model.ListaEvento ListaEvento()
        {
            var lista = new Pratico.Dominio.Model.ListaEvento
            {
                AgendaAreaLazerId = AgendaAreaLazerId,
                Convidaddos = Convidados,
                NomeArea = NomeArea,
                NomeDonoEvento = NomeDonoEvento
            };
            return lista;
        }
    }
}
