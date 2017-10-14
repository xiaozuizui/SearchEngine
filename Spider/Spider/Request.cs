using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

namespace Spider
{
    class Request
    {

        public Request(string uri)
        {
           // webRequest = new HttpWebRequest();
        }
        string RequestUri { get; set; }

        private HttpWebRequest webRequest;
        private UInt32 Index;   //编号
        private Stream ContentStream;


    }
}
