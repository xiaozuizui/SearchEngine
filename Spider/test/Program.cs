using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spider;
using System.Threading;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {


            string uri = "http://www.dlut.edu.cn/";
            WorkManage wm = new WorkManage(4, 4, uri);


           //Thread.Sleep(5000);
        

            while (true)
            {
                wm.RunTask();
                 //if (wm.isFinished())
                    

            }


            //
        }
            

            
               
           
        
    }
}
