using EnglishClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnglishClass.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index2()
        {

            SetCookie("UU", "1", 1);
            return View("Index");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="tel"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        [HttpPost]
        public string Register(string name, string password, string tel, string select)
        {
            string msg = string.Empty;
            bool result = false;
            try
            {
                using (db_EnglishClassEntities db = new db_EnglishClassEntities())
                {
                    tb_User user = new tb_User();
                    user.User_Name = name;
                    user.User_PWD = password;
                    user.Tel = tel;
                    user.State = select;
                    db.tb_User.Add(user);
                    db.SaveChanges();
                    if (user.State == "0")
                    {
                        tb_Plant plant = new tb_Plant();
                        plant.UID = user.UID;
                        plant.State = "0";
                        db.tb_Plant.Add(plant);
                        db.SaveChanges();
                    }
                    result = true;
                }
            }
            catch (Exception e)
            {
                result = false;
                msg = e.Message;
            }
            return "{\"result\":\"" + result + "\",\"Msg\":\"" + msg + "\"}";
        }

        public string CheckedLogin()
        {
            int result = 0;

            if (Request.Cookies["UU"] != null)
            {
                result = 1;
                RemoveCookie("UU");
            }
            return "{\"result\":\"" + result + "\"}";
        }

        /// <summary>
        /// 注册检查重名
        /// </summary>
        /// <param name="name"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckName(string name, string row)
        {
            int result = 0;
            using (db_EnglishClassEntities db = new db_EnglishClassEntities())
            {
                var user = db.tb_User.Where(x => x.User_Name == name && x.State == row).FirstOrDefault();
                if (user != null)
                {
                    result = 1;
                }
            }
            return "{\"result\":\""+result+"\"}";

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        public string Login(string name, string password, string select, bool checkbox)
        {
            string Msg = string.Empty;
            int result = 0;
            try
            {
                using (db_EnglishClassEntities db = new db_EnglishClassEntities())
                {
                    var user = db.tb_User.Where(x => x.User_Name == name && x.User_PWD == password && x.State == select).FirstOrDefault();
                    if (user != null)
                    {
                        int days = 1;
                        if (checkbox)
                        {
                            days = 30;
                        }
                        result = 1;
                        SetCookie("UserName", user.User_Name, days);//存储用户名
                        SetCookie("UID", user.UID.ToString(), days);//存储用户ID
                        SetCookie("Pstate", user.PState, days);//存储学生的模式编号
                        SetCookie("State", user.State, days);//存储用户身份
                    }
                    else
                    {
                        Msg = "账号或密码错误";
                        result = 0;
                    }
                }
            }
            catch (Exception e)
            {
                result = 0;
                Msg = e.Message;
            }
            return "{\"result\":\"" + result + "\",\"Msg\":\"" + Msg + "\"}";
        }


        public ActionResult SignOut()
        {
            RemoveCookie("UserName");//存储用户名
            RemoveCookie("UID");//存储用户ID
            RemoveCookie("Pstate");//存储学生的模式编号
            RemoveCookie("State");//存储用户身份
            return View("Index");
        }
        public void SetCookie(string name, string value, int day)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(day);
            Response.Cookies.Add(cookie);
        }

        public void RemoveCookie(string name)
        {
            HttpCookie cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Now.AddDays(-5);
            Response.Cookies.Add(cookie);
        }

    }
}