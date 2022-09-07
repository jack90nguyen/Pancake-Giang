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
    public class CustomerController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonResult Delete(string id)
        {
            bool isRole = UserInfo.role.is_role;
            bool isStatus = false;
            string message = string.Empty;

            if (isRole)
            {
                isStatus = DbCustomer.Delete(id);

                if (!isStatus)
                    message = "Không thể xóa khách hàng, vui lòng thử lại";
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message });
        }
    }
}
