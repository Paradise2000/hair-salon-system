using FluentValidation;
using projekt_programowanie.Entities;
using System.Security.Claims;

namespace projekt_programowanie.DTOs.Validators
{
    public class AddWorkerAvailabilityValidator : AbstractValidator<AddWorkerAvailabilityDto>
    {
        public AddWorkerAvailabilityValidator(ProjektDbContext db, IHttpContextAccessor httpContext)
        {
            RuleFor(r => r.Date)
                .Custom((value, context) =>
                {
                    if (db.WorkersAvailabilities.Any(w => w.Date == value && w.WorkerId.ToString() == httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)))
                    {
                        context.AddFailure("Już ustawiłeś godziny pracy na ten dzień");
                    }
                });
        }
    }
}
