using FluentValidation;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Validations
{
    public class AndarValidation : AbstractValidator<Andar>
    {
        public AndarValidation()
        {
            RuleFor(c => c.NumTorre)
                .NotEmpty().WithMessage("O campo precisa ser fornecido");
            //.Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
