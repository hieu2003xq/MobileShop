using ado.Models;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ado.Areas.Admin.Data
{
    public class doanhThu
    {
        thuVien thuvien=new thuVien();
        public int Thang { set; get; }
        public string Nam { set; get; }
        public decimal DoanhThu { set; get; }
        public int soLuong { set; get; }
        public void tinhDoanhThu(int maDon)
        {
            donHang x=new donHang();
            var lst = x.docDon(maDon);
          
            for(int i=0;i<lst.Count;i++)
            {
                SqlParameter[] them =
                {
                    new SqlParameter("@Thang",SqlDbType.Int){Value=(lst[i].ngayDat).Month},
                    new SqlParameter("@Nam",SqlDbType.NVarChar){Value=(lst[i].ngayDat).Year},
                    new SqlParameter("@soLuong",SqlDbType.Int){Value=lst[i].soLuong},
                    new SqlParameter("@tongTien",SqlDbType.Money){Value=lst[i].tienTra}
                };
                 
                string[] capNhap = { "soLuong","tongTien" };
                var check1 = thuvien.themORsua("doanhThuTheoThang", them,capNhap);
            }
        }
        public List<doanhThu> doanhThuTheoThang()
        {
            
            string sql = @"select*from doanhThuTheoThang ";
            DataTable ds = thuvien.ExecuteQuery(sql);
            List<doanhThu> lst=new List<doanhThu>();

          foreach(DataRow item in  ds.Rows)
            {
                doanhThu doanhThu = new doanhThu()
                {
                    Thang = Convert.ToInt32(item["Thang"]),
                    Nam = item["Nam"].ToString(),
                    DoanhThu = Convert.ToDecimal(item["tongTien"]),
                    soLuong = Convert.ToInt32(item["soLuong"]),
                };
                 lst.Add(doanhThu);
            }
               
                
            return lst;
        }
    }
}