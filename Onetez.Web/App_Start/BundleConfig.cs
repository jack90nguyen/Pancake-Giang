using System;
using System.Configuration;
using System.Web.Optimization;

namespace Onetez.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bool IsBundle = Convert.ToBoolean(ConfigurationManager.AppSettings["IsBundle"]);

            bundles.Add(new ScriptBundle("~/bundles-app/js").Include(
                "~/Scripts/jquery-2.0.3.min.js",
                "~/Scripts/jquery-1.8.2.min.js",
                "~/Scripts/clipboard.min.js",
                "~/Scripts/app.js")
            );

            bundles.Add(new StyleBundle("~/bundles-app/css").Include(
                "~/Asset/iconfeather/iconfont.css",
                "~/Asset/bulma.min.css",
                "~/Asset/app.css")
            );

            bundles.Add(new ScriptBundle("~/bundles-user-list/js").Include(
                "~/Scripts/user-list.js")
            );

            bundles.Add(new ScriptBundle("~/bundles-sheet-list/js").Include(
                "~/Scripts/sheet-list.js")
            );

            bundles.Add(new ScriptBundle("~/bundles-product-list/js").Include(
                "~/Scripts/product-list.js")
            );

            bundles.Add(new ScriptBundle("~/bundles-order-list/js").Include(
                "~/Scripts/order-list.js")
            );

            bundles.Add(new ScriptBundle("~/bundles-order-crawl/js").Include(
                "~/Scripts/order-crawl.js")
            );

            bundles.Add(new ScriptBundle("~/bundles-ads-list/js").Include(
                "~/Scripts/ads-list.js")
            );


            BundleTable.EnableOptimizations = IsBundle;
        }
    }
}