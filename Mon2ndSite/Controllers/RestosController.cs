using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mon2ndSite.Models;

namespace Mon2ndSite.Controllers
{
    public class RestaurantController : Controller
    {
        private IDal dal;

        public RestaurantController() : this(new Dal())
        {
        }

        public RestaurantController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            List<Resto> listeDesRestaurants = dal.GetRestos();
            return View(listeDesRestaurants);
        }

        public ActionResult CreerRestaurant()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreerRestaurant(Resto resto)
        {
            if (dal.ExistRestaurant(resto.Nom))
            {
                ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
                return View(resto);
            }
            if (!ModelState.IsValid)
                return View(resto);
            dal.NewRestaurant(resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }

        public ActionResult ModifierRestaurant(int? id)
        {
            if (id.HasValue)
            {
                Resto restaurant = dal.GetRestos().FirstOrDefault(r => r.Id == id.Value);
                if (restaurant == null)
                    return View("Error");
                return View(restaurant);
            }
            else
                return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ModifierRestaurant(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);
            dal.EditRestaurant(resto.Id, resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }
    }
}
