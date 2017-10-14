using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spider;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {

            string uri = "http://www.dlut.edu.cn/";
            Request re = new Request(uri);
            re.GenerateWebRequest();
            System.Console.ReadLine();
            string s = re.GetContent();
        }
    }
}
