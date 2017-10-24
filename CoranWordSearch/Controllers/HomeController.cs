using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoranWordSearch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetVersetsWords(string word)
        {
            var words = Models.Elasticsearch.GetWords(word);
            return Json(words, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSourates(string word)
        {
            var words = Models.Elasticsearch.GetWords(word);
            var sourates = Models.Elasticsearch.GetSourates(words);
            return Json(new { Sourates = sourates, VersetWords = words }, JsonRequestBehavior.AllowGet);
        }
    }
}