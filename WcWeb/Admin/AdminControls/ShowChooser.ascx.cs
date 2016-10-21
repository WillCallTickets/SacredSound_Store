using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace WillCallWeb.Admin
{
    public partial class ShowChooser : BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            WillCallWeb.Admin.AdminEvent.ShowChosen += new AdminEvent.ShowChosenEvent(AdminEvent_ShowChosen);
            WillCallWeb.Admin.AdminEvent.ShowNameChanged += new AdminEvent.ShowNameChangeEvent(AdminEvent_ShowNameChanged);
        }
        public override void Dispose()
        {
            WillCallWeb.Admin.AdminEvent.ShowChosen -= new AdminEvent.ShowChosenEvent(AdminEvent_ShowChosen);
            WillCallWeb.Admin.AdminEvent.ShowNameChanged -= new AdminEvent.ShowNameChangeEvent(AdminEvent_ShowNameChanged);
            base.Dispose();
        }
        protected void AdminEvent_ShowNameChanged(object sender, EventArgs e)
        {
            ddlShow.DataBind();
        }
        protected void AdminEvent_ShowChosen(object sender, AdminEvent.ShowChosenEventArgs e)
        {
            ////has the current showrecord changed yet?
            //if (Atx.CurrentShowRecord != null)
            //{
            //    if (ddlShow.Items.Count > 0)
            //    {
            //        //this works for both current and past dates
            //        ListItem li = ddlShow.Items.FindByValue(Atx.CurrentShowRecord.Id.ToString());
            //        if (li == null)
            //        {
            //            //Atx.ShowChooserStartDate = Atx.CurrentShowRecord.FirstDate;
            //            //ddlShow.DataBind();
            //        }
            //    }
            //}
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { }
        }

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;
            cal.SelectedDate = Atx.ShowChooserStartDate;
        }

        protected void clock_DateChange(object sender, WillCallWeb.Components.Util.CalendarClock.CalendarClockChangedEventArgs e)
        {
            //reset start date
            Atx.ShowChooserStartDate = e.ChosenDate;

            //reset current selection
            AdminEvent.OnShowChosen(this, 0);
        }
        
        protected void ddlShow_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //set current show in context - and id
            int idx = int.Parse(ddl.SelectedValue);

            AdminEvent.OnShowChosen(this, idx);
        }
        
        protected void ddlShow_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ddl.SelectedIndex = -1;

            //ensure a selection item
            if(ddl.Items.Count == 0 || ddl.Items[0].Value != "0")
                ddl.Items.Insert(0, new ListItem("<-- SELECT A SHOW -->", "0"));

            //match current show to selection
            int idx = (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.Id : 0;

            ListItem li = ddl.Items.FindByValue(idx.ToString());
            if (li != null)
                li.Selected = true;
            else
                ddl.SelectedIndex = 0;//select default item
        }
        
        protected void SqlShowList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
}
}