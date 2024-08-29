using AgileCommercee.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgileCommercee.Controllers
{
    public class NhaCungCapsController : Controller
    {
        private readonly AgileStoreContext _Context;
        public NhaCungCapsController(AgileStoreContext context)
        {
            _Context = context;
        }

        public IActionResult Index()
        {
            var data = _Context.NhaCungCaps != null ? _Context.NhaCungCaps.ToList() : new List<NhaCungCap>();
            return View(data);
        }

        // GET: NhaCungCaps/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        // POST: NhaCungCaps/Create
        [HttpPost]
        public IActionResult Create(NhaCungCap model, IFormFile FileLogo)
        {
            try
            {
                //upload va cap nhat field Logo
                model.Logo = MyTool.UploadImageToFolder(FileLogo, "NhaCungCaps");
                _Context.Add(model);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        #region Edit Supplier - Nha Cung Cap
        // GET: NhaCungCaps/Edit
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var existedSupplier = _Context.NhaCungCaps.SingleOrDefault(x => x.MaNcc == id);
            if (existedSupplier != null)
            {
                return View(existedSupplier);
            }
            return NotFound();
        }
        // POST: NhaCungCaps/Edit
        [HttpPost]
        public IActionResult Edit(NhaCungCap modelEdit, IFormFile FileLogo)
        {
            var existedSupplier = _Context.NhaCungCaps.SingleOrDefault(x => x.MaNcc == modelEdit.MaNcc);
            try
            {
                //Edit
                existedSupplier.TenCongTy = modelEdit.TenCongTy;
                existedSupplier.NguoiLienLac = modelEdit.NguoiLienLac;
                existedSupplier.Email = modelEdit.Email;
                existedSupplier.DienThoai = modelEdit.DienThoai;
                existedSupplier.DiaChi = modelEdit.DiaChi;
                existedSupplier.MoTa = modelEdit.MoTa;
                if (FileLogo == null)
                {
                    existedSupplier.Logo = modelEdit.Logo;
                }
                else
                {
                    existedSupplier.Logo = MyTool.UploadImageToFolder(FileLogo, "NhaCungCaps");
                }
                //Save
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        #endregion Edit Supplier - Nha Cung Cap
        #region Delete Supplier - Nha Cung Cap
        // GET: Delete/NhaCungCaps/5
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var supplier = await _Context.NhaCungCaps.FirstOrDefaultAsync(m => m.MaNcc == id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Delete/NhaCungCaps/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var supplier = await _Context.NhaCungCaps.FindAsync(id);
            if (supplier != null)
            {
                _Context.NhaCungCaps.Remove(supplier);
            }

            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion Delete Supplier - Nha Cung Cap

    }

}
