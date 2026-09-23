using FluentValidation;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pratico.Dominio.Validations
{
    public class RepresentanteLegalValidation : AbstractValidator<RepresentanteLegal>
    {
        public RepresentanteLegalValidation()
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

            RuleFor(c => c.NomeRep)
                .NotEmpty().WithMessage("O campo Nome Representante precisa ser fornecido");
            //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.NumDoc)
                .NotEmpty().WithMessage("O campo Número Documento precisa ser fornecido");
            //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}