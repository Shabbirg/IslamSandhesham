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
    public class AdminController : Controller
    {
        private MyContext db = new MyContext();

        // GET: Admin
        public ActionResult Index()
        {
            bool res =new HomeController().isLoggedIn();
            ViewBag.isLogin = res;
            if (res)
            {
                return View(db.Admin.ToList());
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }
            
        }
        

        // GET: Admin/Create
        public ActionResult Create()
        {
            bool res = new HomeController().isLoggedIn();
            ViewBag.isLogin = res;
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
        public ActionResult Create([Bind(Include = "id,UserId,Password,Description,IsLoggedIn,LastLoginTime")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {

            bool res = new HomeController().isLoggedIn();
            ViewBag.isLogin = res;
            if (res)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admin.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }


           
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,UserId,Password,Description,IsLoggedIn,LastLoginTime")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            bool res = new HomeController().isLoggedIn();
            ViewBag.isLogin = res;
            if (res)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Admin admin = db.Admin.Find(id);
                if (admin == null)
                {
                    return HttpNotFound();
                }
                return View(admin);
            }
            else
            {
                return RedirectToAction("Admin", "Home", new { isLogin = false });
            }            
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admin.Find(id);
            db.Admin.Remove(admin);
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
