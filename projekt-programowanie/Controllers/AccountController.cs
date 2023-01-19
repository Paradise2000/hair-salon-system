using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        public AccountController(ProjektDbContext context)
        {
            _context = context;
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
        public IActionResult RegisterWorker(RegisterDto dto)
        {
            _context.Workers.Add(new Worker
            {
                FirstName= dto.FirstName,
                LastName= dto.LastName,
                Email= dto.Email,
                Phone= dto.Phone,
                Password= dto.Password
            });

            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        public IActionResult RegisterClient()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterClient(RegisterDto dto)
        {
            _context.Clients.Add(new Client
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Password = dto.Password
            });

            _context.SaveChanges();

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto dto)
        {
            ClaimsIdentity identity = null;
            bool isAuthenticate = false;
            var user =_context.Users.FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);
            
            if(user == null) 
            {
                return RedirectToAction("Login");
            }
            else
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticate = true;
            }

            if (isAuthenticate)
            {
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
