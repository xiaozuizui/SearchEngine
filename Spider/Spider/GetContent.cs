using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winista.Text;
using Winista.Text.HtmlParser;
using StanSoft;

namespace Spider
{
    public class GetContent
    {
        private string HtmlStr{ get; set; }
        private Article text { get; set; }
     //   private Parser htmlParser { get; set; }
        public GetContent(string htmlst)
        {
            HtmlStr = htmlst;
            //  htmlParser = new Parser(new Winista.Text.HtmlParser.Lex.Lexer(HtmlStr));
            Article article = Html2Article.GetArticle(HtmlStr);
        }

        public void Get()
        {

        }

    }
}
