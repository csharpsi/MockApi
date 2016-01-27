using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;
using Humanizer;

namespace MockApi.Web
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString AddClassIfInvalid<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var fullHtmlFieldName = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
            var state = helper.ViewData.ModelState[fullHtmlFieldName];

            if (state == null)
            {
                return MvcHtmlString.Empty;
            }

            if (state.Errors.Count == 0)
            {
                return MvcHtmlString.Empty;
            }

            return new MvcHtmlString("has-error");
        }

        public static SelectList ListOfStatusCodes(this HtmlHelper helper, HttpStatusCode? selectedValue = null)
        {
            var items = typeof(HttpStatusCode).GetEnumNames().Select(value => new SelectListItem { Text = HttpStatusText(value), Value = value });
            return new SelectList(items, nameof(SelectListItem.Value), nameof(SelectListItem.Text), selectedValue?.ToString());
        }

        public static SelectList ListOfHttpMethods(this HtmlHelper helper, HttpMethodType? selectedMethod = null)
        {
            var items = typeof(HttpMethodType).GetEnumNames().Select(x => new SelectListItem { Text = x.ToUpper(), Value = x });
            return new SelectList(items, nameof(SelectListItem.Value), nameof(SelectListItem.Text), selectedMethod?.ToString());
        }

        public static string ToFriendlyString(this HttpStatusCode statusCode)
        {
            return FriendlyStatusCode(statusCode);
        }

        private static string HttpStatusText(string input)
        {
            return FriendlyStatusCode((HttpStatusCode) Enum.Parse(typeof (HttpStatusCode), input));
        }

        private static string FriendlyStatusCode(HttpStatusCode statusCode)
        {
            var code = (int)statusCode;
            var text = statusCode.ToString().Humanize().Transform(To.TitleCase);

            return $"{text} ({code})";
        }
    }
}