using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class userPersonalDetailsController : Controller
    {
        private DataquadEntities db = new DataquadEntities();

        // GET: userPersonalDetails
        public ActionResult Index()
        {
            return View(db.userPersonalDetails.ToList());
        }

        // GET: userPersonalDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userPersonalDetails userPersonalDetails = db.userPersonalDetails.Find(id);
            if (userPersonalDetails == null)
            {
                return HttpNotFound();
            }
            return View(userPersonalDetails);
        }

        // GET: userPersonalDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: userPersonalDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,firstName,lastName,dateOfBirth,nationality,age,gender")] userPersonalDetails userPersonalDetails)
        {
            if (ModelState.IsValid)
            {
                db.userPersonalDetails.Add(userPersonalDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userPersonalDetails);
        }

        // GET: userPersonalDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userPersonalDetails userPersonalDetails = db.userPersonalDetails.Find(id);
            if (userPersonalDetails == null)
            {
                return HttpNotFound();
            }
            return View(userPersonalDetails);
        }

        // POST: userPersonalDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,firstName,lastName,dateOfBirth,nationality,age,gender")] userPersonalDetails userPersonalDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userPersonalDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userPersonalDetails);
        }

        // GET: userPersonalDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userPersonalDetails userPersonalDetails = db.userPersonalDetails.Find(id);
            if (userPersonalDetails == null)
            {
                return HttpNotFound();
            }
            return View(userPersonalDetails);
        }

        // POST: userPersonalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userPersonalDetails userPersonalDetails = db.userPersonalDetails.Find(id);
            db.userPersonalDetails.Remove(userPersonalDetails);
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
