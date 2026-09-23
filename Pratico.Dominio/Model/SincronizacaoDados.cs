using System.Collections.Generic;

namespace Pratico.Dominio.Model
{
    public class SincronizacaoDados
    {
        public long? Codigo { get; set; }
        public IEnumerable<Morador> Moradores { get; set; } = new List<Morador>();
        public IEnumerable<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
        public IEnumerable<PrestadorServico> PrestadoresServico { get; set; } = new List<PrestadorServico>();
        public IEnumerable<VisitanteImovel> VisitantesImovel { get; set; } = new List<VisitanteImovel>();
        public IEnumerable<EmpresaSimplificada> Transportadoras { get; set; } = new List<EmpresaSimplificada>();
        public IEnumerable<RemessaInterna> Remessas { get; set; } = new List<RemessaInterna>();
        public IEnumerable<SyncLog> Excluidos { get; set; } = new List<SyncLog>();
    }
}
