
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
                    /*
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserImage", user.Hinh);
                    */
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
            if(existedUser != null)
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
    }
}
