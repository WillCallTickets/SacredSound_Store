using System;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Customer_Navigation : WillCallWeb.BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageControl();
        }

        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = "history";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "history":
                    controlToLoad = "Customer_SalesHistory";
                    linkHistory.NavigateUrl = string.Empty;
                    break;
                case "change":
                    controlToLoad = "Customer_ChangePass";
                    linkChangePass.NavigateUrl = string.Empty;
                    break;
                case "credit":
                    controlToLoad = "/Components/Customer/Cust_StoreCredit";
                    linkCredit.NavigateUrl = string.Empty;
                    break;
                case "changename":
                    if (Wcss._Config._AllowCustomerInitiatedNameChanges)
                        controlToLoad = "Customer_ChangeName";
                    lnkChangeName.NavigateUrl = string.Empty;
                    break;
            }

            PanelContent.Controls.Add(LoadControl(string.Format(@"{0}.ascx", controlToLoad)));
        }
}
}
