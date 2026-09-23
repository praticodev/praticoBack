using System;

namespace Pratico.Dominio.Model
{
    public class OutBoxMessage : Entity
    {
        public OutBoxMessage()
        {
            Status = Enums.OutBoxMessageStatus.Pendente;
            Tentativas = 0;
        }

        public string Tipo { get; set; }
        public string Conteudo { get; set; }
        public string Status { get; set; }
        public int Tentativas { get; set; }
        public DateTime? DataUltimaTentativa { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public DateTime? ProximaTentativaEm { get; set; }
        public string Erro { get; set; }
    }
}
