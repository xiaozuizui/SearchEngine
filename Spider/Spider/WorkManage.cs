﻿using System;
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
        //bool [] IsFinished { get; set; }
        public bool [] WorkBusy { get; set; }
        public Dictionary<string,int> finisheduri { get; set; }
        public Dictionary<string,int> unfinisheduri { get; set; }
        public bool DictionaryLock { get; set; }
        public int Depth { get; set; }
        public int number { get; set; }
        public bool workStatue { get; set; }
        public WorkManage(int requestcount,int depth,string baseuri)
        {
            DictionaryLock = false;
            RequestCount = requestcount;
            WorkBusy = new bool[requestcount];
            for(int i = 0;i<RequestCount;i++)
                WorkBusy[i] = false;
            Depth = depth;
            finisheduri = new Dictionary<string,int>();
            unfinisheduri = new Dictionary<string, int>();
            unfinisheduri.Add(baseuri, 1);
            number = 1;
            workStatue = false;
        }
        public void RunTask()
        {


                for (int i = 0; i < RequestCount; i++)
                {

                    GenerateWork(i);
                }
                
            
            
            



        }
        async Task  GenerateWork(int i)
        {
            
            if (unfinisheduri.Count!=0&&DictionaryLock==false)
            {
                WorkBusy[i] = true;
                DictionaryLock = true;
                string uri = unfinisheduri.First().Key;
                int depth = unfinisheduri.First().Value;
                
              
                unfinisheduri.Remove(uri);
           
                DictionaryLock = false;

                if (!finisheduri.ContainsKey(uri))
                {
                    
                    if (depth <= Depth)
                    {
                        finisheduri.Add(uri, depth);
                        Request request = new Request(uri);
                        await request.GenerateWebRequest(this, depth);
                        WorkBusy[i] = false;
                        number += 1;
                        System.Console.WriteLine(number+"   "+ uri);
                        // WorkBusy[i] = false;
                        //   RunTask();
                    }
                    else
                    {
                        WorkBusy[i] = false;
                    }
                    
                }
                else
                {
                    WorkBusy[i] = false;
                }
                
            }
            
           
        }


        public bool isFinished()
        {
            if (unfinisheduri.Count != 0)
                return false;
            else
            {
                for(int i =0; i<RequestCount;i++)
                {
                    if (WorkBusy[i])
                        return false;
                }
                return true;
            }
        }
    }
}
