using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MoodWave8.Models;

namespace MoodWave8.Controllers
{
    public class MainsController : Controller
    {
        private MoodwaveEntities db = new MoodwaveEntities();

        // GET: Mains
        public ActionResult Index()
        {
            var mains = db.Mains.Include(m => m.User);
            return View(mains.ToList());
        }

        // GET: Mains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Main main = db.Mains.Find(id);
            if (main == null)
            {
                return HttpNotFound();
            }
            return View(main);
        }

        //GET: Mains/Create
        public ActionResult Create()
        {
            ViewBag.UserName = new SelectList(db.Users, "UserName", "UserName");
            return View();
        }

        // POST: Mains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "index,UserName,Track,Artist,Mood")] Main main)
        {
            if (ModelState.IsValid)
            {
                db.Mains.Add(main);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserName = new SelectList(db.Users, "UserName", "UserName", main.UserName);
            return View(main);
        }

        // GET: Mains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Main main = db.Mains.Find(id);
            if (main == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = new SelectList(db.Users, "UserName", "UserName", main.UserName);
            return View(main);
        }

        // POST: Mains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "index,UserName,Track,Artist,Mood")] Main main)
        {
            if (ModelState.IsValid)
            {
                db.Entry(main).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserName = new SelectList(db.Users, "UserName", "UserName", main.UserName);
            return View(main);
        }

        // GET: Mains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Main main = db.Mains.Find(id);
            if (main == null)
            {
                return HttpNotFound();
            }
            return View(main);
        }

        // POST: Mains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Main main = db.Mains.Find(id);
            db.Mains.Remove(main);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult OurWave()
        {
            return View();
        }
        public ActionResult Search()
        {
            return View();

        }
        public ActionResult SearchTable(Main u)
        {
            List<Main> m = db.Mains.ToList();

            //foreach (Main o in m)
            //{
            //    string t = o.ToString();
            //    if (u.UserName == o.UserName && u.Mood == o.Mood)
            //    {
            //        output.Add(t);
            //    }
            //    //We need a List to ....Print the new List 

            //}
            //Created an object of Main to test what exists in the table

            List<string> output = new List<string>();

            foreach (Main a in m)
            //for (int i = 0; i < m.Count(); i++)
            {
               // Main first = m[0];

                if (u.Mood == a.Mood && u.UserName == a.UserName)
                {
                    //output.Add(first.Mood);
                    //output.Add(first.Track);
                    //output.Add(first.Artist);
                    //output.Add(first.UserName);

                    ViewBag.Test += a.Mood;
                    ViewBag.Test += " " + a.Track + " ";
                    ViewBag.Test += " " + a.Artist + " ";
                    ViewBag.Test += a.UserName;

                    //ViewBag.testing += u.Mood + " " + first.Mood;

                }
                // ViewBag.test = output;
                //  }
            }
                 return View(output);
        }
        public ActionResult Discovery()
        {
            return View();
        
        }
        /*public ActionResult Data()
        {
            return View();
        }*/
        public ActionResult Data()
        {
            List<Temp> T = db.Temps.ToList();
            string track = "";
            string artist = "";
            //Does not handle iteration properly for multiple case
            foreach (Temp t in T)
            {
                track = t.Track;
                artist = t.Artist;
            }
            var json = new { Track = track, Artist = artist };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSimilar(Main u)
        {
            string track = null, artist = null;
            List<Main> m = db.Mains.ToList();
            foreach (Main a in m)
            {
                if (u.Mood == a.Mood && u.UserName == a.UserName)
                {
                    track = a.Track; //need to make an empty list and put these into it
                    artist = a.Artist;
                }
            }
            //Remove all entries from temps database
            foreach (var entity in db.Temps)
                db.Temps.Remove(entity);

            //Add new entry (Need to iterate when you create a list for multiples)
            db.Temps.Add(new Temp { Track = track, Artist = artist, Index = "1" });
            db.SaveChanges();
            return View("NewDiscovery");
        }
         public ActionResult NewDiscovery()
        {
            return View();
        }
      

        //public ActionResult GetSimilar(Main u)
        //{
        //    string jform1 = "http://ws.audioscrobbler.com2.0/?method=track.getsimilar&artist=";
        //    string jform2 = "&track=";
        //    string jform3 = "&api_key=8468e49f31e72233a11b83ea4d4a1e6e&format=json";
        //    //&limit=1


        //    string finaljform = string.Concat(jform1 + u.Artist + jform2 + u.Track + jform3);

        //    http://ws.audioscrobbler.com2.0/?method=track.getsimilar&artist=cher&track=believe&api_key=8468e49f31e72233a11b83ea4d4a1e6e&format=json



        //    return ;
        //}
    }


}
