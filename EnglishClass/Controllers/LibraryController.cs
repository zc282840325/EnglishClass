using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishClass.Models;
using Newtonsoft.Json;

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
        public string  DetailsList(int? VID, int state)
        {
            
           var tb_Library = db.tb_Library.Include(x=>x.tb_Video).Where(x => x.VID == VID && x.State == state).ToList();
            List<tb_Library> list = new List<tb_Library>();
            for (int i = 0; i < tb_Library.Count; i++)
            {
                list.Add(new Models.tb_Library() {VID= tb_Library[i].VID,Answer1=tb_Library[i].Answer1, Answer2 = tb_Library[i].Answer2, Answer3 = tb_Library[i].Answer3, Question = tb_Library[i].Question,Lid= tb_Library[i].Lid,State= tb_Library[i].State,TAnswer= tb_Library[i].TAnswer });
            } 
       var   result = "{\"count\":\"" + tb_Library.Count + "\",\"user\":" + JsonConvert.SerializeObject(list) + "}";
            return result;
        }


        public void AddOrUpdata(int result,int LID)
        {
            int uid = Convert.ToInt32(Request.Cookies["UID"].Value);
            var user = db.tb_Result.Where(x => x.UID == uid && x.LID == LID).FirstOrDefault();
            if (user==null)
            {
                tb_Result rr = new tb_Result();
                rr.Result = result;
                rr.UID = uid;
                rr.LID = LID;
                db.tb_Result.Add(rr);
                db.SaveChanges();
            }
            else
            {
                user.Result = result;
                db.SaveChanges();
            }
        }

        public void Check()
        {
            int uid = Convert.ToInt32(Request.Cookies["UID"].Value);
            var list = db.tb_Result.Where(x => x.UID == uid).ToList();
            var result="";
            for (int i = 0; i < 3; i++)
            {
                var ss = from a in list join b in db.tb_Library on a.LID equals b.Lid
                         where b.State==0 select b;
                if (ss.Count()>=3)
                {
                    result += "";
                }
            }

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
