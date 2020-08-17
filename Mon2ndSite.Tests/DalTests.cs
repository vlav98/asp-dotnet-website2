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
        public void NewResto_AvecUnNouveauResto_GetRestosRenvoitBienLeResto()
        {
            using (IDal dal = new Dal())
            {
                dal.NewResto("La bonne fourchette", "0102030405");
                List<Resto> restos = dal.GetRestos();

                Assert.IsNotNull(restos);
                Assert.AreEqual(1, restos.Count);
                Assert.AreEqual("La bonne fourchette", restos[0].Nom);
                Assert.AreEqual("0102030405", restos[0].Telephone);
            }
        }

        [TestMethod]
        public void EditResto_CreationDUnNouveauRestoEtChangementNomEtTelephone_LaModificationEstCorrecteApresRechargement()
        {
            using (IDal dal = new Dal())
            {
                dal.NewResto("La bonne fourchette", "0102030405");
                dal.EditResto(1, "La bonne cuillère", null);

                List<Resto> restos = dal.GetRestos();
                Assert.IsNotNull(restos);
                Assert.AreEqual(1, restos.Count);
                Assert.AreEqual("La bonne cuillère", restos[0].Nom);
                Assert.IsNull(restos[0].Telephone);
            }
        }

        [TestMethod]
        public void RestoExiste_AvecRestaurauntInexistant_RenvoiQuilExiste()
        {
            bool existe = dal.ExistResto("La bonne fourchette");

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
            dal.AddUser("NouvelUser", "12345");

            User User = dal.GetUser(1);

            Assert.IsNotNull(User);
            Assert.AreEqual("NouvelUser", User.Username);

            User = dal.GetUser("1");

            Assert.IsNotNull(User);
            Assert.AreEqual("NouvelUser", User.Username);
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
            dal.NewResto("La bonne fourchette", "0102030405");
            dal.AddVote(idSondage, 1, idUser);

            bool aVote = dal.Voted(idSondage, idUser.ToString());

            Assert.IsTrue(aVote);
        }

        [TestMethod]
        public void GetResults_AvecQuelquesChoix_RetourneBienLesResults()
        {
            int idSondage = dal.NewPoll();
            int idUser1 = dal.AddUser("User1", "12345");
            int idUser2 = dal.AddUser("User2", "12345");
            int idUser3 = dal.AddUser("User3", "12345");

            dal.NewResto("Resto pinière", "0102030405");
            dal.NewResto("Resto pinambour", "0102030405");
            dal.NewResto("Resto mate", "0102030405");
            dal.NewResto("Resto ride", "0102030405");

            dal.AddVote(idSondage, 1, idUser1);
            dal.AddVote(idSondage, 3, idUser1);
            dal.AddVote(idSondage, 4, idUser1);
            dal.AddVote(idSondage, 1, idUser2);
            dal.AddVote(idSondage, 1, idUser3);
            dal.AddVote(idSondage, 3, idUser3);

            List<Results> results = dal.GetResults(idSondage);

            Assert.AreEqual(3, results[0].NombreDeVotes);
            Assert.AreEqual("Resto pinière", results[0].Nom);
            Assert.AreEqual("0102030405", results[0].Telephone);
            Assert.AreEqual(2, results[1].NombreDeVotes);
            Assert.AreEqual("Resto mate", results[1].Nom);
            Assert.AreEqual("0102030405", results[1].Telephone);
            Assert.AreEqual(1, results[2].NombreDeVotes);
            Assert.AreEqual("Resto ride", results[2].Nom);
            Assert.AreEqual("0102030405", results[2].Telephone);
        }

        [TestMethod]
        public void GetResults_AvecDeuxSondages_RetourneBienLesBonsResults()
        {
            int idSondage1 = dal.NewPoll();
            int idUser1 = dal.AddUser("User1", "12345");
            int idUser2 = dal.AddUser("User2", "12345");
            int idUser3 = dal.AddUser("User3", "12345");
            dal.NewResto("Resto pinière", "0102030405");
            dal.NewResto("Resto pinambour", "0102030405");
            dal.NewResto("Resto mate", "0102030405");
            dal.NewResto("Resto ride", "0102030405");
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

            List<Results> results1 = dal.GetResults(idSondage1);
            List<Results> results2 = dal.GetResults(idSondage2);

            Assert.AreEqual(3, results1[0].NombreDeVotes);
            Assert.AreEqual("Resto pinière", results1[0].Nom);
            Assert.AreEqual("0102030405", results1[0].Telephone);
            Assert.AreEqual(2, results1[1].NombreDeVotes);
            Assert.AreEqual("Resto mate", results1[1].Nom);
            Assert.AreEqual("0102030405", results1[1].Telephone);
            Assert.AreEqual(1, results1[2].NombreDeVotes);
            Assert.AreEqual("Resto ride", results1[2].Nom);
            Assert.AreEqual("0102030405", results1[2].Telephone);

            Assert.AreEqual(1, results2[0].NombreDeVotes);
            Assert.AreEqual("Resto pinambour", results2[0].Nom);
            Assert.AreEqual("0102030405", results2[0].Telephone);
            Assert.AreEqual(2, results2[1].NombreDeVotes);
            Assert.AreEqual("Resto mate", results2[1].Nom);
            Assert.AreEqual("0102030405", results2[1].Telephone);
            Assert.AreEqual(1, results2[2].NombreDeVotes);
            Assert.AreEqual("Resto pinière", results2[2].Nom);
            Assert.AreEqual("0102030405", results2[2].Telephone);
        }

        [TestMethod]
        public void AccueilController_Index_RenvoiVueParDefaut()
        {
            AccueilController controller = new AccueilController();

            ViewResult result = (ViewResult)controller.Index();

            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}