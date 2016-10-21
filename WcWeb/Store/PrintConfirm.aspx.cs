using System;
using System.Collections.Generic;

using Wcss;

public partial class Store_PrintConfirm : WillCallWeb.BasePage
{
    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(false);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e) 
    {
        if (Ctx.SessionAuthorizeNet == null)
        {
            this.Controls.Clear();
            System.Web.UI.WebControls.Literal lit = new System.Web.UI.WebControls.Literal();
            lit.Text = "We're sorry, your invoice could not be found.<br/>It is possible that your shopping session may have timed out.<br/>To remedy this issue, please close this window and re-logon.<br/><br/>";
            lit.Text += string.Format("If you continue to have problems, please <a href=\"/Contact.aspx\">contact us</a>");
            this.Controls.Add(lit);
        }

        string detailsToLoad = "/PurchaseDetails";
        string cartToLoad = "/Cart_Purchase";

        pnlDetails.Controls.Add(LoadControl(string.Format(@"../controls{0}.ascx", detailsToLoad)));
        pnlCart.Controls.Add(LoadControl(string.Format(@"../controls{0}.ascx", cartToLoad)));
    }
}