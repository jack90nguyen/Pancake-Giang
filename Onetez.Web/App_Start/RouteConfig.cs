using System.Web.Mvc;
using System.Web.Routing;

namespace Onetez.Web
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


      #region Tài khoản

      // Danh sách tài khoản
      routes.MapRoute(
          name: "User List",
          url: "user/list",
          defaults: new
          {
            controller = "User",
            action = "Index",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );


      // Tạo tài khoản
      routes.MapRoute(
          name: "User Create",
          url: "user/create",
          defaults: new
          {
            controller = "User",
            action = "Create",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );


      // Thông tin tài khoản
      routes.MapRoute(
          name: "User Info",
          url: "user/info/{id}",
          defaults: new
          {
            controller = "User",
            action = "Info",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );


      // Đăng nhập
      routes.MapRoute(
          name: "User Login",
          url: "login.html",
          defaults: new
          {
            controller = "User",
            action = "Login",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );


      // Đăng xuất
      routes.MapRoute(
          name: "User Logout",
          url: "logout.html",
          defaults: new
          {
            controller = "Home",
            action = "Logout",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      #endregion


      #region Lỗi

      // Page 404
      routes.MapRoute(
          name: "Page 404",
          url: "page/404",
          defaults: new
          {
            controller = "Home",
            action = "Page404",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Page Not Role
      routes.MapRoute(
          name: "Page Not Role",
          url: "page/role",
          defaults: new
          {
            controller = "Home",
            action = "PageRole",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      #endregion



      // Sheet: đơn chưa xử lý
      routes.MapRoute(
          name: "Sheet News",
          url: "sheet/news",
          defaults: new
          {
            controller = "Sheet",
            action = "News",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Sheet: đơn đang xử lý
      routes.MapRoute(
          name: "Sheet Process",
          url: "sheet/process",
          defaults: new
          {
            controller = "Sheet",
            action = "Process",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Sheet: đơn thành công, đơn lỗi
      routes.MapRoute(
          name: "Sheet List",
          url: "sheet/list",
          defaults: new
          {
            controller = "Sheet",
            action = "Index",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Sheet: chuyển đổi data
      routes.MapRoute(
          name: "Sheet Product",
          url: "sheet/product",
          defaults: new
          {
            controller = "Sheet",
            action = "Product",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Sheet: thông kê chốt đơn
      routes.MapRoute(
          name: "Sheet Report",
          url: "sheet/report",
          defaults: new
          {
            controller = "Sheet",
            action = "Report",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Sheet: Chi phí Ads
      routes.MapRoute(
          name: "Ads List",
          url: "sheet/ads",
          defaults: new
          {
            controller = "Ads",
            action = "Ads",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );



      // Xử lý đơn hàng
      routes.MapRoute(
          name: "Order List",
          url: "order/list",
          defaults: new
          {
            controller = "Order",
            action = "Index",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );


      // Import đơn hàng
      routes.MapRoute(
          name: "Order Import",
          url: "order/import",
          defaults: new
          {
            controller = "Order",
            action = "Import",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Quét đơn hàng
      routes.MapRoute(
          name: "Order Crawl",
          url: "order/crawl",
          defaults: new
          {
            controller = "Order",
            action = "Crawl",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );


      // Cấu hình hệ thống
      routes.MapRoute(
          name: "Config System",
          url: "config/system",
          defaults: new
          {
            controller = "Config",
            action = "Index",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Cấu hình shop
      routes.MapRoute(
          name: "Config Shop",
          url: "config/shop/{id}",
          defaults: new
          {
            controller = "Config",
            action = "Shop",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // SQL Server
      routes.MapRoute(
          name: "Config SqlServer",
          url: "config/sql",
          defaults: new
          {
            controller = "Config",
            action = "SqlServer",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );

      // Cấu hình màu tag
      routes.MapRoute(
          name: "Config Color",
          url: "config/color/{type}",
          defaults: new
          {
            controller = "Config",
            action = "Color",
            id = UrlParameter.Optional
          },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );


      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
          namespaces: new[] { "Onetez.Web.Controllers" }
      );
    }
  }
}