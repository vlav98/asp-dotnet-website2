using Mon2ndSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mon2ndSite.Controllers
{
    public class VoteController : Controller
    {
        private IDal dal;

        public VoteController() : this(new Dal())
        {
        }

        public VoteController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index(int id)
        {
            RestoVoteViewModel viewModel = new RestoVoteViewModel
            {
                ListeDesResto = dal.GetRestos().Select(r => new RestoCheckBoxViewModel { Id = r.Id, NomEtTelephone = string.Format("{0} ({1})", r.Nom, r.Telephone) }).ToList()
            };
            if (dal.Voted(id, Request.Browser.Browser))
            {
                return RedirectToAction("AfficheResultat", new { id = id });
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestoVoteViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            User user = dal.GetUser(Request.Browser.Browser);
            if (user == null)
                return new HttpUnauthorizedResult();
            foreach (RestoCheckBoxViewModel restaurantCheckBoxViewModel in viewModel.ListeDesResto.Where(r => r.EstSelectionne))
            {
                dal.AddVote(id, restaurantCheckBoxViewModel.Id, user.Id);
            }
            return RedirectToAction("AfficheResultat", new { id = id });
        }

        public ActionResult AfficheResultat(int id)
        {
            if (!dal.Voted(id, Request.Browser.Browser))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Results> results = dal.GetResults(id);
            return View(results.OrderByDescending(r => r.NombreDeVotes).ToList());
        }
    }
}