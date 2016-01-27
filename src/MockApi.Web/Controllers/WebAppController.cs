using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MockApi.Web.Repository;

namespace MockApi.Web.Controllers
{
    public class WebAppController : Controller
    {
        private readonly IMockRepository mockRepository;

        public WebAppController(IMockRepository mockRepository)
        {
            this.mockRepository = mockRepository;
        }

        public ActionResult Index(string url)
        {
            url = url ?? "/";

            if (url == "/")
            {
                return RedirectToAction("Index", "Mock");
            }

            if (!url.StartsWith("/"))
            {
                url = $"/{url}";
            }

            var method = GetHttpMethod();

            var route = mockRepository.FindActiveResponse(url, method);

            if (route == null)
            {
                Response.StatusCode = 404;
                var error = new { error = $"Cannot find route config that matches {Request.HttpMethod.ToUpper()} '{url}'" };
                return Json(error, JsonRequestBehavior.AllowGet);
            }

            Response.StatusCode = (int)route.StatusCode;

            return Content(route.Data, "application/json");
        }

        private HttpMethodType GetHttpMethod()
        {
            return (HttpMethodType) Enum.Parse(typeof (HttpMethodType), Request.HttpMethod, true);
        }
    }
}