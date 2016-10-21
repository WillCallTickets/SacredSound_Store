using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.Choosers
{
    public partial class Chooser_Merch : BaseControl
    {
        List<string> _errors = new List<string>();

        public int SelectedValue
        {
            get
            {
                return int.Parse(ddlParent.SelectedValue);
            }
        }

        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        #endregion
}
}
