using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using StanSoft;
using Web;
namespace Spider
{
    public class Request
    {

        public Request(string uri)
        {
            RequestUri = uri;
           // webRequest = new HttpWebRequest();
        }

        public async Task GenerateWebRequest(WorkManage wm,int depth,IndexManager indexmanager)
        {
            webRequest = (HttpWebRequest)WebRequest.Create(RequestUri);
            webRequest.Method = "GET";
            //webRequest.Timeout = 100;
            try
            {

                webResponse = (HttpWebResponse)await webRequest.GetResponseAsync();
                ContentStream = webResponse.GetResponseStream();
                string html = GetContent();
                GetLinks getLinks = new GetLinks(html);
                Article article = new Article();
                Html2Article.AppendMode = false;
                Html2Article.Depth = 80;
                article = Html2Article.GetArticle(html);
         

                indexmanager.AddIndex(article.Title, article.Content, DateTime.Now.ToString(), RequestUri);

                
                if (depth < wm.Depth)
                {

                    wm.DictionaryLock = true;
                    foreach (string uri in getLinks.GetUris())
                    {
                        if (!wm.unfinisheduri.ContainsKey(uri))
                            wm.unfinisheduri.Add(uri, depth + 1);
                    }
                    wm.DictionaryLock = false;
                }
                
                
            }
            catch
            {
                wm.request.Add(RequestUri, depth);
             //   System.Console.WriteLine("request fail "+RequestUri);
                
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
