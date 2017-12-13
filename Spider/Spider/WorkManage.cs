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
            request.GenerateWebRequestAsync(this, 1, manager);
            Thread.Sleep(1000);

            for (int i=0;i<RequestCount;i++)
            {
                threads[i].Start(i);
            }
        }

        public void SearchRequest()
        {
            //unfinisheduri = request;
            //request.Clear();
        }

        void GenerateWork(object i)
        {
            int j = (int)i;
            while (true)
            {
                    if (unfinisheduri.Count == 0)
                    {
                        WorkBusy[j] = false;
                        break;
                    }

                string uri;
                int depth;
                lock (unfinisheduri)
                {
                    uri = unfinisheduri.First().Key;
                    depth = unfinisheduri.First().Value;
                    unfinisheduri.Remove(uri);
                }

                        if (!finisheduri.ContainsKey(uri))
                            finisheduri.Add(uri, depth);

                        //System.Console.WriteLine(uri + "      " + depth.ToString());
                        if (depth <= Depth)
                        {
                            System.Console.WriteLine(uri+"      "+depth.ToString());

                            Request request = new Request(uri);
                            request.GenerateWebRequestAsync(this, depth, manager);//创建请求
                        }
            }
        }

        public bool isFinished()
        {
           
                for(int i =0; i<RequestCount;i++)
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
