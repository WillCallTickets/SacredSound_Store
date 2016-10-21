using System;

public partial class Store_Maintenance : System.Web.UI.Page
{
    protected override void OnPreInit(EventArgs e)
    {
        //QualifySsl(false);
        base.OnPreInit(e);
        this.Theme = string.Empty;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //**processing vars reset handled in basePage redirect

        if (!IsPostBack)
        {
            //if the cart has items - clear it

            //if the user is logged in - log them out

            SetPageControl();
        }
    }

    private void SetPageControl()
    {
        //SET UP PAGE BASED UPON QS
        string controlToLoad = "Maintenance";

        Content.Controls.Add(LoadControl(string.Format(@"..\controls\{0}.ascx", controlToLoad)));
    }
}
