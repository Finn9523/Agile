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
        // GET: Loai/Edit
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var existedSupplier = _Context.Loais.SingleOrDefault(x => x.MaLoai == id);
            if (existedSupplier != null)
            {
                return View(existedSupplier);
            }
            return NotFound();
        }
        // POST: Loai/Edit
        [HttpPost]
        public IActionResult Edit(Loai modelEdit, IFormFile FileLogo)
        {
            var existedSupplier = _Context.Loais.SingleOrDefault(x => x.MaLoai == modelEdit.MaLoai);
            try
            {
                //Edit
                existedSupplier.TenLoai = modelEdit.TenLoai;
                existedSupplier.MoTa = modelEdit.MoTa;
                if(FileLogo == null)
                {
                    existedSupplier.Hinh = modelEdit.Hinh;
                }
                else
                {
                    existedSupplier.Hinh = MyTool.UploadImageToFolder(FileLogo, "Loais");
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
        // GET: Loai/Delete
        [HttpGet]
        public IActionResult Delete (int id)
        {
            var existedSupplier = _Context.Loais.SingleOrDefault(x => x.MaLoai == id);
            if(existedSupplier != null)
            {
                return View(existedSupplier);
            }
            return NotFound();
        }
        // POST: Loais/Delete
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            var existedSuppliers = _Context.Loais.SingleOrDefault(x => x.MaLoai == id);
            if (existedSuppliers != null)
            {
                _Context.Loais.Remove(existedSuppliers);
                _Context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        #endregion Delete Category - Loai
    }
}
