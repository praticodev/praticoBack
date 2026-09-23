
using FluentValidation;
using Pratico.Dominio.Model;

namespace Pratico.Dominio.Validations
{
    public class CondominioValidation : AbstractValidator<Condominio>
    {
        public CondominioValidation()
        {
            RuleFor(c => c.RazaoSocial)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            //.Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Cnpj)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
                //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            //RuleFor(c => c.Valor)
            //    .GreaterThan(0).WithMessage("O campo {PropertyName} precisa ser maior que {ComparisonValue}");
        }
    }
}
