using System;
using System.IO;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.EntityClasses;
using Onetez.Core.DbContext;

namespace Onetez.Web.Areas.APIv1.Controllers
{
    public class UserController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonResult ResetPassword(string id)
        {
            bool isRole = UserInfo.role.is_admin;
            bool isStatus = false;
            string message = string.Empty;

            if (isRole)
            {
                var user = DbUser.Get(id);
                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(id, "MD5");

                isStatus = user.Save();

                if (isStatus)
                    message = "Mật khẩu của tài khoản " + user.Username +" là: " + id;
                else
                    message = "Không thể cấp lại mật khẩu, vui lòng thử lại";
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message });
        }


        [Authorize]
        [HttpPost]
        public JsonResult Delete(string id)
        {
            bool isRole = UserInfo.role.is_admin;
            bool isStatus = false;
            string message = string.Empty;

            if (isRole)
            {
                isStatus = DbUser.Delete(id);

                if (!isStatus)
                    message = "Không thể xóa tài khoản, vui lòng thử lại";
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message });
        }
    }
}
