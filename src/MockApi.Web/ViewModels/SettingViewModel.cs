using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Humanizer;
using MockApi.Web.Models;

namespace MockApi.Web.ViewModels
{
    public class SettingViewModel
    {
        public Setting Setting { get; set; }
        public List<SelectListItem> HttpMethods { get; set; }
        public List<SelectListItem> HttpStatusCodes { get; set; }

        public SettingViewModel()
        {
            Setting = new Setting();
            HttpMethods = typeof (HttpMethodType).GetEnumNames().Select(x => new SelectListItem {Text = x.ToUpper(), Value = x}).ToList();
            HttpStatusCodes = typeof (HttpStatusCode)
                .GetEnumNames()
                .Select(x => new SelectListItem {Text = HttpStatusText(x), Value = x.ToString()})
                .ToList();
        }

        public SettingViewModel(Setting model)
            :this()
        {
            Setting = model;
        }

        private static string HttpStatusText(string input)
        {
            var statusCode = (int)(HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), input);
            var text = input.Humanize().Transform(To.TitleCase);

            return $"{text} ({statusCode})";
        }
    }
}