
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
            // Lấy giá trị Roles từ session
            var userRoles = HttpContext.Session.GetInt32("Roles");
            var userId = HttpContext.Session.GetInt32("Id");

            // Kiểm tra giá trị của Roles
            if (userRoles == null && userId != null)
            {
                return RedirectToAction("CustomerLogin", "Accounts");
            }
            else if (userRoles == 0 && userId != null)
            {
                return RedirectToAction("AfterLogin", "Accounts");
            }

            // Nếu Roles không phải null và không phải 0, tiếp tục lấy danh sách người dùng
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
                    HttpContext.Session.SetString("Password", model.Password);
                    HttpContext.Session.SetInt32("Id", user.Id);
                    HttpContext.Session.SetString("Username", user.UserName);
                    HttpContext.Session.SetInt32("Roles", user.Roles ?? 0);

                    // Kiểm tra giá trị user.Roles
                    if (user.Roles == null)
                    {
                        return RedirectToAction("CustomerLogin");
                    }
                    else if (user.Roles == 0)
                    {
                        return RedirectToAction("AfterLogin");
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
        public IActionResult ForgetPass(ForgetPassViewModel model)
        {
            // Kiểm tra xem model có hợp lệ không
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.UserName == model.username && u.Email == model.email);

                if (user != null)
                {
                    HttpContext.Session.SetString("resetusername", model.username);
                    HttpContext.Session.SetString("resetemail", model.email);

                    return RedirectToAction("ResetPassword", "Accounts");
                }
                else
                {
                    ModelState.AddModelError("", "Username hoặc email không hợp lệ. Vui lòng thử lại.");
                }
            }

            // Trả về lại view cùng với model để giữ lại dữ liệu và hiển thị lỗi
            return View(model);
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
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var userName = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("Id");
            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Id == userId);
            if (user != null && BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                _context.SaveChanges();
            }
            return RedirectToAction("CustomerLogin");
        }

        public IActionResult QLTK()
        {
            var userName = HttpContext.Session.GetString("Username");
            var id = HttpContext.Session.GetInt32("Id");
            var user = _context.Users.Where(p => p.UserName == userName && p.Id == id).FirstOrDefault();
            return View(user);
        }
        [HttpPost]
        public IActionResult QLTK(User model, IFormFile Hinh)
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
                if (Hinh != null)
                {
                    user.Hinh = MyTool.UploadImageToFolder(Hinh, "Hinh");
                }
                user.Roles = model.Roles;

                _context.SaveChanges();
                return RedirectToAction("Profile");
            }
            catch (Exception ex) { }
            return View(model);  
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Main");
        }
    }
}
