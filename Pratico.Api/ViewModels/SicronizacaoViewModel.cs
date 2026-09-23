using System;
using System.Collections.Generic;

namespace Pratico.Api.ViewModels
{
    public class SicronizacaoViewModel
    {
        public string Codigo { get; set; }
        public IEnumerable<MoradorViewModel> Moradores { get; set; }
        public IEnumerable<VeiculoViewModel> Veiculos { get; set; }
        public IEnumerable<PrestadorServicoViewModel> PrestadoresServico { get; set; }
        public IEnumerable<VisitanteImovelViewModel> VisitantesImovel { get; set; }
        public IEnumerable<EmpresaSimplificadaViewModel> Transportadoras { get; set; }
        public IEnumerable<RemessaInternaViewModel> Remessas { get; set; }
        public List<DeleteViewModel> Excluidos { get; set; } = new List<DeleteViewModel>();
    }

    public class DeleteViewModel
    {
        public string Entidade { get; set; }
        public Guid Id { get; set; }
    }
}
