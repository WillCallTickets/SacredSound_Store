using System;
using System.Threading;

public partial class Store_ProcessingShipping : WillCallWeb.BasePage
{
    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(true);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Ctx.Cart.IsOverMaxTransactionAllowed)
            base.Redirect("/Store/Cart_Edit.aspx");
    }
}
