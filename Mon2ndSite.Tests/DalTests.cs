using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mon2ndSite.Controllers;
using Mon2ndSite.Models;

namespace Mon2ndSite.Tests
{    
    [TestClass]
    public class DalTests
    {
        private IDal dal;

        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            IDatabaseInitializer<BddContext> init = new DropCreateDatabaseAlways<BddContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new BddContext());

            dal = new Dal();
        }

        [TestCleanup]
        public void ApresChaqueTest()
        {
            dal.Dispose();
        }

        [TestMethod]
        public void NewRestaurant_AvecUnNouveauRestaurant_GetRestaurantsRenvoitBienLeRestaurant()
        {
            using (IDal dal = new Dal())
            {
                dal.NewRestaurant("La bonne fourchette", "0102030405");
                List<Resto> restos = dal.GetRestos();

                Assert.IsNotNull(restos);
                Assert.AreEqual(1, restos.Count);
                Assert.AreEqual("La bonne fourchette", restos[0].Nom);
                Assert.AreEqual("0102030405", restos[0].Telephone);
            }
        }

        [TestMethod]
        public void EditRestaurant_CreationDUnNouveauRestaurantEtChangementNomEtTelephone_LaModificationEstCorrecteApresRechargement()
        {
            using (IDal dal = new Dal())
            {
                dal.NewRestaurant("La bonne fourchette", "0106070809");
                dal.EditRestaurant(1, "La bonne cuillère", null);

                List<Resto> restos = dal.GetRestos();
                Assert.IsNotNull(restos);
                Assert.AreEqual(1, restos.Count);
                Assert.AreEqual("La bonne cuillère", restos[0].Nom);
                Assert.IsNull(restos[0].Telephone);
            }
        }

        [TestMethod]
        public void RestaurantExiste_AvecRestaurauntInexistant_RenvoiQuilExiste()
        {
            bool existe = dal.ExistRestaurant("La bonne fourchette");

            Assert.IsFalse(existe);
        }

        [TestMethod]
        public void GetUser_UserInexistant_RetourneNull()
        {
            User User = dal.GetUser(1);
            Assert.IsNull(User);
        }

        [TestMethod]
        public void GetUser_IdNonNumerique_RetourneNull()
        {
            User User = dal.GetUser("abc");
            Assert.IsNull(User);
        }

        [TestMethod]
        public void AddUser_NouvelUserEtRecuperation_LUserEstBienRecupere()
        {
            dal.AddUser("Nouvel User", "12345");

            User User = dal.GetUser(1);

            Assert.IsNotNull(User);
            Assert.AreEqual("Nouvel User", User.Username);

            User = dal.GetUser("1");

            Assert.IsNotNull(User);
            Assert.AreEqual("Nouvel User", User.Username);
        }

        [TestMethod]
        public void LogIn_LoginMdpOk_AuthentificationOK()
        {
            dal.AddUser("Nouvel User", "12345");

            User User = dal.LogIn("Nouvel User", "12345");

            Assert.IsNotNull(User);
            Assert.AreEqual("Nouvel User", User.Username);
        }

        [TestMethod]
        public void LogIn_LoginOkMdpKo_AuthentificationKO()
        {
            dal.AddUser("Nouvel User", "12345");
            User User = dal.LogIn("Nouvel User", "0");

            Assert.IsNull(User);
        }

        [TestMethod]
        public void LogIn_LoginKoMdpOk_AuthentificationKO()
        {
            dal.AddUser("Nouvel User", "12345");
            User User = dal.LogIn("Nouvel", "12345");

            Assert.IsNull(User);
        }

        [TestMethod]
        public void LogIn_LoginMdpKo_AuthentificationKO()
        {
            User User = dal.LogIn("Nouvel User", "12345");

            Assert.IsNull(User);
        }

        [TestMethod]
        public void ADejaVote_AvecIdNonNumerique_RetourneFalse()
        {
            bool pasVote = dal.Voted(1, "abc");

            Assert.IsFalse(pasVote);
        }

        [TestMethod]
        public void ADejaVote_UserNAPasVote_RetourneFalse()
        {
            int idSondage = dal.NewPoll();
            int idUser = dal.AddUser("Nouvel User", "12345");

            bool pasVote = dal.Voted(idSondage, idUser.ToString());

            Assert.IsFalse(pasVote);
        }

        [TestMethod]
        public void ADejaVote_UserAVote_RetourneTrue()
        {
            int idSondage = dal.NewPoll();
            int idUser = dal.AddUser("Nouvel User", "12345");
            dal.NewRestaurant("La bonne fourchette", "0102030405");
            dal.AddVote(idSondage, 1, idUser);

            bool aVote = dal.Voted(idSondage, idUser.ToString());

            Assert.IsTrue(aVote);
        }
    }
}