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

        [TestMethod]
        public void GetResults_AvecQuelquesChoix_RetourneBienLesResultats()
        {
            int idSondage = dal.NewPoll();
            int idUser1 = dal.AddUser("User1", "12345");
            int idUser2 = dal.AddUser("User2", "12345");
            int idUser3 = dal.AddUser("User3", "12345");

            dal.NewRestaurant("Resto pinière", "0102030405");
            dal.NewRestaurant("Resto pinambour", "0102030405");
            dal.NewRestaurant("Resto mate", "0102030405");
            dal.NewRestaurant("Resto ride", "0102030405");

            dal.AddVote(idSondage, 1, idUser1);
            dal.AddVote(idSondage, 3, idUser1);
            dal.AddVote(idSondage, 4, idUser1);
            dal.AddVote(idSondage, 1, idUser2);
            dal.AddVote(idSondage, 1, idUser3);
            dal.AddVote(idSondage, 3, idUser3);

            List<Resultats> resultats = dal.GetResults(idSondage);

            Assert.AreEqual(3, resultats[0].NombreDeVotes);
            Assert.AreEqual("Resto pinière", resultats[0].Nom);
            Assert.AreEqual("0102030405", resultats[0].Telephone);
            Assert.AreEqual(2, resultats[1].NombreDeVotes);
            Assert.AreEqual("Resto mate", resultats[1].Nom);
            Assert.AreEqual("0102030405", resultats[1].Telephone);
            Assert.AreEqual(1, resultats[2].NombreDeVotes);
            Assert.AreEqual("Resto ride", resultats[2].Nom);
            Assert.AreEqual("0102030405", resultats[2].Telephone);
        }

        [TestMethod]
        public void GetResults_AvecDeuxSondages_RetourneBienLesBonsResultats()
        {
            int idSondage1 = dal.NewPoll();
            int idUser1 = dal.AddUser("User1", "12345");
            int idUser2 = dal.AddUser("User2", "12345");
            int idUser3 = dal.AddUser("User3", "12345");
            dal.NewRestaurant("Resto pinière", "0102030405");
            dal.NewRestaurant("Resto pinambour", "0102030405");
            dal.NewRestaurant("Resto mate", "0102030405");
            dal.NewRestaurant("Resto ride", "0102030405");
            dal.AddVote(idSondage1, 1, idUser1);
            dal.AddVote(idSondage1, 3, idUser1);
            dal.AddVote(idSondage1, 4, idUser1);
            dal.AddVote(idSondage1, 1, idUser2);
            dal.AddVote(idSondage1, 1, idUser3);
            dal.AddVote(idSondage1, 3, idUser3);

            int idSondage2 = dal.NewPoll();
            dal.AddVote(idSondage2, 2, idUser1);
            dal.AddVote(idSondage2, 3, idUser1);
            dal.AddVote(idSondage2, 1, idUser2);
            dal.AddVote(idSondage2, 4, idUser3);
            dal.AddVote(idSondage2, 3, idUser3);

            List<Resultats> resultats1 = dal.GetResults(idSondage1);
            List<Resultats> resultats2 = dal.GetResults(idSondage2);

            Assert.AreEqual(3, resultats1[0].NombreDeVotes);
            Assert.AreEqual("Resto pinière", resultats1[0].Nom);
            Assert.AreEqual("0102030405", resultats1[0].Telephone);
            Assert.AreEqual(2, resultats1[1].NombreDeVotes);
            Assert.AreEqual("Resto mate", resultats1[1].Nom);
            Assert.AreEqual("0102030405", resultats1[1].Telephone);
            Assert.AreEqual(1, resultats1[2].NombreDeVotes);
            Assert.AreEqual("Resto ride", resultats1[2].Nom);
            Assert.AreEqual("0102030405", resultats1[2].Telephone);

            Assert.AreEqual(1, resultats2[0].NombreDeVotes);
            Assert.AreEqual("Resto pinambour", resultats2[0].Nom);
            Assert.AreEqual("0102030405", resultats2[0].Telephone);
            Assert.AreEqual(2, resultats2[1].NombreDeVotes);
            Assert.AreEqual("Resto mate", resultats2[1].Nom);
            Assert.AreEqual("0102030405", resultats2[1].Telephone);
            Assert.AreEqual(1, resultats2[2].NombreDeVotes);
            Assert.AreEqual("Resto pinière", resultats2[2].Nom);
            Assert.AreEqual("0102030405", resultats2[2].Telephone);
        }
    }
}