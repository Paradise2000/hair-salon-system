using FluentValidation;
using projekt_programowanie.Entities;

namespace projekt_programowanie.DTOs.Validators
{
    public class AddServiceValidator : AbstractValidator<AddServiceDto>
    {
        public AddServiceValidator(ProjektDbContext db, IHttpContextAccessor httpContext)
        {
            RuleFor(r => r.ServiceName).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.ServiceName).MinimumLength(3).WithMessage("Nazwa usługi musi mieć przynajmniej 3 litery");

            RuleFor(r => r.ServiceDuration).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.ServiceDuration)
                .Custom((value, context) =>
                {
                    if (value.Minutes % 30 != 0)
                    {
                        context.AddFailure("Czas trwania usługi musi być wielokrotnością 30 min (np. 11:00, 11:30, 12:00 itp.).");
                    }
                });

            RuleFor(r => r.ServicePrice).NotEmpty().WithMessage("Pole nie może być puste");
            RuleFor(r => r.ServicePrice).GreaterThan(1).WithMessage("Cena usługi musi być być większa niż 1zł");

        }
    }
}
