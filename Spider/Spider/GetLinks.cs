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
                Links.Add(match[i].ToString());
            }
            return Links;
        }

    }
}
