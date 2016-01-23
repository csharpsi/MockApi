using System.Web.Mvc;
using LiteDB;
using Sonovate.BackEnd.Models;
using Sonovate.BackEnd.Repository;
using Sonovate.BackEnd.ViewModels;

namespace Sonovate.BackEnd.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingRepository settingRepository;

        public SettingsController(ISettingRepository settingRepository)
        {
            this.settingRepository = settingRepository;
        }

        // GET: Config
        public ActionResult Index()
        {
            var list = settingRepository.GetAll();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new SettingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SettingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            settingRepository.Create(viewModel.Setting);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var objectId = new ObjectId(id);

            var setting = settingRepository.FindById(objectId);

            if (setting == null)
            {
                return HttpNotFound($"Cannot find route config with id '{id}'");
            }

            return View(new SettingViewModel {Setting = setting});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, SettingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var objectId = new ObjectId(id);

            if (!settingRepository.Update(objectId, viewModel.Setting))
            {
                return HttpNotFound($"Cannot find setting with id '{viewModel.Setting?.Id}'");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Remove(string id)
        {
            var objectId = new ObjectId(id);

            var setting = settingRepository.FindById(objectId);

            if (setting == null)
            {
                return HttpNotFound($"Cannot find route config with id '{id}'");
            }

            return View(setting);
        }

        [HttpPost]
        [ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveConfirmed(string id)
        {
            settingRepository.Delete(new ObjectId(id));
            return RedirectToAction("Index");
        }
    }
}