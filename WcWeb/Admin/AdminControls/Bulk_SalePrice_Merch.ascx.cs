using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{   
    public partial class Bulk_SalePrice_Merch : BaseControl
    {
        #region Page Overhead

        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._rowsSelected = (List<int>)ctlState[1];
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[2];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this._rowsSelected;
            return ctlState;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            this.Page.RegisterRequiresControlState(this);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void ValidateDateGreaterThanNow(object sender, ServerValidateEventArgs e)
        {
            if(e.Value.Trim().Length > 0)
            {
                e.IsValid = (Utils.Validation.IsDate(e.Value) && DateTime.Parse(e.Value) > DateTime.Now);
            }
            else
                e.IsValid = false;//value is required
        }

        #endregion

        protected void FormView1_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;

        }
        protected void FormView1_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "publish":
                    
                    //configure date from the last date in the grid
                    EventQ lastRow = null;

                    for (int i = GridView1.Rows.Count - 1; i >= 0; i--)
                    {
                        int idx = (int)GridView1.DataKeys[i]["Id"];

                        //find the verb - if it is not publish - then use this row and exit
                        EventQ seekNonPublish = EventQ.FetchByID(idx);

                        if(seekNonPublish != null && seekNonPublish.Verb != _Enums.EventQVerb.Publish.ToString())
                        {
                            lastRow = seekNonPublish;
                            break;
                        }
                    }

                    if (lastRow == null)
                    {
                        CustomValidator custom = (CustomValidator)FormView1.FindControl("CustomStart");
                        custom.IsValid = false;
                        custom.ErrorMessage = "A date is required.";
                        return;
                    }

                    DateTime postEventPublishDate = lastRow.DateToProcess.Value.AddMinutes(1);

                    ////insert a publish event in the eventq
                    string loweredName = this.Profile.UserName.ToLower();

                    AspnetUser usr = AspnetUser.GetUserByUserName(loweredName);

                    EventQ.Insert(postEventPublishDate, null, null, null, 3, 10, (usr != null) ? usr.UserId : Guid.Empty, loweredName, null, null,
                        _Enums.EventQContext.Merch.ToString(), _Enums.EventQVerb.Publish.ToString(), null, null, null, 
                        Request.UserHostAddress, DateTime.Now, _Config.APPLICATION_ID);

                    ResetSelectedGridRow(int.Parse(GridView1.SelectedValue.ToString()));
                    //FormView1.ChangeMode(FormViewMode.Edit);
                    //FormView1.DataBind();
                    break;
                case "cancel":
                    if(GridView1.SelectedIndex >= 0)
                        GridView1.SelectedIndex = 0;
                    //form.DataSource = new List<int>(0);
                    //form.ChangeMode(FormViewMode.Edit);
                    break;
            }

            //GridView1.DataBind();
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;

            WillCallWeb.Components.Util.CalendarClock process = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("cclStartDate");

            //DateTime process = DateTime.Parse(e.NewValues["DateToProcess"].ToString());
            if (process.SelectedDate < DateTime.Now)
            {
                CustomValidator custom = (CustomValidator)form.FindControl("CustomStart");
                custom.IsValid = false;
                custom.ErrorMessage = "Please enter a date in the future.";
                e.Cancel = true;
            }
        }
        protected void FormView1_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            FormView form = (FormView)sender;

            //if date has changed - we will need to follow the selection as the grid will be reordered
            ResetSelectedGridRow((int)form.SelectedValue);
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;

            DropDownList ddl = (DropDownList)form.FindControl("ddlMerchParents");
            if (ddl != null)
            {
                //assign chosen value to sql data source param
                int idx = int.Parse(ddl.SelectedValue);
                Merch m = Merch.FetchByID(idx);

                e.Values["description"] = string.Format("({0}) {1}", m.Price_Effective.ToString("c"), ddl.SelectedItem.Text);
                e.Values["oldValue"] = ddl.SelectedValue;
            }

            e.Values["context"] = _Enums.EventQContext.Merch.ToString();
            e.Values["verb"] = _Enums.EventQVerb.Merch_SalePriceChange.ToString();
            e.Values["ip"] = Request.UserHostAddress;
        }
        protected void ddlMerchParents_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SELECT m.* FROM [Merch] m WHERE m.[TParentListing] IS NULL; ", 
                SubSonic.DataService.Provider.Name);

            MerchCollection coll = new MerchCollection();
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));
            if (coll.Count > 1)
                coll.Sort("Name", true);

            ddl.Items.Add(new ListItem(" <--Select a merch item--> ", "0"));
            ddl.AppendDataBoundItems = true;

            ddl.DataSource = coll;
            ddl.DataTextField = "DisplayNameWithAttribs";
            ddl.DataValueField = "Id";
        }
        protected void ddlMerchParents_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //since we are adding a static list item - we can get away with this
            if (ddl.SelectedIndex == -1)
                ddl.SelectedIndex = 0;
        }
        protected void ddlMerchParents_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            Label current = (Label)FormView1.FindControl("lblCurrentPrice");
            current.Text = "&nbsp;";

            int idx = int.Parse(ddl.SelectedValue);

            if (idx > 0)
            {
                Merch m = Merch.FetchByID(idx);

                if (m != null)
                    current.Text = m.Price_Effective.ToString();
            }


        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                    {
                        //find next row up that has a diff time than the next row up
                        int thisRow = int.Parse(e.CommandArgument.ToString());
                        if (thisRow > 1)
                        {
                            GridViewRow prev = GridView1.Rows[thisRow - 1];
                            DateTime processToBeat = (DateTime)GridView1.DataKeys[prev.RowIndex]["DateToProcess"];

                            for (int i = prev.RowIndex - 1; i >= 0; i--)
                            {
                                DateTime currentRowProcessDate = (DateTime)GridView1.DataKeys[i]["DateToProcess"];
                                //then if we have a beat match - update the EventQ
                                if (currentRowProcessDate < processToBeat)
                                {
                                    EventQ eq = EventQ.FetchByID((int)GridView1.DataKeys[thisRow]["Id"]);
                                    if (eq != null)
                                    {
                                        if (currentRowProcessDate.AddMinutes(1) >= processToBeat)
                                            eq.DateToProcess = currentRowProcessDate.AddSeconds(1);
                                        else
                                            eq.DateToProcess = currentRowProcessDate.AddMinutes(1);

                                        eq.Save();
                                        break;
                                    }
                                }
                            }

                            GridView1.DataBind();
                        }

                    }
                    break;
                case "down":
                    {
                        //find next row down that has a diff time than the next row down
                        int theRow = int.Parse(e.CommandArgument.ToString());
                        if (theRow < GridView1.Rows.Count - 1)//last row minus one
                        {
                            GridViewRow previous = GridView1.Rows[theRow - 1];
                            DateTime processToBetter = (DateTime)GridView1.DataKeys[previous.RowIndex]["DateToProcess"];

                            for (int i = theRow + 1; i < GridView1.Rows.Count; i++)
                            {
                                DateTime currentProcessDate = (DateTime)GridView1.DataKeys[i]["DateToProcess"];
                                //then if we have a beat match - update the EventQ
                                if (currentProcessDate > processToBetter)//this will order correctly for events that share a process date
                                {
                                    EventQ eqq = EventQ.FetchByID((int)GridView1.DataKeys[theRow]["Id"]);
                                    if (eqq != null)
                                    {
                                        eqq.DateToProcess = currentProcessDate.AddMinutes(1);

                                        eqq.Save();
                                        break;
                                    }
                                }
                            }

                            GridView1.DataBind();
                        }
                    }
                    break;
            }
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                int idx = (int)rowView.Row.ItemArray[rowView.Row.Table.Columns.IndexOf("Id")];
                _Enums.EventQVerb verb = (_Enums.EventQVerb)Enum.Parse(typeof(_Enums.EventQVerb), rowView.Row.ItemArray[rowView.Row.Table.Columns.IndexOf("Verb")].ToString(), true);

                //decide to check box or not
                CheckBox select = (CheckBox)e.Row.FindControl("chkSelect");
                if (select != null)
                {
                    if (verb == _Enums.EventQVerb.Publish)
                    {
                        select.Enabled = false;
                        select.Checked = false;
                    }
                    else
                    {
                        select.Enabled = true;
                        select.Checked = (RowsSelected.FindIndex(delegate(int match) { return (match == idx); }) != -1);
                    }
                }

                //display delete confirm text
                string desc = (verb == (_Enums.EventQVerb.Publish)) ? "this publish event" : rowView.Row.ItemArray[rowView.Row.Table.Columns.IndexOf("Description")].ToString();
                
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
            
                if (verb == (_Enums.EventQVerb.Publish))
                {
                    up.Visible = true;
                    down.Visible = true;
                    up.CommandArgument = e.Row.RowIndex.ToString();
                    down.CommandArgument = e.Row.RowIndex.ToString();
                }
                else
                {
                    up.Visible = false;
                    down.Visible = false;
                }

                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");
                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0} from the event queue?')",
                        Utils.ParseHelper.ParseJsAlert(desc));
            }
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
           // FormView1.DataBind();
        }
        protected void btnApplyDate_Click(object sender, EventArgs e)
        {
            //validate input
            if (Page.IsValid)
            {
                DateTime process = cclProcess.SelectedDate;

                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    //get the row in question and update the eventQ
                    int idx = int.Parse(GridView1.DataKeys[gvr.RowIndex]["Id"].ToString());

                    EventQ eq = EventQ.FetchByID(idx);
                    if (eq != null)
                    {
                        if (eq.Verb == _Enums.EventQVerb.Publish.ToString())
                            EventQ.Delete(idx);

                        CheckBox select = (CheckBox)gvr.FindControl("chkSelect");
                        if (select != null && select.Checked)
                        {
                            eq.DateToProcess = process;
                            eq.Save();
                        }
                    }

                    cclProcess.Reset();
                    //txtProcessDate.Text = string.Empty;

                    //ensure form has a selection
                    if (FormView1.SelectedValue != null)
                        ResetSelectedGridRow((int)FormView1.SelectedValue);
                    else
                        GridView1.DataBind();
                }
            }
        }
        protected void btnApplyPrice_Click(object sender, EventArgs e)
        {
            //validate input
            string input = txtPrice.Text.Trim();
            if (input.Length > 0 && Utils.Validation.IsDecimal(input))
            {
                decimal price = decimal.Parse(input);

                if (price <= 0)
                {
                    CustomProcess.IsValid = false;
                    CustomProcess.ErrorMessage = "Please enter a valid price.";
                    return;
                }

                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    CheckBox select = (CheckBox)gvr.FindControl("chkSelect");
                    if (select != null && select.Checked)
                    {
                        //get the row in question and update the eventQ
                        int idx = int.Parse(GridView1.DataKeys[gvr.RowIndex]["Id"].ToString());

                        EventQ eq = EventQ.FetchByID(idx);
                        if (eq != null)
                        {
                            eq.NewValue = price.ToString();
                            eq.Save();
                        }
                    }
                }

                txtPrice.Text = string.Empty;

                //gird will not be reordered
                GridView1.DataBind();
            }
        }

        private List<int> _rowsSelected = null;
        protected List<int> RowsSelected
        {
            get
            {
                if (_rowsSelected == null)
                    _rowsSelected = new List<int>();

                return _rowsSelected;
            }
            set
            {
                _rowsSelected = value;
            }
        }
        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            RowsSelected.Clear();

            foreach (GridViewRow gvr in GridView1.Rows)
                RowsSelected.Add((int)GridView1.DataKeys[gvr.RowIndex]["Id"]);

            GridView1.DataBind();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            RowsSelected.Clear();
            GridView1.DataBind();
        }
        protected void chkSelect_CheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            GridView1.SelectedIndex = -1;
            GridView1.SelectedIndex = ((GridViewRow)chk.NamingContainer).RowIndex;

            int idx = (int)GridView1.DataKeys[((GridViewRow)chk.NamingContainer).RowIndex]["Id"];
            if (chk.Checked)
            {
                if (RowsSelected.FindIndex(delegate(int match) { return (match == idx); } ) == -1)
                    RowsSelected.Add(idx);
            }
            else
            {
                if (RowsSelected.FindIndex(delegate(int match) { return (match == idx); }) != -1)
                    RowsSelected.Remove(idx);
            }

            GridView1.DataBind();

        }
        protected void Sql_Events_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
            e.Command.Parameters["@context"].Value = _Enums.EventQContext.Merch.ToString();
            e.Command.Parameters["@verb"].Value = _Enums.EventQVerb.Merch_SalePriceChange.ToString();
        }
        protected void ResetSelectedGridRow(int idx)
        {
            GridView1.SelectedIndex = -1;
            GridView1.DataBind();

            foreach (GridViewRow gvr in GridView1.Rows)
            {
                int rowIdx = (int)GridView1.DataKeys[gvr.DataItemIndex].Value;

                if (rowIdx == idx)
                {
                    GridView1.SelectedIndex = gvr.DataItemIndex;

                    break;
                }
            }
        }
    
        protected void SqlEvent_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            int idx = (int)e.Command.Parameters["@newId"].Value;

            ResetSelectedGridRow(idx);
        }

        protected void SqlEvent_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }

        protected void SqlEvent_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }

        protected void FormView1_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            
        }
}
}