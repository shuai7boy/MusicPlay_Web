using MP.Entity.Models;
using MP.Service;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicPlay_Web.Controllers
{
    /// <summary>
    /// web--音乐播放
    /// </summary>
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        /// <summary>
        /// 音乐展示
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 上传音乐信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpLoadMusic()
        {
            return View();
        }

        /// <summary>
        /// 上传封面
        /// </summary>
        /// <returns></returns>
       [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase imgFile, string dir)
        {
            //定义允许上传的文件的扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
            if (String.IsNullOrEmpty(dir))
            {
                dir = "image";
            }
            if (!extTable.ContainsKey(dir))//如果不包含
            {
                return Content(JsonConvert.SerializeObject(new { error = 1, message = "文件格式不正确" }));
            }
            if (imgFile == null)
            {
                return Content(JsonConvert.SerializeObject(new { error = 1, message = "上传文件大小超过限制" }));
            }
            string fileName = imgFile.FileName;
            string fileExt = Path.GetExtension(fileName).ToLower();
            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dir]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Content(JsonConvert.SerializeObject(new { error = 1, message = "上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dir]) + "格式" }));
            }
            //创建文件夹
            string dirPath = "../Files/" + dir + "/";
            if (!Directory.Exists(Request.MapPath(dirPath)))
            {
                //不存在就创建
                Directory.CreateDirectory(Request.MapPath(dirPath));
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff") + fileExt;
            imgFile.SaveAs(Request.MapPath(dirPath + newFileName));
            //判断保存的文件是否存在
            if (System.IO.File.Exists(Request.MapPath(dirPath + newFileName)))
            {
                return Content(JsonConvert.SerializeObject(new { error = 0, url = dirPath + newFileName }));
            }
            else
            {
                return Content(JsonConvert.SerializeObject(new { error = 1, message = "上传文件失败!" }));
            }
        }
       /// <summary>
       /// 上传音乐
       /// </summary>
       /// <returns></returns>
       [HttpPost]
       public ActionResult UploadMusic(HttpPostedFileBase musicFile, string dir)
       {
           //定义允许上传的文件的扩展名
           Hashtable extTable = new Hashtable();
           extTable.Add("image", "gif,jpg,jpeg,png,bmp");
           extTable.Add("flash", "swf,flv");
           extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
           extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");
           if (String.IsNullOrEmpty(dir))
           {
               dir = "image";
           }
           if (!extTable.ContainsKey(dir))//如果不包含
           {
               return Content(JsonConvert.SerializeObject(new { error = 1, message = "文件格式不正确" }));
           }
           if (musicFile == null)
           {
               return Content(JsonConvert.SerializeObject(new { error = 1, message = "上传文件大小超过限制" }));
           }
           string fileName = musicFile.FileName;
           string fileExt = Path.GetExtension(fileName).ToLower();
           if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dir]).Split(','), fileExt.Substring(1).ToLower()) == -1)
           {
               return Content(JsonConvert.SerializeObject(new { error = 1, message = "上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dir]) + "格式" }));
           }
           //创建文件夹
           string dirPath = "../Files/" + dir + "/";
           if (!Directory.Exists(Request.MapPath(dirPath)))
           {
               //不存在就创建
               Directory.CreateDirectory(Request.MapPath(dirPath));
           }
           string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff") + fileExt;
           musicFile.SaveAs(Request.MapPath(dirPath + newFileName));
           //判断保存的文件是否存在
           if (System.IO.File.Exists(Request.MapPath(dirPath + newFileName)))
           {
               return Content(JsonConvert.SerializeObject(new { error = 0, url = dirPath + newFileName }));
           }
           else
           {
               return Content(JsonConvert.SerializeObject(new { error = 1, message = "上传文件失败!" }));
           }
       }
       musicService service = new musicService();
        /// <summary>
        /// 保存音乐信息
        /// </summary>
        /// <returns></returns>
       [HttpPost]
        public ActionResult SaveMusicMsg(music model)
       {
           string result = "ok";
           try { 
           if (model!=null)
           {
               service.Insert(model);
               service.Save();
           }          
           }
           catch(Exception ext)
           {
               result = "no";
           }
           return Content(result);
       }
       /// <summary>
       /// 获取第一条音乐
       /// </summary>
       /// <returns></returns>
       public ActionResult GetFirstMusic()
       {
           music model = new music();
           model=service.GetFirstMusic();
           if (model!=null)
           {
               return Content(JsonConvert.SerializeObject(new {msg=1,title = model.title, author = model.author, url = model.url, pic=model.pic }));
           }
           else
           {
               return Content(JsonConvert.SerializeObject(new { msg=0}));
           }
           
       }
        /// <summary>
        /// 获取点击的音乐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetClickMusic(int id)
       {
           music model=service.FirstOrDefault(c => c.id == id);
           if (model!=null)
           {
                return Content(JsonConvert.SerializeObject(new {msg=1,title = model.title, author = model.author, url = model.url, pic=model.pic }));
           }
           else
           {
               return Content(JsonConvert.SerializeObject(new { msg = 0 }));
           }
       }
        /// <summary>
        /// 获取所有的音乐
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllMusic()
        {
            List<music> list = service.Find().ToList();
            if (list!=null)
            {
                return Json(new { msg = "ok", list = list }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { msg = "no",list="" }, JsonRequestBehavior.AllowGet);
            }
          
        }

        public ActionResult DeleteMusic(int id)
        {
            string result = "ok";
            music model = service.FirstOrDefault(c => c.id == id);
            try { 
            if (model!=null)
            {
                service.Delete(model);
                service.Save();              
            }
            }
            catch
            {
                result = "no";
            }
            return Content(result);
        }
    }
}
