using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using projekt_programowanie.DTOs;
using projekt_programowanie.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace projekt_programowanie.Controllers
{
    public class WorkerController : Controller
    {
        private readonly ProjektDbContext _context;
        private readonly IValidator<AddWorkerAvailabilityDto> _validatorAddWorkerAvailabilityDto;

        public WorkerController(ProjektDbContext context, IValidator<AddWorkerAvailabilityDto> AddWorkerAvailabilityDto)
        {
            _context = context;
            _validatorAddWorkerAvailabilityDto = AddWorkerAvailabilityDto;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddServices()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddServices(AddServiceDto dto)
        {
            _context.Services.Add(new Service
            {
                ServiceName= dto.ServiceName,
                ServiceDuration= dto.ServiceDuration,
                ServicePrice= dto.ServicePrice,
            });

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult GetWorkerAvailabilities(GetWorkerAvailabilityDto dto)
        {
            var PickedService = _context.Services.FirstOrDefault(s => s.Id == 1);
            var Workers = _context.WorkersAvailabilities.Where(d => d.Date >= DateTime.Today).OrderBy(s => s.Date).ToList();
            var DatesAvailability = new List<GetWorkerAvailabilityDto>();

            foreach(var Worker in Workers)
            {
                var Visits = _context.BookedVisits.Where(r => r.StartTime >= DateTime.Today && r.WorkerId == Worker.WorkerId).OrderBy(s => s.StartTime).ToList();

                var WorkerStart = Worker.StartTime.TimeOfDay;
                var WorkerEnd = Worker.EndTime.TimeOfDay;
                TimeSpan? LastVist = null;

                foreach (var Visit in Visits)
                {

                    for (TimeSpan i = WorkerStart; i < WorkerEnd; i = i + new TimeSpan(0, 30, 0))
                    {
                        if(i >= Visit.StartTime.TimeOfDay && i + PickedService.ServiceDuration <= Visit.EndTime.TimeOfDay)
                        {
                            WorkerStart = Visit.EndTime.TimeOfDay;
                            break;
                        }
                        else
                        {
                            var dataWorker = _context.Users.FirstOrDefault(u => u.UserId == Worker.WorkerId);
                            DatesAvailability.Add(new GetWorkerAvailabilityDto
                            {
                                ServiceId = PickedService.Id,
                                WorkerFirstName = dataWorker.FirstName,
                                WorkerPhone = dataWorker.Phone,
                                Start = i,
                                End = i + PickedService.ServiceDuration,
                                Price = PickedService.ServicePrice
                            });
                        }
                    }
                    LastVist = Visit.EndTime.TimeOfDay;
                }

                if(Visits == null)
                {
                    LastVist = Worker.StartTime.TimeOfDay;
                }

                for (TimeSpan i = (TimeSpan)LastVist; i < WorkerEnd; i = i + new TimeSpan(0, 30, 0))
                {
                    var dataWorker = _context.Users.FirstOrDefault(u => u.UserId == Worker.WorkerId);
                    DatesAvailability.Add(new GetWorkerAvailabilityDto
                    {
                        ServiceId = PickedService.Id,
                        WorkerFirstName = dataWorker.FirstName,
                        WorkerPhone = dataWorker.Phone,
                        Start = i,
                        End = i + PickedService.ServiceDuration,
                        Price = PickedService.ServicePrice
                    });
                }
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult AddWorkerAvailabilities()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddWorkerAvailabilities(AddWorkerAvailabilityDto dto)
        {
            var result = _validatorAddWorkerAvailabilityDto.Validate(dto);

            if (!result.IsValid) 
            {
                return View();
            }

            _context.WorkersAvailabilities.Add(new WorkerAvailability
            {
                WorkerId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Date = dto.Date,
                StartTime = dto.Date + dto.Start,
                EndTime = dto.Date + dto.End
            });

            _context.SaveChanges();

            return View();
        }
    }
}
