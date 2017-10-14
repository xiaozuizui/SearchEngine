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

        void RunTask()
        {
            for(int i=0;i<RequestCount;i++)
            {
                GenerateWork(i);
            }

        }
        void GenerateWork(int i)
        {
            WorkBusy[i] = true;

            string uri = unfinisheduri.First();
            unfinisheduri.Remove(uri);
            finisheduri.Add(uri);

            Request request = new Request(uri);

        }
    }
}
