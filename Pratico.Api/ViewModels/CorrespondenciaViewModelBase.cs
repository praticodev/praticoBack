using Pratico.Dominio.Enums;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pratico.Api.ViewModels
{
    public class EmpresaSimplificadaViewModel
    {
        public Guid Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public int Tipo { get; set; }
        public int Codigo { get; set; }
        public Guid Operador { get; set; }
    }

    public class TipoDocumentoEntregaViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
    }

    public class RemessaExternaViewModel
    {
        public string Numero { get; set; }
        public string CodCondominio { get; set; }
        public int QuantidadeItens { get; set; }
        public Guid EmpresaSimplificadaId { get; set; }
        public string NomeEmpresa { get; set; }
        public bool Interna { get; set; }
        public Guid OperadorId { get; set; }

    }
    public class ItemRemessaViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public Guid EmpresaSimplificadaId { get; set; }
    }

    public class RemessaInternaViewModel
    {
        public string Numero { get; set; }
        public Guid RemessaExternaId { get; set; }
        public string CodCondominio { get; set; }
        public int QuantidadeItens { get; set; } = 1;
        public TipoFormatoPacoteRemessa TipoPacote { get; set; }
        public Guid ConjuntoId { get; set; }
        public Guid MoradorId { get; set; }
        public Guid Andar { get; set; }
        public Guid ImovelId { get; set; }
        public Guid? OperadorId { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string Imagem { get; set; }
    }

    public class BaixaRemessaInternaViewModel
    {
        public Guid RemessaId { get; set; }
        public Guid Pessoa { get; set; }
        public Guid Imovel { get; set; }
        public Guid Operador { get; set; }
        public int CodCondominio { get; set; }
    }

    public class ItemRemessaInternaViewModel
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; }
        public string Vendedor { get; set; }
        public int TipoDoc { get; set; }
        public string Numero { get; set; }
        public string NomeRetirante { get; set; }
        public string Rg { get; set; }
        public Guid Operador { get; set; }
    }

}
