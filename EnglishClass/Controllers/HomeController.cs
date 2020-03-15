using EnglishClass.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishClass.Controllers
{
    public class HomeController : Controller
    {
        private db_EnglishClassEntities db = new db_EnglishClassEntities();
        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult About()
        {
            
                return View();
        }

        public PartialViewResult ShowTop()
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            var user = db.tb_Plant.Where(x => x.UID == UID && x.State == "0").FirstOrDefault();

            var now = db.tb_RecordVideo.Where(x => x.UID == 1 && x.CreateTime <= DateTime.Now).ToList();
           
            if (user != null)
            {
                //计划已经完成
                if (user.Nums == now.Count)
                {
                    db.Entry(db.tb_Plant).State = EntityState.Modified;
                    db.SaveChanges();
                }
                TimeSpan d3 = DateTime.Now.Subtract(user.CreateTime);
                ViewData["old_nums"] = user.Nums;
                ViewData["old_days"] = user.Days;
                ViewData["now_nums"] = now.Count;
                ViewData["now_days"] = d3.Days;
                ViewData["future_nums"] = user.Nums - now.Count;
                int sl = d3.Days / now.Count;
                ViewData["future_days"] = sl * (user.Nums - now.Count);
            }
            return PartialView(user);
        }

       public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public string AddorUpdatePlant(int nums,int days)
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            string msg = string.Empty;
            int result = 0;
            tb_Plant user = (tb_Plant)db.tb_Plant.Where(x => x.UID == UID && x.State == "0").FirstOrDefault();
        
             user.Days = days;
             user.Nums = nums;
           //  user.CreateTime = DateTime.Now;
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                result = 1;
            }
            catch (Exception ex)
            {
                msg=ex.Message;
            }
            return "{\"result\":\""+result+"\",\"Msg\":\""+msg+"\"}";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult ShowList()
        {
            var tb_Video = db.tb_Video.Include(t => t.tb_User);
            return PartialView(tb_Video.ToList());
        }
    }
}