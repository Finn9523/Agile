using AgileCommercee.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AgileCommercee.Controllers
{
    public class HangHoasController : Controller
    {
        public readonly AgileStoreContext _context;
        public HangHoasController(AgileStoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("loi", "Còn lỗi");
            }
            var data = _context.HangHoas != null ? _context.HangHoas.ToList() : new List<HangHoa>();
            return View(data);
        }

        public IActionResult Product()
        {
            return RedirectToAction("Index");
        }
        #region Create HangHoa
        // GET: HangHoas/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            return View();
        }
        // POST: HangHoa/Create
        [HttpPost]
        public IActionResult Create(HangHoa model, IFormFile Hinh)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("loi", "Còn lỗi");
            }
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            try
            {
                //upload va cap nhat field logo
                model.Hinh = MyTool.UploadImageToFolder(Hinh, "Hinh");
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        #endregion
        public PartialViewResult Search(string s)
        {
            var results = _context.HangHoas
                .Where(hh => hh.TenHh.Contains(s))
                .ToList();

            return PartialView("_SearchResults", results);
        }
    }
}
