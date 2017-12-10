using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider
{
    public class GetLinks
    {
        public  string webContent { get; set; }

        public List<string> Links { get; set; }

        public GetLinks(string content)
        {
            webContent = content;
            Links = new List<string>();
        }

        public List<string> GetUris()
        {
            // string p = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
            //(https ?| ftp | file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]
            Regex rg = new Regex(@"(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]", RegexOptions.IgnoreCase);
            //Regex rg = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
            MatchCollection match = rg.Matches(webContent);
            for(int i=0;i<match.Count;i++)
            {
                string str = match[i].ToString();
                IsLink(str);
                   // Links.Add(str);
            }
            return Links;
        }

        void IsLink(string http)
        {
            int i1 = http.LastIndexOf(".");
            int length = http.Length;
            string hz = " ";

            if(http[length-1] == '/')
            {
                http = new string(http.ToCharArray(0, length - 1));
                length--;
            }
            
           
                hz = new string(http.ToCharArray(i1+1, length - i1 -1));
           

            if(hz == "jpg"|| hz == "gif"|| hz=="png"|| hz=="css"|| hz == "jsp"||hz =="jsf"||hz == "js")
            {
                return;
            }
            else
            {
                
                Links.Add(http);
            }
           //  hz = new string(http.ToCharArray(i1, length - i1 ));
            // string hz = new string(http.ToCharArray(i1, length - i1));
    

        }

    }
}
