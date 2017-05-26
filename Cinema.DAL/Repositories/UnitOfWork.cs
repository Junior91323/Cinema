using Cinema.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.DAL.Interfaces.Repositories;
using Cinema.DAL.EF;

namespace Cinema.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private CinemaContext DB;
        private MovieRepository MoviesRepository;

        public IMovieRepository Movies
        {
            get
            {
                if (MoviesRepository == null)
                    MoviesRepository = new MovieRepository(DB);
                return MoviesRepository;
            }
        }

        public UnitOfWork()
        {
            DB = new CinemaContext();
        }

        public void Save()
        {
            try
            {
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                //Log ....
                throw new Exception("Saving Error", ex.InnerException);
            }
        }

        private bool Disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                    DB.Dispose();
                }
                this.Disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
