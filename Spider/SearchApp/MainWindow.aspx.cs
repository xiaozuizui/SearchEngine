using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GetArticle;
using Web;
namespace SearchApp
{
    public partial class MainWindow : System.Web.UI.Page
    {

        private string strIndexPath = string.Empty;
        protected string txtTitle = string.Empty;
        protected string txtContent = string.Empty;
        protected long lSearchTime = 0;
        protected IList<Article> list = new List<Article>();
        protected string txtPageFoot = string.Empty;
        protected IndexManager indexManager;

        protected string IndexDic
        {
            get
            {
                return Server.MapPath("/IndexDic");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            indexManager = new IndexManager(IndexDic);
            switch (Action)
            {
              //  case "CreateIndex": CreateIndex(Cover); break;
                case "SearchIndex": SearchIndex(); break;
            }
            
        }


        private void SearchIndex()
        {
            string st = "";
            indexManager.SearchIndex(txtContent, new IndexManager.Page(10, 1), ref st);
        }
        private int PageIndex
        {
            get
            {
                if (Request.Form["pageIndex"] != null)
                {
                    return Convert.ToInt32(Request.Form["pageIndex"]);
                }
                else
                {
                    return 1;
                }
            }
        }

        private int PageSize
        {
            get
            {
                if (Request.Form["pageSize"] != null)
                {
                    return Convert.ToInt32(Request.Form["pageSize"]);
                }
                else
                {
                    return 10;
                }
            }
        }

        /// <summary>
        /// 是否覆盖索引
        /// </summary>
        private bool Cover
        {
            get
            {
                if (Request.Form["cover"] != null)
                {
                    if (Request.Form["cover"] == "1")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else return true;
            }
        }

        /// <summary>
        /// 分页页脚
        /// </summary>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="pageSize">记录条数</param>
        /// <param name="total">记录总数</param>
        /// <param name="cssName">css样式名称</param>
        /// <returns></returns>
        private string GetPageFoot(int currentPageIndex, int pageSize, int total, string cssName)
        {
            currentPageIndex = currentPageIndex <= 0 ? 1 : currentPageIndex;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            string options = string.Empty;
            int pageCount = 0;//总页数
            int pageVisibleCount = 10; // 显示数量
            if (total % pageSize == 0)
            {
                pageCount = total / pageSize;
            }
            else
            {
                pageCount = total / pageSize + 1;
            }
            //如果是整除的话,退后一页
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"page_left\">一页显示<select id=\"pageSize\" name=\"pageSize\" onchange =\"SC.Page.ChangeSize();\">{0}</select>&nbsp;条&nbsp;&nbsp;&nbsp;总共{1}条</div>", SetOption(pageSize), total);
            sb.AppendFormat("<div class=\"page_right\">跳转到第<input type=\"text\" id=\"pageIndex\" name=\"pageIndex\" value=\"{0}\" />页<a href=\"#\" class=\"easyui-linkbutton\" plain=\"true\" iconCls=\"icon-redo\" onclick=\"SC.Page.GotoPage();\">Go</a>共<span id=\"pageCount\">" + pageCount + "</span>&nbsp;页</div><input type=\"hidden\" id=\"isSearch\" name=\"isSearch\" value=\"1\" />", currentPageIndex);

            sb.Append("<div class='" + cssName + "'>");// sbrosus分页样式，需要自己添加哇


            if (currentPageIndex == 1 || total < 1)
            {
                sb.Append("<span ><a href='javascript:void(0)'>首页</a></span>");
                sb.Append("<span ><a href='javascript:void(0)'>上一页</a></span>");
            }
            else
            {
                sb.Append("<span><a onclick=\"SC.Page.GetPage(1)\">首页</a></span>");
                sb.Append("<span><a onclick=\"SC.Page.GetPage(" + (currentPageIndex - 1).ToString() + ")\">上一页</a></span>");
            }
            int i = 1;
            int k = pageVisibleCount > pageCount ? pageCount : pageVisibleCount;
            if (currentPageIndex > pageVisibleCount)
            {
                i = currentPageIndex / pageVisibleCount * pageVisibleCount;
                k = (i + pageVisibleCount) > pageCount ? pageCount : (i + pageVisibleCount);
            }
            for (; i <= k; i++)//k*10防止k为负数
            {
                if (i == currentPageIndex)
                {
                    sb.AppendFormat("<span class='current' ><a href='javascript:void(0)'>{0}</a></span>&nbsp;", i);
                }
                else
                {
                    sb.AppendFormat("<span><a onclick=\"SC.Page.GetPage(" + i + ")\" >{0}</a></span>&nbsp;", i);
                }
            }
            if (currentPageIndex == pageCount || total < 1)
            {
                sb.Append("<span ><a href='javascript:void(0)'>下一页</a></span>");
                sb.Append("<span ><a href='javascript:void(0)'>尾页</a></span>");
            }
            else
            {
                sb.Append("<span><a onclick=\"SC.Page.GetPage(" + (currentPageIndex + 1).ToString() + ")\">下一页</a></span>");
                sb.Append("<span><a onclick=\"SC.Page.GetPage(" + pageCount + ")\">尾页</a></span></div>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据pagesize获取select标签
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private string SetOption(int pageSize)
        {
            StringBuilder sb_options = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                if (pageSize / 10 == i + 1)
                {
                    sb_options.AppendFormat("<option selected=\"selected\">{0}0</option>", (i + 1).ToString());
                }
                else
                {
                    sb_options.AppendFormat("<option>{0}0</option>", (i + 1).ToString());
                }
            }
            if (pageSize == 1000)
            {
                sb_options.Append("<option selected=\"selected\">1000</option>");
            }
            else
            {
                sb_options.Append("<option >1000</option>");
            }

            return sb_options.ToString();
        }

        protected string Action
        {
            get
            {
                if (Request.Form["action"] != null)
                {
                    return Request.Form["action"].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
    }
}