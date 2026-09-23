using System.Collections.Generic;
using System;

namespace Pratico.Api.ViewModels
{
    public class ReservaViewModel
    {
        public Guid PessoaId { get; set; }
        public Guid AreaLazerId { get; set; }
        public int Dia { get; set; }
        public int Mes { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }
        public int CodCond { get; set; }
    }

    public class ConsultaReservaViewModel
    {
        public int CodCondominio { get; set; }
        public Guid Area { get; set; }
        public DateTime Data { get; set; }
    }

    public class FilaEsperaViewModel
    {
        public Guid pessoaId { get; set; }
        public Guid reservaId { get; set; }
    }
}
