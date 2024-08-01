using AgileEcommerce.Data;
using AgileEcommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AgileEcommerce.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AccountContext _context;
        public AccountsController(AccountContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Users != null ? _context.Users.ToList() : new List<User>();
            return View(data);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() { return View(); }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(p => p.UserName == model.UserName);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Hãy thử đăng nhập lại.");
            }
            return View(model);
        }
    }
}
