using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CoranWordSearch.Controllers
{
    public class HomeController : Controller
    {
        private Models.Elasticsearch elasticSearch = new Models.Elasticsearch();
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
            var words = elasticSearch.GetWords(word);
            return Json(words, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSourates(string word)
        {
            var versetWords = elasticSearch.GetWords(word);
            var versets = elasticSearch.GetVersets(word);
            var sourates = elasticSearch.GetSourates(versetWords);
            return Json(new { Sourates = sourates.Select(x=> new Sourate { Versets = versets.Where(v=>v.SourateId == x.SourateId).ToList(), SourateId = x.SourateId, Name = x.Name, VersetsCount = x.VersetsCount}), VersetWords = versetWords, Versets = versets }, JsonRequestBehavior.AllowGet);
        }
    }
}