using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CRME.Models;
using System;
using System.Net;

namespace CustomHelpers
{
    public static class CustomHelpers
    {
        public static IHtmlString Image(this HtmlHelper helper, string src, string alt, object htmlAttributes)
        {
            
            var path = "~/Upload/Sistema/default.png";
            WebRequest request = WebRequest.Create(System.Web.Hosting.HostingEnvironment.MapPath(src));
            try
            {
                if (request.GetResponse() != null)
                {
                    path = src;
                }
            }
            catch (Exception)
            {
            }
           
            TagBuilder tb = new TagBuilder("img");
            tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            tb.Attributes.Add("alt", alt);
            tb.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }
        public static IHtmlString Loading(this HtmlHelper helper, string src, string alt, object htmlAttributes)
        {
            // Build <img> tag
            TagBuilder tb = new TagBuilder("img");
            // Add "src" attribute
            if (src.Contains("holder.js"))
            {
                tb.Attributes.Add("src", "Scripts/aplicacion/" + src);
            }
            else
            {
                tb.Attributes.Add("src", VirtualPathUtility.ToAbsolute(src));
            }
            // Add "alt" attribute
            tb.Attributes.Add("alt", alt);
            tb.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            // return MvcHtmlString. This class implements IHtmlString
            // interface. IHtmlStrings will not be html encoded.
            return new MvcHtmlString(tb.ToString(TagRenderMode.SelfClosing));
        }

        public static IHtmlString ValidationSummaryOnlyMessage(this HtmlHelper helper, string titulo, string mensaje, object htmlAttributes)
        {
            TagBuilder div = new TagBuilder("div");
            div.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            TagBuilder strong = new TagBuilder("strong");
            strong.InnerHtml += titulo;
            div.InnerHtml += strong.ToString(TagRenderMode.Normal) + " " + mensaje;
            if (!helper.ViewData.ModelState.IsValid)
            {
                div.AddCssClass("show");
            }
            else
            {
                div.AddCssClass("hide");
            }
            return new MvcHtmlString(div.ToString(TagRenderMode.Normal));
        }        
    }
}