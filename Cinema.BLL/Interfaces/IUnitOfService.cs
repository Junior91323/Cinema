using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Interfaces
{
    public interface IUnitOfService : IDisposable
    {
        IMovieService MovieService { get; }
    }
}
