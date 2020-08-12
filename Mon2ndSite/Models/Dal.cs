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

        public User GetUser(string idString)
        {
            int id;
            if (int.TryParse(idString, out id))
                return GetUser(id);
            return null;
        }

        public int AddUser(string username, string password)
        {
            string EncodedPW = EncodeMD5(password);
            User user = new User { Username = username, Password = EncodedPW };
            bdd.Users.Add(user);
            bdd.SaveChanges();
            return user.Id;
        }

        public bool Voted(int idPoll, string idString)
        {
            int id;
            if (int.TryParse(idString, out id))
            {
                Poll sondage = bdd.Polls.First(s => s.Id == idPoll);
                if (sondage.Votes == null)
                    return false;
                return sondage.Votes.Any(v => v.User != null && v.User.Id == id);
            }
            return false;
        }

        public int NewPoll()
        {
            Poll sondage = new Poll { Date = DateTime.Now };
            bdd.Polls.Add(sondage);
            bdd.SaveChanges();
            return sondage.Id;
        }

        public List<Resultats> GetResults(int idPoll)
        {
            List<Resto> restaurants = GetRestos();
            List<Resultats> resultats = new List<Resultats>();
            Poll sondage = bdd.Polls.First(s => s.Id == idPoll);
            foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRestaurant = grouping.Key;
                Resto resto = restaurants.First(r => r.Id == idRestaurant);
                int nombreDeVotes = grouping.Count();
                resultats.Add(new Resultats { Nom = resto.Nom, Telephone = resto.Telephone, NombreDeVotes = nombreDeVotes });
            }
            return resultats;
        }

        public void AddVote(int idPoll, int idResto, int idUser)
        {
            Vote vote = new Vote
            {
                Resto = bdd.Restos.First(r => r.Id == idResto),
                User = bdd.Users.First(u => u.Id == idUser)
            };
            Poll sondage = bdd.Polls.First(s => s.Id == idPoll);
            if (sondage.Votes == null)
                sondage.Votes = new List<Vote>();
            sondage.Votes.Add(vote);
            bdd.SaveChanges();
        }

        private string EncodeMD5(string password)
        {
            string encodedPassword= "ChoixResto" + password + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(encodedPassword)));
        }
    }
}