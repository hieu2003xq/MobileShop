using ado.Areas.Admin.Data;
using ado.caching;
using ado.Models;
using ado.thuVienLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ado.Areas.Admin.Controllers
{
    public class QLsanPhamController : Controller
    {     // GET: Admin/QLsanPham
        products sP=new products();
        private readonly test11<products> _redisCacheService;
        private readonly test11<sPbanChay> _redis;
        SeriLogging logging=new SeriLogging();
        public QLsanPhamController()
        {
            _redisCacheService = new test11<products>();
            _redis=new test11<sPbanChay>();
        }
        public ActionResult Index()
        {
            if (Session["tkAD"] != null)
            {
                string path = HttpContext.Request.Path;
                string newPath = path.Replace("/Admin", "");
                List<products> lst=new List<products>();
                if (_redisCacheService.CheckKey(newPath) == false)
                {
                     lst = sP.list();
                    _redisCacheService.AddToList(newPath, lst,TimeSpan.FromMinutes(30));
                }
                else
                {
                    lst= _redisCacheService.ReadDataFromRedis(newPath);
                }
                return View(lst);
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ThemSP(int maSP,string tenSP,string giaBan,string hinhAnh,int maGiam) {
            
            var check = sP.them(maSP, tenSP, giaBan, hinhAnh, maGiam);
            
            if (check == true)
            {
                return Json(new { success = true });
            }return Json(new { success = false });
        }
        public ActionResult XoaSP(int maSP)
        {
            
            var ret = sP.xoa(maSP);
            
            if (ret == true)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getID(int maSP)
        {
           
           
            var lst=sP.list1();
            
            products a=lst.FirstOrDefault(m=>m.maSP==maSP);
            if (a != null)
            {
                return Json(new {success=true,data=a},JsonRequestBehavior.AllowGet);
            }return Json(new {success=false}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult suaSP(int maSP, string tenSP, string giaBan, string hinhAnh, int maGiam)
        {
            
            var ret = sP.sua(maSP, tenSP, giaBan, hinhAnh, maGiam);
            
            if (ret== true)
            {
                return Json(new { success = true, });
            }
          
                return Json(new { success = false });

        
        }
        public ActionResult luuMaSP(int maSP)
        {
            Session["detail"] = maSP;
            if (maSP >0)
            {
                return Json(new {success=true}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult chiTietSP()
        {
            if (Session["tkAD"] != null)
            {
                var maSP = Convert.ToInt32(Session["detail"]);
                
               
                var lst = sP.list1();
              
                products a = lst.FirstOrDefault(m => m.maSP == maSP);
                if (a != null)
                {
                    if (a.maGiam == 1)
                    {
                        ViewBag.tien = a.giaBan - a.giaBan * Convert.ToDecimal(0.1);
                        ViewBag.giam = "10%";
                    }
                    else if (a.maGiam == 2)
                    {
                        ViewBag.tien = a.giaBan - a.giaBan * Convert.ToDecimal(0.2);
                        ViewBag.giam = "20%";
                    }
                    else if (a.maGiam == 3)
                    {
                        ViewBag.tien = a.giaBan - a.giaBan * Convert.ToDecimal(0.3);
                        ViewBag.giam = "30%";
                    }
                    else if (a.maGiam == 4)
                    {
                        ViewBag.tien = a.giaBan - a.giaBan * Convert.ToDecimal(0.4);
                        ViewBag.giam = "30%";
                    }
                    else if (a.maGiam == 5)
                    {
                        ViewBag.tien = a.giaBan - a.giaBan * Convert.ToDecimal(0.5);
                        ViewBag.giam = "50%";
                    }
                    else if (a.maGiam == 6)
                    {
                        ViewBag.tien = a.giaBan;
                        ViewBag.giam = "0%";
                    }
                    return View(a);
                }
                else
                {
                    return RedirectToAction("Index", "QLsanPham");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
           


        }
        public ActionResult ThemCoView()
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
        
        public ActionResult layDuLieu()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            List<products> lst = new List<products>();
            string path = HttpContext.Request.Path;
            string nguonDL = "";
            if (_redisCacheService.CheckKey(path) == false)
            {
                
                lst = sP.docData();
              _redisCacheService.AddToList(path, lst,TimeSpan.FromMinutes(30));
               logging.successLogging(path);
            }
            else
            {
                nguonDL = "Redis";
                lst = _redisCacheService.ReadDataFromRedis(path);
                logging.successLogging(path, nguonDL);

            }

            int recordsTotals = lst.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                lst = lst.Where(m => m.tenSP.ToLower().Contains(searchValue)
                || Convert.ToString(m.maGiam).Contains(searchValue)
                ).ToList();

            }
           
            var totalrowsafterfiltering = lst.Count;
            lst = lst.Skip(start).Take(length).ToList();
            
            return Json(new { data = lst, draw = Request["draw"], recordsTotal = recordsTotals, recordsFiltered = totalrowsafterfiltering });
                    
        }
        
        public ActionResult QLBangTable()
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
        public ActionResult docDsBanChay()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            sPbanChay lst = new sPbanChay();
            List<sPbanChay>ds= new List<sPbanChay>();
            var path=HttpContext.Request.Path;
            if (_redis.CheckKey(path) == false)
            {
                 ds = lst.dsSPBanChay();
                _redis.AddToList(path, ds,TimeSpan.FromMinutes(30));
                logging.successLogging(path);
            }
            else
            {
                ds= _redis.ReadDataFromRedis(path);
                logging.successLogging(path,"Redis");
            }
            int recordsTotals = ds.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                ds = ds.Where(m => m.tenSP.ToLower().Contains(searchValue)
                || Convert.ToString(m.soLuong).Contains(searchValue)
                ).ToList();

            }

            var totalrowsafterfiltering = ds.Count;
            ds = ds.Skip(start).Take(length).ToList();
            return Json(new { data = ds, draw = Request["draw"], recordsTotal = recordsTotals, recordsFiltered = totalrowsafterfiltering });
        }
        public ActionResult dsSPBanChay()
        {
            if (Session["tkAD"] != null)
            {
                return View();
            }else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        
    }
}