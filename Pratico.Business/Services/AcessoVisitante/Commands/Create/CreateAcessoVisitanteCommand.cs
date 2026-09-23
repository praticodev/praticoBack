using MediatR;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Business.Services.AcessoVisitante.Commands.Create
{
    public class CreateAcessoVisitanteCommand : IRequest<Pratico.Dominio.Model.AcessoVisitante>
    {
        public Guid CondominioId { get; set; }
        public Guid ConjuntoId { get; set; }
        public Guid AndarId { get; set; }
        public Guid ImovelId { get; set; }
        public Guid PessoaId { get; set; }
        public string NomeVisitante { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumDoc { get; set; }

        public Pratico.Dominio.Model.AcessoVisitante Visitante()
        {
            var visitante = new Pratico.Dominio.Model.AcessoVisitante
            {
                ConjuntoId = ConjuntoId,
                CondominioId = CondominioId,
                AndarId = AndarId,
                ImovelId = ImovelId,
                PessoaId = PessoaId,
                NomeVisitante = NomeVisitante,
                NumDoc = NumDoc,
                TipoDocumento = TipoDocumento
            };
            return visitante;
        }
    }
}
