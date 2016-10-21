using System;

public partial class Store_ProcessingOrder : WillCallWeb.BasePage
{
    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(true);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Ctx.MonkeyWrench();

        if (Ctx.Cart.IsOverMaxTransactionAllowed)
            base.Redirect("/Store/Cart_Edit.aspx");

        if (!Ctx.Cart.HasItems || Ctx.OrderProcessingVariables == null)
            base.Redirect("~/Default.aspx");

        //Ctx.MonkeyWrench();
    }
}
