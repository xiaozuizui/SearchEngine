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
    public class Request
    {

        public Request(string uri)
        {
            RequestUri = uri;
           // webRequest = new HttpWebRequest();
        }

        public async void GenerateWebRequest(int i,WorkManage wm)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(RequestUri);
            webRequest.Method = "GET";
            webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
            ContentStream = webResponse.GetResponseStream();

            wm.WorkBusy[i] = false;
        }

        public string GetContent()
        {

            return new StreamReader(ContentStream).ReadToEnd();
        }

        string RequestUri { get; set; }

        private HttpWebResponse webResponse;
        private HttpWebRequest webRequest;
        private UInt32 Index;   //编号
        private Stream ContentStream;


    }
}
