using ado.Areas.Admin.Data;
using ado.thuVienLog;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ado.Models
{
    public class donHang
    {
        ado.Models.Connection _connection=new ado.Models.Connection();
        thuVien thuvien= new thuVien();
        SeriLogging logging=new SeriLogging();
        public int STT { set; get; }
        public int maDon { get; set; }
        public string tenKH { set; get; }
        public int maSP { get; set; }
        public string tenSP { set; get; }
        public decimal giaBan { set; get; }
        public string hinhAnh { set; get; }
        public int soLuong { get; set; }
        public DateTime ngayDat { get; set; }
        public string tinhTrangTT { set; get; }
        public string tinhTrangGH { set; get; }
        public string diaChi { set;get; }
        public decimal tienTra { set;get; }
        public string giamGia { set; get; }
        public string SDT { set; get; }
        public int Type { set; get; }
      public bool xoa(int maDon)
        {
            
            SqlParameter[] xoa =
            {
                new SqlParameter("@maDon",System.Data.SqlDbType.Int){Value = maDon},
            };
            int check = thuvien.xoa("chiTietDon", xoa);

            if (check > 0)
            {
                logging.successLogging(HttpContext.Current.Request.Path, "", "", "Xóa đon hàng");
                return true;
            }
            logging.errorLogging(HttpContext.Current.Request.Path, "", "", "Xóa đon hàng");
            return false;
        }
        public bool sua(int maDon,int tinhTrangGH)
        {
            string ttgh = "";
            if (tinhTrangGH == 1)
            {
                ttgh = "Đang giao";
            }
            if (tinhTrangGH == 2)
            {
                ttgh = "Đã giao";
            }

            SqlParameter[] sua =
            {
                new SqlParameter("@maDon",System.Data.SqlDbType.Int){Value=maDon},
                new SqlParameter("@tinhtrangGH",System.Data.SqlDbType.NVarChar){Value=ttgh},
            };
            var check = thuvien.sua("chiTietDon", sua);
            if (check > 0)
            {
                logging.warningLogging(HttpContext.Current.Request.Path, "", "", "sửa đon hàng");
                return true;
            }
            logging.errorLogging(HttpContext.Current.Request.Path, "", "", "sửa đon hàng");
            return false;
        }
        public List<donHang> docDon(int maDon)
        {
           List<donHang> lst=new List<donHang>();
           string sql = @"select STT,chiTietDon.maDon as 'maDon',products.maSP as 'maSP',products.tenSP as 'tenSP',soLuong,products.giaBan as 'giaBan',tinhTrangGH,tinhTrangTT,thanhTien,products.hinhAnh,ngayDat,phanTram
  from chiTietDon inner join donDat1 on chiTietDon.maDon=donDat1.maDon inner join products on chiTietDon.maSP=products.maSP inner join giamGia on products.maGiam=giamGia.maGiam
  where chiTietDon.maDon=@ma";
            SqlParameter[] ds ={
            new SqlParameter("@ma", System.Data.SqlDbType.Int) { Value=maDon},
            };
            var ds1 = thuvien.ExecuteQuery(sql,ds);
           foreach(DataRow item in ds1.Rows)
            {
                int masp = Convert.ToInt32(item["maSP"]);
                int STT = Convert.ToInt32(item["STT"]);
                string tensp = item["tenSP"].ToString();
                string hinhAnh = item["hinhAnh"].ToString();
                int soLuong = Convert.ToInt32(item["soLuong"]);
                int giamGia = Convert.ToInt32(item["phanTram"]);
                DateTime ngayDat = Convert.ToDateTime(item["ngayDat"]);
                decimal giaBan = Convert.ToDecimal(item["giaBan"]);
                decimal tienTra = Convert.ToDecimal(item["thanhTien"]);
                string tinhTrangTT = item["tinhTrangTT"].ToString();
                string tinhTrangGH = item["tinhTrangGH"].ToString();
                donHang a = new donHang()
                {
                    STT=STT,
                    maDon = maDon,
                    maSP = masp,
                    tenSP = tensp,
                    giamGia=giamGia.ToString()+"%",
                    hinhAnh = hinhAnh,
                    soLuong = soLuong,
                    ngayDat = ngayDat,
                    giaBan = giaBan,
                    tienTra = tienTra,
                    tinhTrangTT = tinhTrangTT,
                    tinhTrangGH = tinhTrangGH,

                };
                lst.Add(a);
                a = new donHang();
            }
            if (lst != null)
            {
                return lst;
            }return null;
            }
        public List<donHang> list()
        {

           
           string sql = @"select STT,chiTietDon.maDon,chiTietDon.maSP,products.tenSP,soLuong,chiTietDon.giaBan,tinhTrangGH,tinhTrangTT,thanhTien,chiTietDon.hinhAnh,ngayDat,phanTram
  from chiTietDon inner join donDat1 on chiTietDon.maDon=donDat1.maDon inner join products on chiTietDon.maSP=products.maSP inner join giamGia on products.maGiam=giamGia.maGiam";
            System.Data.DataTable ds = thuvien.ExecuteQuery(sql);
            List<donHang> lst = new List<donHang>();
           foreach(DataRow item in ds.Rows)
            {
                int STT = Convert.ToInt32(item["STT"]);
                int maDon = Convert.ToInt32(item["maDon"]);
                int masp = Convert.ToInt32(item["maSP"]);
                int giamGia = Convert.ToInt32(item["phanTram"]);
                string tensp = item["tenSP"].ToString();
                string hinhAnh = item["hinhAnh"].ToString();
                int soLuong = Convert.ToInt32(item["soLuong"]);
                DateTime ngayDat = Convert.ToDateTime(item["ngayDat"]);
                decimal tienTra = Convert.ToDecimal(item["thanhTien"]);
                decimal giaBan = Convert.ToDecimal(item["giaBan"]);
                string tinhTrangTT = item["tinhTrangTT"].ToString();
                string tinhTrangGH = item["tinhTrangGH"].ToString();
                donHang a = new donHang()
                {
                   STT=STT,
                    maDon = maDon,
                    maSP = masp,
                    tenSP = tensp,
                    giaBan = giaBan*1000,
                    hinhAnh = hinhAnh,
                    soLuong = soLuong,
                    ngayDat = ngayDat,   
                    tienTra = tienTra*1000,
                    giamGia=giamGia.ToString()+"%",
                    tinhTrangTT = tinhTrangTT,
                    tinhTrangGH = tinhTrangGH,


                };
                lst.Add(a);
            }
            return lst;
        }
        public List<donHang> lstDangGiao()
        {
            
            string sql = @"select STT,chiTietDon.maDon,products.maSP,products.tenSP,soLuong,products.giaBan,tinhTrangGH,tinhTrangTT,thanhTien,products.hinhAnh,ngayDat,phanTram
      from chiTietDon inner join donDat1 on chiTietDon.maDon=donDat1.maDon inner join products on chiTietDon.maSP=products.maSP inner join giamGia on products.maGiam=giamGia.maGiam
     where tinhTrangGH='Đang giao'";
            List<donHang> lst = new List<donHang>();
            System.Data.DataTable ds = thuvien.ExecuteQuery(sql);
          foreach(DataRow item in ds.Rows)
            {
                int STT = Convert.ToInt32(item["STT"]);
                int maDon = Convert.ToInt32(item["maDon"]);
                int masp = Convert.ToInt32(item["maSP"]);
                int giamGia = Convert.ToInt32(item["phanTram"]);
                string tensp = item["tenSP"].ToString();
                string hinhAnh = item["hinhAnh"].ToString();
                int soLuong = Convert.ToInt32(item["soLuong"]);
                DateTime ngayDat = Convert.ToDateTime(item["ngayDat"]);
                decimal tienTra = Convert.ToDecimal(item["thanhTien"]);
                decimal giaBan = Convert.ToDecimal(item["giaBan"]);
                string tinhTrangTT = item["tinhTrangTT"].ToString();
                string tinhTrangGH = item["tinhTrangGH"].ToString();
                donHang a = new donHang()
                {
                    STT = STT,
                    maDon = maDon,
                    maSP = masp,
                    tenSP = tensp,
                    hinhAnh = hinhAnh,
                    soLuong = soLuong,
                    ngayDat = ngayDat,
                    giaBan=giaBan,
                    giamGia = giamGia.ToString()+"%",
                    tienTra = tienTra,
                    tinhTrangTT = tinhTrangTT,
                    tinhTrangGH = tinhTrangGH,

                };
                lst.Add(a);
                
            }
            
            return lst;
        }
        public List<donHang> lstDaGiao()
        {

            
            string sql = @"select STT,chiTietDon.maDon,products.maSP,products.tenSP,soLuong,products.giaBan,tinhTrangGH,tinhTrangTT,thanhTien,products.hinhAnh,ngayDat,phanTram
      from chiTietDon inner join donDat1 on chiTietDon.maDon=donDat1.maDon inner join products on chiTietDon.maSP=products.maSP inner join giamGia on products.maGiam=giamGia.maGiam
where tinhTrangGH=N'Đã giao'";
            List<donHang> lst = new List<donHang>();
            System.Data.DataTable ds = thuvien.ExecuteQuery(sql);
            foreach(DataRow item in ds.Rows)
            {
                int STT = Convert.ToInt32(item["STT"]);
                int maDon = Convert.ToInt32(item["maDon"]);
                int masp = Convert.ToInt32(item["maSP"]);
                string tensp = item["tenSP"].ToString();
                string hinhAnh = item["hinhAnh"].ToString();
                int soLuong = Convert.ToInt32(item["soLuong"]);
                DateTime ngayDat = Convert.ToDateTime(item["ngayDat"]);
                decimal tienTra = Convert.ToDecimal(item["thanhTien"]);
                decimal giaBan = Convert.ToDecimal(item["giaBan"]);
                string giamGia = item["phanTram"].ToString();
                string tinhTrangTT = item["tinhTrangTT"].ToString();
                string tinhTrangGH = item["tinhTrangGH"].ToString();
                donHang a = new donHang()
                {
                    STT=STT,
                    maDon = maDon,
                    maSP = masp,
                    tenSP = tensp,
                    hinhAnh = hinhAnh,
                    soLuong = soLuong,
                    ngayDat = ngayDat,
                    giamGia = giamGia+"%",
                    tienTra = tienTra*1000,
                    giaBan=giaBan*1000,
                    tinhTrangTT = tinhTrangTT,
                    tinhTrangGH = tinhTrangGH,

                };
                lst.Add(a);

            }
            return lst;
        }
       
        public bool themDon(List<Class1> lst, string tenKH, string diaChi, string tienTra, string SDT, List<gioHang> list, List<products> dsSP)
        {
            List<donHang> lstDon = new List<donHang>();
            donHang don = new donHang();
            List<products> b = new List<products>();
            decimal tien = decimal.Parse(tienTra);
            products a = new products();
            for (int i = 0; i < lst.Count; i++)
            {
                a = dsSP.Find(m => m.maSP == lst[i].maSP);
                b.Add(a);
            }
            for (int i = 0; i < b.Count; i++)
            {
                don.SDT = SDT;
                don.tienTra = tien;
                don.giamGia = b[i].maGiam.ToString();
                don.diaChi = diaChi;
                don.tenKH = tenKH;
                don.maSP = b[i].maSP;
                don.tenSP = b[i].tenSP;
                don.giaBan = b[i].giaBan;
                don.hinhAnh = b[i].hinhAnh;
                don.soLuong = lst[i].soLuong;
                don.ngayDat = DateTime.Now;
                don.tinhTrangTT = "Trả Sau";
                don.tinhTrangGH = "Đang giao";
                lstDon.Add(don);
                don = new donHang();
            }
            
            
           
            SqlParameter[] ds =
            {
                new SqlParameter("@tenKH",SqlDbType.NVarChar){Value=tenKH},
                new SqlParameter("@SDT",SqlDbType.NVarChar){Value=SDT},
                new SqlParameter("@diaChi",SqlDbType.NVarChar){Value=diaChi},
            };
           var add = thuvien.them("khachHang", ds);
            string sql1= @"  select top(1) maKH
                                       from khachHang
                              order by maKH desc";
            var check = thuvien.ExecuteQuery(sql1);
            int maKH=0;
            foreach(DataRow item in check.Rows)
            {
                 maKH= Convert.ToInt32(item["maKH"]);
            }
           
           
            SqlParameter[] ds1 =
            {
                new SqlParameter("@maKH",SqlDbType.Int){Value=maKH},
                new SqlParameter("@ngayDat",SqlDbType.DateTime){Value=DateTime.Now},
                new SqlParameter("@tongTien",SqlDbType.Money){Value=tien},
            };
            var add1 = thuvien.them("donDat1", ds1);
            
            string sql3= @"  select top(1) maDon
                                       from donDat1
                              order by maKH desc";
            System.Data.DataTable doc = thuvien.ExecuteQuery(sql3);
            int maDon =0;
           foreach(DataRow item in doc.Rows)
            {
                maDon = Convert.ToInt32(item["maDon"]);
            }
            
            for (int i = 0; i < lstDon.Count; i++)
            {
                gioHang gioHang = list.Find(m => m.maSP == lstDon[i].maSP);
               
               
                SqlParameter[] addlist =
                {
                    new SqlParameter("@maDon",SqlDbType.Int){Value=maDon},
                    new SqlParameter("@maSP",SqlDbType.Int){Value=lstDon[i].maSP},
                    new SqlParameter("@tenSP",SqlDbType.NVarChar){Value=lstDon[i].tenSP},
                    new SqlParameter("@soLuong",SqlDbType.Int){Value=lstDon[i].soLuong},
                    new SqlParameter("@giaBan",SqlDbType.Money){Value=b[i].giaBan},
                    new SqlParameter("@tinhTrangGH",SqlDbType.NVarChar){Value=lstDon[i].tinhTrangGH},
                    new SqlParameter("@tinhTrangTT",SqlDbType.NVarChar){Value=lstDon[i].tinhTrangTT},
                    new SqlParameter("@thanhTien",SqlDbType.Money){Value=gioHang.thanhTien},
                     new SqlParameter("@hinhAnh",SqlDbType.NVarChar){Value=lstDon[i].hinhAnh},

                };
               
                list.Remove(gioHang);
                var addDon = thuvien.them("chiTietDon", addlist);
                if(addDon < 0)
                {
                    break;
                }
                if (i + 1 == lstDon.Count)
                {
                    logging.successLogging(HttpContext.Current.Request.Path, "", "", "Thêm đon hàng");
                    return true;
                }
                
            }
            logging.errorLogging(HttpContext.Current.Request.Path, "", "", "Thêm đon hàng");
            return false;



        }
        public bool themDonCK(List<Class1> lst, string tenKH, string diaChi, string tienTra, string SDT, List<gioHang> list, List<products> dsSP)
        {

            List<donHang> lstDon = new List<donHang>();
            donHang don = new donHang();
            List<products> b = new List<products>();
            decimal tien = decimal.Parse(tienTra);
            products a = new products();
            for (int i = 0; i < lst.Count; i++)
            {
                a = dsSP.Find(m => m.maSP == lst[i].maSP);
                b.Add(a);
            }
            for (int i = 0; i < b.Count; i++)
            {
                don.SDT = SDT;
                don.tienTra = tien;
                don.giamGia = b[i].maGiam.ToString();
                don.diaChi = diaChi;
                don.tenKH = tenKH;
                don.maSP = b[i].maSP;
                don.tenSP = b[i].tenSP;
                don.giaBan = b[i].giaBan;
                don.hinhAnh = b[i].hinhAnh;
                don.soLuong = lst[i].soLuong;
                don.ngayDat = DateTime.Now;
                don.tinhTrangTT = "Đã Thanh Toán";
                don.tinhTrangGH = "Đang giao";
                lstDon.Add(don);
                don = new donHang();
            }


            
            SqlParameter[] ds =
            {
                new SqlParameter("@tenKH",SqlDbType.NVarChar){Value=tenKH},
                new SqlParameter("@SDT",SqlDbType.NVarChar){Value=SDT},
                new SqlParameter("@diaChi",SqlDbType.NVarChar){Value=diaChi},
            };
            var add = thuvien.them("khachHang", ds);
            string sql1 = @"  select top(1) maKH
                                       from khachHang
                              order by maKH desc";
            var check = thuvien.ExecuteQuery(sql1);
            int maKH = 0;
            foreach (DataRow item in check.Rows)
            {
                maKH = Convert.ToInt32(item["maKH"]);
            }

            
            SqlParameter[] ds1 =
            {
                new SqlParameter("@maKH",SqlDbType.Int){Value=maKH},
                new SqlParameter("@ngayDat",SqlDbType.DateTime){Value=DateTime.Now},
                new SqlParameter("@tongTien",SqlDbType.Money){Value=tien},
            };
            var add1 = thuvien.them("donDat1", ds1);

            string sql3 = @"  select top(1) maDon
                                       from donDat1
                              order by maKH desc";
            System.Data.DataTable doc = thuvien.ExecuteQuery(sql3);
            int maDon = 0;
            foreach (DataRow item in doc.Rows)
            {
                maDon = Convert.ToInt32(item["maDon"]);
            }

            for (int i = 0; i < lstDon.Count; i++)
            {
                gioHang gioHang = list.Find(m => m.maSP == lstDon[i].maSP);

               
                SqlParameter[] addlist =
                {
                    new SqlParameter("@maDon",SqlDbType.Int){Value=maDon},
                    new SqlParameter("@maSP",SqlDbType.Int){Value=lstDon[i].maSP},
                    new SqlParameter("@tenSP",SqlDbType.NVarChar){Value=lstDon[i].tenSP},
                    new SqlParameter("@soLuong",SqlDbType.Int){Value=lstDon[i].soLuong},
                    new SqlParameter("@giaBan",SqlDbType.Money){Value=b[i].giaBan},
                    new SqlParameter("@tinhTrangGH",SqlDbType.NVarChar){Value=lstDon[i].tinhTrangGH},
                    new SqlParameter("@tinhTrangTT",SqlDbType.NVarChar){Value=lstDon[i].tinhTrangTT},
                    new SqlParameter("@thanhTien",SqlDbType.Money){Value=gioHang.thanhTien},
                     new SqlParameter("@hinhAnh",SqlDbType.NVarChar){Value=lstDon[i].hinhAnh},

                };

                list.Remove(gioHang);
                var addDon = thuvien.them("chiTietDon", addlist);
                if (addDon < 0)
                {
                    break;
                }
                if (i + 1 == lstDon.Count)
                {
                    return true;
                }

            }
            return false;



        }
        public string layMaDon()
        {
            
           string sql = @"  select top(1) maDon
                                       from donDat1
                              order by maKH desc";
            System.Data.DataTable check = thuvien.ExecuteQuery(sql);
            int maDon = 0;
          foreach(DataRow item in  check.Rows)
            {
                maDon = Convert.ToInt32(item["maDon"]);
            }
            return (maDon+1).ToString()+"XX";
        }

    }
}