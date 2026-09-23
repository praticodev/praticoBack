using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.ViewModels
{
    public class RepresentanteLegalViewModel
    {
        public Guid Id { get; set; }
        public string NomeRep { get; set; }
        public int TipoDoc { get; set; }
        public string NumDoc { get; set; }
        public string Cpf { get; set; }
        public int Sexo { get; set; }
        public int EstCivil { get; set; }
        public string RazaoSocialRep { get; set; }
        public string NomeFantasiaRep { get; set; }
        public string CnpjRep { get; set; }
        public string InscEstadualRep { get; set; }
        public string InscMunicipalRep { get; set; }
        public string DataNascimentoRep { get; set; }
        public int Cargo { get; set; }
        public string IniPeriodo { get; set; }
        public string FimPeriodo { get; set; }
        public bool AssinaDoc { get; set; }
        public Guid CondominioId { get; set; }
        public virtual IEnumerable<CondominioViewModel> Condominios { get; set; }
    }
}
