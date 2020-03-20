using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishClass.Models;

namespace EnglishClass.Controllers
{
    public class UserController : Controller
    {
        private db_EnglishClassEntities db = new db_EnglishClassEntities();

        // GET: User
        public ActionResult Index()
        {
            return View(db.tb_User.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_User tb_User = db.tb_User.Find(id);
            if (tb_User == null)
            {
                return HttpNotFound();
            }
            return View(tb_User);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UID,User_Name,User_PWD,State,Tel,PState")] tb_User tb_User)
        {
            if (ModelState.IsValid)
            {
                db.tb_User.Add(tb_User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tb_User);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_User tb_User = db.tb_User.Find(id);
            if (tb_User == null)
            {
                return HttpNotFound();
            }
            return View(tb_User);
        }

        // POST: User/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UID,User_Name,User_PWD,State,Tel,PState")] tb_User tb_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_User).State = EntityState.Modified;
                db.SaveChanges();
                string redirectUrl = "Details/" + tb_User.UID;
                return RedirectToAction(redirectUrl);
            }
            return View(tb_User);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_User tb_User = db.tb_User.Find(id);
            if (tb_User == null)
            {
                return HttpNotFound();
            }
            return View(tb_User);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_User tb_User = db.tb_User.Find(id);
            db.tb_User.Remove(tb_User);
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
