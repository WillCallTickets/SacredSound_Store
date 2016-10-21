using System;

namespace WillCallWeb.Admin
{
    public partial class ShowEditor : WillCallWeb.BasePage
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //if (controlToLoad == "ShowDate_TicketPostPurchase")
            //{
            //    Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, null, true);
            //}
            //else
            //{
            //    //set opacity for nav events
            //    if (this.HasControls() && this.UpdatePanel1.Visible)
            //        Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#editor", true);
            //}
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            WillCallWeb.Admin.AdminEvent.ShowChosen += new Admin.AdminEvent.ShowChosenEvent(EventHandler_ShowChosenChanged);
        }
        public override void Dispose()
        {
            WillCallWeb.Admin.AdminEvent.ShowChosen -= new Admin.AdminEvent.ShowChosenEvent(EventHandler_ShowChosenChanged);

            base.Dispose();
        }
        private void EventHandler_ShowChosenChanged(object sender, Admin.AdminEvent.ShowChosenEventArgs e)
        {
            //reset the current show
            Atx.SetCurrentShowRecord(e.ChosenId);
            Atx.CurrentActId = 0;
            Atx.CurrentVenueId = 0;

            //if we choose a show - move away from the create page
            if (Request.QueryString["p"] == null || Request.QueryString["p"].ToLower() == "showpicker")
                base.Redirect("ShowEditor.aspx?p=details");
            else
                base.Redirect(Request.RawUrl);
        }

        protected override void OnPreInit(EventArgs e)
        {
            string req = Request.QueryString["p"];

            QualifySsl(req != null);

            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageControl();
        }
        private string controlToLoad = "ShowPicker";
        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            switch (controlToLoad.ToLower())
            {
                case "bundle":
                    controlToLoad = "ShowTicket_Bundle";
                    break;
                case "showpicker":
                    controlToLoad = "ShowCreator";
                    Atx.SetCurrentShowRecord(0);
                    break;
                case "details":
                    controlToLoad = "ShowDetails";
                    break;
                case "showlinks":
                    controlToLoad = "ShowLinks";
                    break;
                case "showdate":
                    controlToLoad = "ShowDate_Details";
                    break;
                case "tickets":
                    controlToLoad = "ShowDate_Tickets";
                    break;
                case "reqs":
                    controlToLoad = "ShowDate_TicketRequirement";
                    break;
                case "postp":
                    controlToLoad = "ShowDate_TicketPostPurchase";
                    break;
                case "acts":
                    controlToLoad = "ShowDate_Acts";
                    break;
                case "promoter":
                    controlToLoad = "ShowPromoters";
                    break;
                //case "hardprint":
                //    controlToLoad = "ShowDate_TicketPrinter";
                //    break;
            }

            //testing if(controlToLoad != "ShowCreator")
            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}