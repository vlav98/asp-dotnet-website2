using System.Data.Entity;

namespace Mon2ndSite.Models
{
    public class BddContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Resto> Restos { get; set; }
        public DbSet<User> Users{ get; set; }
    }
}