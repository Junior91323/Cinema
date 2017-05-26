using Cinema.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.DAL.EF
{
    public class CinemaContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public CinemaContext() : base("DefaultConnection")
        {
        }
    }
}

