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
            try
            {
                int total = 0;
                var items = DB.MovieService.GetMovies(ApplicationSettings.MoviesPageSize, page, out total).ToList();
                Mapper.Initialize(cfg => { cfg.CreateMap<MovieDTO, MovieVM>(); });
                var res = Mapper.Map<IEnumerable<MovieDTO>, IEnumerable<MovieVM>>(items);

                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = ApplicationSettings.MoviesPageSize, TotalItems = total };

                IndexViewModel<MovieVM> ivm = new IndexViewModel<MovieVM> { PageInfo = pageInfo, Items = res };

                if (Request.IsAuthenticated)
                    ViewBag.UserId = User.Identity.GetUserId();

                return View(ivm);

            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create(MovieVM model, HttpPostedFileBase image)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Mapper.Initialize(cfg => { cfg.CreateMap<MovieVM, MovieDTO>(); });
                MovieDTO res = Mapper.Map<MovieVM, MovieDTO>(model);
                res.UserId = User.Identity.GetUserId();

                if (image != null)
                {
                    res.PosterURL = SaveImage(image);
                }

                DB.MovieService.CreateMovie(res);

                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                var item = DB.MovieService.GetMovie(id);

                if (item == null)
                    return HttpNotFound();

                Mapper.Initialize(cfg => { cfg.CreateMap<MovieDTO, MovieVM>(); });
                MovieVM res = Mapper.Map<MovieDTO, MovieVM>(item);

                return View(res);
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(MovieVM model, HttpPostedFileBase image)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Mapper.Initialize(cfg => { cfg.CreateMap<MovieVM, MovieDTO>(); });
                MovieDTO res = Mapper.Map<MovieVM, MovieDTO>(model);

                if (image != null)
                {
                    res.PosterURL = SaveImage(image);
                }

                DB.MovieService.UpdateMovie(res);

                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var item = DB.MovieService.GetMovie(id);

                if (item == null)
                    return HttpNotFound();

                DB.MovieService.DeleteMovie(id);

                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult Info(int id)
        {
            try
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
            catch
            {
                return new HttpStatusCodeResult(500);
            }
        }

        string SaveImage(HttpPostedFileBase image)
        {
            string res = "";
            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };
            var fileName = Path.GetFileName(image.FileName);
            var ext = Path.GetExtension(image.FileName);
            if (allowedExtensions.Contains(ext))
            {
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
            }
            return res;
        }
    }
}