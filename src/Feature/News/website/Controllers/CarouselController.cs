using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasicCompany.Feature.News.Models;
using Sitecore.Data.Fields;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;

namespace BasicCompany.Feature.News.Controllers
{
    public class CarouselController : Controller
    {
        // GET: Carousel
        public ActionResult Index()
        {
            var model = new Carousel();
            List<Slide> slides = new List<Slide>();

            var dataSource = RenderingContext.Current?.Rendering.Item;
            MultilistField slidesField = dataSource.Fields["Slides"];

            if (slidesField?.Count > 0)
            {
                var slideItems = slidesField.GetItems();

                foreach (var slideItem in slideItems)
                {
                    var titleField = slideItem.Fields["Title"];
                    var title = titleField?.Value;

                    var subTitle = new MvcHtmlString(FieldRenderer.Render
                        (slideItem, "Sub Title"));

                    var image = new MvcHtmlString(FieldRenderer.Render
                        (slideItem, "Image"));

                    var callToAction = new MvcHtmlString(FieldRenderer.Render
                        (slideItem, "Call To Action"
                        , "class=btn animated fadeInUp"));

                    slides.Add(new Slide
                    {
                        Title = title,
                        SubTitle = subTitle,
                        Image = image,
                        CallToAction = callToAction
                    });
                }
                model.Slides = slides;
            }
            return View(model);
        }
    }
}