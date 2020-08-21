namespace Mon2ndSite.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public virtual Resto Resto { get; set; }
        public virtual User User { get; set; }
    }
}