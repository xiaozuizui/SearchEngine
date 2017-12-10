using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using GetArticle;
using System.Diagnostics;
using Web;
namespace Spider
{
    public class Request
    {
        public TimeSpan timeSpan = new TimeSpan(0,0,1);
        public Stopwatch stopwatch = new Stopwatch();

        public Request(string uri)
        {
            RequestUri = uri;

           // webRequest = new HttpWebRequest();
        }

        public async Task GenerateWebRequestAsync(WorkManage wm,int depth,IndexManager indexmanager)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(RequestUri);
            webRequest.Method = "GET";
            //webRequest.Timeout = 100;
           webRequest.KeepAlive = true;
            webRequest.Timeout = 10;
            try
            {


                webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
            }
            catch
            { }
                //Thread.Sleep(timeSpan);
               //ThreadPool
                ContentStream = webResponse.GetResponseStream();
                string html = GetContent();
                GetLinks getLinks = new GetLinks(html);
                Article article = new Article();
                Html2Article.AppendMode = false;
                Html2Article.Depth = 80;
                article = Html2Article.GetArticle(html);


                indexmanager.AddIndex(article.Title, article.Content,  RequestUri);

                
                if (depth < wm.Depth)
                {

                    lock (wm.unfinisheduri)
                    {
                        foreach (string uri in getLinks.GetUris())
                        {
                            if (!wm.unfinisheduri.ContainsKey(uri))
                                wm.unfinisheduri.Add(uri, depth + 1);
                        }
                    }
          
                }
            webRequest.Abort();
            webResponse.Close();

        }




        public string GetContent()
        {
            return new StreamReader(ContentStream).ReadToEnd();
        }

        string RequestUri { get; set; }

        private HttpWebResponse webResponse;
        private HttpWebRequest webRequest;
     //   private UInt32 Index;   //编号
        private Stream ContentStream;


    }
}
