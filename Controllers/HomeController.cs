using ado.caching;
using ado.Models;
using ado.thuVienLog;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Security.Application;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using System.Windows;

namespace ado.Controllers
{
    

    public class HomeController : Controller
    {
        private readonly test11<products> _redisCacheService;
        SeriLogging logging = new SeriLogging();
        public HomeController()
        {
            _redisCacheService = new test11<products>();
        }
        products sp=new products();
        [HttpGet]
        public ActionResult Index()
        {
            var path = "/Home/Index";
            List<products>lst1=new List<products>();
            List<products> lst = new List<products>();
            string nguonDL = "";
            if (_redisCacheService.CheckKey(path) == false)
            {
                lst1 = sp.list();
                lst = sp.sPNew();
                nguonDL = "SQL";
                _redisCacheService.AddToList(path, lst1, TimeSpan.FromMinutes(30));
                _redisCacheService.AddToList(path+"1", lst, TimeSpan.FromMinutes(30));
            }
            else
            {
               
                lst1 = _redisCacheService.ReadDataFromRedis(path);
                lst = _redisCacheService.ReadDataFromRedis(path+"1");
                nguonDL = "Redis";
            }
            logging.successLogging(path,nguonDL,"Thành công");
            ViewBag.topNew = lst1.OrderByDescending(m => m.maSP).Take(3);
                return View(lst);
           
        }
        
        
    }
}