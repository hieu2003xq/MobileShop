using ado.caching;
using ado.thuVienLog;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace ado.Models
{
    [Serializable]
    public class products
    {
        SeriLogging logging= new SeriLogging();
        private readonly test11<products> _redisCacheService;
        public products()
        {
            _redisCacheService = new test11<products>();
        }
        thuVien thuvien = new thuVien();
        
        public int maSP { set; get; }
        
        public string tenSP { set; get; }
        
        public decimal giaBan { set; get; }
        
        public string hinhAnh { set; get; }
        
        public int maGiam { set; get; }
        public List<products> list()
        {
         
           string sql = @"select  maSP,tenSp,giaBan,hinhAnh,phanTram
                       from products inner join giamGia on products.maGiam=giamGia.maGiam";
            DataTable ds = thuvien.ExecuteQuery(sql);
            List<products> lst = new List<products>();
            foreach (DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int phanTram = Convert.ToInt32(item["phanTram"]);
                products a = new products()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban * 1000,
                    hinhAnh = hinhAnh,
                    maGiam = phanTram,
                };
                lst.Add(a);
            }
            return lst;
        }
        public List<products> list1()
        {
           
            string sql = @"select*  
                          from products ";
            DataTable ds = thuvien.ExecuteQuery(sql);
            List<products> lst = new List<products>();
            foreach (DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int phanTram = Convert.ToInt32(item["maGiam"]);
                products a = new products()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban * 1000,
                    hinhAnh = hinhAnh,
                    maGiam = phanTram,
                };
                lst.Add(a);
            }
            return lst;
        }
        public bool them(int maSP, string tenSP, string giaBan, string hinhAnh, int maGiam)
        {
            var a = Sanitizer.GetSafeHtmlFragment(tenSP);
            var b = Sanitizer.GetSafeHtmlFragment(giaBan);
            var c = Sanitizer.GetSafeHtmlFragment(maGiam.ToString());
            
            if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b) && !string.IsNullOrEmpty(c))
            {
                SqlParameter[] them =
                {
                    new SqlParameter("@maSP",SqlDbType.Int){Value=maSP},
                    new SqlParameter("@tenSP",SqlDbType.NVarChar){Value=tenSP},
                    new SqlParameter("@giaBan",SqlDbType.Money){Value=Convert.ToDecimal(giaBan)},
                    new SqlParameter("@hinhAnh",SqlDbType.NVarChar){Value=hinhAnh},
                    new SqlParameter("@maGiam",SqlDbType.Int){Value=maGiam},
                };
                int check = thuvien.them("products", them);
                if (check > 0)
                {
                    logging.successLogging(HttpContext.Current.Request.Path, "", "", "Thêm");
                    return true;
                }
                logging.errorLogging(HttpContext.Current.Request.Path, "", "", "Thêm");
                return false;
                
            }
            return false;
        }
        public bool xoa(int maSP)
        {
           
            SqlParameter[] xoa ={
                new SqlParameter("@maSP",SqlDbType.Int){Value=maSP},
            };
            int check = thuvien.xoa("products", xoa);
            if (check > 0)
            {
                logging.successLogging(HttpContext.Current.Request.Path,"", "", "Xóa");
                return true;
            }
            logging.errorLogging(HttpContext.Current.Request.Path, "", "", "Xóa");
            return false;
        }
        public products chiTiet(int maSP)
        { 
            string sql = @"select*from products where maSP=@maSP";
            SqlParameter[] chiTiet ={
                new SqlParameter("@maSP",SqlDbType.Int){Value=maSP},
            };
            var ds=thuvien.ExecuteQuery(sql, chiTiet);
            products a=new products();
            foreach(DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int maGiam = Convert.ToInt32(item["maGiam"]);
                a.maSP=masp;
                a.tenSP=tensp;
                a.giaBan=giaban;
                a.hinhAnh = hinhAnh;
                a.maGiam=maGiam;
            }
            logging.successLogging(HttpContext.Current.Request.Path, "", "", "Xem chi tiết");
            return a;
            
        }
        public bool sua(int maSP, string tenSP, string giaBan, string hinhAnh, int maGiam)
        {
            var a = Sanitizer.GetSafeHtmlFragment(tenSP);
            var b = Sanitizer.GetSafeHtmlFragment(giaBan);
            var c = Sanitizer.GetSafeHtmlFragment(maGiam.ToString());

            if (!string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b) && !string.IsNullOrEmpty(c))
            {
                
                SqlParameter[] sua =
                {
                    new SqlParameter("@maSP",SqlDbType.Int){Value=maSP},
                    new SqlParameter("@tenSP",SqlDbType.NVarChar){Value=tenSP},
                    new SqlParameter("@giaBan",SqlDbType.Money){Value=Convert.ToDecimal(giaBan)/1000},
                    new SqlParameter("@hinhAnh",SqlDbType.NVarChar){Value=hinhAnh},
                    new SqlParameter("@maGiam",SqlDbType.Int){Value=maGiam},
                };
                int check = thuvien.sua("products", sua);
                if (check > 0)
                {
                    logging.warningLogging(HttpContext.Current.Request.Path, "", "", "Sửa Thành Công");
                    return true;
                }
                logging.warningLogging(HttpContext.Current.Request.Path, "", "", "Sửa Thất Bại");
                return false;
            }return false;
        }
        public List<products> docData()
        {
            
            string sql = @" select maSP,tenSP,giaBan,hinhAnh,phanTram
  from products inner join giamGia on products.maGiam=giamGia.maGiam";
            DataTable ds = thuvien.ExecuteQuery(sql);
            List<products> lst = new List<products>();
           foreach(DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int phanTram = Convert.ToInt32(item["phanTram"]);
                products a = new products()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban*1000,
                    hinhAnh = hinhAnh,
                    maGiam= phanTram,
                };
                lst.Add(a);
            }
            logging.successLogging(HttpContext.Current.Request.Path, "", "", "Xem");
            return lst;
        }
        public List<products> sPNew()
        {
            string sql = @"select top(4) maSP,tenSp,giaBan,hinhAnh,phanTram
                               from products inner join giamGia on products.maGiam=giamGia.maGiam
                              order by maSP desc  ";
            DataTable ds= thuvien.ExecuteQuery(sql);
            List<products> lst = new List<products>();
           foreach(DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int maGiam = Convert.ToInt32(item["phanTram"]);
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
        public List<products> docData1()
        {

            string sql = @" select maSP,tenSP,giaBan,hinhAnh,phanTram
  from products inner join giamGia on products.maGiam=giamGia.maGiam";
            DataTable ds = thuvien.ExecuteQuery(sql);
            List<products> lst = new List<products>();
            foreach (DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int phanTram = Convert.ToInt32(item["phanTram"]);
                products a = new products()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban * 1000,
                    hinhAnh = hinhAnh,
                    maGiam = phanTram,
                };
                lst.Add(a);
            }
            return lst;

        }


    }
}