using System;

namespace WillCallWeb.Admin
{
    public partial class DownloadEditor : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#downloadeditor", true);
        }
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string req = Request.QueryString["downloaditem"];
            if (req != null && Utils.Validation.IsInteger(req))
            {
                int idx = int.Parse(req);

                if (idx > 0)
                {
                    Atx.SetCurrentMerchRecord(idx);

                    if (Atx.CurrentMerchRecord.IsChild)
                        Atx.SetCurrentMerchRecord(Atx.CurrentMerchRecord.TParentListing.Value);
                }
                else if (idx == 0 && Atx.CurrentMerchRecord != null)
                    Atx.SetCurrentMerchRecord(0);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {   
            SetPageControl();
        }

        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = "Download_Item";
            string selectorTitle = "Download";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "downloads":
                //case "itemedit":
                    controlToLoad = "Download_Item";
                    selectorTitle = "Download";
                    break;
            }

            this.Menu_DownloadSelection1.Title = selectorTitle;
            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\Downloads\{0}.ascx", controlToLoad)));
        }
    }
}