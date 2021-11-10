using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasicCompany.Feature.News.Models
{
    public class Slide
    {
        public string Title { get; set; }
        public MvcHtmlString SubTitle { get; set; }
        public MvcHtmlString Image { get; set; }
        public MvcHtmlString CallToAction { get; set; }
    }
}