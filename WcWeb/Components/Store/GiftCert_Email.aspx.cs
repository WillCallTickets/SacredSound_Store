using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace WillCallWeb.Components.Store
{
    public partial class GiftCert_Email : BasePage
    {
        protected string _email = null;
        protected string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string reqEmail = Request.QueryString["eml"];

            if(reqEmail != null)
                Email = reqEmail.Trim();            
        }
}
}