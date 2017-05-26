using Cinema.BLL.Interfaces;
using Cinema.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Services
{
    public class UnitOfServices : IUnitOfService
    {
        bool Disposed = false;
        private IUnitOfWork DB;
        private IMovieService Movies;

        public IMovieService MovieService
        {
            get
            {
                if (Movies == null)
                    Movies = new Services.MovieService(DB);
                return Movies;
            }
        }

        public UnitOfServices(IUnitOfWork db)
        {
            DB = db;
        }

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
