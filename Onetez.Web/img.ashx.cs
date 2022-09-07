using System;
using System.Drawing;
using System.IO;
using System.Web;
using ThumbnailSharp;

namespace Onetez.Web
{
    /// <summary>
    /// Summary description for img
    /// </summary>
    public class img : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string imgUrl = context.Request.QueryString["i"];
            int imgWidth = 0;
            int.TryParse(HttpContext.Current.Request.QueryString["w"], out imgWidth);
            int imgHeight = 0;
            int.TryParse(HttpContext.Current.Request.QueryString["h"], out imgHeight);

            context.Response.ContentType = "image/jpeg";
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.BufferOutput = false;

            try
            {
                string path = HttpContext.Current.Server.MapPath(string.Format("~/{0}", imgUrl));
                uint size = Convert.ToUInt32(imgHeight);
                Format fileFormat = Format.Jpeg;
                if (imgUrl.ToLower().EndsWith(".png"))
                    fileFormat = Format.Png;
                else if (imgUrl.ToLower().EndsWith(".bmp"))
                    fileFormat = Format.Bmp;
                else if (imgUrl.ToLower().EndsWith(".gif"))
                    fileFormat = Format.Gif;
                else if (imgUrl.ToLower().EndsWith(".tiff"))
                    fileFormat = Format.Tiff;
                else
                    fileFormat = Format.Jpeg;

                Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(
                    thumbnailSize: size,
                    imageFileLocation: path,
                    imageFormat: fileFormat
                );

                Image thumbmage = System.Drawing.Image.FromStream(resultStream, true, true);
                thumbmage.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                thumbmage.Dispose();
            }
            catch (Exception)
            {
                string path = HttpContext.Current.Server.MapPath(string.Format("~/{0}", "/Images/default.jpg"));
                uint size = Convert.ToUInt32(imgHeight);

                Stream resultStream = new ThumbnailCreator().CreateThumbnailStream(
                    thumbnailSize: size,
                    imageFileLocation: path,
                    imageFormat: Format.Jpeg
                );

                Image thumbmage = System.Drawing.Image.FromStream(resultStream, true, true);
                thumbmage.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                thumbmage.Dispose();
            }
        }

        public bool ThumbnailCallback()
        {
            return true;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}