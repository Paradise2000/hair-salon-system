using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_programowanie.DTOs;
using projekt_programowanie.Entities;
using projekt_programowanie.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;

namespace projekt_programowanie.Controllers
{
    [Authorize(Roles = "Worker")]
    public class WorkerController : Controller
    {
        private readonly ProjektDbContext _context;
        private readonly IValidator<AddWorkerAvailabilityDto> _validatorAddWorkerAvailabilityDto;
        private readonly IValidator<AddServiceDto> _validatorAddServiceDto;

        public WorkerController(ProjektDbContext context,
            IValidator<AddWorkerAvailabilityDto> AddWorkerAvailabilityDto,
            IValidator<AddServiceDto> AddServiceDto)
        {
            _context = context;
            _validatorAddWorkerAvailabilityDto = AddWorkerAvailabilityDto;
            _validatorAddServiceDto = AddServiceDto;
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
            var result = _validatorAddServiceDto.Validate(dto);

            if (!result.IsValid)
            {
                this.ModelState.Clear();
                result.AddToModelState(this.ModelState);
                return View(dto);
            }

            _context.Services.Add(new Service
            {
                ServiceName= dto.ServiceName,
                ServiceDuration= dto.ServiceDuration,
                ServicePrice= dto.ServicePrice,
            });

            _context.SaveChanges();

            return View("Message", new MessageViewModel("Pomyślnie dodano usługę", MessageType.Success, "Worker", "AddServices"));
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
                this.ModelState.Clear();
                result.AddToModelState(this.ModelState);
                return View(dto);
            }

            _context.WorkersAvailabilities.Add(new WorkerAvailability
            {
                WorkerId = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                Date = dto.Date,
                StartTime = dto.Start,
                EndTime = dto.End,
                isCancelled = false
            });

            _context.SaveChanges();

            return View("Message", new MessageViewModel("Pomyślnie dodano twoją dostępność", MessageType.Success, "Worker", "AddWorkerAvailabilities"));
        }

        [HttpGet]
        public IActionResult GetClientsBookings()
        {
            var CurrentUser = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var BookedVisits = _context.BookedVisits
                .Where(r => r.WorkerId == CurrentUser && r.isCancelled == false)
                .OrderBy(r => r.StartTime)
                .Select(v => new GetClientsBookingsDto
                {
                    Id= v.Id,
                    ClientFirstName = v.Client.FirstName,
                    ClientLastName = v.Client.LastName,
                    ClientPhone= v.Client.Phone,
                    ServiceName = v.Service.ServiceName,
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

            var BookedVisit = _context.BookedVisits.FirstOrDefault(r => r.WorkerId == CurrentUser && r.Id == Id);
            BookedVisit.isCancelled = true;

            _context.SaveChanges();

            return View("Message", new MessageViewModel("Odwołano twoją wizytę", MessageType.Success, "Worker", "GetClientsBookings"));
        }

    }
}
