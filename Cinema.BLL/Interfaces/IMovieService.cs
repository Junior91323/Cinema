using Cinema.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.BLL.Interfaces
{
   public interface IMovieService
    {
        void CreateMovie(MovieDTO item);
        void UpdateMovie(MovieDTO item);
        MovieDTO GetMovie(int id);
        void DeleteMovie(int id);
        IEnumerable<MovieDTO> GetMovies();
        IEnumerable<MovieDTO> GetMovies(int pageSize, int page, out int total);
        void Dispose();
    }
}
