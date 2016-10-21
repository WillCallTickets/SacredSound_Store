using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class Admin_ProcessingShipmentBatch : WillCallWeb.BasePage
{
    protected int _count = 0;
    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(true);
        base.OnPreInit(e);

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        string redir = Request.QueryString["redir"];

        if (Atx.TransactionProcessingVariables != null && redir != null)
        {
            Atx.TransactionProcessingVariables = null;
            HtmlMeta refresh = (HtmlMeta)this.Page.Header.FindControl("metaRefresh");
            if (refresh != null)
                refresh.Content = string.Format("1; URL=/Admin/Shipping_Tickets.aspx?p={0}", redir);
        }
        else if (redir != null)
        {
            HtmlMeta refresh = (HtmlMeta)this.Page.Header.FindControl("metaRefresh");
            if (refresh != null)
                refresh.Content = string.Format("1; URL=/Admin/Shipping_Tickets.aspx?p={0}", redir);
        }
        else if (redir == null)
            throw new Exception(string.Format("{0}: Redirection variable is null.", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt")));
    }
}
