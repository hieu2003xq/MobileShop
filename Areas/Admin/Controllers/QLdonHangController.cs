using ado.Areas.Admin.Data;
using ado.caching;
using ado.Models;
using ado.thuVienLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ado.Areas.Admin.Controllers
{
    public class QLdonHangController : Controller
    {
        // GET: Admin/QLdonHang
        private donHang don = new donHang();
        doanhThu doanhThu1 = new doanhThu();
        private readonly test11<donHang> _redisCacheService;
        SeriLogging logging=new SeriLogging();
        public QLdonHangController()
        {
            _redisCacheService = new test11<donHang>();
           
        }
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["tkAD"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult layDsDon()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string path = HttpContext.Request.Path;
            List<donHang>lst=new List<donHang>();
            if (_redisCacheService.CheckKey(path) == false)
            {
                lst = don.list();
                _redisCacheService.AddToList(path, lst,TimeSpan.FromMinutes(30));
                logging.successLogging(path);
            }
            else
            {

                lst = _redisCacheService.ReadDataFromRedis(path);
                logging.successLogging(path,"Redis");
            }
           
            int recordsTotals = lst.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                lst = lst.Where(m => m.tenSP.ToLower().Contains(searchValue)
                || m.tenKH.ToLower().Contains(searchValue)
                || m.tinhTrangTT.ToLower().Contains(searchValue)
                || m.tinhTrangGH.ToLower().Contains(searchValue)
                ).ToList();

            }

            var totalrowsafterfiltering = lst.Count;
            lst = lst.Skip(start).Take(length).ToList();
            return Json(new { data = lst, draw = Request["draw"], recordsTotal = recordsTotals, recordsFiltered = totalrowsafterfiltering });
        }
        public ActionResult xoaDon(int maDon)
        {
            
            donHang a = new donHang();
            var ret = a.xoa(maDon);
            if (ret ==true)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult docDon(int maDon)
        {
            donHang a = new donHang();
            
            var b = a.docDon(maDon);
          
            return Json(new { data = b, success = true });

        }
        [HttpPost]
        public ActionResult suaDon(int maDon, int tinhtrangGH)
        {
            sPbanChay sp=new sPbanChay();
            donHang don = new donHang();
            
            
            var ret = don.sua(maDon,tinhtrangGH);
            
            if (ret ==true )
            {
                if (tinhtrangGH == 2)
                {
                   
                    var list = don.list();
                   
                    var check=list.Where(m=>m.maDon==maDon).ToList();   
                    foreach(var item in check)
                    {
                        sp.themSPBanChay(item.maSP, item.tenSP, item.giaBan.ToString(), item.hinhAnh, item.soLuong);
                    }
                    doanhThu1.tinhDoanhThu(maDon);

                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult donDangGiao()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];

            string path = HttpContext.Request.Path;
            List<donHang> lst = new List<donHang>();
            if (_redisCacheService.CheckKey(path) == false)
            {
                lst = don.lstDangGiao();
                _redisCacheService.AddToList(path, lst, TimeSpan.FromMinutes(30));
                logging.successLogging(path);
            }
            else
            {
                lst = _redisCacheService.ReadDataFromRedis(path);
                logging.successLogging(path, "Redis");
            }

            int recordsTotals = lst.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                lst = lst.Where(m => m.tenSP.ToLower().Contains(searchValue)
                || m.tenKH.ToLower().Contains(searchValue)
                || m.tinhTrangTT.ToLower().Contains(searchValue)
                ).ToList();

            }

            var totalrowsafterfiltering = lst.Count;
            lst = lst.Skip(start).Take(length).ToList();
            return Json(new { data = lst, draw = Request["draw"], recordsTotal = recordsTotals, recordsFiltered = totalrowsafterfiltering });
        }
        public ActionResult hienThiDonDG()
        {
            if (Session["tkAD"] != null)
            {
                return View();
            }else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public ActionResult donDaGiao()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];

            string path = HttpContext.Request.Path;
            List<donHang> lst = new List<donHang>();
            if (_redisCacheService.CheckKey(path) == false)
            {
                lst = don.lstDaGiao();
                _redisCacheService.AddToList(path, lst, TimeSpan.FromMinutes(30));
                logging.successLogging(path);
            }
            else
            {
                lst = _redisCacheService.ReadDataFromRedis(path);
                logging.successLogging(path,"Redis");
            }

            int recordsTotals = lst.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                lst = lst.Where(m => m.tenSP.ToLower().Contains(searchValue)
                || m.tenKH.ToLower().Contains(searchValue)
                || m.tinhTrangTT.ToLower().Contains(searchValue)
                ).ToList();

            }

            var totalrowsafterfiltering = lst.Count;
            lst = lst.Skip(start).Take(length).ToList();
            return Json(new { data = lst, draw = Request["draw"], recordsTotal = recordsTotals, recordsFiltered = totalrowsafterfiltering });
        }
        public ActionResult hienThiDonDaGiao()
        {
            if (Session["tkAD"] != null)
            {
                return View();
            }else { return RedirectToAction("Index", "Login"); }
        }
        public ActionResult doanhThuThangNay()
        {
            if (Session["tkAD"] != null)
            {
                logging.successLogging(HttpContext.Request.Path);
                var doanhThu = doanhThu1.doanhThuTheoThang();
                return View(doanhThu);
            }
            else { return RedirectToAction("Index", "Login"); }
        }
    }
}