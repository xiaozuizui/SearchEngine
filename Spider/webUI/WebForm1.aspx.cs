using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Web;
using Web.Model;
namespace webUI
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        IndexManager indexManger;
        protected IList<Rec>
        protected void Page_Load(object sender, EventArgs e)
        {
            indexManger = new IndexManager();
            //
            //indexManger.SetIndexWriter(true);
            

            //

        }
    }
}