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

        public async Task GenerateWebRequest(WorkManage wm,int depth)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(RequestUri);
            webRequest.Method = "GET";
            try
            {
                webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
                ContentStream = webResponse.GetResponseStream();
                GetLinks getLinks = new GetLinks(this.GetContent());

                wm.DictionaryLock = true;
                foreach (string uri in getLinks.GetUris())
                {
                    if (!wm.unfinisheduri.ContainsKey(uri))
                        wm.unfinisheduri.Add(uri, depth + 1);
                }

                wm.DictionaryLock = false;
                
            }
            catch
            {
                System.Console.WriteLine("!!");
                
            }
           
            
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
