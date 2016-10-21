using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Editor_InvoiceFee : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               GridView1.DataBind();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#invoicefeeeditor", true);
        }

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.DataSource = _Lookits.InvoiceFees;

            string[] keyNames = { "Id" };
            grid.DataKeyNames = keyNames;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormView1.DataBind();
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;
            DataControlRowState state = e.Row.RowState;

            if (typ == DataControlRowType.DataRow)
            {
                InvoiceFee entity = (InvoiceFee)e.Row.DataItem;

                Literal text = (Literal)e.Row.FindControl("LiteralDescription");
                if (text != null && entity != null && entity.Description != null)
                    text.Text = (entity.Description.Length > 50) ?
                        string.Format("{0}...", entity.Description.Substring(0, 50)) : entity.Description;

                LinkButton button = (LinkButton)e.Row.FindControl("btnDelete");
                if (button != null && entity != null)
                    button.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(entity.Name));

                Button btnActivate = (Button)e.Row.FindControl("btnActivate");
                if (btnActivate != null && entity != null)
                {
                    btnActivate.Text = (entity.IsActive && entity.IsOverride) ? "Deactivate" : "Activate";
                    btnActivate.CommandName = btnActivate.Text;
                    btnActivate.Enabled = entity.IsOverride || (!entity.IsActive);
                }
            }
        }
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];

            try
            {   
                _Lookits.InvoiceFees.DeleteFeeFromCollection(idx);
                grid.SelectedIndex = 0;
                grid.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = ex.Message;
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "activate":
                case "deactivate":
                    int idx = int.Parse(e.CommandArgument.ToString());
                    string sql = string.Empty;
                    InvoiceFee fee = (InvoiceFee)_Lookits.InvoiceFees.Find(idx);
                    
                    if(fee.IsOverride)
                    {
                        if (cmd == "activate" && (!fee.IsActive))
                        {
                            sql = "UPDATE [InvoiceFee] SET [bActive] = 0 WHERE [bOverride] = 1; UPDATE [InvoiceFee] SET [bActive] = 1 WHERE [Id] = @idx; ";
                        }
                        if(cmd == "deactivate" && fee.IsActive)
                            sql = "UPDATE [InvoiceFee] SET [bActive] = 0 WHERE [Id] = @idx; ";
                    }
                    else if(!fee.IsActive)
                    {
                        sql = "UPDATE [InvoiceFee] SET [bActive] = 0 WHERE [bOverride] = 0; UPDATE [InvoiceFee] SET [bActive] = 1 WHERE [Id] = @idx; ";
                    }

                    try
                    {
                        if (sql.Trim().Length > 0)
                        {
                            SubSonic.QueryCommand qcmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                            qcmd.Parameters.Add("@idx", idx, System.Data.DbType.Int32);

                            SubSonic.DataService.ExecuteQuery(qcmd);

                            _Lookits.RefreshLookup(_Enums.LookupTableNames.InvoiceFees.ToString());
                            grid.SelectedIndex = _Lookits.InvoiceFees.GetList().FindIndex(delegate(InvoiceFee match) { return (match.Id == idx); });
                            grid.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        CustomValidation.IsValid = false;
                        CustomValidation.ErrorMessage = ex.Message;
                    }

                    break;
            }
        }
        #endregion

        #region FormView

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            InvoiceFeeCollection coll = new InvoiceFeeCollection();
            InvoiceFee addFee = (InvoiceFee)_Lookits.InvoiceFees.Find(idx);
            if(addFee != null)
                coll.Add(addFee);

            form.DataSource = coll;
            string[] keyNames = { "Id" };
            form.DataKeyNames = keyNames;
        }
        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    //name,price, description
                    TextBox txtName = (TextBox)form.FindControl("txtName");
                    TextBox txtPrice = (TextBox)form.FindControl("txtPrice");
                    TextBox txtDescription = (TextBox)form.FindControl("txtDescription");

                    string name = txtName.Text.Trim();
                    string price = txtPrice.Text.Trim();
                    string description = txtDescription.Text.Trim();

                    if (name.Length == 0)
                        throw new Exception("Name is required.");
                    if (price.Length == 0)
                        throw new Exception("Price is required.");
                    if (!Utils.Validation.IsDecimal(price))
                        throw new Exception("Please enter a valid price.");
                    if (description.Length == 0)
                        throw new Exception("Description is required.");

                    decimal prc = decimal.Parse(price);

                    InvoiceFee entity = (InvoiceFee)_Lookits.InvoiceFees.Find((GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"]  : 0);

                    if (entity != null)
                    {
                        if (entity.Name != name)
                            entity.Name = name;

                        if (entity.Price != prc)
                            entity.Price = prc;

                        if (entity.Description != description)
                            entity.Description = description;

                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.InvoiceFees.ToString());
                    }

                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;

                    e.Cancel = true;
                }

                GridView1.DataBind();
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    //name,price, description
                    CheckBox chkOverride = (CheckBox)form.FindControl("chkOverride");
                    TextBox txtName = (TextBox)form.FindControl("txtName");
                    TextBox txtPrice = (TextBox)form.FindControl("txtPrice");
                    TextBox txtDescription = (TextBox)form.FindControl("txtDescription");

                    string name = txtName.Text.Trim();
                    string price = txtPrice.Text.Trim();
                    string description = txtDescription.Text.Trim();

                    if (name.Length == 0)
                        throw new Exception("Name is required.");
                    if (price.Length == 0)
                        throw new Exception("Price is required.");
                    if(!Utils.Validation.IsDecimal(price))
                        throw new Exception("Please enter a valid price.");
                    if (description.Length == 0)
                        throw new Exception("Description is required.");

                    decimal prc = decimal.Parse(price);

                    InvoiceFee newItem = _Lookits.InvoiceFees.AddFeeToCollection(chkOverride.Checked, name, prc, description);

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.InvoiceFees.ToString());

                    form.ChangeMode(FormViewMode.Edit);

                    GridView1.SelectedIndex = 0;//collection is order by ID descending

                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;

                    e.Cancel = true;
                }

                GridView1.DataBind();
            }
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            GridReq.DataBind();
        }

        #endregion

        #region Requirements Grid

        protected void GridReq_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.Visible = false;

            if (FormView1.SelectedValue != null)
            {
                int idx = (int)FormView1.DataKey["Id"];
                if (idx > 0)
                {
                    InvoiceFee ent = (InvoiceFee)_Lookits.InvoiceFees.Find(idx);

                    if (ent != null && ent.IsOverride || ent.RequiredInvoiceFeeRecords().Count > 0)
                    {
                        RequiredCollection coll = new RequiredCollection();
                        foreach(RequiredInvoiceFee reqFee in ent.RequiredInvoiceFeeRecords())
                            coll.Add(reqFee.RequiredRecord);

                        grid.DataSource = coll;
                        string[] keyNames = { "Id" };
                        grid.DataKeyNames = keyNames;
                        grid.Visible = true;
                    }
                }
            }
        }
        protected void GridReq_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;

            FormReq.DataBind();
        }
        protected void GridReq_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormReq.DataBind();
        }
        protected void GridReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;
            DataControlRowState state = e.Row.RowState;

            if (typ == DataControlRowType.DataRow)
            {
                Required entity = (Required)e.Row.DataItem;

                Literal start = (Literal)e.Row.FindControl("litStart");
                if (start != null && entity != null && entity.DateStart > System.Data.SqlTypes.SqlDateTime.MinValue.Value)
                    start.Text = entity.DateStart.ToString("MM/dd/yyyy hh::mmtt");

                Literal end = (Literal)e.Row.FindControl("litEnd");
                if (end != null && entity != null && entity.DateEnd < System.Data.SqlTypes.SqlDateTime.MaxValue.Value)
                    end.Text = entity.DateEnd.ToString("MM/dd/yyyy hh::mmtt");
            }
        }
        protected void GridReq_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];

            try
            {   
                Required.Delete(idx);//deletes the join by cascade
                _Lookits.RefreshLookup(_Enums.LookupTableNames.InvoiceFees.ToString());
                grid.SelectedIndex = 0;

                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = ex.Message;
            }
        }

        #endregion

        #region Requirements Form

        protected void FormReq_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            form.Visible = false;

            if (FormView1.SelectedValue != null)
            {
                int idx = (int)FormView1.DataKey["Id"];
                if (idx > 0)
                {
                    InvoiceFee ent = (InvoiceFee)_Lookits.InvoiceFees.Find(idx);

                    //ensure that the invoice fee in question is override
                    if (ent != null && ent.IsOverride)
                    {
                        RequiredCollection coll = new RequiredCollection();

                        //then see if we need to get info on a specific requirement
                        int reqIdx = (GridReq.SelectedDataKey != null) ? (int)GridReq.SelectedDataKey["Id"] : 0;

                        RequiredInvoiceFee invfee = ent.RequiredInvoiceFeeRecords().GetList().Find(delegate(RequiredInvoiceFee match) { return (match.TRequiredId == reqIdx); } );
                        if (invfee != null)
                            coll.Add(invfee.RequiredRecord);

                        //Required req = new Required(reqIdx);
                        //if(req != null)
                        //    coll.Add(req);

                        form.DataSource = coll;
                        string[] keyNames = { "Id" };
                        form.DataKeyNames = keyNames;
                        form.Visible = true;
                    }
                }
            }
        }
        protected void FormReq_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

            if (form.DataItem != null)
            {
                Required ent = (Required)form.DataItem;

                //start and end times


                //context - item is bound to enum
                DropDownList ddlContext = (DropDownList)form.FindControl("ddlContext");
                if (ddlContext != null)
                {
                    ddlContext.DataBind();
                    ddlContext.SelectedIndex = -1;
                    ListItem li = ddlContext.Items.FindByText(ent.RequiredContext.ToString());
                    if(li != null)
                        li.Selected = true;
                }

                //list is filled by context
                DropDownList ddlIdx = (DropDownList)form.FindControl("ddlIdx");
                if (ddlIdx != null)
                {
                    List<ListItem> list = new List<ListItem>();
                    bool isInList = false;

                    switch (ent.RequiredContext)
                    {
                        //if the current idx is not found within the current selection - then add it to the list

                        case _Enums.RequirementContext.merch:
                            //fill with items listed by parent/children
                            MerchCollection merch = new MerchCollection();
                            //MerchCollection parentMerch = new MerchCollection();
                            MerchCollection childMerch = new MerchCollection();

                            //get current items - atx.merchparents is sorted by name

                            foreach (Merch parent in Atx.MerchParents)
                            {
                                if (isInList == false && ent.Id_IsInRequiredListing(parent.Id.ToString()))
                                    isInList = true;

                                merch.Add(parent);
                                
                                if (parent.ChildMerchRecords().Count > 1)
                                {
                                    childMerch.AddRange(parent.ChildMerchRecords());

                                    if (childMerch.Count > 0)
                                    {
                                        childMerch.Sort("Name", true);
                                        foreach (Merch child in childMerch)
                                        {
                                            if (isInList == false && ent.Id_IsInRequiredListing(child.Id.ToString()))
                                                isInList = true;

                                            merch.Add(child);
                                        }
                                    }

                                    childMerch.Clear();//clear within proper scope
                                }
                            }

                            //now we have a properly sorted collection
                            if (!isInList && ent.IdxListing != null)
                            {
                                MerchCollection mcoll = new SubSonic.Select().From(Merch.Schema)
                                    .Where("Id").In(ent.IdxListing).ExecuteAsCollection<MerchCollection>();

                                if (mcoll.Count > 0)
                                {
                                    foreach (Merch itm in mcoll)
                                    {
                                        int i = 0;
                                        //if it is a parent...
                                        merch.Insert(i++, itm);
                                        if (itm.IsParent)
                                        {
                                            childMerch.AddRange(itm.ChildMerchRecords());
                                            if (childMerch.Count > 0)
                                            {
                                                childMerch.Sort("Name", true);
                                                foreach (Merch child in childMerch)
                                                    merch.Insert(i++, child);
                                            }

                                            childMerch.Clear();//clear within proper scope
                                        }
                                    }
                                }
                            }

                            foreach (Merch m in merch)
                            {
                                list.Add(new ListItem(string.Format("ID: {0} -{1} {2}{3}", m.Id.ToString(), (m.IsChild) ? "----->>>" : string.Empty, m.DisplayNameWithAttribs, 
                                    (m.IsParent && m.ChildMerchRecords().Count > 0) ? " (all styles)" : string.Empty), 
                                    m.Id.ToString()));
                            }
                            break;
                        case _Enums.RequirementContext.ticket:
                            //fill with future tickets
                            ShowTicketCollection coll1 = new ShowTicketCollection();
                            coll1.AddRange(Atx.SaleTickets);

                            coll1.Sort("DtDateOfShow", true);

                            foreach(ShowTicket st in coll1)//does this get us non active tickets?
                            {
                                //mark the ticket as found when added to the list
                                //so that if it is not found in the current items - we can still add it to the list
                                if(isInList == false && ent.Id_IsInRequiredListing(st.Id.ToString()))
                                    isInList = true;
                                
                                list.Add(new ListItem(string.Format("{8}: {0} - {1}{2} {3} {4} s{5} {6} {7}", st.Id.ToString(), (st.IsPackage) ? " PKG" : string.Empty, 
                                    st.DateOfShow.ToString("M/d/yy h:mmtt"), st.ShowDateRecord.ShowRecord.ShowNamePart, 
                                    st.Price.ToString("c"), st.ServiceCharge.ToString("n2"), st.SalesDescription_Derived, st.CriteriaText_Derived,
                                    (st.VendorRecord.Name.ToLower() == "boxoffice") ? "Box" : "Onl"
                                    
                                    ), 
                                    st.Id.ToString()));
                            }

                            //if the item(s) are not in the current list - add them - pref at the beginning of the list
                            if (!isInList && ent.IdxListing != null)
                            {
                                ShowTicketCollection coll = new SubSonic.Select().From(ShowTicket.Schema)
                                    .Where("Id").In(ent.IdxListing).ExecuteAsCollection<ShowTicketCollection>();
                                
                                if (coll.Count > 0)
                                {
                                    foreach(ShowTicket st in coll)
                                        list.Insert(0, new ListItem(string.Format("{8}: {0} - {1} {2} {3} s{4} {5}", st.Id.ToString(),
                                            st.DateOfShow.ToString("M/d/yy h:mmtt"), st.ShowDateRecord.ShowRecord.ShowNamePart, 
                                            st.Price.ToString("c"), st.ServiceCharge.ToString("n2"), st.CriteriaText_Derived,
                                            (st.VendorRecord.Name.ToLower() == "boxoffice") ? "Box" : "Onl"
                                            ), 
                                            st.Id.ToString()));
                                }
                            }
                            break;
                        case _Enums.RequirementContext.show:
                            //fill with future shows
                            ShowDateCollection coll2 = new ShowDateCollection();
                            coll2.AddRange(Atx.SaleShowDates_All);
                            coll2.Sort("DtDateOfShow", true);

                            foreach(ShowDate sd in coll2)
                            {
                                Show show = sd.ShowRecord;

                                int contained = list.FindIndex(delegate(ListItem match) { return match.Value == show.Id.ToString(); } );
                                if (contained < 0)
                                {
                                    if(isInList == false && ent.Id_IsInRequiredListing(show.Id.ToString()))
                                        isInList = true;
                                
                                    list.Add(new ListItem(string.Format("ID: {0} - {1}", show.Id.ToString(), show.Name), show.Id.ToString()));
                                }
                            }

                            //if the item(s) are not in the current list - add them - pref at the beginning of the list
                            if (!isInList && ent.IdxListing != null)
                            {
                                ShowCollection coll = new SubSonic.Select().From(Show.Schema)
                                    .Where("Id").In(ent.IdxListing).ExecuteAsCollection<ShowCollection>();
                                
                                if (coll.Count > 0)
                                {
                                    foreach(Show show in coll)
                                       list.Insert(0, new ListItem(string.Format("ID: {0} - {1}", show.Id.ToString(), show.Name), show.Id.ToString()));
                                }
                            }
                            break;
                        case _Enums.RequirementContext.showdate:
                            //fill with future showdates
                            ShowDateCollection coll3 = new ShowDateCollection();
                            coll3.AddRange(Atx.SaleShowDates_All);
                            coll3.Sort("DtDateOfShow", true);

                            foreach(ShowDate sd in coll3)
                            {
                                if(isInList == false && ent.Id_IsInRequiredListing(sd.Id.ToString()))
                                    isInList = true;
                                
                                list.Add(new ListItem(string.Format("ID: {0} - {1} {2}", sd.Id.ToString(), 
                                    sd.DateOfShow.ToString("M/d/yy h:mmtt"), sd.ShowRecord.ShowNamePart), 
                                    sd.Id.ToString()));
                            }

                            //if the item(s) are not in the current list - add them - pref at the beginning of the list
                            if (!isInList && ent.IdxListing != null)
                            {
                                ShowDateCollection coll = new SubSonic.Select().From(ShowDate.Schema)
                                    .Where("Id").In(ent.IdxListing).ExecuteAsCollection<ShowDateCollection>();
                                
                                if (coll.Count > 0)
                                {
                                    foreach(ShowDate sd in coll)
                                       list.Insert(0, new ListItem(string.Format("ID: {0} - {1} {2}", sd.Id.ToString(), 
                                        sd.DateOfShow.ToString("M/d/yy h:mmtt"), sd.ShowRecord.ShowNamePart), 
                                        sd.Id.ToString()));
                                }
                            }
                            break;
                        default:
                            ddlIdx.Enabled = false;
                            break;
                    }

                    //find missing elems

                    //bind
                    if (list.Count > 0)
                    {
                        ddlIdx.DataSource = list;
                        ddlIdx.DataTextField = "Text";
                        ddlIdx.DataValueField = "Value";
                        ddlIdx.DataBind();

                        if (ddlIdx.SelectedValue != ent.RequiredIdx.ToString())
                        {
                            ddlIdx.SelectedIndex = -1;
                            ListItem li = ddlIdx.Items.FindByValue(ent.RequiredIdx.ToString());
                            if(li != null)
                                li.Selected = true;
                        }
                    }
                }
            }
        }
        protected void FormReq_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        protected void FormReq_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    Required entity = Required.FetchByID((form.SelectedValue != null) ? (int)form.SelectedValue  : 0);

                    //start and end times
                    WillCallWeb.Components.Util.CalendarClock start = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockStart");
                    WillCallWeb.Components.Util.CalendarClock end = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockEnd");
                    DateTime startDate = (start.HasSelection) ? start.SelectedDate : System.Data.SqlTypes.SqlDateTime.MinValue.Value;
                    DateTime endDate = (end.HasSelection) ? end.SelectedDate : System.Data.SqlTypes.SqlDateTime.MaxValue.Value;

                    //context does not change
                    TextBox txtRequiredIds = (TextBox)form.FindControl("txtRequiredIds");
                    string inputIdx = (txtRequiredIds != null) ? txtRequiredIds.Text.Trim() : null;
                    TextBox txtDescription = (TextBox)form.FindControl("txtDescription");
                    string description = (txtDescription != null) ? txtDescription.Text.Trim() : null;

                    //name,qty, amount
                    TextBox txtQty = (TextBox)form.FindControl("txtQty");
                    TextBox txtAmount = (TextBox)form.FindControl("txtAmount");

                    //active - exclusive
                    CheckBox chkActive = (CheckBox)form.FindControl("chkActive");
                    bool active = chkActive.Checked;
                    CheckBox chkExclusive = (CheckBox)form.FindControl("chkExclusive");
                    bool exclusive = chkExclusive.Checked;

                    int qty = (txtQty.Text.Trim().Length > 0) ? (Utils.Validation.IsInteger(txtQty.Text.Trim())) ? int.Parse(txtQty.Text.Trim()) : -1 : 0;//will throw error if not an int
                    decimal amount = (txtAmount.Text.Trim().Length > 0) ? (Utils.Validation.IsDecimal(txtAmount.Text.Trim())) ? decimal.Parse(txtAmount.Text.Trim()) : -1.0M : 0;

                    if (qty < 0)
                        throw new Exception("Qty must be an integer.");
                    if (amount < 0)
                        throw new Exception("Amount must be a monetary value.");

                    //check context requirements
                    switch (entity.RequiredContext)
                    {
                        case _Enums.RequirementContext.ticket:
                        case _Enums.RequirementContext.merch:
                        case _Enums.RequirementContext.merchshipping:
                        case _Enums.RequirementContext.ticketshipping:
                            //requires a chosen id
                            if(inputIdx.Length == 0)
                                throw new Exception("This requirement context requires ids/methods to be specified.");
                            break;
                        case _Enums.RequirementContext.minmerchpurchase:
                        case _Enums.RequirementContext.minticketpurchase:
                        case _Enums.RequirementContext.mintotalpurchase:
                            //requires an amount
                            if(amount <= 0)
                                throw new Exception("This requirement context needs an amount.");
                            break;
                    }                    

                    if (entity != null)
                    {
                        if (entity.IsActive != active)
                            entity.IsActive = active;
                        if (entity.IsExclusive != exclusive)
                            entity.IsExclusive = exclusive;

                        if(entity.DateStart != startDate)
                            entity.DateStart = startDate;
                        if(entity.DateEnd != endDate)
                            entity.DateEnd = endDate;

                        //context cannot be updated here

                        if (entity.IdxListing != inputIdx)
                            entity.IdxListing = (inputIdx.Length == 0) ? null : inputIdx;

                        if (entity.RequiredQty != qty)
                            entity.RequiredQty = qty;

                        if (entity.MinimumAmount != amount)
                            entity.MinimumAmount = amount;

                        if (entity.Description != description)
                            entity.Description = (description.Length == 0) ? null : description;

                        entity.Save();

                        _Lookits.RefreshLookup(_Enums.LookupTableNames.InvoiceFees.ToString());

                        GridView1.DataBind();//bind from the beginning
                    }

                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;

                    e.Cancel = true;
                }
            }
        }
        protected void FormReq_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            if (Page.IsValid)
            {
                FormView form = (FormView)sender;

                try
                {
                    //context
                    DropDownList ddlContext = (DropDownList)form.FindControl("ddlCOntext");
                    //ddlcontext defaults to NA
                    _Enums.RequirementContext ctx = _Enums.RequirementContext.NA;
                    if(ddlContext.SelectedIndex != -1)
                        ctx = (_Enums.RequirementContext)Enum.Parse(typeof(_Enums.RequirementContext), ddlContext.SelectedItem.Text, true);

                    if(ctx == _Enums.RequirementContext.NA)
                        throw new Exception("A context must be selected.");

                    //required name - only required if context is a shipping method
                    TextBox txtRequiredIds = (TextBox)form.FindControl("txtRequiredIds");
                    if(txtRequiredIds.Text.Trim().Length == 0 && ctx.ToString().ToLower().IndexOf("shipping") != -1)
                        throw new Exception("When the context is shipping- a shipping method must be specified. Choose from TODO: UPS, USPS all.");

                    string name = (txtRequiredIds.Text.Trim().Length > 0) ? txtRequiredIds.Text.Trim() : null;

                    //new rquirement - then new join
                    Required newReq = new Required();
                    newReq.DtStamp = DateTime.Now;
                    newReq.IsActive = true;
                    newReq.IsExclusive = false;

                    if(name != null)
                        newReq.IdxListing = name;
                    newReq.RequiredContext = ctx;

                    newReq.Save();

                    //now join
                    RequiredInvoiceFee newInvFee = new RequiredInvoiceFee();
                    newInvFee.DtStamp = newReq.DtStamp;
                    newInvFee.TInvoiceFeeId = (int)GridView1.SelectedDataKey["Id"];
                    newInvFee.TRequiredId = newReq.Id;
                    newInvFee.Save();

                    _Lookits.RefreshLookup(_Enums.LookupTableNames.InvoiceFees.ToString());

                    form.ChangeMode(FormViewMode.Edit);

                    GridReq.SelectedIndex = GridReq.Rows.Count;

                    GridView1.DataBind();//bind at the source

                    e.Cancel = false;
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidation.IsValid = false;
                    CustomValidation.ErrorMessage = ex.Message;

                    e.Cancel = true;
                }
            }
        }

        #endregion
    }
}