using AgileCommercee.Data;
using AgileCommercee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("loi", "Còn lỗi");
            }
            var data = _context.HangHoas != null ? _context.HangHoas.ToList() : new List<HangHoa>();
            return View(data);
        }

        public IActionResult IndexCus()
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
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
                model.Hinh = MyTool.UploadImageToFolder(Hinh, "HangHoas");
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
        [HttpGet]
        public IActionResult Search() { return View(); }
        [HttpPost]
        public IActionResult Search(string? keyword)
        {
            var data = _context.HangHoas.AsQueryable();
            if (keyword != null)
            {
                data = data.Where(p => p.TenHh.Contains(keyword));
            }
            var result = data.Select(hh => new KetQuaTimKiemVM
            {
                MaHh = hh.MaHh,
                TenHh = hh.TenHh,
                DonGia = hh.DonGia.Value,
                NgaySx = hh.NgaySx,
                MaLoai = hh.MaLoai,
            }).ToList();

            return PartialView("TimKiemPartial", result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.HangHoas.FirstOrDefaultAsync(m => m.MaHh == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> DetailsCus(int? id)
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.HangHoas.FirstOrDefaultAsync(m => m.MaHh == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: HangHoas/Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            var existedProduct = _context.HangHoas.SingleOrDefault(x => x.MaHh == id);
            if (existedProduct != null)
            {
                return View(existedProduct);
            }
            return NotFound();
        }
        // POST: HangHoas/Edit
        [HttpPost]
        public IActionResult Edit(HangHoa modelEdit, IFormFile Hinh)
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            var existedProduct = _context.HangHoas.SingleOrDefault(x => x.MaHh == modelEdit.MaHh);
            try
            {
                //Edit
                existedProduct.TenHh = modelEdit.TenHh;
                existedProduct.MoTa = modelEdit.MoTa;
                existedProduct.MaLoai = modelEdit.MaLoai;
                existedProduct.DonGia = modelEdit.DonGia;
                existedProduct.NgaySx = modelEdit.NgaySx;
                existedProduct.GiamGia = modelEdit.GiamGia;
                existedProduct.SoLanXem = modelEdit.SoLanXem;
                existedProduct.MoTa = modelEdit.MoTa;
                existedProduct.MaNcc = modelEdit.MaNcc;

                if (Hinh == null)
                {
                    existedProduct.Hinh = modelEdit.Hinh;
                }
                else
                {
                    existedProduct.Hinh = MyTool.UploadImageToFolder(Hinh, "HangHoas");
                }
                //Save
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

