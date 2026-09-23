using System;
using System.ComponentModel.DataAnnotations;

namespace Pratico.Dominio.Model
{
    public class Veiculo : Entity 
    {
        public Guid ImovelId { get; set; }
        public Guid VeiculoFabricanteId { get; set; }
        public string Placa { get; set; }
        public string Cor { get; set; }
        public string Modelo { get; set; }
        public int Tipo { get; set; }
        public string Imagem { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public Imovel Imovel { get; set; }
        public Guid CondominioId { get; set; }
        public VeiculoFabricante Fabricante { get; set; }
    }
}
