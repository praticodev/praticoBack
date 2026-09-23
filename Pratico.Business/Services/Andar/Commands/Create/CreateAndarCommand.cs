using MediatR;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Business.Services.Andar.Commands.Create
{
    public class CreateAndarCommand : IRequest<Pratico.Dominio.Model.Andar>
    {
        public string Observacoes { get; set; }
        public int NumTorre { get; set; }
        public int NumAndarInterno { get; set; }
        public Guid ConjuntoId { get; set; }
        public Conjunto Conjunto { get; set; }
        public IEnumerable<Pratico.Dominio.Model.Imovel> Imoveis { get; set; }

        public Pratico.Dominio.Model.Andar Andar()
        {
            var andar = new Pratico.Dominio.Model.Andar
            {
                Conjunto = Conjunto,
                Imoveis = Imoveis,
                NumAndarInterno = NumAndarInterno,
                NumTorre = NumTorre,
                Observacoes = Observacoes
            };
            return andar;
        }
    }
}
