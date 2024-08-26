using AgileCommercee.Data;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NhaCungCap model, IFormFile Logo)
        {
            if (Logo != null)
            {
                model.Logo = MyTool.UploadImageToFolder(Logo, "NhaCC");
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
