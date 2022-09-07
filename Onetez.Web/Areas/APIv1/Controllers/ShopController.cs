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
  public class ShopController : BaseController
  {
    [Authorize]
    [HttpPost]
    public JsonResult Delete(int id)
    {
      var isRole = UserInfo.role.is_admin;
      var isStatus = false;
      var message = string.Empty;

      if (isRole)
      {
        isStatus = DbShop.Delete(id);

        if (isStatus)
        {
          // Xóa sản phẩm của shop
          DbConfig.SqlUpdate($"delete Products where ShopId = '{id}'", out message);

          // Xóa Sheet
          DbConfig.SqlUpdate($"delete Sheets where ShopId = '{id}'", out message);

          // Xóa Sheet
          DbConfig.SqlUpdate($"delete Orders where ShopId = '{id}'", out message);

        }
        else
          message = "Không thể xóa Shop này, vui lòng thử lại";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message });
    }
  }
}
