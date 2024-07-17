using Serilog.Context;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ado.thuVienLog
{
    public class SeriLogging
    {
        public void successLogging(string path, string nguonLayDL="SQL",string thongTinLog="Thành công",string chiTiet="Xem", string tinhTrang = "Truy cập đến")
        {
            using (LogContext.PushProperty("Dữ liệu được lấy từ:", nguonLayDL))
            using (LogContext.PushProperty(tinhTrang, path))
            {
                managerLogging.Logger.Information(thongTinLog);
                
            }
            managerLogging.Logger1.Information("Thành công truy cập: {RequestMethod} - {RequestPath} - {StatusCode} {Exception} - {SourceContext} - {User} - {chiTiet}-{nguonDL}", HttpContext.Current.Request.HttpMethod,path, 200, "Elmah.Io.TestException", "System.Private.CoreLib", "TTUser",chiTiet,nguonLayDL);
        }
        public void errorLogging(string path, string nguonLayDL="SQL", string thongTinLog="Thất bại",string chiTiet="Xem", string tinhTrang = "Truy cập đến")
        {
           
            using (LogContext.PushProperty("Dữ liệu được lấy từ:", nguonLayDL))
            using (LogContext.PushProperty(tinhTrang, path))
            {
                managerLogging.Logger.Error(thongTinLog);
            }
            managerLogging.Logger1.Error("Truy Cập thất bại: {RequestMethod} - {RequestPath} - {StatusCode} {Exception} - {SourceContext} - {User} - {chiTiet} - {nguonDL}", HttpContext.Current.Request.HttpMethod, HttpContext.Current.Request.Path, 500, "Elmah.Io.TestException", "System.Private.CoreLib", "TTUser",chiTiet,nguonLayDL);
        }
        public void warningLogging(string path, string nguonLayDL = "SQL", string thongTinLog = "Cảnh báo", string chiTiet="Xem", string tinhTrang = "Truy cập đến")
        {
            
            using (LogContext.PushProperty("Dữ liệu được lấy từ:", nguonLayDL))
            using (LogContext.PushProperty(tinhTrang, path))
            {
                managerLogging.Logger.Warning(thongTinLog);
            }
            managerLogging.Logger1.Warning("Thành công truy cập: {RequestMethod} - {RequestPath} - {StatusCode} {Exception} - {SourceContext} - {User} - {chiTiet} - {nguonDL}", HttpContext.Current.Request.HttpMethod, HttpContext.Current.Request.Path,404, "Elmah.Io.TestException", "System.Private.CoreLib", "TTUser",chiTiet,nguonLayDL);
        }
    }
}