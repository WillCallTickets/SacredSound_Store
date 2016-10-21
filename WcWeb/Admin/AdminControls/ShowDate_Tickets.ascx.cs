using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class ShowDate_Tickets : BaseControl, IPostBackEventHandler
    {
        List<string> _errors = new List<string>();

        #region PostBack Handling for ticket links

        public void RaisePostBackEvent(string eventArgument)
        {
            List<string> parts = new List<string>();
            parts.AddRange(eventArgument.ToLower().Split('~'));

            if (parts.Count > 0)
            {
                string cmd = parts[0].ToLower();

                switch (cmd)
                {
                    case "setcampaign":
                        int campaignId = int.Parse(parts[1]);
                        Atx.SetCurrentAccessCampaign(campaignId);
                        base.Redirect("/Admin/ProductAccess.aspx?p=campaign");
                        break;
                    case "linkticket":
                        int showId = int.Parse(parts[1]);
                        int dateId = int.Parse(parts[2]);
                        int tickId = int.Parse(parts[3]);

                        //if we are in the same show.....
                        if (showId == Atx.CurrentShowRecord.Id)
                        {
                            //here we do nothing
                        }
                        else //we are linked to a different show
                        {
                            //change the current show
                            Atx.SetCurrentShowRecord(showId);

                            //if there is a showdate selected - then we should choose that selection
                            GridView1.DataBind();

                            if(dateId == 0)
                                GridView1.SelectedIndex = 0;
                            else
                            {
                                GridView1.SelectedIndex = -1;
                                foreach(GridViewRow gvr in GridView1.Rows)
                                {
                                    if (dateId == (int)GridView1.DataKeys[gvr.DataItemIndex]["Id"])
                                    {
                                        GridView1.SelectedIndex = GridView1.Rows[0].DataItemIndex;
                                        break;
                                    }
                                }
                            }
                        }

                        //Date Grid
                        if (GridView1.SelectedValue != null && dateId != (int)GridView1.SelectedValue)
                        {
                            GridView1.SelectedIndex = -1;
                            foreach (GridViewRow gvr in GridView1.Rows)
                            {
                                int rowId = (int)GridView1.DataKeys[gvr.RowIndex].Values["Id"];
                                if (rowId == dateId)
                                {
                                    GridView1.SelectedIndex = gvr.RowIndex;
                                    break;
                                }
                            }

                            GridView1.DataBind();
                        }

                        //this is always different
                        GridViewEntity.DataBind();

                        //is next grid set yet?
                        GridViewEntity.SelectedIndex = -1;
                        foreach (GridViewRow gvr in GridViewEntity.Rows)
                        {
                            int rowId = (int)GridViewEntity.DataKeys[gvr.RowIndex].Values["Id"];
                            if (rowId == tickId)
                            {
                                GridViewEntity.SelectedIndex = gvr.RowIndex;
                                break;
                            }
                        }

                        litShowTitle.Text = Atx.CurrentShowRecord.Name;

                        break;
                }
            }

            //rebind?
        }

        #endregion

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

        protected ShowTicket CreateDosTicket(int parentId, decimal dosPrice)
        {
            //create a new ticket and transfer all info except for allotment
            //allotment has to be done manually - by a button or by a process
            try
            {
                ShowTicket parent = ShowTicket.FetchByID(parentId);
                
                if (parent != null)
                {
                    //be sure that there isnot alreadfy a dos ticket for this ticket
                    if(parent.DosShowTicketRecord != null)
                        throw new Exception("Dos Ticket already exists for this ticket!");

                    System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);

                    //zero price indicates using parent price
                    ShowTicket dosTicket = ShowTicket.CreateSingleTicket(parent.VendorRecord, parent.ShowDateRecord, 
                        string.Empty, (dosPrice == 0) ? parent.Price : dosPrice, true, string.Empty,
                        0, parent.SalesDescription_Derived, parent.CriteriaText_Derived, parent.TAgeId, 
                        false, mem.UserName, (Guid)mem.ProviderUserKey);

                    //create the link
                    ShowTicketDosTicket stdt = new ShowTicketDosTicket();
                    stdt.DtStamp = DateTime.Now;
                    stdt.ParentId = parent.Id;
                    stdt.DosId = dosTicket.Id;
                    stdt.Save();

                    //keep the unlock codes in sync
                    dosTicket.UnlockCode = parent.UnlockCode;
                    dosTicket.ShowDateRecord.ShowTicketRecords().SaveAll();

                    string val = string.Format("DOS: {0} + {1} - {2}: {3}", dosTicket.Price.ToString("c"), dosTicket.ServiceCharge.ToString("n2"), dosTicket.AgeDescription, dosTicket.CriteriaText_Derived);

                    EventQ.LogEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, mem.UserName, (Guid)mem.ProviderUserKey, null, 
                        _Enums.EventQContext.ShowDate, _Enums.EventQVerb._TicketAdded, val, 
                        string.Format("ShowDate: {0} - {1}", dosTicket.ShowDateRecord.DateOfShow.ToString(), dosTicket.TShowDateId.ToString()), dosTicket.ShowDateRecord.ShowRecord.Name);

                    return dosTicket;          
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidator validation = (CustomValidator)FormView1.FindControl("CustomValidation");
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }
            }

            return null;
        }

        protected void btnDos_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            int idx = int.Parse(btn.CommandArgument);
            string cmd = btn.CommandName;

            if (cmd.ToLower() == "create")
            {
                ShowTicket st = CreateDosTicket(idx, 0);

                if (st != null)
                {
                    //set current id to new ticket and rebind controls
                    //ensure Show has latest data
                    int index = Atx.CurrentShowRecord.Id;
                    Atx.SetCurrentShowRecord(index);//refresh data

                    GridViewEntity.SelectedIndex = st.DisplayOrder;
                    GridViewEntity.DataBind();
                }
            }
            else
            {
                //change view to the chosen ticket
                ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                if (selectedDate != null)
                {
                    ShowTicket selectedTicket = selectedDate.ShowTicketRecords().GetList().Find(delegate(ShowTicket match) { return (match.Id == idx); });
                    ////set the index of the moved item
                    if (selectedTicket != null)
                    {
                        //decide index to go to based on is parent/dos
                        GridViewEntity.SelectedIndex = (selectedTicket.IsDosTicket) ? selectedTicket.ParentShowTicketRecord.DisplayOrder : selectedTicket.DosShowTicketRecord.DisplayOrder;
                        GridViewEntity.DataBind();
                    }
                }
            }
        }

        #region GridView1 - Show Date Listing

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            GridViewEntity.DataBind();
        }

        #endregion

        #region GridViewEntity - JShowTicketListing

        protected void GridViewEntity_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);

            if (rowView != null)
            {
                DataRow row = rowView.Row;

                LinkButton select = (LinkButton)e.Row.FindControl("btnSelect");
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");

                CheckBox pkg = (CheckBox)e.Row.FindControl("chkPackage");
                int linkCount = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("LinkCount"));
                pkg.Checked = linkCount > 0;

                Literal vendor = (Literal)e.Row.FindControl("litVendor");
                int vendorId = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("TVendorId"));
                
                Vendor v = (Vendor)_Lookits.Vendors.Find(vendorId);
                if (vendor != null && v != null)
                    vendor.Text = v.Name;

                Literal litAvailable = (Literal)e.Row.FindControl("litAvailable");
                if (litAvailable != null)
                {
                    int avail = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iAllotment")) -
                        (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("pendingStock")) -
                        (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("iSold"));
                    litAvailable.Text = avail.ToString();
                }


                string theName = string.Empty;

                //ent.[PriceText], ent.[mPrice], ent.[DosText], ent.[mDosPrice], ent.[mServiceCharge], a.[Name] as 'AgeName', ent.[CriteriaText],
                if (row != null)
                {
                    string criteria = row.ItemArray.GetValue(row.Table.Columns.IndexOf("CriteriaText")).ToString();
                    string description = row.ItemArray.GetValue(row.Table.Columns.IndexOf("SalesDescription")).ToString();

                    //if(criteria != null && criteria.Trim().Length > 75)
                    //    criteria = string.Format("{0}...", criteria.Trim().Substring(0,72).Trim());
                    //if (description != null && description.Trim().Length > 75)
                    //    description = string.Format("{0}...", description.Trim().Substring(0, 72).Trim());

                    theName = System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} svc {2}<div>Ages {3}</div>{4}{5}",
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("PriceText")),
                        ((decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("mPrice"))).ToString("c"),
                        ((decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("mServiceCharge"))).ToString("n2"),
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("AgeName")),
                        (description != null && description.Trim().Length > 0) ? string.Format("<div>{0}</div>", description.Trim()) : string.Empty,
                        (criteria != null && criteria.Trim().Length > 0) ? string.Format("<div>{0}</div>", criteria.Trim()) : string.Empty), 
                        @"\s+", " ").Trim();
                }

                Literal litDesc = (Literal)e.Row.FindControl("litDesc");
                if (litDesc != null && theName.Trim().Length > 0)
                    litDesc.Text = theName;

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0} from this show?')",
                        Utils.ParseHelper.ParseJsAlert(theName));

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);

                    int rowCount = (selectedDate != null) ? selectedDate.ShowTicketRecords().Count : 0;

                    down.Enabled = (e.Row.RowIndex < rowCount - 1);
                }
            }
        }
        protected void GridViewEntity_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }
        protected void GridViewEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                    if (selectedDate != null)
                    {
                        ShowTicket moved = selectedDate.ShowTicketRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                        ////set the index of the moved item
                        grid.SelectedIndex = moved.DisplayOrder;
                        grid.DataBind();
                    }
                    break;
            }

            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);
        }
        protected void GridViewEntity_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;

            int idx = (int)grid.DataKeys[e.RowIndex].Value;

            try
            {
                ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
           
                e.Cancel = ((selectedDate == null) || (!selectedDate.ShowTicketRecords().DeleteFromCollection(idx)));

                int index = Atx.CurrentShowRecord.Id;

                //reset show data
                Atx.SetCurrentShowRecord(index);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                CustomValidator validation = (CustomValidator)grid.Rows[e.RowIndex].FindControl("CustomValidation");
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

        #endregion

        #region FormView1

        /// <summary>
        /// This will only copy tickets from other show dates
        /// </summary>
        protected void ddlCopy_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            ShowTicketCollection coll = new ShowTicketCollection();
            ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);

            if (selectedDate != null)
            {
                foreach (ShowDate sd in Atx.CurrentShowRecord.ShowDateRecords())
                {
                    if (sd.Id != selectedDate.Id)
                        coll.AddRange(sd.ShowTicketRecords().GetList().FindAll(delegate(ShowTicket match) { return ((! match.IsPackage) && (! match.IsDosTicket)); }));
                }
            }

            ddl.DataSource = coll;
            ddl.DataTextField = "CopyListing";
            ddl.DataValueField = "Id";
        }
        protected void ddlCopy_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.Enabled = (ddl.Items.Count > 1);//allow for header item

            Button btn = (Button)FormView1.FindControl("btnCopy");
            if (btn != null)
                btn.Enabled = ddl.Enabled;
        }

        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            
            int selectedDateId = (GridView1.SelectedValue != null) ? int.Parse(GridView1.SelectedValue.ToString()) : 0;
            ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(selectedDateId);

            DataRowView rowView = (DataRowView)form.DataItem;
            DataRow row = (rowView != null) ? rowView.Row : null;
            bool isParent = (row != null) ? (bool)row.ItemArray.GetValue(row.Table.Columns.IndexOf("bDosTicket")) : true;

            if (form.CurrentMode == FormViewMode.Insert)
            {
                //do packages
                DropDownList selection = (DropDownList)form.FindControl("ddlDateList");

                if (selection != null && selection.Items.Count == 1)//allow for selection value
                {
                    //get a list of showdates
                    ShowDateCollection dates = new ShowDateCollection();
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT sd.* FROM [ShowDate] sd, [Show] s WHERE s.[ApplicationId]= @appId AND sd.[TShowId] = s.[Id] AND sd.[dtDateOfShow] >= (getDate()) ", 
                        SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);
                    dates.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

                    //dont include current date - will be automatically added
                    dates.GetList().RemoveAll(delegate(ShowDate match) { return (selectedDateId == match.Id); });
                    if (dates.Count > 1)
                        dates.Sort("DtDateOfShow", true);
                    selection.DataSource = dates;
                    selection.AppendDataBoundItems = true;
                    selection.DataTextField = "ListingString";
                    selection.DataValueField = "Id";
                    selection.DataBind();
                }
                //end packages
            }

            //VENDOR
            DropDownList ddlVendor = (DropDownList)form.FindControl("ddlVendor");

            if (ddlVendor != null)
            {
                if (ddlVendor.Items.Count == 0)
                {
                    Vendor online =  _Lookits.Vendors.GetVendor_Online();
                    ddlVendor.Items.Add(new ListItem(online.Name, online.Id.ToString()));
                }

                ddlVendor.DataTextField = "Text";
                ddlVendor.DataValueField = "Value";
                ddlVendor.DataBind();

                ddlVendor.SelectedIndex = -1;

                int vidx = (row == null) ? _Lookits.Vendors.GetVendor_Online().Id : 
                    (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("TVendorId"));

                ListItem itm = ddlVendor.Items.FindByValue(vidx.ToString());

                if (itm != null)
                    itm.Selected = true;
                else
                    ddlVendor.SelectedIndex = 0;

                ddlVendor.Enabled = (!_Config._AllSales_OnlineOnly && isParent);
            }

            //fill ages drop down
            DropDownList ddlAges = (DropDownList)form.FindControl("ddlAges");
            if (ddlAges != null)
            {
                //if (ddlAges.Items.Count == 0)
                ddlAges.DataSource = _Lookits.Ages;

                ddlAges.DataTextField = "Name";
                ddlAges.DataValueField = "Id";
                ddlAges.DataBind();

                ddlAges.SelectedIndex = -1;
                int ageId = (row != null) ? (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("TAgeId")) : 0;
                if (ageId > 0)
                    ddlAges.Items.FindByValue(ageId.ToString()).Selected = true;
                else 
                {
                    if (selectedDate != null)
                    {
                        ListItem li = ddlAges.Items.FindByValue(selectedDate.AgeRecord.Id.ToString());
                        if (li != null)
                            li.Selected = true;
                    }
                }
            }

            CheckBox chkHideShipMethod = (CheckBox)form.FindControl("chkHideShipMethod");
            if (chkHideShipMethod != null)
                chkHideShipMethod.Enabled = _Config._Allow_HideShipMethod;

            if (selectedDate != null && form.CurrentMode == FormViewMode.Edit)
            {
                ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find(form.SelectedValue);
                bool isDos = (row != null) && (bool)row.ItemArray.GetValue(row.Table.Columns.IndexOf("bDosTicket"));

                if(isDos)
                {
                    ((TextBox)form.FindControl("txtCode")).Enabled = false;
                    ((Button)form.FindControl("btnGenerate")).Enabled = false;
                }

                CheckBox chkAllowShip = (CheckBox)form.FindControl("chkAllowShip");
                CheckBox chkAllowWillCall = (CheckBox)form.FindControl("chkAllowWillCall");
                if (chkAllowShip != null && chkAllowWillCall != null)
                {
                    chkAllowShip.Enabled = (_Config._Shipping_Tickets_Active) && (!isDos);
                    chkAllowWillCall.Enabled = (_Config._Shipping_Tickets_Active) && (!isDos);
                }

                Button dos = (Button)form.FindControl("btnDos");
                if (dos != null)
                {
                    //parent tickets without DOS tickets
                    bool hasDos = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("DosId")) > 0;
                    bool isPackage = (int)row.ItemArray.GetValue(row.Table.Columns.IndexOf("LinkCount")) > 0;

                    dos.Enabled = (!isPackage);

                    if(dos.Enabled)
                    {
                        if (isDos && selectedTicket != null)
                        {
                            dos.Text = "View Parent Ticket";
                            dos.CommandName = "View";    
                            dos.ToolTip = "View the parent ticket associated with this ticket.";
                            dos.CommandArgument = selectedTicket.Id.ToString();
                        }
                        else if (hasDos && selectedTicket != null)
                        {
                            dos.Text = "View DOS Ticket";
                            dos.CommandName = "View";     
                            dos.ToolTip = "View the DOS ticket associated with this ticket";
                            dos.CommandArgument = selectedTicket.Id.ToString();
                        }
                    }

                    //cant create a dos for a pkg ticket
                    //need to be allowed will call for dos ticket
                    //dos.Enabled = (!isDos) && (!hasDos) && (!isPackage);                    
                }

                Literal litDos = (Literal)form.FindControl("litDos");
                if(litDos != null)
                    litDos.Visible = isDos;
                

                //set clocks to default of AM
                if (selectedTicket != null)
                {
                    if (selectedTicket.PublicOnsaleDate == Utils.Constants._MinDate)
                    {
                        WillCallWeb.Components.Util.CalendarClock clock = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockOnsale");
                        if (clock != null)
                        {
                            clock.AMPM.SelectedIndex = -1;
                            ListItem li = clock.AMPM.Items.FindByText("AM");
                            if (li != null)
                                li.Selected = true;
                        }
                    }
                    if (selectedTicket.UnlockDate == Utils.Constants._MinDate)
                    {
                        WillCallWeb.Components.Util.CalendarClock clock = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockUnlockStart");
                        if (clock != null)
                        {
                            clock.AMPM.SelectedIndex = -1;
                            ListItem li = clock.AMPM.Items.FindByText("AM");
                            if (li != null)
                                li.Selected = true;
                        }
                    }
                }

                //dipslay a list of product access campaigns
                //use form commands to set the current campaign when clicked for better navigation
                Literal litProductAccess = (Literal)form.FindControl("litProductAccess");
                if (selectedTicket != null && litProductAccess != null)
                {
                    int tixIdx = selectedTicket.Id;

                    ProductAccessCollection paColl = new ProductAccessCollection();
                    //List<ProductAccess> paColl = new List<ProductAccess>();

                    string sql = "SELECT	pa.* ";
                    sql += "FROM	[ProductAccess] pa, [ProductAccessProduct] pap ";
                    sql += "WHERE	pap.[vcContext] = 'ticket' AND pap.[TParentId] = @idx AND pa.[Id] = pap.[TProductAccessId] ";
                    
                    Wcss._DatabaseCommandHelper cmd = new Wcss._DatabaseCommandHelper(sql);
                    cmd.AddCmdParameter("idx", tixIdx, DbType.Int32);
                    cmd.PopulateCollectionByReader<ProductAccessCollection>(paColl);
                    
                    foreach (ProductAccess pa in paColl)
                    {
                        string href = Page.ClientScript.GetPostBackClientHyperlink(this, string.Format("setcampaign~{0}", pa.Id)).Replace("'", "&#39;");
                        litProductAccess.Text += string.Format("<li style=\"padding-left:12px;font-size:14px;\"><a href=\"{0}\">&raquo; {1}</a></li>", href, pa.CampaignName);
                    }
                }

                Literal link = (Literal)form.FindControl("litLink");

                if (selectedTicket != null && selectedTicket.IsPackage && link != null)
                {
                    foreach (ShowTicket st in selectedTicket.LinkedShowTickets)
                    {
                        string href = Page.ClientScript.GetPostBackEventReference(this, string.Format("linkticket~{0}~{1}~{2}",
                            st.TShowId, st.TShowDateId, st.Id)).Replace("'", "&#39;");

                        link.Text += string.Format("<a class=\"btnadmin\" href=\"javascript: {0}\">{1}</a>", href, st.ShowDateRecord.ListingString);
                    }
                }

                Literal litLink = (Literal)form.FindControl("litUnlockLink");
                if (row != null && litLink != null && selectedTicket != null && selectedTicket.IsUnlockActive)
                {
                    string linkage = string.Format("http://{0}/Store/ChooseTicket.aspx?sid={1}", Request.Url.Host, selectedTicket.TShowId);

                    litLink.Text = string.Format("<a href=\"{0}&mp={1}\" target=\"_blank\">{0}&mp={1}</a>", linkage, selectedTicket.UnlockCode);
                }

                Button lot = (Button)FormView1.FindControl("btnLottery");
                if (lot != null)
                {
                    if (selectedTicket != null && selectedTicket.IsLotteryTicket)
                        lot.Text = "Remove Lottery";
                    else
                        lot.Text = "Convert To Lottery";
                }

                FormLottery.DataBind();
            }
        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {     
                case "bundle":
                    base.Redirect(string.Format("/Admin/ShowEditor.aspx?p=bundle&tixid={0}", e.CommandArgument.ToString()));
                    break;
                case "viewsales":
                    base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&tixid={0}", e.CommandArgument.ToString()));
                    break;
                case "lottery":
                    //get current ticket
                    ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);

                    if (selectedDate != null)
                    {
                        ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find(form.SelectedValue);

                        if (selectedTicket != null)
                        {
                            if (selectedTicket.IsLotteryTicket)
                            {
                                selectedTicket.LotteryRecords().Clear();
                            }
                            else
                            {
                                selectedTicket.LotteryRecords().Clear();

                                Lottery lot = new Lottery();
                                lot.DtStamp = DateTime.Now;
                                lot.TShowTicketId = selectedTicket.Id;
                                lot.TShowDateId = selectedTicket.TShowDateId;
                                lot.TShowId = selectedTicket.TShowId;
                                lot.IsActive_Signup = true;
                                lot.IsActive_Fulfillment = true;
                                lot.EstablishQty = 0;

                                lot.Save();
                                selectedTicket.LotteryRecords().Add(lot);
                            }
                        }
                    }

                    GridViewEntity.DataBind();
                    break;
                case "generatecode":
                    string qry = "UPDATE ShowTicket SET UnlockCode = @newCode WHERE [ID] = @idx ";
                    qry += "IF EXISTS(SELECT * FROM ShowTicketDosTicket WHERE [ParentId] = @idx) BEGIN UPDATE ShowTicket SET UnlockCode = @newCode ";
                    qry+= "FROM [ShowTicket] st, [ShowTicketDosTicket] dos WHERE dos.[ParentId] = @idx AND dos.[DosId] = st.[Id] END";
  
                    SubSonic.QueryCommand com = new SubSonic.QueryCommand(qry, SubSonic.DataService.Provider.Name);
                    com.Parameters.Add("@idx", int.Parse(e.CommandArgument.ToString()), DbType.Int32);
                    string newCode = Utils.ParseHelper.GenerateRandomPassword(7);
                    com.Parameters.Add("@newCode", newCode);
                    SubSonic.DataService.ExecuteQuery(com);

                    ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                    if (sd != null)
                    {
                        ShowTicket st = (ShowTicket)sd.ShowTicketRecords().Find(int.Parse(e.CommandArgument.ToString()));
                        if (st != null)
                            st.UnlockCode = newCode;
                    }

                    GridViewEntity.DataBind();
                    break;

                case "addpackage":
                    ListBox listing = (ListBox)form.FindControl("listAddedPackages");
                    DropDownList selection = (DropDownList)form.FindControl("ddlDateList");
                    if (listing != null && selection != null && selection.SelectedIndex > 0)
                    {
                        ListItem chosen = selection.SelectedItem;

                        foreach (ListItem existing in listing.Items)//return if the date is already in the list
                            if (chosen.Value == existing.Value)
                                return;

                        listing.Items.Add(chosen);
                        listing.SelectedIndex = -1;
                    }
                    break;
                case "removepackage":
                    ListBox list = (ListBox)form.FindControl("listAddedPackages");
                    if (list != null && list.SelectedIndex > -1)
                    {
                        list.Items.Remove(list.SelectedItem);
                        list.SelectedIndex = -1;
                    }
                    break;
                case "copy":
                    //ensure valid input
                    DropDownList ddlCopy = (DropDownList)form.FindControl("ddlCopyTicket");
                    if (ddlCopy != null)//aslo ensures that we are in insert mode
                    {
                        int idx = int.Parse(ddlCopy.SelectedValue);

                        try
                        {
                            if (idx == 0)
                                throw new Exception("You must select a ticket to copy in order to copy.");

                            //get ticket chosen
                            ShowTicket chosen = ShowTicket.FetchByID(idx);

                            if(chosen == null)
                                throw new Exception("Sorry, that ticket could not be found.");

                            //copy ticket info and add to this date
                            ShowDate selDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                            if (selDate != null)
                            {
                                System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);

                                ShowTicket copiedTicket = chosen.CopyShowTicketComplete(selDate, mem.UserName, (Guid)mem.ProviderUserKey);

                                form.ChangeMode(FormViewMode.Edit);

                                //ensure Show has latest data - refresh
                                int index = Atx.CurrentShowRecord.Id;
                                Atx.SetCurrentShowRecord(index);

                                GridViewEntity.SelectedIndex = copiedTicket.DisplayOrder;
                                GridViewEntity.DataBind();
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
                            }
                        }
                    }
                    break;
                case "cancel":
                    break;
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
            _errors.Clear();

            try
            {
                ShowTicket st;

                //TODO: allow specification of DosTicket

                //TextBox txtPriceText = (TextBox)form.FindControl("txtPriceText");
                TextBox txtPrice = (TextBox)form.FindControl("txtPrice");
                if (txtPrice != null)
                {
                    string input = txtPrice.Text.Trim();
                    Utils.Validation.ValidateRequiredField(_errors, "Price", input);
                    Utils.Validation.ValidateNumericField(_errors, "Price", input);
                }

                DropDownList ddlAges = (DropDownList)form.FindControl("ddlAges"); 
                TextBox txtCriteria = (TextBox)form.FindControl("txtCriteria"); 
                TextBox txtDescription = (TextBox)form.FindControl("txtDescription");
                ListBox dateList = (ListBox)form.FindControl("listAddedPackages");

                bool IsPackage = dateList.Items.Count > 0;
                bool createDOS = false;
                
                CheckBox chkDos = (CheckBox)form.FindControl("chkDosTicket");
                TextBox txtDosPrice = (TextBox)form.FindControl("txtDosPrice");

                decimal dosPrice = 0;

                if (chkDos.Checked)
                {
                    //cannot be a pkg ticket
                    if(IsPackage)
                        _errors.Add("DOS tickets cannot be created for package tickets");

                    if (txtDosPrice != null)
                    {
                        string input = txtDosPrice.Text.Trim();
                        Utils.Validation.ValidateRequiredField(_errors, "DOS Price", input);
                        Utils.Validation.ValidateNumericField(_errors, "DOS Price", input);
                    }

                    if (_errors.Count == 0)
                    {
                        dosPrice = decimal.Parse(txtDosPrice.Text.Trim());

                        createDOS = true;
                    }
                }


                ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridView1.SelectedValue);
                if (selectedDate == null)
                    _errors.Add("Selected date is null");


                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
                
                decimal price = decimal.Round(decimal.Parse(txtPrice.Text.Trim()), 2);
                int ageIdx = int.Parse(ddlAges.SelectedValue);

                
                //vendors
                Vendor chosenVendor = null;
                DropDownList ddlVendor = (DropDownList)form.FindControl("ddlVendor");
                if (ddlVendor != null)
                    chosenVendor = (Vendor)_Lookits.Vendors.Find(int.Parse(ddlVendor.SelectedValue));
                if (chosenVendor == null || _Config._AllSales_OnlineOnly)
                    chosenVendor = _Lookits.Vendors.GetVendor_Online();

                System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);

                if (IsPackage)
                    //create an active ticketpage for each show involved
                    st = ShowTicket.CreateTicketPackage(chosenVendor, selectedDate, dateList.Items, string.Empty, price, false, 
                        txtDescription.Text.Trim(), txtCriteria.Text.Trim(), ageIdx, false, mem.UserName, (Guid)mem.ProviderUserKey);
                else
                    st = ShowTicket.CreateSingleTicket(chosenVendor, selectedDate, string.Empty, price, false, string.Empty, 
                        0, txtDescription.Text.Trim(), txtCriteria.Text.Trim(), ageIdx, false, mem.UserName, (Guid)mem.ProviderUserKey);

                if (createDOS)
                {
                    //although this returns a DOS ticket - we can ignore becuase we will default to the parents info/editor
                    CreateDosTicket(st.Id, dosPrice);
                }

                form.ChangeMode(FormViewMode.Edit);

                //ensure Show has latest data
                int index = Atx.CurrentShowRecord.Id;
                Atx.SetCurrentShowRecord(index);

                GridViewEntity.SelectedIndex = st.DisplayOrder;
                GridViewEntity.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                }
            }
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();

            if (GridView1.SelectedDataKey["Id"] == null || form.SelectedValue == null)
                return;

            ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find((int)GridView1.SelectedDataKey["Id"]);
            if (sd == null)
                throw new Exception("Show Date is null");
            ShowTicket st = (ShowTicket)sd.ShowTicketRecords().Find((int)form.SelectedValue);
            CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");

            if (st != null)
            {                
                bool isParent = (!st.IsDosTicket);

                #region Deal with activation and deactivation

                bool oldActive = (bool)e.OldValues["bActive"];
                bool newActive = (bool)e.NewValues["bActive"];                
                //
                if((!oldActive) && newActive)//Activating
                {
                    if (st != null && st.IsPackage)
                    {
                        if ((!st.ShowDateRecord.ShowRecord.IsActive) || st.ShowDateRecord.ShowRecord.IsSoldOut)
                            _errors.Add("The show group is marked as inactive or sold out. You must put the show group and dates back on sale before activating this ticket");
                        else if ((!st.ShowDateRecord.IsActive) || (st.ShowDateRecord.ShowStatusRecord.Name != Wcss._Enums.ShowDateStatus.OnSale.ToString()))
                            _errors.Add("The show date is marked as inactive or not on sale. You must put the date back on sale before activating this ticket");

                        foreach (ShowTicketPackageLink linked in st.ShowTicketPackageLinkRecords())
                        {
                            if (linked.ShowTicketRecord.ShowDateRecord.ShowRecord.IsSoldOut)
                            {
                                _errors.Add("There are linked show group(s) marked as inactive or sold out. You must put the linked show group(s) and linked date(s) back on sale before activating this ticket");
                                break;
                            }
                            else if (linked.ShowTicketRecord.ShowDateRecord.IsSoldOut)
                            {
                                _errors.Add("There are linked show date(s) marked as inactive or not on sale. You must put the linked date(s) back on sale before activating this ticket");
                                break;
                            }
                        }

                        if (Utils.Validation.IncurredErrors(_errors, validation))
                        {
                            CheckBox chkActive = (CheckBox)form.FindControl("chkActive");

                            if (chkActive != null)
                                chkActive.Checked = false;

                            e.Cancel = true;
                            return;
                        }
                        
                    }
                }
                #endregion

                TextBox txtPrice = (TextBox)form.FindControl("txtPrice");
                if (txtPrice != null)
                {
                    string input = txtPrice.Text.Trim();
                    Utils.Validation.ValidateRequiredField(_errors, "Price", input);
                    Utils.Validation.ValidateNumericField(_errors, "Price", input);
                }
                TextBox txtService = (TextBox)form.FindControl("txtService");
                if (txtService != null)
                {
                    string input = txtService.Text.Trim();
                    Utils.Validation.ValidateRequiredField(_errors, "Service Fee", input);
                    Utils.Validation.ValidateNumericField(_errors, "Service Fee", input);
                }
                TextBox txtAllot = (TextBox)form.FindControl("txtAllotment");
                if (txtAllot != null)
                {
                    string input = txtAllot.Text.Trim();
                    Utils.Validation.ValidateIntegerField(_errors, "Allotment", input);
                    Utils.Validation.ValidateIntegerRange(_errors, "Allotment", input, 0, 2500);
                }
                TextBox txtMaxPer = (TextBox)form.FindControl("txtMaxPer");
                if (txtMaxPer != null)
                {
                    string input = txtMaxPer.Text.Trim();
                    Utils.Validation.ValidateIntegerField(_errors, "Max Per Order", input);
                    Utils.Validation.ValidateIntegerRange(_errors, "Max Per Order", input, 0, 50);
                }

                //handle shipping changes
                //when specifying a flat method for shipping - allow shipping must be enabled and willcalloptions
                object o1 = e.NewValues["bShipSeparate"];
                object o2 = e.NewValues["bAllowShipping"];
                
                if (o1 != null && o2 != null)
                {
                    if ((bool)o1 == true)
                    {
                        //we must have a ship method specified!!!
                        object o3 = e.NewValues["vcFlatMethod"];
                        if (o3 == null || o3.ToString().Trim().Length == 0)
                            _errors.Add("Flat shipping method must be specified when using ship separate.");


                        e.NewValues["bAllowShipping"] = true;
                    }
                }


                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
                
                if (txtAllot != null)
                {
                    try
                    {
                        int oldAllot = int.Parse(e.OldValues["iAllotment"].ToString());
                        int newAllot = int.Parse(e.NewValues["iAllotment"].ToString());

                        //#w
                        //string sql = "UPDATE [ShowTicket] SET [iAllotment] = @allotment FROM [ShowTicket] st WHERE st.[Id] = @idx AND @allotment >= (st.[--ending] + st.[iSold]); ";
                        string sql = "DECLARE @pending int; CREATE TABLE #addTickets ( idx int ); INSERT #addTickets(idx) SELECT @idx as 'idx'; ";
                        sql += "INSERT #addTickets(idx) SELECT DISTINCT link.[LinkedShowTicketId] as 'idx' FROM [ShowTicketPackageLink] link, [ShowTicket] st ";
                        sql += "WHERE @idx = st.[Id] AND link.[ParentShowTicketId] = st.[Id]; ";
                        sql += "SELECT @pending = ISNULL(SUM([iQty]),0) FROM [TicketStock] WHERE [tShowTicketId] IN (SELECT [idx] FROM #addTickets) ";

                        sql += "UPDATE [ShowTicket] SET [iAllotment] = @allotment FROM [ShowTicket] st WHERE st.[Id] = @idx AND @allotment >= (@pending + st.[iSold]); ";
                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@idx", st.Id, DbType.Int32);
                        cmd.Parameters.Add("@allotment", newAllot, DbType.Int32);

                        //successful if we have affected rows
                        int rows = SubSonic.DataService.ExecuteQuery(cmd);

                        //if not successful - raise exception
                        if (rows == 0)
                        {
                            if (validation != null)
                            {
                                validation.IsValid = false;
                                validation.ErrorMessage = "Allotment cannot be set to less than the total of pending and sold tickets. Please note that all other changes have been discarded.";
                            }

                            e.Cancel = true;
                            form.DataBind();

                            return;
                        }
                        
                    }
                    catch(Exception ex)
                    {
                        if (validation != null)
                        {
                            validation.IsValid = false;
                            validation.ErrorMessage = ex.Message;
                        }

                        e.Cancel = true;
                        
                        return;
                    }
                }

                DropDownList age = (DropDownList)form.FindControl("ddlAges");
                e.NewValues["TAgeId"] = int.Parse(age.SelectedValue);

                bool unlockActive = bool.Parse(e.NewValues["bUnlockActive"].ToString());
                //if we are making unlock active - there must be a code
                string code = e.NewValues["UnlockCode"].ToString();                    
                if (unlockActive && code != null && (code.Trim().Length == 0 || code.Trim().Length < 4))
                    e.NewValues["UnlockCode"] = Utils.ParseHelper.GenerateRandomPassword(7);

                WillCallWeb.Components.Util.CalendarClock onsale = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockOnsale");
                WillCallWeb.Components.Util.CalendarClock offsale = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockOffsale");
                WillCallWeb.Components.Util.CalendarClock cutoff = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockCutoff");
                WillCallWeb.Components.Util.CalendarClock unlockStart = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockUnlockStart");
                WillCallWeb.Components.Util.CalendarClock unlockEnd = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockUnlockEnd");

                //PUBLIC ONsale must be specified and be later than unlock start
                string unlockError = string.Empty;
                if (unlockActive && (!onsale.HasSelection))
                {
                    if (validation != null)
                    {
                        validation.IsValid = false;
                        validation.ErrorMessage = "A public onsale date must be specified for a coded ticket.";
                    }
                    e.Cancel = true;
                    return;
                }
                else if (unlockActive && unlockStart.HasSelection)
                {
                    DateTime goPublic = onsale.SelectedDate;
                    DateTime beginCodeSales = unlockStart.SelectedDate;

                    if (beginCodeSales > goPublic)
                    {
                        if (validation != null)
                        {
                            validation.IsValid = false;
                            validation.ErrorMessage = "The starting code date must precede the public onsale date.";
                        }
                        e.Cancel = true;
                        return;
                    }
                }

                e.NewValues["dtPublicOnsale"] = (isParent && onsale.HasSelection) ? onsale.SelectedDate.ToString("MM/dd/yyyy hh:mm tt") : string.Empty;
                e.NewValues["dtEndDate"] = (offsale.HasSelection) ? offsale.SelectedDate.ToString("MM/dd/yyyy hh:mm tt") : string.Empty;
                e.NewValues["dtShipCutoff"] = (isParent && cutoff.HasSelection) ? cutoff.SelectedDate.ToString("MM/dd/yyyy hh:mm tt") : Utils.Constants._MinDate.ToString("MM/dd/yyyy hh:mm tt");
                e.NewValues["dtUnlockDate"] = (unlockStart.HasSelection) ? unlockStart.SelectedDate.ToString("MM/dd/yyyy hh:mm tt") : string.Empty;
                e.NewValues["dtUnlockEndDate"] = (unlockEnd.HasSelection) ? unlockEnd.SelectedDate.ToString("MM/dd/yyyy hh:mm tt") : string.Empty;


                Vendor chosenVendor = null;
                DropDownList ddlVendor = (DropDownList)form.FindControl("ddlVendor");
                if (ddlVendor != null)
                    chosenVendor = (Vendor)_Lookits.Vendors.Find(int.Parse(ddlVendor.SelectedValue));
                if (chosenVendor == null || _Config._AllSales_OnlineOnly)
                    chosenVendor = _Lookits.Vendors.GetVendor_Online();

                if(isParent)
                    e.NewValues["TVendorId"] = chosenVendor.Id;
                else
                    e.NewValues["TVendorId"] = st.TVendorId;



                e.NewValues["mFlatShip"] = 0;
                e.NewValues["dtBackorder"] = null;
            }
        }
        protected void SqlEntity_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            GridView1.DataBind();
        }
        protected void SqlEntity_Updating(object sender, SqlDataSourceCommandEventArgs e)
        {   
            //to allow hideshipmethod - it must be will call only
            bool allowShip = (bool)e.Command.Parameters["@bAllowShipping"].Value;
            bool allowWc = (bool)e.Command.Parameters["@bAllowWillCall"].Value;
            bool hideShip = (bool)e.Command.Parameters["@bHideShipMethod"].Value;

            if (hideShip && ((!allowWc) || allowShip))
            {
                throw new Exception("To hide shipping, no ship method can be selected and will call must be specified.");
            }
        }
        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                CustomValidator validation = (CustomValidator)FormView1.FindControl("CustomValidation");
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = e.Exception.Message;// "The starting code date must precede the public onsale date.";
                }
                e.ExceptionHandled = true;
                e.KeepInEditMode = true;

                return;
            }

            FormView form = (FormView)sender;

            //reset show data
            int index = (int)form.SelectedValue;

            //if allotment or price changed - record history
            if (e.OldValues["mPrice"].ToString() != e.NewValues["mPrice"].ToString())
            {
                HistoryPricing hist = new HistoryPricing();
                hist.DtStamp = DateTime.Now;
                System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(System.Web.HttpContext.Current.Profile.UserName);
                hist.UserId = (Guid)mem.ProviderUserKey;
                hist.TShowTicketId = (int)form.SelectedValue;
                hist.DateAdjusted = DateTime.Now;
                hist.OldPrice = decimal.Parse(e.OldValues["mPrice"].ToString());
                hist.NewPrice = decimal.Parse(e.NewValues["mPrice"].ToString());
                hist.Context = _Enums.HistoryInventoryContext.AdvancePrice;

                hist.Save();

                ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find((int)GridView1.SelectedDataKey["Id"]);
                if (sd == null)
                    throw new Exception("Showdate is null");
                ShowTicket st = (ShowTicket)sd.ShowTicketRecords().Find((int)e.Keys["Id"]);
                if (st == null)
                    throw new Exception("ShowTicket is null");

                foreach (ShowTicketPackageLink link in st.ShowTicketPackageLinkRecords())
                {
                    HistoryPricing histLink = new HistoryPricing();
                    histLink.DtStamp = DateTime.Now;
                    histLink.UserId = (Guid)mem.ProviderUserKey;
                    histLink.TShowTicketId = link.LinkedShowTicketId;
                    histLink.DateAdjusted = DateTime.Now;
                    histLink.OldPrice = decimal.Parse(e.OldValues["mPrice"].ToString());
                    histLink.NewPrice = decimal.Parse(e.NewValues["mPrice"].ToString());
                    histLink.Context = _Enums.HistoryInventoryContext.AdvancePrice;

                    histLink.Save();
                }
            }
            if (e.OldValues["mServiceCharge"].ToString() != e.NewValues["mServiceCharge"].ToString())
            {
                HistoryPricing hist = new HistoryPricing();
                hist.DtStamp = DateTime.Now;
                System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(System.Web.HttpContext.Current.Profile.UserName);
                hist.UserId = (Guid)mem.ProviderUserKey;
                hist.TShowTicketId = (int)form.SelectedValue;
                hist.DateAdjusted = DateTime.Now;
                hist.OldPrice = decimal.Parse(e.OldValues["mServiceCharge"].ToString());
                hist.NewPrice = decimal.Parse(e.NewValues["mServiceCharge"].ToString());
                hist.Context = _Enums.HistoryInventoryContext.ServiceCharge;

                hist.Save();

                ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find((int)GridView1.SelectedDataKey["Id"]);
                if (sd == null)
                    throw new Exception("Showdate is null");
                ShowTicket st = (ShowTicket)sd.ShowTicketRecords().Find((int)e.Keys["Id"]);
                if (st == null)
                    throw new Exception("ShowTicket is null");

                foreach (ShowTicketPackageLink link in st.ShowTicketPackageLinkRecords())
                {
                    HistoryPricing histLink = new HistoryPricing();
                    histLink.DtStamp = DateTime.Now;
                    histLink.UserId = (Guid)mem.ProviderUserKey;
                    histLink.TShowTicketId = link.LinkedShowTicketId;
                    histLink.DateAdjusted = DateTime.Now;
                    histLink.OldPrice = decimal.Parse(e.OldValues["mServiceCharge"].ToString());
                    histLink.NewPrice = decimal.Parse(e.NewValues["mServiceCharge"].ToString());
                    histLink.Context = _Enums.HistoryInventoryContext.ServiceCharge;

                    histLink.Save();
                }
            }
            if (e.OldValues["iAllotment"].ToString() != e.NewValues["iAllotment"].ToString())
            {
                ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find((int)GridView1.SelectedDataKey["Id"]);
                if (sd == null)
                    throw new Exception("Showdate is null");
                ShowTicket st = (ShowTicket)sd.ShowTicketRecords().Find((int)form.SelectedValue);
                if (st == null)
                    throw new Exception("ShowTicket is null");

                int currentAllotment = int.Parse(e.OldValues["iAllotment"].ToString());
                int adjustment = (int.Parse(e.NewValues["iAllotment"].ToString())) - (int.Parse(e.OldValues["iAllotment"].ToString()));

                Inventory.AdjustInventoryHistory(
                    st,
                    currentAllotment,
                    adjustment,
                    System.Web.HttpContext.Current.Profile.UserName,
                    _Enums.HistoryInventoryContext.Allotment
                    );
            }

            Atx.SetCurrentShowRecord(Atx.CurrentShowRecord.Id);

            GridViewEntity.DataBind();
        }

        #endregion

        protected void CustomShipOptions_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator val = (CustomValidator)source;

            CheckBox ship = (CheckBox)FormView1.FindControl("chkAllowShip");
            CheckBox call = (CheckBox)FormView1.FindControl("chkAllowWillCall");

            if (Page.IsValid && ship != null && call != null)
            {
                //must have at least one option selected
                if ((!ship.Checked) && (!call.Checked))
                {
                    args.IsValid = false;
                }
            }
        }

        protected void FormLottery_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;

            if (GridView1.SelectedDataKey["Id"] == null || form.SelectedValue == null)
                return;

            ShowDate sd = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find((int)GridView1.SelectedDataKey["Id"]);
            if (sd == null)
                throw new Exception("Showdate is null");
            ShowTicket st = (ShowTicket)sd.ShowTicketRecords().Find((int)form.SelectedValue);
            if (st == null)
                throw new Exception("ShowTicket is null");
            CustomValidator validation = (CustomValidator)FormView1.FindControl("CustomValidation");

            if (st != null)
            {
            }
        }
}
}
