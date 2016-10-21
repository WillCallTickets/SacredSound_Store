using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class ShowDate_Details : BaseControl
    {
        #region Collections and Page Objects

        protected ShowDateCollection OrderedCollection
        {
            get
            {
                ShowDateCollection coll = new ShowDateCollection();
                coll.AddRange(Atx.CurrentShowRecord.ShowDateRecords());
                coll.SortBy_DateToOrderBy();
                return coll;
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/ShowEditor.aspx");

            litShowTitle.Text = Atx.CurrentShowRecord.Name;

            if (!IsPostBack)
            {   
                GridView1.DataBind();
            }
        }

        #region GridView

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);
        }
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.DataSource = OrderedCollection;
            string[] keyNames = { "Id" };
            grid.DataKeyNames = keyNames;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {   
            GridView grid = (GridView)sender;

            if (grid.DataSource != null && OrderedCollection.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;

            if (typ == DataControlRowType.DataRow)
            {
                ShowDate ent = (ShowDate)e.Row.DataItem;

                LinkButton button = (LinkButton)e.Row.FindControl("btnDelete");

                if (button != null && ent != null)
                    button.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(
                            string.Format("{0} {1} {2}", ent.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"), ent.AgesString, ent.StatusName)));
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            FormView1.DataBind();
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            CustomValidator validation = (CustomValidator)grid.FindControl("CustomValidation");
            
            //cannot delete the only active date
            if (OrderedCollection.Count == 1)
            {
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = string.Format("The only show date cannot be deleted.");
                }
                e.Cancel = true;
            }
            else
            {
                int idx = (int)grid.DataKeys[e.RowIndex].Value;

                //if this show date is active and it is the only active date.....
                ShowDate sd = (ShowDate)OrderedCollection.Find(idx);
                if (sd != null)
                {
                    if (sd.IsActive && OrderedCollection.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive); }).Count == 1)
                    {
                        if (validation != null)
                        {
                            validation.IsValid = false;
                            validation.ErrorMessage = string.Format("The only ACTIVE show date cannot be deleted.");
                        }
                        e.Cancel = true;
                        return;
                    }

                    try
                    {
                        e.Cancel = (!Atx.CurrentShowRecord.DeleteShowDate(idx));
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);

                        if (validation != null)
                        {
                            validation.IsValid = false;
                            validation.ErrorMessage = ex.Message;
                        }

                        e.Cancel = true;
                    }

                    if (!e.Cancel)
                    {
                        grid.SelectedIndex = e.RowIndex - 1;
                        grid.DataBind();
                    }
                }
            }
        }

        #endregion

        #region Details

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            ShowDateCollection selected = new ShowDateCollection();
            ShowDate addDate = (ShowDate)OrderedCollection.Find(idx);
            if(addDate != null)    
                selected.Add(addDate);

            FormView1.DataSource = selected;
            string[] keyNames = { "Id" };
            FormView1.DataKeyNames = keyNames;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            ShowDate entity = (ShowDate)FormView1.DataItem;

            if (form.CurrentMode == FormViewMode.Insert)
            {
                ShowDateCollection coll = new ShowDateCollection();
                coll.AddRange(Atx.CurrentShowRecord.ShowDateRecords().GetList().FindAll(delegate(ShowDate match) { return (match.IsActive == true); }));
                if(coll.Count > 1)
                    coll.Sort("DtDateOfShow", true);

                WillCallWeb.Components.Util.CalendarClock start = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockStartDate");
                start.SelectedValue = coll[coll.Count - 1].DateOfShow.AddDays(1).ToString("MM/dd/yyyy hh:mm tt");

                CheckBox first = (CheckBox)form.FindControl("chkCopyFirst");
                CheckBox previous = (CheckBox)form.FindControl("chkCopyPrevious");
                first.Visible = false;
                previous.Visible = false;

                int count = coll.Count;

                if (count > 0)
                {
                    first.Visible = true;
                    first.Text = string.Format(" Copy information from {0}?", coll[0].DateOfShow.ToString("MM/dd/yyyy hh:mmtt"));

                    if (count > 1)
                    {
                        previous.Visible = true;
                        previous.Text = string.Format(" Copy information from {0}?", coll[count - 1].DateOfShow.ToString("MM/dd/yyyy hh:mmtt"));
                    }
                }
            }

            if (form.CurrentMode == FormViewMode.Edit)
            {
                TextBox doors = (TextBox)form.FindControl("txtDoorTime");
                doors.Text = entity.DateOfShow.ToString("hh:mmtt");

                //activate controls based on config
                CheckBox chkRsvp = (CheckBox)form.FindControl("chkRsvp");
                TextBox txtRsvp = (TextBox)form.FindControl("txtRsvp");
                HyperLink lnkRsvp = (HyperLink)form.FindControl("lnkRsvp");
                lnkRsvp.Enabled = txtRsvp.Enabled = chkRsvp.Enabled = _Config._Facebook_RSVP_ShowDates_Active;

                if (txtRsvp.Text.Trim().Length > 0)
                {
                    lnkRsvp.Visible = true;
                    //lnkRsvp.NavigateUrl = string.Format("{0}{1}", (!txtRsvp.Text.Trim().ToLower().StartsWith("http")) ? "http://" : string.Empty, txtRsvp.Text.Trim());

                    lnkRsvp.NavigateUrl = string.Format("//{0}", txtRsvp.Text.Trim());
                }
                else
                    lnkRsvp.Visible = false;
            }

            //fill lists here
            DropDownList ddlAges = (DropDownList)form.FindControl("ddlAges");

            if (ddlAges != null)
            {
                if (ddlAges.Items.Count == 0)
                    ddlAges.DataSource = _Lookits.Ages;

                ddlAges.DataTextField = "Name";
                ddlAges.DataValueField = "Id";
                ddlAges.DataBind();

                ddlAges.SelectedIndex = -1;
                if (entity != null && entity.Id > 0)
                    ddlAges.Items.FindByValue(entity.TAgeId.ToString()).Selected = true;
                else
                {
                    ShowDateCollection coll = new ShowDateCollection();
                    coll.AddRange(Atx.CurrentShowRecord.ShowDateRecords());
                    if (coll.Count > 1)
                        coll.Sort("DtDateOfShow", true);
                    ShowDate first = coll[0];

                    if (first != null)
                    {
                        ListItem li = ddlAges.Items.FindByValue(first.TAgeId.ToString());
                        if (li != null)
                            li.Selected = true;
                    }
                }
            }

            DropDownList ddlStatus = (DropDownList)form.FindControl("ddlStatus");

            if (ddlStatus != null)
            {
                if (ddlStatus.Items.Count == 0)
                    ddlStatus.DataSource = _Lookits.ShowStatii;

                ddlStatus.DataTextField = "Name";
                ddlStatus.DataValueField = "Id";
                ddlStatus.DataBind();

                ddlStatus.SelectedIndex = -1;

                if (entity != null && entity.Id > 0)
                    ddlStatus.Items.FindByValue(entity.TStatusId.ToString()).Selected = true;
                else
                    ddlStatus.Items.FindByText("OnSale").Selected = true;
            }
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "viewsales":
                    base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&shodateid={0}", e.CommandArgument.ToString()));
                    break;
                case "cancel":
                    //just rebind the control to reset
                    form.ChangeMode(FormViewMode.Edit);
                    break;
            }

        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;

            try
            {
                WillCallWeb.Components.Util.CalendarClock start = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockStartDate");

                DateTime dos = start.SelectedDate;

                if(dos.Year < 1990)
                    throw new Exception("Please select a valid year.");
                
                ShowDateCollection exists = new ShowDateCollection();
                exists.AddRange(OrderedCollection.GetList().FindAll(
                    delegate(ShowDate match) { return (match.IsActive && match.TShowId == Atx.CurrentShowRecord.Id && match.DateOfShow == dos); }));

                if (exists.Count > 0)
                {
                    CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = string.Format("This show date already exists within the show group. Please choose from the list above and edit.");
                    }

                    e.Cancel = true;
                    return;
                }

                CheckBox copyFirst = (CheckBox)form.FindControl("chkCopyFirst");
                CheckBox copyPrevious = (CheckBox)form.FindControl("chkCopyPrevious");                

                int count = OrderedCollection.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive); }).Count;
                ShowDate newItem = null;

                //if we are copying - we have all the input we need at this point
                if (count > 0)
                {
                    System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);

                    if (copyFirst != null && copyFirst.Checked)
                    {
                        //todo test
                        newItem = Atx.CurrentShowRecord.AddShowDateFromShowDate(dos, null,
                            OrderedCollection.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive); })[0], 
                            mem.UserName, (Guid)mem.ProviderUserKey);
                    }
                    else if (copyPrevious != null && copyPrevious.Checked)
                    {
                        //todo test
                        //if we do it this way: "count - 1" is guaranteed be zero in a one item collection
                        //we will always have one active showDate
                        newItem = Atx.CurrentShowRecord.AddShowDateFromShowDate(dos, null,
                            OrderedCollection.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive); })[count - 1],
                            mem.UserName, (Guid)mem.ProviderUserKey);
                    }

                    CheckBox chkLate = (CheckBox)form.FindControl("chkLate");
                    if (chkLate != null)
                        newItem.IsLateNightShow = chkLate.Checked;
                }


                //if we are not copying
                if(newItem == null)
                {
                    newItem = new ShowDate();
                    newItem.DtStamp = DateTime.Now;
                    newItem.TShowId = Atx.CurrentShowRecord.Id;
                    newItem.IsAutoBilling = true;
                    newItem.Billing = null;
                    newItem.IsActive = true;
                    newItem.DateOfShow = DateTime.Parse(dos.ToString("yyyy-MM-dd hh:mmtt"));

                    TextBox txtShowTime = (TextBox)form.FindControl("txtShowTime");
                    DropDownList ddlAges = (DropDownList)form.FindControl("ddlAges");
                    DropDownList ddlStatus = (DropDownList)form.FindControl("ddlStatus");
                    DropDownList ddlShowTimeAmPm = (DropDownList)form.FindControl("ddlShowTimeAmPm");

                    CheckBox chkLate = (CheckBox)form.FindControl("chkLate");
                    if (chkLate != null)
                        newItem.IsLateNightShow = chkLate.Checked;

                    // a blank time will equate to TBA
                    string time = txtShowTime.Text.Trim();                    
                    if (time.Length > 0)
                        newItem.ShowTime = DateTime.Parse(string.Format("{0} {1} {2}", dos.ToString("MM/dd/yyyy"), 
                            time, ddlShowTimeAmPm.SelectedValue)).ToString("hh:mm tt");//space!

                    newItem.TAgeId = int.Parse(ddlAges.SelectedValue);
                    newItem.TStatusId = int.Parse(ddlStatus.SelectedValue);

                    newItem.Save();

                    //also copy headliner
                    if(count > 0)//from ordered collection above
                    {
                        ShowDate firstShow = OrderedCollection[0];
                        JShowActCollection jcoll = new JShowActCollection();
                        jcoll.AddRange(firstShow.JShowActRecords());

                        if (jcoll.Count > 0)
                        {
                            if (jcoll.Count > 1)
                                jcoll.Sort("DisplayOrder", true);

                            JShowAct head = jcoll[0];

                            JShowAct act = new JShowAct();
                            act.ActText = head.ActText;
                            act.TopBilling = head.TopBilling;
                            act.DisplayOrder = head.DisplayOrder;
                            act.DtStamp = DateTime.Now;
                            act.Featuring = head.Featuring;
                            act.PostText = head.PostText;
                            act.PreText = head.PreText;
                            act.TActId = head.TActId;
                            act.TShowDateId = newItem.Id;

                            act.Save();

                            newItem.JShowActRecords().Add(act);
                        }
                    }

                    Atx.CurrentShowRecord.ShowDateRecords().Add(newItem);
                    Atx.CurrentShowRecord.ShowDateRecords().SaveAll();
                }

                FormView1.ChangeMode(FormViewMode.Edit);

                GridView1.SelectedIndex = count;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }

                e.Cancel = true;
            }
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedDataKey["Id"]);

            try
            {
                if (sd != null)
                {
                    CheckBox chkActive = (CheckBox)form.FindControl("chkActive");
                    if (sd.IsActive != chkActive.Checked)
                    {
                        //if it is only active show date and is marked inactive - bad
                        if (chkActive.Checked == false && sd.ShowRecord.ShowDateRecords().GetList()
                            .FindAll(delegate(ShowDate match) { return (match.IsActive); }).Count == 1)
                            throw new Exception("The only active show date cannot be marked as inactive.");

                        sd.IsActive = chkActive.Checked;
                    }

                    CheckBox chkRsvp = (CheckBox)form.FindControl("chkRsvp");
                    TextBox txtRsvp = (TextBox)form.FindControl("txtRsvp");

                    if (chkRsvp != null && sd.UseFbRsvp != chkRsvp.Checked)
                        sd.UseFbRsvp = chkRsvp.Checked;
                    if (txtRsvp != null && (txtRsvp.Text != sd.FbRsvpUrl || txtRsvp.Text.ToLower().IndexOf("//") != -1))
                    {
                        string rsvp = txtRsvp.Text.ToLower().Replace("http://", string.Empty).Replace("https://", string.Empty).TrimStart(new char[] {'/'}).Trim();
                        sd.FbRsvpUrl = rsvp;
                    }

                    CheckBox chkLate = (CheckBox)form.FindControl("chkLate");
                    if (chkLate != null && sd.IsLateNightShow != chkLate.Checked)
                        sd.IsLateNightShow = chkLate.Checked;

                    CheckBox chkPrivate = (CheckBox)form.FindControl("chkPrivate");
                    if (chkPrivate != null && sd.IsPrivateShow != chkPrivate.Checked)
                        sd.IsPrivateShow = chkPrivate.Checked;

                    TextBox txtTitle = (TextBox)form.FindControl("txtTitle");
                    if (txtTitle != null && sd.ShowDateTitle != txtTitle.Text.Trim())
                        sd.ShowDateTitle = txtTitle.Text.Trim();

                    //doing this within the try and catch block will validate the date!
                    TextBox doors = (TextBox)form.FindControl("txtDoorTime");
                    if (doors.Text.Trim().ToUpper() != sd.DateOfShow.ToString("hh:mmtt").ToUpper())
                        sd.DateOfShow = DateTime.Parse(string.Format("{0} {1}", sd.DateOfShow.ToString("MM/dd/yyyy"), doors.Text.Trim()));

                    //ensure that tickets are sync'd with showdate
                    foreach (ShowTicket st in sd.ShowTicketRecords())
                    {
                        if (st.DateOfShow != sd.DateOfShow)
                        {
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.AppendFormat("UPDATE ShowTicket SET dtDateOfShow = '{0}' WHERE Id = {1}", sd.DateOfShow.ToString("MM/dd/yyyy hh:mmtt"), st.Id);
                            Utils.DataHelper.ExecuteQuery(sb, _Config.DSN);
                        }
                    }

                    TextBox show = (TextBox)form.FindControl("txtShowTime");
                    string showTime = show.Text.Trim();
                    if (sd.ShowTime != showTime)
                    {
                        if (showTime.Length == 0) sd.ShowTime = null;
                        else if (Utils.Validation.IsDate(showTime))
                        {
                            DateTime dummy = DateTime.Parse(string.Format("{0} {1}", sd.DateOfShow.ToString("MM/dd/yyyy"), showTime));//doesn't matter what date we use
                            sd.ShowTime = dummy.ToString("hh:mm tt");//get the space
                        }   
                        else
                            sd.ShowTime = showTime;
                    }

                    DropDownList ddlAges = (DropDownList)form.FindControl("ddlAges");
                    DropDownList ddlStatus = (DropDownList)form.FindControl("ddlStatus");

                    if (sd.TAgeId != int.Parse(ddlAges.SelectedValue))
                        sd.TAgeId = int.Parse(ddlAges.SelectedValue);

                    bool deactivateChildTickets = false;
                    
                    if (sd.TStatusId != int.Parse(ddlStatus.SelectedValue))
                    {
                        //if the show is old out - we cannot change to onsale
                        //i think this is the only conflict
                        if (ddlStatus.SelectedItem.Text == Wcss._Enums.ShowDateStatus.OnSale.ToString() && sd.ShowRecord.IsSoldOut)
                            throw new Exception("The parent show group is marked as sold out. You must change its status before resetting the status for this date.");

                        //mark parent as sold out if all showdates are sold out

                        sd.TStatusId = int.Parse(ddlStatus.SelectedValue);

                        if(ddlStatus.SelectedItem.Text != Wcss._Enums.ShowDateStatus.OnSale.ToString())
                            deactivateChildTickets = true;
                    }

                    TextBox txtBilling = (TextBox)form.FindControl("txtBilling");
                    if (txtBilling != null && sd.Billing != txtBilling.Text.Trim())
                        sd.Billing = txtBilling.Text.Trim();

                    TextBox txtPricing = (TextBox)form.FindControl("txtPricing");
                    if (txtPricing != null && sd.PricingText != txtPricing.Text.Trim())
                        sd.PricingText = txtPricing.Text.Trim();

                    TextBox txtStatus = (TextBox)form.FindControl("txtStatus");
                    if (txtStatus != null && sd.StatusText != txtStatus.Text.Trim())
                        sd.StatusText = txtStatus.Text.Trim();

                    TextBox txtNotes = (TextBox)form.FindControl("txtNotes");
                    if (txtNotes != null && sd.DisplayNotes != txtNotes.Text.Trim())
                        sd.DisplayNotes = txtNotes.Text.Trim();

                    Atx.CurrentShowRecord.ShowDateRecords().SaveAll();

                    if (deactivateChildTickets)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        sb.AppendLine("CREATE TABLE #relatedTix ( [idx] int ); ");
                        sb.AppendLine("INSERT #relatedTix(idx) SELECT st.[Id] as 'idx' FROM [ShowTicket] st WHERE st.[TShowDateId] = @showDateId; ");
                        sb.AppendLine("INSERT #relatedTix(idx) SELECT DISTINCT link.[LinkedShowTicketId] as 'idx' FROM [ShowTicketPackageLink] link, #relatedTix rel ");
                        sb.AppendLine("WHERE rel.[idx] = link.[ParentShowTicketId]; ");
                        sb.AppendLine("UPDATE [ShowTicket] SET [bActive] = @active WHERE [Id] IN (SELECT [idx] FROM #relatedTix); ");

                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@showDateId", sd.Id, System.Data.DbType.Int32);
                        cmd.Parameters.Add("@active", false, System.Data.DbType.Boolean);

                        SubSonic.DataService.ExecuteQuery(cmd);
                    }

                    
                    //ensure Show has latest data
                    Atx.ResetCurrentShowRecord();

                    if (Atx.CurrentShowRecord.AllShowDatesSoldOut)
                    {
                        Atx.CurrentShowRecord.IsSoldOut = true;
                        Atx.CurrentShowRecord.Save();
                    }

                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;

                    e.Cancel = true;
                }
            }
        }

        #endregion

        protected void btnChangeShowName_Click(object sender, EventArgs e)
        {
            if (!Atx.CurrentShowRecord.ShowNameMatches(false))
            {
                Atx.CurrentShowRecord.ShowNameMatches(true);

                AdminEvent.OnShowNameChanged(this);

                Atx.ResetCurrentShowRecord();

                litShowTitle.Text = Atx.CurrentShowRecord.Name;
            }
        }
}
}