using FluentValidation;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Validations
{
    public class DocumentoImovelValidation : AbstractValidator<DocumentoImovel>
    {
        public DocumentoImovelValidation()
        {
            RuleFor(c => c.NomeDoc)
                .NotEmpty().WithMessage("Nome do arquivo não definido");
        }
    }
}
