using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Sonovate.BackEnd
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
    }
}