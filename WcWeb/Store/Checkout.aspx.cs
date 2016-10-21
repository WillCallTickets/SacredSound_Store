using System;

public partial class Store_Checkout : WillCallWeb.BasePage
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

        string controlToLoad = @"\Checkout";

        panelCheckout.Controls.Add(LoadControl(string.Format(@"..\Controls{0}.ascx", controlToLoad)));
    }
}
