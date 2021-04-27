using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IslamSandhesham.Models;

namespace IslamSandhesham.Controllers
{
    public class SectionsController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Sections
        public ActionResult Index()
        {
            bool res =isLoggedIn();
            if (res)
            {
                return View(db.Sections.ToList());
            }
            else
            {
                return RedirectToAction("Admin","Home", new { isLogin = false });
            }
        }
        public bool isLoggedIn()
        {
            Admin admin = db.Admin.OrderBy(w => w.id).FirstOrDefault();
            bool isLoggedIn = false;
            isLoggedIn = admin.IsLoggedIn;
            if (isLoggedIn)
            {
                TimeSpan ts = DateTime.Now - (DateTime)admin.LastLoginTime;
                if (ts.Days > 1 || ts.Hours > 1 || ts.Minutes > 45)
                {
                    isLoggedIn = false;
                    admin.IsLoggedIn = false;
                    db.SaveChanges();
                }
            }
            return isLoggedIn;
        }

        // GET: Sections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sections sections = db.Sections.Find(id);
            if (sections == null)
            {
                return HttpNotFound();
            }
            return View(sections);
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            bool res = isLoggedIn();
            if (res)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,SectionId,Description,ClassName,Type")] Sections sections)
        {
            bool res = isLoggedIn();
            if (res)
            {
                if (ModelState.IsValid)
                {
                    db.Sections.Add(sections);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(sections);
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }
            
        }

        // GET: Sections/Edit/5
        public ActionResult Edit(int? id)
        {
           
            bool res = isLoggedIn();
            if (res)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Sections sections = db.Sections.Find(id);
                if (sections == null)
                {
                    return HttpNotFound();
                }
                return View(sections);
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,SectionId,Description,ClassName,Type")] Sections sections)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sections).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sections);
        }

        // GET: Sections/Delete/5
        public ActionResult Delete(int? id)
        {
            bool res = isLoggedIn();
            if (res)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Sections sections = db.Sections.Find(id);
                if (sections == null)
                {
                    return HttpNotFound();
                }
                return View(sections);
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }            

        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sections sections = db.Sections.Find(id);
            db.Sections.Remove(sections);
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
    }
}
