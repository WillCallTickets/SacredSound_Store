using System;

namespace WillCallWeb.Controls
{
    public partial class HeaderBar : WillCallWeb.BaseControl
    {
        private string pageName = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            pageName = this.Page.ToString().ToLower();
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
}
}