using Mon2ndSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mon2ndSite.Controllers
{
    public class RestoController : Controller
    {
        private IDal dal;

        public RestoController() : this(new Dal())
        {
        }

        public RestoController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            List<Resto> listeDesRestos = dal.GetRestos();
            return View(listeDesRestos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Resto resto)
        {
            if (dal.ExistResto(resto.Nom))
            {
                ModelState.AddModelError("Nom", "Ce nom de restaurant existe déjà");
                return View(resto);
            }
            if (!ModelState.IsValid)
                return View(resto);
            dal.NewResto(resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
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
        public ActionResult Edit(Resto resto)
        {
            if (!ModelState.IsValid)
                return View(resto);
            dal.EditResto(resto.Id, resto.Nom, resto.Telephone);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
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
            /*if (id == null)
            {
                return NotFound();
            }
            var resto = await _context.resto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resto == null)
            {
                return NotFound();
            }
            return View(resto);*/
            /*Restos restos = new Restos();
            Resto resto = restos.ObtenirListerestos().FirstOrDefault(c => c.Id == id);
            if (resto != null)
            {
                ViewData.Add("Id", resto.Id);
                ViewData.Add(new KeyValuePair<string, object>("FirstName", resto.FirstName));
                ViewData.Add(new KeyValuePair<string, object>("LastName", resto.LastName));
                ViewData.Add(new KeyValuePair<string, object>("Username", resto.Username));
                ViewData.Add(new KeyValuePair<string, object>("Email", resto.Email));
                return View("Detail");
            }
            return View("NotFound");*/
        }
    }
}