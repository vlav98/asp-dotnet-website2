using Mon2ndSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mon2ndSite.Controllers
{
    public class AccueilController : Controller
    {
        // GET: Accueil
        private IDal dal;

        public AccueilController() : this(new Dal())
        {
        }

        public AccueilController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            int idSondage = dal.NewPoll();
            return RedirectToAction("Index", "Vote", new { id = idSondage });
        }
    }
}