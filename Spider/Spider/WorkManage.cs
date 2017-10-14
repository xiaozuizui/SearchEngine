using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spider;

namespace Spider
{
    public class WorkManage
    {
        int RequestCount { get; set; } //并行工作数量
        bool [] IsFinished { get; set; }
        public bool [] WorkBusy { get; set; }
        public Dictionary<string,int> finisheduri { get; set; }
        public Dictionary<string,int> unfinisheduri { get; set; }

        public int depth { get; set; }

        public WorkManage(int requestcount,int depth,string baseuri)
        {
            RequestCount = requestcount;
            for(int i = 0;i<RequestCount;i++)
                WorkBusy[i] = false;
            this.depth = depth;
            finisheduri = new Dictionary<string,int>();
            unfinisheduri = new Dictionary<string, int>();
            finisheduri.Add(baseuri, 1);
        }
        void RunTask()
        {
            while(true)
            {
                for (int i = 0; i < RequestCount; i++)
                {
                    if(!WorkBusy[i])
                        GenerateWork(i);
                }
            }
           

        }
        void GenerateWork(int i)
        {
            WorkBusy[i] = true;

            string uri = unfinisheduri.First().Key;
            int depth = unfinisheduri.First().Value;
            unfinisheduri.Remove(uri);
            finisheduri.Add(uri,depth);

            Request request = new Request(uri);
            request.GenerateWebRequest(i,this);
            
        }
    }
}
