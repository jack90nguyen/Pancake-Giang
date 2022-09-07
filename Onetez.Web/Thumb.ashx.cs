using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Onetez.Web
{
    /// <summary>
    /// Summary description for Thumb
    /// </summary>
    public class Thumb : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string imgUrl = context.Request.QueryString["i"];

            if (!string.IsNullOrEmpty(imgUrl))
            {
                string domain = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;

                imgUrl = imgUrl.ToLower();

                string filename = new Uri(imgUrl).Segments.Last();
                string fileDecode = HttpUtility.UrlDecode(filename);
                string folder = (imgUrl + "*").Replace(domain, "").Replace(fileDecode + "*", "");
                string fileFormat = imgUrl.ToLower().EndsWith(".png") ? ".png" : ".jpg";
                string folderThumb1 = folder.Replace("storedata", "thumb/100");
                string folderThumb2 = folder.Replace("storedata", "thumb/400");

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();

                    Image originalImage = System.Drawing.Image.FromStream(responseStream, true, true);

                    #region Thumbnail 100px

                    int imgHeight1 = 100;
                    int imgWidth1 = imgHeight1 * originalImage.Size.Width / originalImage.Size.Height;

                    string path1 = AppDomain.CurrentDomain.BaseDirectory + folderThumb1;
                    if (!Directory.Exists(path1))
                        Directory.CreateDirectory(path1);

                    Image resizedImage1 = ResizeImage(originalImage, new Size(imgWidth1, imgHeight1));
                    resizedImage1.Save(path1 + fileDecode, (fileFormat == ".png" ? ImageFormat.Png : ImageFormat.Jpeg));
                    resizedImage1.Dispose();

                    string str1 = folderThumb1 + fileDecode;

                    #endregion

                    #region Thumbnail 400px

                    int imgHeight2 = 400;
                    int imgWidth2 = imgHeight2 * originalImage.Size.Width / originalImage.Size.Height;

                    string path2 = AppDomain.CurrentDomain.BaseDirectory + folderThumb2;
                    if (!Directory.Exists(path2))
                        Directory.CreateDirectory(path2);

                    Image resizedImage2 = ResizeImage(originalImage, new Size(imgWidth2, imgHeight2));
                    resizedImage2.Save(path2 + fileDecode, (fileFormat == ".png" ? ImageFormat.Png : ImageFormat.Jpeg));
                    resizedImage2.Dispose();

                    string str2 = folderThumb2 + fileDecode;

                    #endregion

                    context.Response.Write(str1 + " || " + str2);
                }
                catch (Exception)
                {
                    context.Response.Write("File not found | file: " + filename + " | decode: " + fileDecode + " | folder: " + folder);
                }
            }
            else
            {
                context.Response.Write("File not found");
            }
        }

        private static Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            //float nPercent = 0;
            //nPercent = ((float)sourceHeight / (float)size.Height);
            //int destHeight = (int)(size.Height);
            //int destWidth = (int)(sourceWidth / nPercent);

            int destWidth = (int)(size.Width);
            int destHeight = (int)(size.Height);

            Image b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
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