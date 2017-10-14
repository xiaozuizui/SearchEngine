using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider
{
    class WorkManage
    {
        int RequestCount { get; set; }
        bool [] IsFinished { get; set; }

        List<string> finisheduri;
        List<string> unfinisheduri;

        void RunTask()
        {
            string uri = unfinisheduri.First();
            unfinisheduri.Remove(uri);
            finisheduri.Add(uri);

        }
    }
}
