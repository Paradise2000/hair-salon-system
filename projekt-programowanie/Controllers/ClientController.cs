using Azure.Identity;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Newtonsoft.Json;
using projekt_programowanie.DTOs;
using projekt_programowanie.Entities;
using projekt_programowanie.Models;
using System.Data;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace projekt_programowanie.Controllers
{
    [Authorize(Roles = "Client")]
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
            var WorkersAvailabilities = _context.WorkersAvailabilities.Include(r => r.Worker).Where(r => r.Date > DateTime.Today && r.isCancelled == false).OrderBy(s => s.Date).ToList();
            var DatesAvailability = new List<GetWorkerAvailabilityDto>();
            int tempIterator = 0;

            foreach(var WorkerAvailability in WorkersAvailabilities)
            {
                var Visits = _context.BookedVisits.Where(r => r.WorkerAvailabilityId == WorkerAvailability.Id && r.WorkerId == WorkerAvailability.WorkerId).OrderBy(o => o.StartTime).ToList();

                for(TimeSpan i = WorkerAvailability.StartTime; i + PickedService.ServiceDuration <= WorkerAvailability.EndTime; i = i + new TimeSpan(0, 30, 0))
                {
                    bool flaga = false;

                    foreach(var visit in Visits)
                    {
                        if (visit.StartTime.TimeOfDay < i + PickedService.ServiceDuration && i < visit.EndTime.TimeOfDay && visit.isCancelled == false)
                        {
                            //jest kolizja wizytowa
                            flaga = true;
                        }
                    }

                    if (flaga == false)
                    {
                        DatesAvailability.Add(new GetWorkerAvailabilityDto
                        {
                            TempDataId= tempIterator,
                            ServiceId = PickedService.Id,
                            WorkerId = WorkerAvailability.WorkerId,
                            WorkerAvailabilityId = WorkerAvailability.Id,
                            WorkerFirstName = WorkerAvailability.Worker.FirstName,
                            WorkerPhone = WorkerAvailability.Worker.Phone,
                            Date = WorkerAvailability.Date,
                            Start = i,
                            End = i + PickedService.ServiceDuration,
                            Price = PickedService.ServicePrice
                        });
                        tempIterator++;
                    }
                }
            }

            TempData["DaneTemp"] = JsonConvert.SerializeObject(DatesAvailability);
            //DatesAvailability.OrderBy(r => r.Start).ToList();
            return View(DatesAvailability);
        }

        [HttpPost]
        public IActionResult VisitBooking(int TempId)
        {
            var DaneTemp = JsonConvert.DeserializeObject<List<GetWorkerAvailabilityDto>>((string)TempData["DaneTemp"]);
            var dto = DaneTemp.FirstOrDefault(r => r.TempDataId== TempId);

            if(dto == null)
            {
                return View("Message", new MessageViewModel("Ten termin nie istnieje", MessageType.Error, "Home", "Index"));
            }

            if(_context.WorkersAvailabilities.FirstOrDefault(c => c.Id == dto.WorkerAvailabilityId).isCancelled == true)
            {
                return View("Message", new MessageViewModel("Ten termin nie jest już dostępny", MessageType.Error, "Home", "Index"));
            }

            if (_context.BookedVisits.Where(r => r.StartTime.TimeOfDay < dto.End && dto.Start < r.EndTime.TimeOfDay && r.isCancelled == false).Count() != 0)
            {
                return View("Message", new MessageViewModel("Ta wizyta została już zarezerwowana", MessageType.Error, "Home", "Index"));
            }

            _context.BookedVisits.Add(new BookedVisit
            {
                StartTime = dto.Date.Date + dto.Start,
                EndTime = dto.Date.Date + dto.End,
                WorkerId= dto.WorkerId,
                ClientId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                ServiceId = dto.ServiceId,
                isCancelled = false,
                WorkerAvailabilityId = dto.WorkerAvailabilityId
            });

            _context.SaveChanges();

            return View("Message", new MessageViewModel("Zarezerwowano wizytę", MessageType.Success, "Home", "Index"));
        }

        [HttpGet]
        public IActionResult BookedVisits()
        {
            var CurrentUser = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var BookedVisits = _context.BookedVisits
                .Where(r => r.ClientId == CurrentUser && r.isCancelled == false)
                .OrderBy(r => r.StartTime)
                .Select(v => new BookedVisitsDto
                {
                    Id = v.Id,
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

        [HttpPost]
        public IActionResult CancelBooking(int Id)
        {
            var CurrentUser = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var BookedVisit = _context.BookedVisits.FirstOrDefault(r => r.ClientId == CurrentUser && r.Id == Id);
            BookedVisit.isCancelled = true;

            _context.SaveChanges();

            return View("Message", new MessageViewModel("Odwołano wizytę", MessageType.Success, "Client", "BookedVisits"));
        }
    }
}
