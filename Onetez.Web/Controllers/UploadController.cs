using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Onetez.Core.DbContext;
using Onetez.Core.Libs;
using Onetez.Dal.Models;

namespace Onetez.Web.Controllers
{
    public class UploadController : BaseController
    {
        public string FolderUpload = "/Upload/";

        public ActionResult Index()
        {
            var results = new List<ImageModel>();
            
            if(Request.QueryString["folder"] != null)
                FolderUpload += Request.QueryString["folder"].Replace("_", "/");
            else
                FolderUpload += "Upload/";

            if(!FolderUpload.EndsWith("/"))
                FolderUpload += "/";

            string path = AppDomain.CurrentDomain.BaseDirectory + FolderUpload;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (string item in Request.Files)
            {
                var fileUpload = Request.Files[item];
                if (fileUpload.FileName != "")
                {
                    var fileName = Path.GetFileName(ConvertString.RenameFile(fileUpload.FileName));

                    var image = new ImageModel();

                    if (CheckFileUpload(fileName))
                    {
                        try
                        {
                            if (System.IO.File.Exists(path + fileName))
                                fileName = "2-" + fileName;

                            fileUpload.SaveAs(Path.Combine(path, fileName));

                            image.id = DbConfig.GenerateId();
                            image.name = fileName;
                            image.link = FolderUpload + fileName;
                        }
                        catch (Exception ex)
                        {
                            image.id = "false";
                            image.name = ex.Message;
                        }
                    }
                    else
                    {
                        image.id = "false";
                        image.name = "File not allowed";
                    }

                    results.Add(image);
                }
            }


            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public bool CheckFileUpload(string fileName)
        {
            bool isAllow = true;

            string[] notAllow = new string[] { ".exe", ".js", ".html", ".asp", ".aspx" };

            for (int i = 0; i < notAllow.Length; i++)
            {
                if(fileName.Contains(notAllow[i]))
                {
                    isAllow = false;
                    break;
                }
            }

            return isAllow;
        }

        [HttpPost]
        public JsonResult DeleteFile(string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string getFile = path + file;
            try
            {
                if (System.IO.File.Exists(getFile))
                    System.IO.File.Delete(getFile);
                return Json(new { status = true });
            }
            catch (Exception)
            {
                return Json(new { status = false, msg = "Can not delete file!" });
                throw;
            }
        }
    }
}
