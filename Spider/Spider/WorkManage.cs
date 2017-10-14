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

        public bool DictionaryLock { get; set; }
        public int Depth { get; set; }

        public WorkManage(int requestcount,int depth,string baseuri)
        {
            RequestCount = requestcount;
            WorkBusy = new bool[requestcount];
            for(int i = 0;i<RequestCount;i++)
                WorkBusy[i] = false;
            Depth = depth;
            finisheduri = new Dictionary<string,int>();
            unfinisheduri = new Dictionary<string, int>();
            unfinisheduri.Add(baseuri, 1);
        }
        public void RunTask()
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
         
            if(unfinisheduri.Count!=0&&DictionaryLock==false)
            {
                WorkBusy[i] = true;
                string uri = unfinisheduri.First().Key;
                int depth = unfinisheduri.First().Value;
                unfinisheduri.Remove(uri);

                if(!finisheduri.ContainsKey(uri))
                {
                    finisheduri.Add(uri, depth);
                    if (depth <= Depth)
                    {
                        Request request = new Request(uri);
                        request.GenerateWebRequest(i, this, depth);
                    }
                }
                

            }


        }
    }
}
