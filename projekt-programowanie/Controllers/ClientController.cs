using Microsoft.AspNetCore.Mvc;
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
    }
}
