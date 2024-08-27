
using AgileCommercee.Data;
using AgileCommercee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;

namespace AgileCommercee.Controllers
{
    public class AccountsController : Controller
    {
        private readonly AgileStoreContext _context;
        public AccountsController(AgileStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Users != null ? _context.Users.ToList() : new List<User>();
            return View(data);
        }

        public IActionResult Main()
        {
            var data = _context.Users != null ? _context.Users.ToList() : new List<User>();
            return View(data);
        }
        public IActionResult AfterLogin()
        {
            return View();
        }

        public IActionResult CustomerLogin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model, IFormFile Hinh)
        {
            if (Hinh != null)
            {
                model.Hinh = MyTool.UploadImageToFolder(Hinh, "Hinh");
            }
            try
            {
                    var user = new User
                    {
                        UserName = model.UserName,
                        FullName = model.FullName,
                        Email = model.Email,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        Hinh = model.Hinh
                    };
                    _context.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("Main");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Main");
        }

        [HttpGet]
        public IActionResult RegisterAD() { return View(); }

        [HttpPost]
        public IActionResult RegisterAD(AdminRegisterViewModel model, IFormFile Hinh)
        {
            if (Hinh != null)
            {
                model.Hinh = MyTool.UploadImageToFolder(Hinh, "Hinh");
            }
            try
            {
                var user = new User
                {
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Email = model.Email,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Hinh = model.Hinh,
                    Roles = model.Roles 
                };
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("AfterLogin");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("AfterLogin");
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
                    /*
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserImage", user.Hinh);
                    */
                    if (user.Roles == 0)
                    {
                        return RedirectToAction("AfterLogin");
                    }
                    else if (user.Roles == null)
                    {
                        return RedirectToAction("CustomerLogin");
                    }
                }
                ModelState.AddModelError("", "Hãy thử đăng nhập lại.");
            }
            return View(model);
        }

        public IActionResult QLTK()
        {
            return View();
        }
    }
}
