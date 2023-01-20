using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_programowanie.DTOs;
using projekt_programowanie.Entities;
using System.Security.Claims;

namespace projekt_programowanie.Controllers
{
    public class AccountController : Controller
    {
        private readonly ProjektDbContext _context;
        private readonly IValidator<RegisterWorkerDto> _validatorRegisterWorkerDto;
        private readonly IValidator<RegisterClientDto> _validatorRegisterClientDto;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(ProjektDbContext context,
            IValidator<RegisterWorkerDto> RegisterWorkerDto,
            IValidator<RegisterClientDto> RegisterClientDto,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _validatorRegisterWorkerDto = RegisterWorkerDto;
            _validatorRegisterClientDto = RegisterClientDto;
            _passwordHasher = passwordHasher;
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

            return RedirectToAction("Index", "Home");
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

            return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Login");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);

            if(result == PasswordVerificationResult.Failed)
            {
                return RedirectToAction("Login");
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

            return RedirectToAction("Index", "Home");
        }
    }
}
