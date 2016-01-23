using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Sonovate.BackEnd.Repository;

namespace Sonovate.BackEnd.Controllers
{
    public class WebAppController : Controller
    {
        private readonly ISettingRepository settingRepository;

        public WebAppController(ISettingRepository settingRepository)
        {
            this.settingRepository = settingRepository;
        }

        public ActionResult Index(string url, HttpStatusCode status = HttpStatusCode.OK)
        {
            url = url ?? "/";

            if (url == "/")
            {
                return RedirectToAction("Index", "Settings");
            }

            if (!url.StartsWith("/"))
            {
                url = $"/{url}";
            }

            var method = GetHttpMethod();

            var route = settingRepository.FindRoute(url, status, method);

            Response.StatusCode = (int)status;

            if (route == null)
            {
                return TryFuzzyFind(url, status, method);
            }

            return Content(route.ResponseJson, "application/json");
        }

        private ActionResult TryFuzzyFind(string url, HttpStatusCode statusCode, HttpMethodType httpMethod)
        {
            var routes = settingRepository.FindAllParameterisedRoutes(statusCode, httpMethod);

            var matches = (from route in routes
                let distance = route.Route.DiceCoefficient(url)
                where distance > .6d
                select new {Route = route, Distance = distance}).ToList();

            if (!matches.Any())
            {
                Response.StatusCode = 404;
                var error = new {error = $"Cannot find route config that matches {Request.HttpMethod.ToUpper()} '{url}' for the status code '{statusCode}'"};
                return Json(error, JsonRequestBehavior.AllowGet);
            }

            if (matches.Count > 1)
            {
                throw new DuplicateNameException($"The given url '{url}' was a fuzzy match to {matches.Count} routes.");
            }

            var content = matches.First().Route.ResponseJson;
            return Content(content, "application/json");
        }

        private HttpMethodType GetHttpMethod()
        {
            return (HttpMethodType) Enum.Parse(typeof (HttpMethodType), Request.HttpMethod, true);
        }
    }
}