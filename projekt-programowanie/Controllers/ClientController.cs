using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using projekt_programowanie.DTOs;
using projekt_programowanie.Entities;
using System.Security.Claims;

namespace projekt_programowanie.Controllers
{
    public class ClientController : Controller
    {
        private readonly ProjektDbContext _context;

        public ClientController(ProjektDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ServiceChoice_GetGetWorkerAvailabilities()
        {
            //walidacja czy usługa istnieje

            var options = _context.Services.ToList();

            return View(options);
        }

        [HttpPost]
        public IActionResult GetWorkerAvailabilities(int ServiceId)
        {
            var PickedService = _context.Services.FirstOrDefault(s => s.Id == ServiceId);
            var WorkersAvailabilities = _context.WorkersAvailabilities.Include(r => r.Worker).Where(r => r.Date > DateTime.Today).OrderBy(s => s.Date).ToList();
            var DatesAvailability = new List<GetWorkerAvailabilityDto>();

            foreach(var WorkerAvailability in WorkersAvailabilities)
            {
                var StartTimeW = WorkerAvailability.Date + WorkerAvailability.StartTime;
                var EndTimeW = WorkerAvailability.Date + WorkerAvailability.EndTime;
                var Visits = _context.BookedVisits.Where(r => r.StartTime >= StartTimeW && r.EndTime <= EndTimeW && r.WorkerId == WorkerAvailability.WorkerId).OrderBy(o => o.StartTime).ToList();

                for(TimeSpan i = WorkerAvailability.StartTime; i + PickedService.ServiceDuration <= WorkerAvailability.EndTime; i = i + new TimeSpan(0, 30, 0))
                {
                    bool flaga = false;

                    foreach(var visit in Visits)
                    {
                        if (visit.StartTime.TimeOfDay < i + PickedService.ServiceDuration && i < visit.EndTime.TimeOfDay)
                        {
                            //jest kolizja wizytowa
                            flaga = true;
                        }
                    }

                    if (flaga == false)
                    {
                        DatesAvailability.Add(new GetWorkerAvailabilityDto
                        {
                            ServiceId = PickedService.Id,
                            WorkerId = WorkerAvailability.WorkerId,
                            WorkerFirstName = WorkerAvailability.Worker.FirstName,
                            WorkerPhone = WorkerAvailability.Worker.Phone,
                            Date = WorkerAvailability.Date,
                            Start = i,
                            End = i + PickedService.ServiceDuration,
                            Price = PickedService.ServicePrice
                        });
                    }
                }

            }

            return View(DatesAvailability);
        }

        [HttpPost]
        public IActionResult VisitBooking(GetWorkerAvailabilityDto dto)
        {
            //walidacja potrzebna
            //Musi być authorize tylko dla klienta

            _context.BookedVisits.Add(new BookedVisit
            {
                StartTime = dto.Date.Date + dto.Start,
                EndTime = dto.Date.Date + dto.End,
                WorkerId= dto.WorkerId,
                ClientId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                ServiceId = dto.ServiceId,
                isCancelled = false
            });

            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        public IActionResult BookedVisits()
        {
            var CurrentUser = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var BookedVisits = _context.BookedVisits
                .Where(r => r.ClientId == CurrentUser)
                .Select(v => new BookedVisitsDto
                {
                    ServiceName = v.Service.ServiceName,
                    WorkerFirstName = v.Worker.FirstName,
                    WorkerPhone = v.Worker.Phone,
                    Date = v.StartTime.Date,
                    Start = v.StartTime.TimeOfDay,
                    End = v.EndTime.TimeOfDay,
                    Price = v.Service.ServicePrice
                }).ToList();

            return View(BookedVisits);
        }
    }
}
