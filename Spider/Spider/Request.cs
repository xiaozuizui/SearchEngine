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

        public async void GenerateWebRequestAsync(WorkManage wm,int depth,IndexManager indexmanager)
        {
            
            try
            {
                webRequest = (HttpWebRequest)WebRequest.Create(RequestUri);//创建Request实例
                webRequest.Method = "GET";//方法为GET
                webRequest.KeepAlive = true;//持续型链接
                webRequest.Timeout = 100;//超时值为100ms

                webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();//获取当前请求的响应
                ContentStream = webResponse.GetResponseStream();//获取响应的字节流
                string html = GetContent();//转化为HTML文本
                GetLinks getLinks = new GetLinks(html);
                Article article = new Article();
                Html2Article.AppendMode = false;
                Html2Article.Depth = 80;
                article = Html2Article.GetArticle(html);

                wm.sum++;
                indexmanager.AddIndex(article.Title, article.Content, RequestUri);


                if (depth < wm.Depth)
                {

                    
                        foreach (string uri in getLinks.GetUris())
                        {
                        if (!wm.unfinisheduri.ContainsKey(uri))
                            lock (wm.unfinisheduri)
                            {
                                wm.unfinisheduri.Add(uri, depth + 1);
                            }
                        }
                    

                }
                webRequest.Abort();
                webResponse.Close();
            }
            catch
            {

            }
                //Thread.Sleep(timeSpan);
               //ThreadPool
                

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
