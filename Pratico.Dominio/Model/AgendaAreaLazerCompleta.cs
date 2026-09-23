using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class AgendaAreaLazerCompleta
    {
        public string NomeArea { get; set; }
        public string DonoDaReserva { get; set; }
        public DateTime InicioReserva { get; set; }
        public DateTime FimReserva { get; set; }
    }
}
