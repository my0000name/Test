using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using TestMVC.Models;


namespace TestMVC.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public ActionResult Index()
        {
            List<Movie> list = new List<Movie>();
            if (HttpRuntime.Cache.Get("movies") != null)
            {
                list = HttpRuntime.Cache.Get("movies") as List<Movie>;
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    Movie m = new Movie();
                    m.Genre = "Genre:" + i;
                    m.ID = i + new Random().Next(1, 1000);
                    m.Price = i * new Random().Next(1, 10);
                    m.ReleaseDate = DateTime.Now.AddDays(i);
                    m.Title = "Title~" + i;
                    list.Add(m);
                }
                HttpRuntime.Cache.Insert("movies", list);
            }
            
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Movie movie)
        {
            Movie m = new Movie();
            List<Movie> list = HttpRuntime.Cache.Get("movies") as List<Movie>;
            m = list.Find(r => r.ID == movie.ID);
            if (ModelState.IsValid)
            {
                m.Title = "eeee";
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public ActionResult Edit(int id)
        {
            Movie movie = new Movie();
            List<Movie> list = HttpRuntime.Cache.Get("movies") as List<Movie>;
            movie = list.Find(r => r.ID == id);
            return View(movie);
        }
    }
}