using Sitecore.Mvc.Presentation;
using Sitecore.Data.Managers;
using System.Linq;
using System.Web.Mvc;

namespace BasicCompany.Feature.News.Controllers
{
    public class NewsController : Controller
    {

        public ActionResult ListNews()
        {
            var newsRoot = RenderingContext.Current.ContextItem;
            var news = ItemManager.GetChildren(newsRoot).ToList();
            return View(news);
        }
    }
}
