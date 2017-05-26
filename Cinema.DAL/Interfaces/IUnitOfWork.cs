using Cinema.DAL.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movies { get; }
        void Save();
    }
}
