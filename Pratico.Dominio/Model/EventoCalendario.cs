using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Model
{
    public class EventoCalendario
    {
        public string Title { get; set; } = "";
        public string Start { get; private set; }
        public string End { get; private set; }

        public void InsereIniciio(DateTime inicio)
        {
            this.Start = inicio.ToString("yyyy-MM-dd");
        }

        public void InsereFinal(DateTime final)
        {
            this.End = final.ToString("yyyy-MM-dd");
        }
    }
}
