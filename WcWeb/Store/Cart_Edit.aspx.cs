using System;
using System.Web.Services;

using WillCallWeb;
using WillCallWeb.StoreObjects;
using Wcss;

public partial class Store_Cart_Edit : WillCallWeb.BasePage
{
    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(false);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //avoid double clicks! always reset
        Ctx.OrderProcessingVariables = null;

        string controlToLoad = (_Config.UseNewCart) ? @"Cart_SPC\Cart_2015" : @"\Cart";

        panelCart.Controls.Add(LoadControl(string.Format(@"..\Controls{0}.ascx", controlToLoad)));
    }

}
