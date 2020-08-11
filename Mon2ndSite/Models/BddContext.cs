﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mon2ndSite.Models
{
    public class BddContext : DbContext
    {
        public DbSet<Poll> Sondages { get; set; }
        public DbSet<Resto> Restos { get; set; }
    }
}