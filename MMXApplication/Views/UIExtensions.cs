using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace MMXApplication.Views {
    public static class UIExtensions {

        // Extension method 
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, object routeValues, string imagePath, string alt) {     
            var url = new UrlHelper(html.ViewContext.RequestContext);      
            // build the <img> tag     
            var imgBuilder = new TagBuilder("img");     
            imgBuilder.MergeAttribute("src", url.Content(imagePath));     
            imgBuilder.MergeAttribute("alt", alt);     
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);      
            // build the <a> tag     
            var anchorBuilder = new TagBuilder("a");     
            anchorBuilder.MergeAttribute("href", url.Action(action, routeValues));     
            anchorBuilder.InnerHtml = imgHtml; 
            // include the <img> tag inside     
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);      
            return MvcHtmlString.Create(anchorHtml); 
        }

        public static MvcHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName,
            object routeValues, AjaxOptions options, object htmlAttributes) {
            var tagBuilder = new TagBuilder("img");
            tagBuilder.MergeAttribute("src", imageUrl);
            tagBuilder.MergeAttribute("alt", altText);
            var link = helper.ActionLink("[replace]", actionName, routeValues, options, htmlAttributes).ToHtmlString();            
            return MvcHtmlString.Create(link.Replace("[replace]", tagBuilder.ToString(TagRenderMode.SelfClosing)));

        }
    }
}