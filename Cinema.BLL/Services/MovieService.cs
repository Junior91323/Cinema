using Cinema.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.BLL.DTO;
using Cinema.DAL.Interfaces;
using Cinema.DAL.Entities;
using AutoMapper;

namespace Cinema.BLL.Services
{
    public class MovieService : IMovieService
    {
        private IUnitOfWork DB;
        public MovieService(IUnitOfWork db)
        {
            DB = db;
        }

        public void CreateMovie(MovieDTO item)
        {
            try
            {
                if (item == null)
                    throw new NullReferenceException("item is null");

                Movie movie = DB.Movies.Get(item.Id);

                if (movie != null)
                    throw new Exception("Movie already exist");

                Movie res = new Movie
                {
                    Title = item.Title,
                    Producer = item.Producer,
                    Year = item.Year,
                    Description = item.Description,
                    PosterURL = item.PosterURL,
                    UserId = item.UserId,
                    CreatedDate = DateTime.Now,
                    ModifyDate = DateTime.Now
                };
                DB.Movies.Create(res);
                DB.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public void DeleteMovie(int id)
        {
            try
            {
                DB.Movies.Delete(id);
                DB.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public void Dispose()
        {
            DB.Dispose();
        }

        public MovieDTO GetMovie(int id)
        {

                var item = DB.Movies.Get(id);

            if (item != null)
            {
                Mapper.Initialize(cfg => { cfg.CreateMap<Movie, MovieDTO>(); });
                return Mapper.Map<Movie, MovieDTO>(item);
            }

            return null;

        }

        public IEnumerable<MovieDTO> GetMovies()
        {
            try
            {
                Mapper.Initialize(cfg => { cfg.CreateMap<Movie, MovieDTO>(); });
                return Mapper.Map<IEnumerable<Movie>, List<MovieDTO>>(DB.Movies.GetAll().OrderByDescending(x => x.ModifyDate).ToList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public IEnumerable<MovieDTO> GetMovies(int pageSize, int page, out int total)
        {
            try
            {
                var res = DB.Movies.GetAll().OrderByDescending(x => x.ModifyDate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                total = DB.Movies.GetAll().Count();
                Mapper.Initialize(cfg => { cfg.CreateMap<Movie, MovieDTO>(); });
                return Mapper.Map<IEnumerable<Movie>, List<MovieDTO>>(res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public void UpdateMovie(MovieDTO item)
        {
            try
            {
                if (item == null)
                    throw new NullReferenceException("item is null");

                Movie movie = DB.Movies.Get(item.Id);

                if (movie == null)
                    throw new Exception(String.Format("Item with id: {0} is not found!", item.Id));


                movie.Id = item.Id;
                movie.Title = item.Title;
                movie.Producer = item.Producer;
                movie.Year = item.Year;
                movie.Description = item.Description;
                movie.PosterURL = item.PosterURL;
                movie.ModifyDate = DateTime.Now;

                DB.Movies.Update(movie);
                DB.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}
