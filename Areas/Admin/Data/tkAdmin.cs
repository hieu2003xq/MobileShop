using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ado.Models;
using System.Data;

namespace ado.Areas.Admin.Data
{
    public class tkAdmin
    {
        thuVien thuvien=new thuVien();
        public string Gmail { set; get; }
        public string tenDN { set; get; }
        public string matKhau { set; get; }
        public bool ktra(string username, string password)
        {

            string sql = @"select*from tkAdmin where tenDN=@tenDN and matKhau=@matKhau";
            SqlParameter[] check =
            {
                new SqlParameter("@tenDN",System.Data.SqlDbType.NVarChar){Value=username},
               new SqlParameter("@matKhau",System.Data.SqlDbType.NVarChar){Value=password},
            };
           var doc=thuvien.ExecuteQuery(sql, check);
            if (doc!=null)
            {
                return true;
            }
            return false;
        }
        public List<tkAdmin> listCheck(string ten) {
            string sql = @"select*
                       from tkAdmin where tenDN!=@tenDN";
            SqlParameter[] check =
           {
                new SqlParameter("@tenDN",System.Data.SqlDbType.NVarChar){Value=ten},
            };
            DataTable ds = thuvien.ExecuteQuery(sql,check);
            List<tkAdmin> lst = new List<tkAdmin>();
            foreach (DataRow item in ds.Rows)
            {
                
                string tenDN = item["tenDN"].ToString();
                
                tkAdmin a = new tkAdmin()
                {
                    tenDN = tenDN,
                };
                lst.Add(a);
            }
            return lst;
        }
    }
}