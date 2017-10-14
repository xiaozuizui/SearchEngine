using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spider;

namespace Spider
{
    class WorkManage
    {
        int RequestCount { get; set; } //并行工作数量
        bool [] IsFinished { get; set; }
        bool [] WorkBusy { get; set; }
        List<string> finisheduri;
        List<string> unfinisheduri;

        public WorkManage(int requestcount)
        {
            RequestCount = requestcount;
            for(int i = 0;i<RequestCount;i++)
                WorkBusy[i] = false;
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

            string uri = unfinisheduri.First();
            unfinisheduri.Remove(uri);
            finisheduri.Add(uri);

            Request request = new Request(uri);
            request.GenerateWebRequest(i);
            
        }
    }
}
