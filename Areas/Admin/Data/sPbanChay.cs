using ado.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ado.Areas.Admin.Data
{
    public class sPbanChay
    {
        thuVien thuvien=new thuVien();
        public int maSP { set; get; }
        public string tenSP { set; get; }
        public decimal giaBan { set; get; }
        public string hinhAnh { set; get; }
        public int soLuong { set; get; } 
        public List<sPbanChay> dsSPBanChay()
        {
            
            string sql= @" select top(3) soLuong,maSP,tenSP,hinhAnh,giaBan
            from sPbanChay
             order by soLuong desc";
            DataTable ds = thuvien.ExecuteQuery(sql);
            List<sPbanChay> lst = new List<sPbanChay>();
            foreach(DataRow item in ds.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                int soLuong = Convert.ToInt32(item["soLuong"]);
                string tensp = item["tenSP"].ToString();
                var giaban = Convert.ToDecimal(item["giaBan"]);
                string hinhAnh = item["hinhAnh"].ToString();
                
                sPbanChay a = new sPbanChay()
                {
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaban*1000,
                    hinhAnh = hinhAnh,
                    soLuong=soLuong,
                };
                lst.Add(a);
            }
           
            return lst;
        }
        public void themSPBanChay(int maSP,string tenSP,string giaBan,string hinhAnh,int soLuong)
        {

           
            SqlParameter[] them =
               {
                    new SqlParameter("@maSP",SqlDbType.Int){Value=maSP},
                    new SqlParameter("@tenSP",SqlDbType.NVarChar){Value=tenSP},
                    new SqlParameter("@giaBan",SqlDbType.Money){Value=Convert.ToDecimal(giaBan)/1000},
                    new SqlParameter("@hinhAnh",SqlDbType.NVarChar){Value=hinhAnh},
                    new SqlParameter("@soLuong",SqlDbType.Int){Value=soLuong},
                };
           
            string[] capNhap = { "soLuong" };
            var add = thuvien.themORsua("sPbanChay",them,capNhap);
           
        }
    }
}