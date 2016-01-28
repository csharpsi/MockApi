using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MockApi.Web.Models;
using MockApi.Web.Repository;
using MockApi.Web.ViewModels;

namespace MockApi.Web.Controllers
{
    public class MockController : Controller
    {
        private readonly IMockRepository mockRepository;

        public MockController(IMockRepository mockRepository)
        {
            this.mockRepository = mockRepository;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            return View(mockRepository.ListAll());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Mock());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Mock mock)
        {
            if (!ModelState.IsValid)
            {
                return View(mock);
            }

            var existing = mockRepository.Find(mock.Path, mock.Verb);

            if (existing != null)
            {
                ModelState.AddModelError("", $"There seems to be an existing mock with the path '{mock.Verb.ToString().ToUpper()} {mock.Path}'");
                return View(mock);
            }

            mockRepository.Create(mock);

            return RedirectToAction("Edit", new {id = mock.Id});
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            return View(mock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Mock model)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            // otherwise Responses gets overwritten. 
            model.Responses = mock.Responses;

            mockRepository.Update(id, model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditResponse(string id, int code)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            var response = mock.Responses.FirstOrDefault(x => (int) x.StatusCode == code);

            if (response == null)
            {
                return ResponseNotFound(id, (HttpStatusCode) code);
            }

            var viewModel = new MockResponseViewModel(mock, response);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditResponse(string id, int code, MockResponseViewModel model)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            var response = mock.Responses.FirstOrDefault(x => (int)x.StatusCode == code);

            if (response == null)
            {
                return ResponseNotFound(id, (HttpStatusCode)code);
            }

            model.Mock = mock;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var index = mock.Responses.FindIndex(x => (int) x.StatusCode == code);
            mock.Responses[index] = model.MockResponse;

            mockRepository.Update(id, mock);

            return RedirectToAction("Edit", new {id});
        }

        [HttpGet]
        public ActionResult NewResponse(string id)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            var viewModel = new MockResponseViewModel(mock, new MockResponse());

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewResponse(string id, MockResponseViewModel model)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            model.Mock = mock;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (mock.Responses.Any(x => x.StatusCode == model.MockResponse.StatusCode))
            {
                ModelState.AddModelError("", $"A response already exists with the status code {model.MockResponse.StatusCode.ToFriendlyString()}");
                return View(model);
            }

            mock.Responses.Add(model.MockResponse);

            if (mock.Responses.Count == 1)
            {
                mock.ActiveStatusCode = model.MockResponse.StatusCode;
            }

            mockRepository.Update(id, mock);

            return RedirectToAction("Edit", new {id});
        }

        [HttpGet]
        public ActionResult MakeActive(string id, int code)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            var response = mock.Responses.FirstOrDefault(x => (int)x.StatusCode == code);

            if (response == null)
            {
                return ResponseNotFound(id, (HttpStatusCode)code);
            }

            mock.ActiveStatusCode = response.StatusCode;

            mockRepository.Update(id, mock);

            return RedirectToAction("Edit", new {id});
        }

        [HttpGet]
        public ActionResult Remove(string id)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            return View(mock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Remove")]
        public ActionResult RemoveConfirmed(string id)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            mockRepository.Delete(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult RemoveStatus(string id, HttpStatusCode code)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            var response = mock.Responses.FirstOrDefault(x => x.StatusCode == code);

            if (response == null)
            {
                return ResponseNotFound(id, code);
            }
            
            return View(new MockResponseViewModel(mock, response));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("RemoveStatus")]
        public ActionResult RemoveStatusConfirm(string id, HttpStatusCode code)
        {
            var mock = mockRepository.FindById(id);

            if (mock == null)
            {
                return MockNotFound(id);
            }

            var response = mock.Responses.FirstOrDefault(x => x.StatusCode == code);

            if (response == null)
            {
                return ResponseNotFound(id, code);
            }

            mock.Responses = mock.Responses.Where(x => x.StatusCode != code).ToList();

            mockRepository.Update(id, mock);

            return RedirectToAction("Edit", new {id});
        }

        private HttpNotFoundResult MockNotFound(string id)
        {
            return HttpNotFound($"Cannot find a mock with the id '{id}'");
        }

        private HttpNotFoundResult ResponseNotFound(string id, HttpStatusCode code)
        {
            return HttpNotFound($"Cannot find a response with the status code '{code.ToFriendlyString()}' on the mock with id '{id}'");
        }
    }
}