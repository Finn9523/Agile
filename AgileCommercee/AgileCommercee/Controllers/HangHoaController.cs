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

        public IActionResult IndexCus()
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
        // POST: HangHoas/Create
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
                //upload field logo
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

        [HttpGet]
        public IActionResult Details() { return View(); }

        [HttpPost]
        public IActionResult Details(int Id, IFormFile Hinh)
        {
            var product = _context.HangHoas.SingleOrDefault(x => x.MaHh == Id);
            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult DetailsCus() { return View(); }
        [HttpPost]
        public IActionResult DetailsCus(int Id, IFormFile Hinh)
        {
            var product = _context.HangHoas.SingleOrDefault(x => x.MaHh == Id);
            if (product != null)
            {
                return View(product);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var existedProduct = _context.HangHoas.SingleOrDefault(x => x.MaHh == Id);
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            if (existedProduct != null)
            {
                return View(existedProduct);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(HangHoa model, IFormFile Hinh)
        {
            var existedProduct = _context.HangHoas.SingleOrDefault(x => x.MaHh == model.MaHh);
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            try
            {
                existedProduct.TenHh = model.TenHh;
                ViewBag.existedProduct.MaLoai = model.MaLoai;
                existedProduct.MoTaDonVi = model.MoTaDonVi;
                existedProduct.DonGia = model.DonGia;
                existedProduct.NgaySx = model.NgaySx;
                existedProduct.GiamGia = model.GiamGia;
                existedProduct.MoTa = model.MoTa;
                ViewBag.existedProduct.MaNcc = model.MaNcc;
                if (Hinh == null)
                {
                    existedProduct.Hinh = model.Hinh;
                }
                else
                {
                    existedProduct.Hinh = MyTool.UploadImageToFolder(Hinh, "Hinh");
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        { 
            var product = _context.HangHoas.SingleOrDefault(x => x.MaHh == Id);
            if(product != null)
            {
                return View(product);
            }
            return NotFound();
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int Id) 
        {
            var product = _context.HangHoas.SingleOrDefault(x => x.MaHh == Id);
            if(product != null)
            {
                _context.HangHoas.Remove(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}

