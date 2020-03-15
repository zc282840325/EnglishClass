﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnglishClass.Models;

namespace EnglishClass.Controllers
{
    public class VideoController : Controller
    {
        private db_EnglishClassEntities db = new db_EnglishClassEntities();

        // GET: Video
        public ActionResult Index()
        {
            var tb_Video = db.tb_Video.Include(t => t.tb_User);
            return View(tb_Video.ToList());
        }

        // GET: Video/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Video tb_Video = db.tb_Video.Find(id);
            if (tb_Video == null)
            {
                return HttpNotFound();
            }
            return View(tb_Video);
        }

        // GET: Video/Create
        public ActionResult Create()
        {
            ViewBag.UID = new SelectList(db.tb_User, "UID", "User_Name");
            return View();
        }

        // POST: Video/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VID,UID,VName,VAddress,CreateTime,Remake,Vimage")] tb_Video tb_Video)
        {
            tb_Video.CreateTime = DateTime.Now;
            tb_Video.UID = Convert.ToInt32(Request.Cookies["UID"].Value);
            if (ModelState.IsValid)
            {
                db.tb_Video.Add(tb_Video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UID = new SelectList(db.tb_User, "UID", "User_Name", tb_Video.UID);
            return View(tb_Video);
        }

        // GET: Video/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Video tb_Video = db.tb_Video.Find(id);
            if (tb_Video == null)
            {
                return HttpNotFound();
            }
            ViewBag.UID = new SelectList(db.tb_User, "UID", "User_Name", tb_Video.UID);
            return View(tb_Video);
        }

        // POST: Video/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VID,UID,VName,VAddress,CreateTime,Remake,Vimage")] tb_Video tb_Video)
        {
            tb_Video.UID = Convert.ToInt32(Request.Cookies["UID"].Value);
            if (ModelState.IsValid)
            {
                db.Entry(tb_Video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UID = new SelectList(db.tb_User, "UID", "User_Name", tb_Video.UID);
            return View(tb_Video);
        }

        // GET: Video/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Video tb_Video = db.tb_Video.Find(id);
            if (tb_Video == null)
            {
                return HttpNotFound();
            }
            return View(tb_Video);
        }

        // POST: Video/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tb_Video tb_Video = db.tb_Video.Find(id);
            db.tb_Video.Remove(tb_Video);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UpLoadProcess(HttpPostedFileBase imgFile)
        {

            Hashtable hash = new Hashtable();
            if (Request.Files.Count <= 0)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "请选择文件";
                return Json(hash);
            }
            imgFile = Request.Files[0];
            string fileTypes = "gif,jpg,jpeg,png,bmp,mp4";
            string fileName = imgFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "Upload file extension is an extension that is not allowed.";
                return Json(hash);
            }

            string filePathName = string.Empty;
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Resource");
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "Save failed" }, id = "id" });
            }
            string ex = Path.GetExtension(imgFile.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            imgFile.SaveAs(Path.Combine(localPath, filePathName));
            return Json(new
            {
                error = 0,
                message = "/Resource" + "/" + filePathName
            });

        }
        [HttpPost]
        public ActionResult ProcessRequest(HttpPostedFileBase file)
        {
            Hashtable hash = new Hashtable();
            if (Request.Files.Count <= 0)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "请选择文件";
                return Json(hash);
            }
            file = Request.Files[0];
            string fileTypes = "gif,jpg,jpeg,png,bmp,mp4";
            string fileName = file.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();

            if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                hash = new Hashtable();
                hash["error"] = 1;
                hash["message"] = "Upload file extension is an extension that is not allowed.";
                return Json(hash);
            }
            string filePathName = string.Empty;
            string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Resource");
            if (Request.Files.Count == 0)
            {
                return Json(new { jsonrpc = 2.0, error = new { code = 102, message = "Save failed" }, id = "id" });
            }
            string ex = Path.GetExtension(file.FileName);
            filePathName = Guid.NewGuid().ToString("N") + ex;
            if (!System.IO.Directory.Exists(localPath))
            {
                System.IO.Directory.CreateDirectory(localPath);
            }
            file.SaveAs(Path.Combine(localPath, filePathName));
            return Json(new
            {
                error = 0,
                message = "/Resource" + "/" + filePathName
            });

        }

        public PartialViewResult ChatWith()
        {
            return PartialView();

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