using AgileCommercee.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgileCommercee.Models;

namespace AgileCommercee.Controllers
{
    public class HangHoasController : Controller
    {
        public readonly AgileStoreContext _context;
        public HangHoasController(AgileStoreContext context)
        {
            _context = context;
        }

        // GET: HangHoas
        public IActionResult Index()
        {
            var data = _context.HangHoas
                .Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
                .ToList();
            return View(data);
        }

        public IActionResult Product()
        {
            return RedirectToAction("Index");
        }
        #region Create Product - Hang Hoa
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
        #endregion Create Product - Hang Hoa
        #region Search Product - Hang Hoa
        public PartialViewResult Search(string s)
        {
            var results = _context.HangHoas
                .Where(hh => hh.TenHh.Contains(s))
                .ToList();

            return PartialView("_SearchResults", results);
        }
        #endregion Search Product - Hang Hoa
        #region Edit Product - Hang Hoa
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
        #endregion Edit Product - Hang Hoa
        #region Delete Product - Hang Hoa
        // GET: HangHoas/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.HangHoas
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        // POST: Loais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Loais = new SelectList(_context.Loais.ToList(), "MaLoai", "TenLoai");
            ViewBag.NhaCungCaps = new SelectList(_context.NhaCungCaps.ToList(), "MaNcc", "TenCongTy");
            var product = await _context.HangHoas.FindAsync(id);
            if (product != null)
            {
                _context.HangHoas.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion Delete Product - Hang Hoa
        #region Details Product - Hang Hoa
        // GET: Loais/Details/5
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
        #endregion Details Product - Hang Hoa
    }
}
