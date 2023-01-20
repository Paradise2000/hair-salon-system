using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            return RedirectToAction("Index", "Home");
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
                EndTime = dto.End
            });

            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        public IActionResult GetClientsBookings()
        {
            var CurrentUser = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var BookedVisits = _context.BookedVisits
                .Where(r => r.WorkerId == CurrentUser)
                .Select(v => new GetClientsBookingsDto
                {
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

    }
}
