using AutoMapper;
using Cinema.BLL.DTO;
using Cinema.BLL.Interfaces;
using Cinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Helpers;
using Cinema.Utils;
using System.Runtime.CompilerServices;
using PagedList;

namespace Cinema.Controllers
{
    public class MoviesController : Controller
    {
        IUnitOfService DB;

        public MoviesController(IUnitOfService db)
        {
            DB = db;
        }

        public ActionResult Index(int page = 1)
        {

            //int total = 0;
            //var items = DB.MovieService.GetMovies(ApplicationSettings.MoviesPageSize, page, out total).ToList();
            var items = DB.MovieService.GetMovies();
            Mapper.Initialize(cfg => { cfg.CreateMap<MovieDTO, MovieVM>(); });
            var res = Mapper.Map<IEnumerable<MovieDTO>, IEnumerable<MovieVM>>(items);

            //PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = ApplicationSettings.MoviesPageSize, TotalItems = total };

            //IndexViewModel<MovieVM> ivm = new IndexViewModel<MovieVM> { PageInfo = pageInfo, Items = res };

            if (Request.IsAuthenticated)
                ViewBag.UserId = User.Identity.GetUserId();

            return View(res.ToPagedList(page, ApplicationSettings.MoviesPageSize));

        }
        public ActionResult Update(int? id)
        {
            if (id != null)
            {
                var item = DB.MovieService.GetMovie((int)id);
                if (item != null)
                {
                    Mapper.Initialize(cfg => { cfg.CreateMap<MovieDTO, MovieVM>(); });
                    MovieVM res = Mapper.Map<MovieDTO, MovieVM>(item);

                    if (res.UserId != User.Identity.GetUserId())
                        return RedirectToAction("Index");

                    return View(res);
                }
            }
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken()]
        public ActionResult Update(MovieVM model, HttpPostedFileBase image)
        {

            if (image != null)
                if (!ApplicationSettings.AllowedExtensions.Contains(Path.GetExtension(image.FileName).ToLower()))
                    ModelState.AddModelError("", "Invalid file extension. Acceptable " + ApplicationSettings.AllowedExtensions);

            if (ModelState.IsValid)
            {
                var item = DB.MovieService.GetMovie(model.Id);
                if (item != null)
                {
                    if (item.UserId != User.Identity.GetUserId())
                        return RedirectToAction("Index");
                }

                Mapper.Initialize(cfg => { cfg.CreateMap<MovieVM, MovieDTO>(); });
                MovieDTO res = Mapper.Map<MovieVM, MovieDTO>(model);
                res.UserId = User.Identity.GetUserId();
                if (image != null)
                {
                    res.PosterURL = SaveImage(image); ;
                }
                if (item == null)
                    DB.MovieService.CreateMovie(res);
                else
                    DB.MovieService.UpdateMovie(res);

                return RedirectToAction("Index");
            }

            return View(model);
        }



        //[HttpGet]
        //[Authorize]
        //public ActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken()]
        //public ActionResult Create(MovieVM model, HttpPostedFileBase image)
        //{

        //    if (image != null)
        //        if (ApplicationSettings.AllowedExtensions.Contains(Path.GetExtension(image.FileName)))
        //            ModelState.AddModelError("", "Invalid file extension. Acceptable ");


        //    if (ModelState.IsValid)
        //    {

        //        Mapper.Initialize(cfg => { cfg.CreateMap<MovieVM, MovieDTO>(); });
        //        MovieDTO res = Mapper.Map<MovieVM, MovieDTO>(model);
        //        res.UserId = User.Identity.GetUserId();

        //        res.PosterURL = SaveImage(image); ;

        //        DB.MovieService.CreateMovie(res);

        //        return RedirectToAction("Index");
        //    }
        //    return View(model);
        //}



        //[Authorize]
        //public ActionResult Edit(int id)
        //{

        //    var item = DB.MovieService.GetMovie(id);

        //    if (item == null)
        //        return HttpNotFound();

        //    Mapper.Initialize(cfg => { cfg.CreateMap<MovieDTO, MovieVM>(); });
        //    MovieVM res = Mapper.Map<MovieDTO, MovieVM>(item);

        //    return View(res);

        //}

        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken()]
        //public ActionResult Edit(MovieVM model, HttpPostedFileBase image)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    Mapper.Initialize(cfg => { cfg.CreateMap<MovieVM, MovieDTO>(); });
        //    MovieDTO res = Mapper.Map<MovieVM, MovieDTO>(model);

        //    if (image != null)
        //    {
        //        res.PosterURL = SaveImage(image);
        //    }

        //    DB.MovieService.UpdateMovie(res);

        //    return RedirectToAction("Index");

        //}

        public ActionResult Delete(int id)
        {
            var item = DB.MovieService.GetMovie(id);

            if (item == null)
                return HttpNotFound();

            if (item.UserId != User.Identity.GetUserId())
                return RedirectToAction("Index");

            DB.MovieService.DeleteMovie(id);

            return RedirectToAction("Index");

        }

        public ActionResult Info(int id)
        {
            var item = DB.MovieService.GetMovie(id);

            if (item == null)
                return HttpNotFound();

            if (Request.IsAuthenticated)
                ViewBag.UserId = User.Identity.GetUserId();

            Mapper.Initialize(cfg => { cfg.CreateMap<MovieDTO, MovieVM>(); });
            MovieVM res = Mapper.Map<MovieDTO, MovieVM>(item);

            res.User = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(res.UserId).UserName;
            return View(res);

        }

        string SaveImage(HttpPostedFileBase image)
        {
            string res = "";
            var fileName = Path.GetFileName(image.FileName);
            var ext = Path.GetExtension(image.FileName);
            string name = Path.GetFileNameWithoutExtension(fileName);
            string myfile = String.Format("{0}_{1}{2}", name, Guid.NewGuid(), ext);
            string path = Path.Combine(@"\Img", myfile);
            WebImage img = new WebImage(image.InputStream);

            string size = ApplicationSettings.MovieImageSize;
            int w = Int32.Parse(size.Split('*')[0]);
            int h = Int32.Parse(size.Split('*')[0]);

            if (img.Width > w)
                img.Resize(w, h);
            res = path;
            img.Save(Server.MapPath(path));

            return res;
        }
    }
}