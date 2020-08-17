using Mon2ndSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mon2ndSite.Controllers
{
    public class PollController : Controller
    {
        // GET: Poll
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreerPoll(Poll poll)
        {
            return RedirectToAction("Index");
        }
    }
}