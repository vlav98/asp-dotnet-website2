using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mon2ndSite.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual List<Vote> Votes { get; set; }
    }
}