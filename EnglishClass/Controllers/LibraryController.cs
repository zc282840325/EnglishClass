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
    public class LibraryController : Controller
    {
        private db_EnglishClassEntities db = new db_EnglishClassEntities();

        // GET: Library
        public ActionResult Index()
        {
            var tb_Library = db.tb_Library.Include(t => t.tb_Video);
            return View(tb_Library.ToList());
        }

        // GET: Library/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Library tb_Library = db.tb_Library.Find(id);
            if (tb_Library == null)
            {
                return HttpNotFound();
            }
            return View(tb_Library);
        }

        // GET: Library/Create
        public ActionResult Create()
        {
            ViewBag.VID = new SelectList(db.tb_Video, "VID", "VName");
            return View();
        }

        // POST: Library/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lid,VID,State,Question,Answer1,Answer2,Answer3,TAnswer")] tb_Library tb_Library)
        {


            if (ModelState.IsValid)
            {
                db.tb_Library.Add(tb_Library);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VID = new SelectList(db.tb_Video, "VID", "VName", tb_Library.VID);
            return View(tb_Library);
        }

        // GET: Library/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Library tb_Library = db.tb_Library.Find(id);
            if (tb_Library == null)
            {
                return HttpNotFound();
            }
            List<ViewModel> list1 = new List<ViewModel>() { new ViewModel(0, "简单"),new ViewModel(1,"一般"),new ViewModel(2,"困难") };
            List<ViewModel> list2 = new List<ViewModel>() { new ViewModel(0, "A"), new ViewModel(1, "B"), new ViewModel(2, "C") };
            ViewBag.VID = new SelectList(db.tb_Video, "VID", "VName", tb_Library.VID);
            ViewBag.State = new SelectList(list1, "ID", "Name", tb_Library.State);
            ViewBag.TAnswer = new SelectList(list2, "ID", "Name", tb_Library.TAnswer);
            return View(tb_Library);
        }

        // POST: Library/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Lid,VID,State,Question,Answer1,Answer2,Answer3,TAnswer")] tb_Library tb_Library)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_Library).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VID = new SelectList(db.tb_Video, "VID", "VName", tb_Library.VID);
            return View(tb_Library);
        }

        // GET: Library/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Library tb_Library = db.tb_Library.Find(id);
            if (tb_Library == null)
            {
                return HttpNotFound();
            }
            return View(tb_Library);
        }

        // POST: Library/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_Library tb_Library = db.tb_Library.Find(id);
            db.tb_Library.Remove(tb_Library);
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

        public string CheckNums(int vid,int state)
        {
            int result = 0;
            var user = db.tb_Library.Where(x => x.VID == vid && x.State == state).ToList();
            if (user!=null)
            {
                if (user.Count==3)
                {
                    result = 1;
                }
            }
            return "{\"result\":\""+result+"\"}";
        }
    }
}
