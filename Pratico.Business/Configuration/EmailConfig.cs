using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Business.Configuration
{
    public class EmailConfig
    {
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int Porta { get; set; }
        public string Host { get; set; }
    }
}