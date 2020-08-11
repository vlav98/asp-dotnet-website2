using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mon2ndSite.Models
{
    public interface IDal : IDisposable
    {
        void NewRestaurant(string Name, string Phone);
        void EditRestaurant(int id, string Name, string Phone);
        List<Resto> GetRestos();
    }
}
