using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class ShowDate_TicketRequirement : BaseControl
    {
        List<string> _errors = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Atx.CurrentShowRecord == null)
                base.Redirect("/Admin/ShowEditor.aspx");

            litShowTitle.Text = Atx.CurrentShowRecord.Name;

            if (!IsPostBack)
            {
                GridDates.DataBind();
            }
        }

        #region GridDates

        protected void GridDates_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            GridTickets.DataBind();
        }
        protected void GridDates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (FormReq.CurrentMode != FormViewMode.Edit)
                FormReq.ChangeMode(FormViewMode.Edit);
        }
        protected void GridDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            GridTickets.DataBind();
        }

        #endregion

        #region GridTickets

        protected void GridTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(GridDates.SelectedValue);

            if (rowView != null)
            {
                DataRow row = rowView.Row;

                LinkButton select = (LinkButton)e.Row.FindControl("btnSelect");

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

                    if (criteria != null && criteria.Trim().Length > 75)
                        criteria = string.Format("{0}...", criteria.Trim().Substring(0, 72).Trim());
                    if (description != null && description.Trim().Length > 75)
                        description = string.Format("{0}...", description.Trim().Substring(0, 72).Trim());

                    theName = System.Text.RegularExpressions.Regex.Replace(string.Format("{0} {1} svc {2}<div>Ages {3}</div>{4}{5}",
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("PriceText")),
                        ((decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("mPrice"))).ToString("c"),
                        ((decimal)row.ItemArray.GetValue(row.Table.Columns.IndexOf("mServiceCharge"))).ToString("n2"),
                        row.ItemArray.GetValue(row.Table.Columns.IndexOf("AgeName")),
                        (description != null && description.Trim().Length > 0) ? string.Format("<div>{0}</div>", description.Trim()) : string.Empty,
                        (criteria != null && criteria.Trim().Length > 0) ? criteria.Trim() : string.Empty),
                        @"\s+", " ").Trim();
                }

                Literal litDesc = (Literal)e.Row.FindControl("litDesc");
                if (litDesc != null && theName.Trim().Length > 0)
                    litDesc.Text = theName;
            }
        }
        protected void GridTickets_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            GridReqs.DataBind();
        }
        protected void GridTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormReq.CurrentMode != FormViewMode.Edit)
                FormReq.ChangeMode(FormViewMode.Edit);
        }

        #endregion

        #region GridReqs

        protected void GridReqs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataRowView rowView = (DataRowView)e.Row.DataItem;

            if (rowView != null)
            {
                DataRow row = rowView.Row;

                string theName = string.Empty;
                if (row != null)
                {
                    //get the ids
                    //sql to put together the ticket descriptions separated by ~
                    //Get description back and separate 
                    //display as a UL
                    string ids = row.ItemArray.GetValue(row.Table.Columns.IndexOf("vcIdx")).ToString().Trim();

                    if (ids.Trim().Length > 0)
                    {
                        //make sure input is valid as we will be inserting directly into sql
                        string[] idInput = ids.Split(',');

                        //this will throw an error if we have invalid data
                        foreach (string s in idInput)
                            int.Parse(s);


                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("SELECT st.[Id] as 'Value', ('ID: ' + CAST(st.[Id] as varchar) + ' - ' + CONVERT(varchar,st.[dtDateOfShow], 101) + ' ' + ");
                        sb.Append("LTRIM(SUBSTRING(CONVERT(varchar,st.[dtDateOfShow], 100), LEN(CONVERT(varchar,st.[dtDateOfShow], 100)) - 7, LEN(CONVERT(varchar,st.[dtDateOfShow], 100)))) + ' ' + ");
                        sb.Append("SUBSTRING(s.[Name], 23, LEN(s.[Name])) + ' ' + a.[Name] + ' ' + CAST(st.[mPrice] as varchar) + ' s' + CAST(st.[mServiceCharge] as varchar) + ");
                        sb.Append("' ' + ISNULL(st.[SalesDescription],'') + ISNULL(st.[CriteriaText],'')) as 'Text' ");
                        sb.Append("FROM [ShowTicket] st, [Show] s, [Age] a ");
                        sb.AppendFormat("WHERE st.[Id] IN ({0}) AND st.[TShowId] = s.[Id] AND st.[TAgeId] = a.[Id] ORDER BY s.[Name] DESC ", ids);

                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                        //cmd.Parameters.Add("@vcIdx", ids, System.Data.DbType.String);

                        try
                        {
                            System.Data.DataSet ds = SubSonic.DataService.GetDataSet(cmd);
                            System.Data.DataTable dt = ds.Tables[0];

                            if (dt.Rows.Count > 0)
                                theName = "<ul>";

                            foreach (System.Data.DataRow rw in dt.Rows)
                                theName += string.Format("<li>{0}</li>", rw.ItemArray.GetValue(rw.Table.Columns.IndexOf("Text")).ToString());

                            if (dt.Rows.Count > 0)
                                theName += "</ul>";
                        }
                        catch (Exception ex)
                        {
                            CustomValidator validation = (CustomValidator)e.Row.FindControl("CustomValidation");

                            _Error.LogException(ex);

                            if (validation != null)
                            {
                                validation.IsValid = false;
                                validation.ErrorMessage = ex.Message;
                            }
                        }
                    }

                    Literal litDesc = (Literal)e.Row.FindControl("litDesc");
                    if (litDesc != null && theName.Trim().Length > 0)
                        litDesc.Text = theName;
                }
            }
        }
        protected void GridReqs_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormReq.DataBind();
        }
        protected void GridReqs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormReq.CurrentMode != FormViewMode.Edit)
                FormReq.ChangeMode(FormViewMode.Edit);
        }

        #endregion

        #region FormReq

        protected void FormReq_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
        }
        protected void FormReq_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
            _errors.Clear();

            try
            {
                int selectedDateId = (GridDates.SelectedValue != null) ? int.Parse(GridDates.SelectedValue.ToString()) : 0;
                ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(selectedDateId);
                if (selectedDate != null && GridTickets.SelectedValue != null && (int)GridTickets.SelectedValue > 0)
                {
                    ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find((int)GridTickets.SelectedValue);

                    //we must have a ticket selected
                    if (selectedTicket != null)
                    {
                        //the only req at this point is that we have ids in the text box
                        string ids = e.Values["vcIdx"].ToString().Trim();

                        Utils.Validation.ValidateRequiredField(_errors, "Ids To Match", ids);

                        if (_errors.Count == 0)
                        {
                            //we will try to construct an id list here - if it fails the error will be caught in the outer try/catch
                            string[] idInput = ids.Split(',');

                            foreach (string s in idInput)
                                int.Parse(s);
                        }
                    }
                }

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }


                form.ChangeMode(FormViewMode.Edit);

                //ensure Show has latest data
                int index = Atx.CurrentShowRecord.Id;
                Atx.SetCurrentShowRecord(index);

                //GridReqs.SelectedIndex = ;
                GridReqs.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                    e.Cancel = true;
                }
            }
        }
        protected void FormReq_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;
            CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
            _errors.Clear();

            try
            {
                int selectedDateId = (GridDates.SelectedValue != null) ? int.Parse(GridDates.SelectedValue.ToString()) : 0;
                ShowDate selectedDate = (ShowDate)Atx.CurrentShowRecord.ShowDateRecords().Find(selectedDateId);
                if (selectedDate != null && GridTickets.SelectedValue != null && (int)GridTickets.SelectedValue > 0)
                {
                    ShowTicket selectedTicket = (ShowTicket)selectedDate.ShowTicketRecords().Find((int)GridTickets.SelectedValue);

                    //we must have a ticket selected
                    if (selectedTicket != null)
                    {
                        //the only req at this point is that we have ids in the text box
                        string ids = e.NewValues["vcIdx"].ToString().Trim();

                        Utils.Validation.ValidateRequiredField(_errors, "Ids To Match", ids);

                        if (_errors.Count == 0)
                        {
                            //we will try to construct an id list here - if it fails the error will be caught in the outer try/catch
                            string[] idInput = ids.Split(',');

                            foreach (string s in idInput)
                                int.Parse(s);
                        }
                    }
                }                

                WillCallWeb.Components.Util.CalendarClock clockStart = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockStart");
                WillCallWeb.Components.Util.CalendarClock clockEnd = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockEnd");
                e.NewValues["dtStart"] = (clockStart.HasSelection) ? clockStart.SelectedDate.ToString("MM/dd/yyyy hh:mm tt") : null;
                e.NewValues["dtEnd"] = (clockEnd.HasSelection) ? clockEnd.SelectedDate.ToString("MM/dd/yyyy hh:mm tt") : null;

                if (Utils.Validation.IncurredErrors(_errors, validation))
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = ex.Message;
                    e.Cancel = true;
                }
            }
        }
        protected void FormReq_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
        {
            //if (e.Exception != null)
            //{
            //    CustomValidator validation = (CustomValidator)FormView1.FindControl("CustomValidation");
            //    if (validation != null)
            //    {
            //        validation.IsValid = false;
            //        validation.ErrorMessage = e.Exception.Message;// "The starting code date must precede the public onsale date.";
            //    }
            //    e.ExceptionHandled = true;
            //    e.KeepInEditMode = true;

            //    return;
            //}

            
            FormView form = (FormView)sender;

            //reset show data
            int index = (int)form.SelectedValue;

            Atx.SetCurrentShowRecord(Atx.CurrentShowRecord.Id);

            GridTickets.DataBind();
        }
        protected void SqlEntity_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            GridDates.DataBind();
        }
        protected void SqlEntity_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            //check for values
            e.Command.Parameters["@vcRequiredContext"].Value = _Enums.RequirementContext.ticket.ToString();
        }
        protected void FormReq_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            if (e.Exception != null)
            {
                CustomValidator validation = (CustomValidator)FormReq.FindControl("CustomValidation");
                if (validation != null)
                {
                    validation.IsValid = false;
                    validation.ErrorMessage = e.Exception.Message;// "The starting code date must precede the public onsale date.";
                }
                e.ExceptionHandled = true;
                e.KeepInInsertMode = true;

                return;
            }

            FormView form = (FormView)sender;

            //reset show data
            //int index = (int)form.SelectedValue;

            //refresh data
            Atx.SetCurrentShowRecord(Atx.CurrentShowRecord.Id);

            //get the selected ticket

            GridReqs.DataBind();
            GridReqs.SelectedIndex = GridReqs.Rows.Count - 1;
        }

        #endregion
}
}