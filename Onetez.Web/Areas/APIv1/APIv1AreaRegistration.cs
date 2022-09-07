using System.Web.Mvc;

namespace Onetez.Web.Areas.APIv1
{
    public class APIv1AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "APIv1";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "APIv1_default",
                "APIv1/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}