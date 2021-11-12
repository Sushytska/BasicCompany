using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Managers;

namespace BasicCompany.Feature.Blog.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult List()
        {
            var blogRoot = RenderingContext.Current.ContextItem;
            var blog = ItemManager.GetChildren(blogRoot).Where(x => x.TemplateID == Templates.Post.Id).ToList();
            return View(blog);
        }
    }
}