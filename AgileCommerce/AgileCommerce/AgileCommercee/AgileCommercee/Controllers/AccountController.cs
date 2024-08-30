
using AgileCommercee.Data;
using AgileCommercee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public IActionResult Login() { return View(); }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(p => p.UserName == model.UserName);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetInt32("Id", user.Id);
                    HttpContext.Session.SetString("Username", user.UserName);
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

        public IActionResult ForgetPass()
        {
            return View(); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgetPass(string username, string email)
        {
            // Kiểm tra sự tồn tại của username và email
            var user = _context.Users.FirstOrDefault(u => u.UserName == username && u.Email == email);
            HttpContext.Session.SetString("resetusername", username);
            HttpContext.Session.SetString("resetemail", email);
            if (user != null)
            {
                return RedirectToAction("ResetPassword","Accounts");

            }
            else { return View(); }
            
        }
        public IActionResult ResetPassword()
        {
            var user = HttpContext.Session.GetString("resetusername");
            var email = HttpContext.Session.GetString("resetemail");
            var cus = _context.Users.Where(p => p.UserName == user && p.Email == email).FirstOrDefault();

            var ResetUser = new ResetPasswordViewModel
            {
                username = user,
                email = email,
                NewPassword = "",
                ConfirmPassword = ""
            };
            return View(ResetUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var user = HttpContext.Session.GetString("resetusername");
            var email = HttpContext.Session.GetString("resetemail");
            var cus = _context.Users.Where(p => p.UserName == user && p.Email == email).FirstOrDefault();

            cus.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            _context.SaveChanges();
            return View(model);
        }


        public IActionResult Profile()
        {
            var userName = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetInt32("Id");
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Id == id);
            return View(user);
        }

        public IActionResult QLTK()
        {
            var userName = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetInt32("Id");
            var user = _context.Users.Where(p => p.UserName == userName && p.Id == id).FirstOrDefault();
            return View(user);
        }
        [HttpPost]
        public IActionResult QLTK(User model)
        {
            try
            {
                var userName = HttpContext.Session.GetString("Username");
                var id = HttpContext.Session.GetInt32("Id");
                var user = _context.Users.Where(p => p.UserName == userName && p.Id == id).FirstOrDefault();

                user.FullName = model.FullName;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.Hinh = model.Hinh;
                user.Roles = model.Roles;

                _context.SaveChanges();
                return RedirectToAction("Profile");
            }
            catch (Exception ex) { }
            return View(model);  
        }
    }
}
