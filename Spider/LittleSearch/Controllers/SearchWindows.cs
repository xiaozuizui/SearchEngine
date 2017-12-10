using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web;
using Spider;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LittleSearch.Controllers
{
    public class SearchWindows : Controller
    {
        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public string Index()//主方法
        {
            WorkManage wm = new WorkManage(1, 1, "www.dlut.edu.cn");

            IndexManager indexManager = wm.manager;
            string s = "";
            indexManager.SearchIndex("大连理工",new IndexManager.Page(10,1),ref s);
            return s;
        }

    }
}
