using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using Wcss;

namespace WillCallWeb.Admin
{
    public partial class DisplayList : WillCallWeb.BasePage
    {
        protected List<string> _listData = new List<string>();
        public List<string> ListData
        {
            get
            {
                return _listData;
            }
            set
            {
                _listData = value;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litList.DataBind();
            }
        }
        protected void litList_DataBinding(object sender, EventArgs e)
        {
            if (Atx.CurrentDisplayList != null && Atx.CurrentDisplayList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (String s in Atx.CurrentDisplayList)
                    sb.AppendFormat("{0}<br/>", s);

                Atx.CurrentDisplayList = null;

                litList.Text = sb.ToString();
            }
            else
                litList.Text = "No Data";
        }
}
}