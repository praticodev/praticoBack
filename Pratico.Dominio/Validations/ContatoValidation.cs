using FluentValidation;
using Pratico.Dominio.Model;

namespace Pratico.Dominio.Validations
{
    public class ContatoValidation : AbstractValidator<Contato>
    {
        public ContatoValidation()
        {
            RuleFor(c => c.Ddd)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(c => c.Telefone)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            //.Length(2, 1000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Email).NotEmpty()
                .WithMessage("O campo {PropertyName} precisa ser fornecido")
                .EmailAddress().WithMessage("Digite um E-mail válido");
        }
    }
}
