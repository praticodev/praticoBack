using System;
using System.Collections.Generic;

namespace Pratico.Dominio.Model
{
    public class Imovel : Entity
    {
        public Guid AndarId { get; set; }
        public int Conjunto { get; set; }
        public int NumAndar { get; set; }
        public string NumImovel { get; set; }
        public Decimal Area { get; set; }
        public int NumImovelinterno { get; set; }
        public Decimal Fracao { get; set; }
        public Guid? Proprietario { get; set; }
        public Andar Andar { get; set; }
        public GerenciamentoDocImovel DocImovel { get; set; }
        public virtual IEnumerable<DocumentoImovel> Documentos { get; set; }
        public virtual IEnumerable<MoradorImovel> MoradoresImovel { get; set; }
        public virtual IEnumerable<InquilinoImovel> InquilinosImovel { get; set; }
        public virtual IEnumerable<ProprietarioImovel> ProprietariosImovel { get; set; }
    }
}
