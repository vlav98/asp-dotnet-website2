using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public bool ExistRestaurant(string name)
        {
            Resto foundResto = bdd.Restos.FirstOrDefault(resto => resto.Nom == name);
            if (foundResto != null)
                return true;
            return false;
        }

        public User LogIn(string username, string password)
        {
            string EncodedPW = EncodeMD5(password);
            return bdd.Users.FirstOrDefault(u => u.Username == username && u.Password == EncodedPW);
        }

        public User GetUser(int id)
        {
            return bdd.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string idStr)
        {
            switch (idStr)
            {
                case "Chrome":
                    return CreeOuRecupere("Nico", "1234");
                case "IE":
                    return CreeOuRecupere("Jérémie", "1234");
                case "Firefox":
                    return CreeOuRecupere("Delphine", "1234");
                default:
                    return CreeOuRecupere("Timéo", "1234");
            }
        }

        public int AddUser(string username, string password)
        {
            string EncodedPW = EncodeMD5(password);
            User user = new User { Username = username, Password = EncodedPW };
            bdd.Users.Add(user);
            bdd.SaveChanges();
            return user.Id;
        }

        public bool Voted(int idPoll, string idStr)
        {
            User user = GetUser(idStr);
            if (user != null)
            {
                Poll poll = bdd.Polls.First(s => s.Id == idPoll);
                if (poll.Votes == null)
                    return false;
                return poll.Votes.Any(v => v.User != null && v.User.Id == user.Id);
            }
            return false;
        }

        public int NewPoll()
        {
            Poll poll = new Poll { Date = DateTime.Now };
            bdd.Polls.Add(poll);
            bdd.SaveChanges();
            return poll.Id;
        }

        public void AddVote(int idPoll, int idResto, int idUser)
        {
            Vote vote = new Vote
            {
                Resto = bdd.Restos.First(r => r.Id == idResto),
                User = bdd.Users.First(u => u.Id == idUser)
            };
            Poll poll = bdd.Polls.First(s => s.Id == idPoll);
            if (poll.Votes == null)
                poll.Votes = new List<Vote>();
            poll.Votes.Add(vote);
            bdd.SaveChanges();
        }

        private string EncodeMD5(string password)
        {
            string encodedPassword= "ChoixResto" + password + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(encodedPassword)));
        }

        private User CreeOuRecupere(string nom, string motDePasse)
        {
            User user = LogIn(nom, motDePasse);
            if (user == null)
            {
                int id = AddUser(nom, motDePasse);
                return GetUser(id);
            }
            return user;
        }
    }
}