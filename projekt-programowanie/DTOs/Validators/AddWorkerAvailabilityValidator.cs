using FluentValidation;
using projekt_programowanie.Entities;
using System.Security.Claims;

namespace projekt_programowanie.DTOs.Validators
{
    public class AddWorkerAvailabilityValidator : AbstractValidator<AddWorkerAvailabilityDto>
    {
        public AddWorkerAvailabilityValidator(ProjektDbContext db, IHttpContextAccessor httpContext)
        {
            RuleFor(r => new { r.Date, r.Start, r.End })
               .Custom((value, context) =>
               {
                   var workerAvailabilities = db.WorkersAvailabilities.Where(r => r.WorkerId == int.Parse(httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) && r.Date == value.Date.Date);

                   foreach(var workerAvailability in workerAvailabilities)
                   {
                       if (value.Start < workerAvailability.EndTime && workerAvailability.StartTime < value.End)
                       {
                           context.AddFailure("Date","Podany przez ciebie czas dostępności koliduje z innymi czasami dostępności, które już podałeś.");
                       }
                   }
               });

            RuleFor(r => r.Date)
                .Custom((value, context) =>
                {
                    if (DateTime.Now > value)
                    {
                        context.AddFailure("Data dostępności musi dotyczyć przyszłości, nie może również dotyczeć dnia dzisiejszego.");
                    }
                });

            RuleFor(r => r.Start)
                .Custom((value, context) =>
                {
                    if(value.Minutes % 30 != 0)
                    {
                        context.AddFailure("Początek dostępności musi być wielokrotnością 30 min (np. 11:00, 11:30, 12:00 itp.).");
                    }
                });

            RuleFor(r => r.End)
                .Custom((value, context) =>
                {
                    if (value.Minutes % 30 != 0)
                    {
                        context.AddFailure("Koniec dostępności musi być wielokrotnością 30 min (np. 11:00, 11:30, 12:00 itp.).");
                    }
                });
        }
    }
}
