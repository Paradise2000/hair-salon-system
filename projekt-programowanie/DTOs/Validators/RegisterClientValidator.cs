using FluentValidation;
using projekt_programowanie.Entities;

namespace projekt_programowanie.DTOs.Validators
{
    public class RegisterClientValidator : AbstractValidator<RegisterClientDto>
    {
        public RegisterClientValidator(ProjektDbContext db, IHttpContextAccessor httpContext)
        {
            RuleFor(r => r.FirstName).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.FirstName).MinimumLength(2).WithMessage("Imię musi mieć co najmniej 2 litery");

            RuleFor(r => r.LastName).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.LastName).MinimumLength(2).WithMessage("Nazwisko musi mieć co najmniej 2 litery");

            RuleFor(r => r.Email).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.Email).EmailAddress().WithMessage("Błednie podany adres email");

            RuleFor(r => r.Phone).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.Phone).Length(9).WithMessage("Telefon musi składać się z 9 cyfr");

            RuleFor(r => r.Password).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.Password).MinimumLength(8).WithMessage("Hasło musi składać się przynajmniej z 8 liter");
        }
    }
}
