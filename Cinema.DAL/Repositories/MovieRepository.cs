using Cinema.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.DAL.Entities;
using Cinema.DAL.EF;
using System.Data.Entity;

namespace Cinema.DAL.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private CinemaContext DB;

        public MovieRepository(CinemaContext context)
        {
            this.DB = context;
        }
        public void Create(Movie item)
        {
            if (item != null)
                DB.Movies.Add(item);
        }

        public void Delete(int id)
        {
            Movie item = DB.Movies.Find(id);

            if (item != null)
                DB.Movies.Remove(item);
        }

        public IEnumerable<Movie> Find(Func<Movie, bool> predicate)
        {
            IEnumerable<Movie> items = DB.Movies.Where(predicate);
            return items;
        }

        public Movie Get(int id)
        {
            return DB.Movies.Find(id);
        }

        public IQueryable<Movie> GetAll()
        {
            return DB.Movies;
        }

        public void Update(Movie item)
        {
            if (item != null)
                DB.Entry(item).State = EntityState.Modified;
        }
    }
}
