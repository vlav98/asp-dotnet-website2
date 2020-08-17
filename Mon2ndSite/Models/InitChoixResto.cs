using Mon2ndSite.Models;
using System.Data.Entity;

namespace Mon2ndSite
{
    public class InitChoixResto : DropCreateDatabaseAlways<BddContext>
    {
        protected override void Seed(BddContext context)
        {
            context.Restos.Add(new Resto { Id = 1, Nom = "Resto pinambour", Telephone = "123" });
            context.Restos.Add(new Resto { Id = 2, Nom = "Resto pinière", Telephone = "456" });
            context.Restos.Add(new Resto { Id = 3, Nom = "Resto toro", Telephone = "789" });

            base.Seed(context);
        }
    }
}