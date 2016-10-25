using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Website.Helpers
{
    public static class LabelExtensions
    {
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return LabelFor(html, expression, null, htmlAttributes);
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return LabelHelper(html,
                               ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                               ExpressionHelper.GetExpressionText(expression),
                               labelText,
                               htmlAttributes);
        }

        private static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, string labelText = null, object htmlAttributes = null)
        {
            var innerText = labelText;
            if (innerText == null)
            {
                var displayName = metadata.DisplayName;
                if (displayName == null)
                {
                    var propertyName = metadata.PropertyName;
                    if (propertyName == null)
                    {
                        innerText = htmlFieldName.Split(new[] { '.' }).Last();
                    }
                    else
                    {
                        innerText = propertyName;
                    }
                }
                else
                    innerText = displayName;
            }

            if (string.IsNullOrEmpty(innerText))
                return MvcHtmlString.Empty;

            var tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for",
                                      TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));

            if (htmlAttributes != null)
            {
                tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            tagBuilder.SetInnerText(innerText);

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}