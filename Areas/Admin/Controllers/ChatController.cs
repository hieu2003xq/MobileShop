using ado.Areas.Admin.Data;
using ado.Hubs;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ado.Areas.Admin.Controllers
{
    public class ChatController : Controller
    {
        // GET: Admin/Chat
        public ActionResult Index()
        {
            if (Session["tkAD"] != null)
            {
                return View();
            }
            else { return RedirectToAction("Index", "Login"); }
        }
        public ActionResult loadDS(string ten)
        {
            tkAdmin tkAdmin = new tkAdmin();
            var lst = tkAdmin.listCheck(ten);
            if (lst != null)
            {
                return Json(new {success=true,data=lst},JsonRequestBehavior.AllowGet);
            }return Json(new { success = false },JsonRequestBehavior.AllowGet);
        }
    }
}