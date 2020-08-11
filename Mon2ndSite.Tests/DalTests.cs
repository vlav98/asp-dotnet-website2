using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mon2ndSite.Models;

namespace Mon2ndSite.Tests
{    
    [TestClass]
    public class DalTests
    {
        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            IDatabaseInitializer<BddContext> init = new DropCreateDatabaseAlways<BddContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new BddContext());
        }

        [TestMethod]
        public void NewRestaurant_AvecUnNouveauRestaurant_GetRestaurantsRenvoitBienLeRestaurant()
        {
            using (IDal dal = new Dal())
            {
                dal.NewRestaurant("La bonne fourchette", "01 02 03 04 05");
                List<Resto> restos = dal.GetRestos();

                Assert.IsNotNull(restos);
                Assert.AreEqual(1, restos.Count);
                Assert.AreEqual("La bonne fourchette", restos[0].Nom);
                Assert.AreEqual("01 02 03 04 05", restos[0].Telephone);
            }
        }

        [TestMethod]
        public void EditRestaurant_CreationDUnNouveauRestaurantEtChangementNomEtTelephone_LaModificationEstCorrecteApresRechargement()
        {
            using (IDal dal = new Dal())
            {
                dal.NewRestaurant("La bonne fourchette", "01 02 03 04 05");
                dal.EditRestaurant(1, "La bonne cuillère", null);

                List<Resto> restos = dal.GetRestos();
                Assert.IsNotNull(restos);
                Assert.AreEqual(1, restos.Count);
                Assert.AreEqual("La bonne cuillère", restos[0].Nom);
                Assert.IsNull(restos[0].Telephone);
            }
        }
    }
}
