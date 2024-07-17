using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ado.Models
{
    public class gioHang
    {
        thuVien thuvien=new thuVien();
        public int maSP { set; get; }
        public string tenSP { set; get; }
        public decimal giaBan { set; get; }
        public string hinhAnh { set; get; }
        public int soLuong { set; get; }
        public int giamGia { set; get; }
        public decimal thanhTien
        {
            get
            {
                return soLuong * (giaBan-((decimal)giaBan*giamGia/100));
            }
        }
        public gioHang(int id)
        {
            maSP = id;
            
            string sql = @"select maSP,tenSP,giaBan,hinhAnh,phanTram 
                         from products inner join giamGia on products.maGiam=giamGia.maGiam where @maSP=maSP";
            SqlParameter[] sp =
            {
                new SqlParameter("@maSP",System.Data.SqlDbType.Int){Value=maSP},
            };
            System.Data.DataTable ds =thuvien.ExecuteQuery(sql,sp);   
            products a = new products();
           foreach(DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int maGiam = Convert.ToInt32(item["phanTram"]);
                a = new products()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban,
                    hinhAnh = hinhAnh,
                    maGiam = maGiam,
                };
            }
            maSP=a.maSP;
            tenSP = a.tenSP;
            giaBan=a.giaBan;
            hinhAnh =a.hinhAnh;
            giamGia = a.maGiam;
            soLuong = 1;
        }
        public gioHang(int id,int soLuong1)
        {
            maSP = id;
            
            string sql = @"select maSP,tenSP,giaBan,hinhAnh,phanTram 
                         from products inner join giamGia on products.maGiam=giamGia.maGiam where @ma=maSP";
            SqlParameter[] sp =
             {
                new SqlParameter("@maSP",System.Data.SqlDbType.Int){Value=maSP},
            };
            System.Data.DataTable ds = thuvien.ExecuteQuery(sql, sp);
            products a = new products();
            foreach (DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                int maGiam = Convert.ToInt32(item["phanTram"]);
                a = new products()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban,
                    hinhAnh = hinhAnh,
                    maGiam = maGiam,
                };
            }
            maSP = a.maSP;
            tenSP = a.tenSP;
            giaBan = a.giaBan;
            hinhAnh = a.hinhAnh;
            giamGia = a.maGiam;
            soLuong = soLuong1;
        }

    }
}