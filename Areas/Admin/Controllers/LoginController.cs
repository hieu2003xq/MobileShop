using ado.Areas.Admin.Data;
using ado.Hubs;
using ado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ado.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public tkAdmin tkAD = new tkAdmin();
        Connection connection=new Connection();
        taoToken tao=new taoToken();
        public ActionResult Index()
        {
            return View();
        }
        // GET: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string tenDN, string matKhau)
        {
            
            var check = tkAD.ktra(tenDN, matKhau);
            
            if (check)
            {
                
                Session["tkAD"] = tenDN;
                
                return Json(new {success=true});
            }
            return Json(new { success = false });

        }
        public ActionResult LogOut()
        {
            managerConnection managerConnection = new managerConnection();
            managerConnection.RemoveConnection(Session["tkAD"].ToString());
            Session.Clear();
            Session.Abandon();// xoa bo toan` bo session
            return RedirectToAction("Index","Login");
        }
    }
}