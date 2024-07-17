using ado.Models;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ado.Areas.Admin.Data;
using Microsoft.Security.Application;
using System.Drawing;
using System.Globalization;
using ado.caching;
using ado.thuVienLog;

namespace ado.Controllers
{
    public class sanPhamController : Controller
    {
        // GET: sanPham
        test11<products> _redis=new test11<products>();
        SeriLogging logging=new SeriLogging();
        public List<products> laySP()
        {
            string strCon = @"Data Source=DESKTOP-93SUM6B\SQLEXPRESS;Initial Catalog=qLBH;Integrated Security=True";
            var connection = new SqlConnection(strCon);
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = @"select*from products";
            var reader = cmd.ExecuteReader();
            List<products> lst = new List<products>();
            while (reader.Read())
            {
                int masp = Convert.ToInt32(reader["maSP"]);
                string tensp = reader["tenSP"].ToString();
                var giaban = Convert.ToDecimal(reader["giaBan"]);
                string hinhAnh = reader["hinhAnh"].ToString();
                int maGiam = Convert.ToInt32(reader["maGiam"]);
                products a = new products()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban,
                    hinhAnh = hinhAnh,
                    maGiam = maGiam,
                };
                lst.Add(a);
            }
            return lst;
        }
        public List<gioHang> layGioHang() {
            List<gioHang> lst = Session["gioHang"] as List<gioHang>;
            if (lst == null)
            {
                lst=new List<gioHang>();
                Session["gioHang"] = lst;
            }return lst;
        }
        public List<products> xemGanDay()
        {
            List<products> lst = Session["LSX"] as List<products>;
            if (lst == null)
            {
                lst = new List<products>();
                Session["LSX"] = lst;
            }
            return lst;
        }
        private int tongSL()
        {
            int tong = 0;
            List<gioHang> lst = Session["gioHang"] as List<gioHang>;
            if (lst != null)
            {
                tong = lst.Sum(n => n.soLuong);
            }return tong;
        }
        private double tongTien()
        {
            double tong = 0;
            List<gioHang> lst = Session["gioHang"] as List<gioHang>;
            if (lst != null)
            {
                tong = Convert.ToDouble(lst.Sum(n => n.thanhTien));
            }
            return tong;
        }
        public ActionResult Index()
        {
            var path = HttpContext.Request.Path;
            string nguonDL = "";
            products a = new products();
            List<products> lst = new List<products>();
            if (_redis.CheckKey(path) == false)
            {
                nguonDL = "SQL";
                lst = a.list();
                _redis.AddToList(path, lst, TimeSpan.FromMinutes(30));
            }
            else
            {
                nguonDL = "Redis";
                lst=_redis.ReadDataFromRedis(path);
            }
            logging.successLogging(path, nguonDL, "Thành công");
            return View(lst);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult getID(int maSP)
        {
            Session["id"] = maSP;
            products a = new products();  
            var ds = xemGanDay();
            
            var lst=a.list();
           
            var check = lst.FirstOrDefault(m => m.maSP == maSP);
            if(check != null)
            {
                var check1=ds.FirstOrDefault(m => m.maSP == maSP);
                if (check1 == null)
                {
                    ds.Add(check);
                }
                else
                {
                    ds.Remove(check1);
                    ds.Add(check);
                }
               
            }
 
            return Json(new { success = true});
        }
        
        public ActionResult detail() {
           
            products x=new products();
            int maSP = Convert.ToInt32(Session["id"]);
            if (maSP != null)
            {
                products a = x.chiTiet(maSP);

                if (a.maGiam == 1)
                {
                    ViewBag.tien = ((a.giaBan - a.giaBan * Convert.ToDecimal(0.1))*1000);
                    ViewBag.giam = "10%";
                }
                else if (a.maGiam == 2)
                {
                    ViewBag.tien = ((a.giaBan - a.giaBan * Convert.ToDecimal(0.2))*1000);
                    ViewBag.giam = "20%";
                }
                else if (a.maGiam == 3)
                {
                    ViewBag.tien = ((a.giaBan - a.giaBan * Convert.ToDecimal(0.3))*1000);
                    ViewBag.giam = "30%";
                }
                else if (a.maGiam == 4)
                {
                    ViewBag.tien = ((a.giaBan - a.giaBan * Convert.ToDecimal(0.4))*1000);
                    ViewBag.giam = "30%";
                }
                else if (a.maGiam == 5)
                {
                    ViewBag.tien = ((a.giaBan - a.giaBan * Convert.ToDecimal(0.5))*1000);
                    ViewBag.giam = "50%";
                }
                else if (a.maGiam == 6)
                {
                    ViewBag.tien = (a.giaBan*1000);
                    ViewBag.giam = "";
                }         
                return View(a);
            }
            else { return View("Index"); };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult themGioHang1(int maSP,int soLuong)
        {
            int a = Convert.ToInt32(maSP);
            if (a != null)
            {
                List<gioHang> lst = layGioHang();
                gioHang sP = lst.Find(x => x.maSP == a);
                if (sP != null)
                {
                    sP.soLuong += soLuong;
                    return Json(new { success = true });
                }
                else
                {
                    sP = new gioHang(a,soLuong);
                    lst.Add(sP);
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult themGioHang(int maSP) {
            int a = Convert.ToInt32(maSP);
            if (a != null) {
                List<gioHang> lst = layGioHang();
                gioHang sP = lst.Find(x => x.maSP == a);
                if (sP != null)
                {
                    sP.soLuong++;
                    return Json(new { success = true });
                }
                else
                {
                    sP = new gioHang(a);
                    lst.Add(sP);
                    return Json(new { success = true });
                }
               }
            return Json(new { success = false });

        }
        public ActionResult hienThiCart()
        {
          return View();
        }
        [HttpPost]
        public ActionResult xoaSP(int maSP)
        {
            List<gioHang>lst=layGioHang();
            gioHang a=lst.FirstOrDefault(m=>m.maSP == maSP);
            if (a!=null) {
                    lst.Remove(a);
                    return Json(new { success = true,tongtien=tongTien()});
                
            }
            return Json(new { success = false });

        }
        [HttpPost]
        public ActionResult load()
        {
            List<gioHang> lst = layGioHang();
            if (lst.Count == 0)
            {
                return Json(new {count=0});
            }
           
           
            return Json(new { data=lst,count=lst.Count,tongtien=tongTien(), success = true });
        }
        [HttpPost]
        public ActionResult update(int maSP,int soLuong)
        {
            List<gioHang> lst = layGioHang();
            gioHang a=lst.FirstOrDefault(m=>m.maSP==maSP);
            if (a != null)
            {
                a.soLuong = soLuong;
                if (soLuong == 0)
                {
                    lst.Remove(a);
                    return Json(new { success = true, tongtien = tongTien() });
                }
                
                return Json(new { success = true, tongtien = a.thanhTien });
            }
            else { return Json(new { success = false }); }
        }
       
        [HttpPost]
        public ActionResult getData()
        {
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            donHang don = new donHang();
           
            var lst = don.list();
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
            return Json(new { data = lst, draw = Request["draw"], recordsTotal = recordsTotals, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult datHang(List<Class1> lst,string tenKH,string diaChi,string tienTra,string SDT)
        {
            var e = Sanitizer.GetSafeHtmlFragment(tenKH);
            var c = Sanitizer.GetSafeHtmlFragment(diaChi);
            var d = Sanitizer.GetSafeHtmlFragment(SDT);
            if (!string.IsNullOrEmpty(d) && !string.IsNullOrEmpty(e) && !string.IsNullOrEmpty(c))
            {
                List<gioHang> list = layGioHang();
                List<products> dsSP = laySP();
                donHang x=new donHang();
                var tien = tongTien()*1000;
                var check = x.themDon(lst, tenKH, diaChi, tien.ToString(), SDT, list, dsSP);
                if (check == true)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            else
            {
                return Json(new {success=false});
            }


        }
        [HttpPost]
        public ActionResult BuyNow(int maSP,int soLuong)
        {
            Class1 lst=new Class1();
            lst.maSP = maSP;
            lst.soLuong = soLuong;
            products sP=new products();
            var a =sP.list();
            var check = a.FirstOrDefault(m => m.maSP == maSP);
            if (check != null)
            {
                Session["thongTinbuyNow"]= lst;
                return Json(new { success = true});
            }
            else
            {
                return Json(new {success=false});
            }
        }
        public ActionResult checkOutBuyNow()
        {
            Class1 lst=new Class1();
            lst = Session["thongTinbuyNow"] as Class1;
            if (lst != null)
            {
                products sP = new products();
              
                var a = sP.list();
               
                var check = a.FirstOrDefault(m => m.maSP == lst.maSP);
                ViewBag.soLuong = lst.soLuong;
                ViewBag.tongtien = (lst.soLuong * check.giaBan);
                ViewBag.giamgia = check.maGiam+"%";
                ViewBag.tienTra = ((lst.soLuong * (check.giaBan - ((decimal)check.giaBan * check.maGiam / 100))));
                return View(check);
            }
            else
            {
                return RedirectToAction("Index", "sanPham");
            }
           
        }
        public ActionResult checkOut()
        {
            return View();
        }
        [HttpPost]
        
        public ActionResult checkOUt1() {
            List<gioHang> lst = layGioHang();
            if (lst.Count == 0)
            {
                return Json(new { count = 0 });
            }
            else
            {
                var tong = Convert.ToDouble(lst.Sum(m => m.giaBan));
                return Json(new { data = lst, count = lst.Count, tongtien = tong*1000, thanhTien=tongTien()*1000,success = true });
            }
        }
        public ActionResult VnpayReturn()
        {

            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        List<gioHang> lst = layGioHang();
                       List<products>dssp=laySP();
                        List<Class1> x = Session["dsDon"] as List<Class1>;
                        donHang y = Session["don1"] as donHang;
                        donHang new1 = new donHang();
                        new1.themDonCK(x, y.tenKH, y.diaChi,(tongTien()*1000).ToString(), y.SDT, lst, dssp);
                       lst.Clear();
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                        ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult thanhToanDon(List<Class1>ds,string tenKH, string diaChi, string tienTra, string SDT,int Type)
        {
            var b = Sanitizer.GetSafeHtmlFragment(tenKH);
            var c = Sanitizer.GetSafeHtmlFragment(diaChi);
            var d = Sanitizer.GetSafeHtmlFragment(SDT);
            if (!string.IsNullOrEmpty(d) && !string.IsNullOrEmpty(b) && !string.IsNullOrEmpty(c))
            {
                
                Session["dsDon"] = ds;
                donHang x=new donHang();
                x.tenKH=tenKH;
                x.diaChi=diaChi;
                x.SDT=SDT;
                Session["don1"] = x;
                var laymaDOn = x.layMaDon();
                var url = UrlPayment(Type, ds,DateTime.Now,tenKH,laymaDOn);
                var code = new { success = true, url = url };
                return Json(code);
            }
            else { return Json(new { success = false }); }
           
        }
         
        public ActionResult thongTinDon(List<Class1> lst, string giamGia)
        {
            if (lst != null)
            {
                if (lst.Count > 0)
                {
                    Session["giamGia"] = giamGia;
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public string UrlPayment(int TypePaymentVN, List<Class1> a,DateTime ngayDat,string tenKH,string maDon)
        {
            var urlPayment = "";
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key
            var lst = layGioHang();
            var ds = new List<donHang>();
            var donHang=new donHang();
            for(int i=0;i<a.Count;i++)
            {
                var check = lst.FirstOrDefault(m => m.maSP == a[i].maSP);
                if (check != null)
                {
                    donHang.giaBan = check.thanhTien;
                    ds.Add(donHang);
                }
            } 
            VnPayLibrary vnpay = new VnPayLibrary();
            var tongTien1 = ((long)tongTien() * 100000).ToString();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", tongTien1); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", ngayDat.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + tenKH);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", maDon.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return urlPayment;
        }
        public donHang laydon()
        {
          donHang a=new donHang();
            a = Session["Hang"] as donHang;
            return a;
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult docDon(int maDon)
        {
            donHang a = new donHang();
            
            var b=a.docDon(maDon);
            
               return Json(new { data = b ,success=true});
           
        }
        [HttpPost]
        public ActionResult banChay()
        {

            sPbanChay sPbanChay = new sPbanChay();
            
            var ds = sPbanChay.dsSPBanChay();
            
            if (ds != null)
            {
                return Json(new { data = ds, success = true });
            }
            return Json(new { success = false });

        }
        public ActionResult XemGanDay1()
        {
            List<products> a = xemGanDay();
            if(a != null)
            {
                var list = a;
                return Json(new {success=true,data=list });

            }return Json(new {success = false});

        }
        [HttpPost]
        public ActionResult loadSP()
        {
            products a = new products();
           
            var lst = a.list();
           
            if (lst != null)
            {
                var data = lst.OrderBy(m => m.maSP).Take(4);
                return Json(new { data = data, success = true });
            }
            return Json(new { success = false });
        }
        public ActionResult timKiemSP(string timKiem)
        {
            products a=new products();
            List<products>lstTK=new List<products>();
             
            var lst = a.list();
            
            if (!string.IsNullOrEmpty(timKiem))
            {
                 lstTK = lst.Where(m => m.tenSP.ToLower().Contains(timKiem)).ToList();
                if (lstTK.Count>0)
                {
                    return Json(new { success = true, data = lstTK });
                }
                else if (lstTK.Count == 0)
                {
                    return Json(new { success = false });
                }
            }
           
            return Json(new { success = false });
        }

    }
        
    }
