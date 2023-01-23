using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_programowanie.DTOs;
using projekt_programowanie.Entities;
using projekt_programowanie.Models;
using System.Security.Claims;

namespace projekt_programowanie.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProjektDbContext _context;
        private readonly IValidator<RegisterWorkerDto> _validatorRegisterWorkerDto;
        private readonly IValidator<RegisterClientDto> _validatorRegisterClientDto;
        private readonly IValidator<UpdateDataDto> _validatorUpdateDataDto;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(ProjektDbContext context,
            IValidator<RegisterWorkerDto> RegisterWorkerDto,
            IValidator<RegisterClientDto> RegisterClientDto,
            IValidator<UpdateDataDto> UpdateDataDto,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _validatorRegisterWorkerDto = RegisterWorkerDto;
            _validatorRegisterClientDto = RegisterClientDto;
            _passwordHasher = passwordHasher;
            _validatorUpdateDataDto= UpdateDataDto;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterWorker()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterWorker(RegisterWorkerDto dto)
        {
            var result = _validatorRegisterWorkerDto.Validate(dto);

            if (!result.IsValid)
            {
                this.ModelState.Clear();
                result.AddToModelState(this.ModelState);
                return View(dto);
            }

            var newUser = new Worker()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var hashedPassowrd = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.Password = hashedPassowrd;
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return View("Message", new MessageViewModel("Proces rejestracji przebiegł pomyślnie", MessageType.Success, "Home", "Index"));
        }

        [HttpGet]
        public IActionResult RegisterClient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterClient(RegisterClientDto dto)
        {
            var result = _validatorRegisterClientDto.Validate(dto);

            if (!result.IsValid)
            {
                this.ModelState.Clear();
                result.AddToModelState(this.ModelState);
                return View(dto);
            }

            var newUser = new Client()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var hashedPassowrd = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.Password = hashedPassowrd;
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return View("Message", new MessageViewModel("Proces rejestracji przebiegł pomyślnie", MessageType.Success, "Home", "Index"));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto dto)
        {        
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if(user == null) 
            {
                ViewBag.message = "Logowanie nie powiodło się, spróbuj ponownie";
                return View();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                ViewBag.message = "Logowanie nie powiodło się, spróbuj ponownie";
                return View();
            }
            
            ClaimsIdentity identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);
            var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return View("Message", new MessageViewModel("Zostałeś wylogowany", MessageType.Success, "Home", "Index"));
        }

        [HttpGet]
        [Authorize]
        public IActionResult UpdateData()
        {
            var CurrentUser = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var data = _context.Users.FirstOrDefault(r => r.UserId == CurrentUser);

            var dto = new UpdateDataDto
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                Phone = data.Phone,
            };

            return View(dto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateData(UpdateDataDto dto)
        {
            var result = _validatorUpdateDataDto.Validate(dto);

            if (!result.IsValid)
            {
                this.ModelState.Clear();
                result.AddToModelState(this.ModelState);
                return View(dto);
            }

            var CurrentUser = int.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var data = _context.Users.FirstOrDefault(r => r.UserId == CurrentUser);

            data.FirstName = dto.FirstName;
            data.LastName = dto.LastName;
            data.Email = dto.Email;
            data.Phone = dto.Phone;

            if(dto.ChangePasswordChecked == true)
            {
                var hashedPassowrd = _passwordHasher.HashPassword(data, dto.Password);

                data.Password = hashedPassowrd;
            }
            
            _context.SaveChanges();

            return View("Message", new MessageViewModel("Twoje dane zostały zaaktualizowane", MessageType.Success, "Home", "Index"));
        }
    }
}
