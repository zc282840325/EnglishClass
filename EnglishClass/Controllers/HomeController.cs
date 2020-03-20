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
            List<tb_Video> list = db.tb_Video.OrderBy(x => x.VID).Where(x=>x.State==1).Take(3).ToList();

            return View(list);
        }
    
        public ActionResult About()
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            var user = db.tb_User.Where(x => x.UID == UID).FirstOrDefault();
            var state = "";
            if (user.PState == null)
            {
                state = "0";
            }
            else
            {
                state = user.PState;
            }
            ViewData["PState"] = state;
            var tb_Video = db.tb_Video.Include(t => t.tb_User).OrderBy(x => x.VID).ToList().Count;
            if (tb_Video%3==0)
            {
                ViewData["list"] = tb_Video % 3;
            }
            else
            {
                ViewData["list"] = tb_Video %3+1;
            }
      
            return View();
        }
        public string UpdateState(string state)
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            int result = 0;
            string msg = string.Empty;
            try
            {
                tb_User user = db.tb_User.Where(x => x.UID == UID).FirstOrDefault();
                user.PState = state;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                result = 1;
            }
            return "{\"result\":\""+result+"\",\"msg\":\""+ msg + "\"}";
        }

        public string InitState()
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            int result = 0;
            string msg = string.Empty;
            try
            {
                tb_User user = db.tb_User.Where(x => x.UID == UID).FirstOrDefault();
                if (user.PState==null)
                {
                    result = 0;
                }
                else if (user.PState == "1")
                {
                    result = 1;
                }
                else 
                {
                    result = 2;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                result = 3;
            }
            return "{\"result\":\"" + result + "\",\"msg\":\"" + msg + "\"}";
        }

        public string Check(int VID)
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            int result = 0;
            string msg = string.Empty;
            var user = db.tb_User.Where(x => x.UID == UID).FirstOrDefault();
            if (user.PState == null)
            {
                result = 0;
                msg = "请选择模式之后，在观看视频！";
            }
            //专家模式
            else if (user.PState == "1")
            {
               
                var Record = db.tb_RecordVideo.Where(x => x.UID == UID).Select(X=>X.VID).ToList();
                if (Record.Where(x=>x==VID).Count()>0)
                {
                    result = 2;
                }
                else
                {
                    var video = db.tb_Video.OrderBy(x=>x.VID).ToList();
                    if (video.Count <= 0)
                    {
                        result = 1;
                        msg = "请联系管理员，发生了未知的错误！";
                    }
                    for (int i = 0; i < Record.Count; i++)
                    {
                        var vv = db.tb_Video.Find(Record[i]);
                        if (video.Contains(vv))
                        {
                            video.Remove(vv);
                        }
                    }
                    if (video[0].VID!=VID)
                    {
                        result = 1;
                        msg = "请先观看"+ video[0].VName+"视频！";
                    }
                    else
                    {
                        result = 2;
                    }
                }
            }
            else
            {
                result = 2;
            }
            return "{\"result\":\"" + result + "\",\"msg\":\"" + msg + "\"}";
        }
        public void DeleteState()
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            tb_User user = db.tb_User.Where(x => x.UID == UID).FirstOrDefault();
            user.PState = null;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

        }
        public PartialViewResult ShowTop()
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            int fday = 0;
            var user = db.tb_Plant.Where(x => x.UID == UID && x.State == "0").FirstOrDefault();

            var now = db.tb_RecordVideo.Where(x => x.UID == 1 && x.CreateTime <= DateTime.Now).ToList();
           
            if (user != null)
            {
                TimeSpan d3 = DateTime.Now.Subtract(user.CreateTime);
                int sl;
                //计划已经完成
                if (user.Nums == now.Count)
                {
                    db.Entry(db.tb_Plant).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (now.Count==0)
                {
                    sl = 0;
                    ViewData["now_days"] = 0;
                    fday= user.Days;
                }
                else
                {
                    sl = d3.Days / now.Count;
                    ViewData["now_days"] = d3.Days;
                    fday= sl * (user.Nums - now.Count);
                }
            
                ViewData["old_nums"] = user.Nums;
                ViewData["old_days"] = user.Days;
                ViewData["now_nums"] = now.Count;
          
                ViewData["future_nums"] = user.Nums - now.Count;

                ViewData["future_days"] = fday;
                ViewData["pid"] = user.PID;
            }
            else
            {
                ViewData["old_nums"] = 0;
                ViewData["old_days"] = 0;
                ViewData["now_nums"] = 0;

                ViewData["future_nums"] = 0;

                ViewData["future_days"] = 0;
                ViewData["pid"] = 0;
            }
      
            return PartialView(user);
        }

       public ActionResult Contact()
        {
            int uid = Convert.ToInt32(Request.Cookies["UID"].Value);
            var msglist = db.tb_Message.Where(x => x.SID == uid).ToList();
            ViewData["mlist"] = msglist.Count;
            var tlist = db.tb_Result.Where(x => x.UID == uid&&x.Result==0).ToList();
            var llist = db.tb_Result.Where(x => x.UID == uid).ToList();
            ViewData["tlist"] = tlist.Count;
            ViewData["flist"] = llist.Count- tlist.Count;
            ViewData["llist"] = llist.Count;
            return View();
        }
        public string AddorUpdatePlant(int nums,int days)
        {
            int UID = int.Parse(Request.Cookies["UID"].Value);
            string msg = string.Empty;
            int result = 0;
            var user = (tb_Plant)db.tb_Plant.Where(x => x.UID == UID && x.State == "0").FirstOrDefault();
  
            if (user==null)
            {
                tb_Plant plant = new tb_Plant();
                plant.State = "0";
                plant.UID = UID;
                plant.Days = days;
                plant.Nums = nums;
                plant.CreateTime = DateTime.Now;
                db.tb_Plant.Add(plant);
                db.SaveChanges();
            }
            else
            {
                user.Days = days;
                user.Nums = nums;
                db.Entry(user).State = EntityState.Modified;
            }
           
           //  user.CreateTime = DateTime.Now;
            try
            {
              
                db.SaveChanges();
                result = 1;
            }
            catch (Exception ex)
            {
                msg=ex.Message;
            }
            return "{\"result\":\""+result+"\",\"Msg\":\""+msg+"\"}";
        }

        public void Delete(int pid)
        {
            tb_Plant user = db.tb_Plant.Find(pid);
            db.tb_Plant.Remove(user);
            //user.State = "1";
            //db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public PartialViewResult ShowList(int page)
        {
            var tb_Video = db.tb_Video.Include(t => t.tb_User).OrderBy(x => x.VID);
            var count = tb_Video.Count();
            if (page == 0)
            {
                page = 1;
            }
            int list = (page - 1) * 3;
            if (list!=0)
            {
                list -= 1;
            }
            var ss = tb_Video.Take(3).Skip(list);
            return PartialView(ss.ToList());
        }
    }
}