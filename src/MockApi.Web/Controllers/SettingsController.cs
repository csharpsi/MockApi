using System.Web.Mvc;
using LiteDB;
using MockApi.Web.Models;
using MockApi.Web.Repository;
using MockApi.Web.ViewModels;

namespace MockApi.Web.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISettingRepository settingRepository;

        public SettingsController(ISettingRepository settingRepository)
        {
            this.settingRepository = settingRepository;
        }

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
            if (!TryCreate(viewModel.Setting))
            {
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var objectId = new ObjectId(id);

            var setting = settingRepository.FindById(objectId);

            if (setting == null)
            {
                return HttpNotFound(GetNotFoundMessage(id));
            }

            return View(new SettingViewModel(setting));
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
                return HttpNotFound(GetNotFoundMessage(id));
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
                return HttpNotFound(GetNotFoundMessage(id));
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

        [HttpGet]
        public ActionResult Copy(string id)
        {
            var setting = settingRepository.FindById(new ObjectId(id));

            if (setting == null)
            {
                return HttpNotFound(GetNotFoundMessage(id));
            }

            return View(new SettingViewModel(setting));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(SettingViewModel viewModel)
        {
            if (!TryCreate(viewModel.Setting))
            {
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        private static string GetNotFoundMessage(string id)
        {
            return $"Cannot find route setting with id '{id}'";
        }

        private bool TryCreate(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            var existing = settingRepository.FindRoute(setting.Route, setting.StatusCode, setting.HttpMethod);

            if (existing != null)
            {
                ModelState.AddModelError("", $"A route setting already exists for the path '{setting.Route}', the status '{setting.StatusCode}' and the method '{setting.HttpMethod}'");
                return false;
            }

            settingRepository.Create(setting);
            return true;
        }
    }
}