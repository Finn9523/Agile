
using AgileCommercee.Data;
using AgileCommercee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using NuGet.Packaging.Signing;

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
            if (userRoles == 1 && userId != null)
            {
                return RedirectToAction("CustomerLogin", "Accounts");
            }
            else if (userRoles == 0 && userId != null)
            {
                return RedirectToAction("AfterLogin", "Accounts");
            }

            var products = _context.HangHoas.ToList();
            ViewBag.Products = products;
            return View(products);
        }
        public IActionResult AfterLogin()
        {
            var products = _context.HangHoas.ToList();
            return View(products);
        }

        public IActionResult CustomerLogin()
        {
            var products = _context.HangHoas.ToList();
            return View(products);
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
                    Hinh = model.Hinh,
                    Roles = 1
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
                    Roles = 0
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
                    HttpContext.Session.SetString("Password", model.Password);
                    HttpContext.Session.SetInt32("Id", user.Id);
                    HttpContext.Session.SetString("Username", user.UserName);
                    HttpContext.Session.SetInt32("Roles", user.Roles ?? 0);
                    if (user.Roles == 0)
                    {
                        return RedirectToAction("AfterLogin");
                    }
                    else if (user.Roles == 1)
                    {
                        return RedirectToAction("CustomerLogin");
                    }
                }
                ModelState.AddModelError("", "Hãy thử đăng nhập lại.");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var existedUser = _context.Users.SingleOrDefault(x => x.Id == id);
            if (existedUser != null)
            {
                return View(existedUser);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Account model, IFormFile FileLogo)
        {
            var existedUser = _context.Users.SingleOrDefault(x => x.Id == model.Id);
            if (existedUser != null)
            {
                try
                {
                    existedUser.FullName = model.FullName;
                    existedUser.UserName = model.UserName;
                    existedUser.Address = model.Address;
                    existedUser.PhoneNumber = model.PhoneNumber;
                    if (FileLogo != null)
                    {
                        existedUser.Hinh = MyTool.UploadImageToFolder(FileLogo, "Hinh");
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                }
            }
            return NotFound();
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
            var username = HttpContext.Session.GetString("Username");
            ViewBag.UserName = username;
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = HttpContext.Session.GetString("Username");
                var userId = HttpContext.Session.GetInt32("Id");
                var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.Id == userId);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash))
                {
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
                    _context.SaveChanges();
                }
                return RedirectToAction("Main");
            }
            return View(model);
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

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var product = _context.Users.SingleOrDefault(x => x.Id == Id);
            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int Id)
        {
            var user = _context.Users.SingleOrDefault(x => x.Id == Id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
