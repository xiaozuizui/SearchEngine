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
            RequestUri = uri;
           // webRequest = new HttpWebRequest();
        }

        public async void GenerateWebRequest()
        {
            webRequest = (HttpWebRequest)WebRequest.Create(RequestUri);
            webRequest.Method = "GET";
            webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
            ContentStream = webResponse.GetResponseStream();

        }
        string RequestUri { get; set; }

        private HttpWebResponse webResponse;
        private HttpWebRequest webRequest;
        private UInt32 Index;   //编号
        private Stream ContentStream;


    }
}
