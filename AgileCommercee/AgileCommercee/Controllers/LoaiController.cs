using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var data = _Context.Loais != null ? _Context.Loais.ToList() : new List<Loai>();
            return View(data);
        }
        #region Details Category - Loai
        // GET: Loais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var category = await _Context.Loais.FirstOrDefaultAsync(m => m.MaLoai == id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        #endregion Details Category - Loai
        #region Create Category - Loai
        // GET: Loai/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Loai/Create
        [HttpPost]
        public IActionResult Create(Loai model, IFormFile FileLogo)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("loi", "Còn lỗi");
            }
            /*
            if (FileLogo != null)
            {
                model.Hinh = MyTool.UploadImageToFolder(FileLogo, "Hinh");
            }
            */
            try
            {
                model.Hinh = MyTool.UploadImageToFolder(FileLogo, "Loais");
                _Context.Add(model);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
            }
            return View();
        }
        #endregion Create Category - Loai

        #region Edit Category - Loai
        // GET: Loais/Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var existedCategory = _Context.Loais.SingleOrDefault(x => x.MaLoai == id);
            if (existedCategory != null)
            {
                return View(existedCategory);
            }
            return NotFound();
        }
        // POST: Loais/Edit
        [HttpPost]
        public IActionResult Edit(Loai modelEdit, IFormFile FileLogo)
        {
            var existedCategory = _Context.Loais.SingleOrDefault(x => x.MaLoai == modelEdit.MaLoai);
            try
            {
                //Edit
                existedCategory.TenLoai = modelEdit.TenLoai;
                existedCategory.MoTa = modelEdit.MoTa;
                if(FileLogo == null)
                {
                    existedCategory.Hinh = modelEdit.Hinh;
                }
                else
                {
                    existedCategory.Hinh = MyTool.UploadImageToFolder(FileLogo, "Loais");
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
        #endregion Edit Category - Loai

        #region Delete Category - Loai
        // GET: Loais/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _Context.Loais
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        // POST: Loais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _Context.Loais.FindAsync(id);
            if (category != null)
            {
                _Context.Loais.Remove(category);
            }

            await _Context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion Delete Category - Loai
        /*
        private bool CategoryExists(int id)
        {
            return _Context.Loais.Any(e => e.MaLoai == id);
        }
        */
    }
}
