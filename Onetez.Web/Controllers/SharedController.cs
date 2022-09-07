using System.Web.Mvc;
using Onetez.Core.Libs;
using Onetez.Core.DbContext;
using Onetez.Dal.EntityClasses;

namespace Onetez.Web.Controllers
{
  public class SharedController : BaseController
  {
    [ChildActionOnly]
    public ActionResult _Header()
    {
      ViewBag.UserInfo = UserInfo;

      if(UserInfo != null)
      {
        var user = DbUser.Get(UserInfo.id);
        user.Online = DateTimeNow;
        user.Save();
      }

      return PartialView();
    }
  }
}