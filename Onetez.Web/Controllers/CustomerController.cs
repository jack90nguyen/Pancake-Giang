using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.EntityClasses;
using Onetez.Core.DbContext;

namespace Onetez.Web.Controllers
{
    public class CustomerController : BaseController
    {
        #region Danh sách khách hàng

        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            // USER: kiểm tra quyền
            if (!UserInfo.role.is_role)
                return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });


            var results = DbCustomer.GetList();

            return View(results);
        }

        #endregion


        #region Thông tin khách hàng

        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            // USER: kiểm tra quyền
            if (!UserInfo.role.is_role)
                return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });


            var customer = string.IsNullOrEmpty(id) ? new CustomersEntity() : DbCustomer.Get(id);

            if (customer != null)
            {
                return View(customer);
            }
            else
            {
                return RedirectToAction("Page404", "Home", new { url = Request.RawUrl });
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string id, CustomersEntity model, bool isPopup)
        {
            var customer = string.IsNullOrEmpty(id) ? new CustomersEntity() : DbCustomer.Get(id);

            if (string.IsNullOrEmpty(model.Name))
            {
                ViewBag.Notification = Shared.RenderNotification("Nhập các thông tin bắt buộc", false);
            }
            else
            {
                if (string.IsNullOrEmpty(id))
                    customer.Id = DbConfig.GenerateId();

                customer.Name = model.Name;
                customer.Phone = model.Phone;
                customer.Email = model.Email;
                customer.Save();

                if (isPopup)
                    return RedirectToAction("Edit", "Customer", new { id = customer.Id, mode = "popup", @event = "close" });
                else
                    return RedirectToAction("Index", "Customer");
            }

            return View(customer);
        }

        #endregion
    }
}
