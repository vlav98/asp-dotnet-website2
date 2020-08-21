using System;
using System.Collections.Generic;

namespace Mon2ndSite.Models
{
    public interface IDal : IDisposable
    {
        void NewResto(string Name, string Phone);
        void EditResto(int id, string Name, string Phone);
        List<Resto> GetRestos();
        bool ExistResto(string name);
        User LogIn(string username, string password);
        User GetUser(int id);
        User GetUser(string idString);
        int AddUser(string username, string password);
        bool Voted(int idSondage, string idString);
        void AddVote(int idSondage, int v, int idUser);
        int NewPoll();
        List<Results> GetResults(int id);
    }
}
