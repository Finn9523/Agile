using AgileCommercee.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AgileCommercee.Controllers
{
    public class LoaisController : Controller
    {
        private readonly AgileStoreContext _Context;
        public LoaisController(AgileStoreContext context)
        {
            _Context = context;
        }
        public IActionResult Index()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("loi", "Còn lỗi");
            }
            var data = _Context.Loais != null ? _Context.Loais.ToList() : new List<Loai>();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Loai model, IFormFile FileLogo)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("loi", "Còn lỗi");
            }
            if (FileLogo != null)
            {
                model.Hinh = MyTool.UploadImageToFolder(FileLogo, "Hinh");
            }
            try
            {
                _Context.Add(model);
                _Context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            return View();
        }
    }
}
