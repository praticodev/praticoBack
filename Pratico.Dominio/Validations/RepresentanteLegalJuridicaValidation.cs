using FluentValidation;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Validations
{
    public class RepresentanteLegalJuridicaValidation : AbstractValidator<RepresentanteLegal>
    {
        public RepresentanteLegalJuridicaValidation()
        {
            RuleFor(c => c.Cargo)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            //.Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.FimPeriodo)
                .NotEmpty().WithMessage("O campo Fim Período precisa ser fornecido");
            //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.IniPeriodo)
                .NotEmpty().WithMessage("O campo Início Período precisa ser fornecido");
            //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.RazaoSocialRep)
                .NotEmpty().WithMessage("O campo Início Nome Razão Social precisa ser fornecido");
            //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.CnpjRep)
                .NotEmpty().WithMessage("O campo CNPJ precisa ser fornecido");
            //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
