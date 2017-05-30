using Cinema.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.DAL.Entities;
using Cinema.DAL.EF;
using System.Data.Entity;
using System.Linq.Expressions;

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

        public IQueryable<Movie> Find(Expression<Func<Movie, Boolean>> predicate)
        {
            IQueryable<Movie> items = DB.Movies;
            items = items.Where(predicate);
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
