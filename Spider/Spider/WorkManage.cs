using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spider;
using Web;
using System.Diagnostics;

using GetArticle;
using System.Threading;

namespace Spider
{
    public class WorkManage
    {
        int RequestCount { get; set; } //并行工作数量
        //bool [] IsFinished { get; set; }
        public bool [] WorkBusy { get; set; }

        public Thread[] threads;
        public string baseU;
        public Dictionary<string,int> finisheduri { get; set; }
        public Dictionary<string,int> unfinisheduri { get; set; }
        //public Dictionary<string,int> request { get; set; }

        public bool DictionaryLock { get; set; }
        public int Depth { get; set; }
        public int sum;

       // public Html2Article Html2Article;
        public IndexManager manager;

        public WorkManage(int requestcount,int depth,string baseuri)
        {
            baseU = baseuri;

            sum = 1;
            threads = new Thread[requestcount];
            WorkBusy = new bool[requestcount];

            for (int i=0;i<requestcount;i++)
            {
                WorkBusy[i] = true;
                threads[i] = new Thread(new  ParameterizedThreadStart(GenerateWork));
                threads[i].IsBackground = true;
            }


            
            

            RequestCount = requestcount;
           
            Depth = depth;
            finisheduri = new Dictionary<string,int>();
            unfinisheduri = new Dictionary<string, int>();
            //request = new Dictionary<string, int>();

            //unfinisheduri.Add(baseuri, 1);
            
            manager = new IndexManager("./index");
            manager.SetIndexWriter(true);

            
        }

        public void  RunTaskAsync()
        {
            Request request = new Request(baseU);
            request.GenerateWebRequestAsync(this, 1, manager);//首先获取baseUrl的链接
            Thread.Sleep(1000);

            for (int i=0;i<RequestCount;i++)//RequestCount为运行线程数量
            {
                threads[i].Start(i);//启动每个线程
            }
        }

        public void SearchRequest()
        {
            //unfinisheduri = request;
            //request.Clear();
        }

        void GenerateWork(object i)//i为线程标识
        {
            int j = (int)i;
            while (true)
            {
                if (unfinisheduri.Count == 0)//查看是否有未抓取的URL
                {
                        WorkBusy[j] = false;//如果没有待抓取的URL，将当前线程状态设为空闲
                        break;
                }
                string uri;
                int depth;

                lock (unfinisheduri)//先将容器上锁，从未抓取的URL中拿出URL
                {
                    uri = unfinisheduri.First().Key;
                    depth = unfinisheduri.First().Value;
                    unfinisheduri.Remove(uri);
                }
                if (!finisheduri.ContainsKey(uri))
                    finisheduri.Add(uri, depth);
                if (depth <= Depth)//拿出的URL的depth满足设置的值则发生Web请求
                {
                    System.Console.WriteLine(uri+"      "+depth.ToString());
                    Request request = new Request(uri);
                    request.GenerateWebRequestAsync(this, depth, manager);//创建请求
                }
            }
        }

        public bool isFinished()
        {
            for (int i = 0; i < RequestCount; i++)//每个线程工作状态都为空闲则完成
            {
                if (WorkBusy[i])
                    return false;
            }
            return true;
            
        }

        public void SaveIndex()
        {
            manager.SaveIndex();
            manager.SetIndexWriter(false);

        }
    }
}
