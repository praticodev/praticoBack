using FluentValidation;
using Pratico.Dominio.Model;

namespace Pratico.Dominio.Validations
{
    public class ConjuntoValidation : AbstractValidator<Conjunto>
    {
        public ConjuntoValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            //.Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
