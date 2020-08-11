using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mon2ndSite.Models
{
    public class Dal : IDal
    {
        private BddContext bdd;

        public Dal()
        {
            bdd = new BddContext();
        }

        public List<Resto> GetRestos()
        {
            return bdd.Restos.ToList();
        }

        public void Dispose()
        {
            bdd.Dispose();
        }

        public void NewRestaurant(string Name, string Phone)
        {
            bdd.Restos.Add(new Resto { Nom = Name, Telephone = Phone });
            bdd.SaveChanges();
        }

        public void EditRestaurant(int id, string Name, string Phone)
        {
            Resto foundResto = bdd.Restos.FirstOrDefault(resto => resto.Id == id);
            if (foundResto != null)
            {
                foundResto.Nom = Name;
                foundResto.Telephone = Phone;
                bdd.SaveChanges();
            }
        }
    }
}