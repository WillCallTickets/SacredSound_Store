using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.ProductAccessor
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CampaignList : BaseControl
    {
        List<string> _errors = new List<string>();

        #region Page Overhead

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                GridListing.DataBind();
            }

            btnUserList.Enabled = (Atx.CurrentAccessCampaign != null);
        }

        protected void btnUserList_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            base.Redirect("/Admin/ProductAccess.aspx?p=usr");
        }
        protected void btnCampaignMailer_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            base.Redirect("/Admin/ProductAccess.aspx?p=mlr");
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            _Lookits.RefreshLookup(_Enums.LookupTableNames.ProductAccessors, Profile.UserName);
        }

        #endregion

        #region GridListing

        protected void GridListing_Init(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.PageSize = 25;
        }
        protected void GridListing_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (Atx.ProductAccessCampaigns.Count > 0)
            {
                Atx.ProductAccessCampaigns.Sort("IDisplayOrder", true);
                grid.DataSource = Atx.ProductAccessCampaigns;
            }
        }
        protected void GridListing_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            GridViewRow gvr = e.Row;

            if (gvr != null && gvr.RowType == DataControlRowType.DataRow)
            {
                ProductAccess pa = (ProductAccess)gvr.DataItem;

                ActivationWindow aw = pa.ActivationWindowRecord;

                if (aw != null)
                {
                    Literal litPublicStart = (Literal)gvr.FindControl("litPublicStart");
                    Literal litPublicEnd = (Literal)gvr.FindControl("litPublicEnd");

                    if (litPublicStart != null)
                        litPublicStart.Text = (aw.DatePublicStart != Utils.Constants._MinDate) ? aw.DatePublicStart.ToString("MM/dd/yyyy hh:mmtt") : string.Empty;
                    if (litPublicEnd != null)
                        litPublicEnd.Text = (aw.DatePublicEnd != DateTime.MaxValue) ? aw.DatePublicEnd.ToString("MM/dd/yyyy hh:mmtt") : string.Empty;
                }

                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

                if (delete != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(pa.CampaignName));

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);
                    down.Enabled = (e.Row.RowIndex < (((ICollection)grid.DataSource).Count - 1));
                }
            }
        }
        protected void GridListing_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
            {
                if (Atx.CurrentAccessCampaign != null)
                {
                    int idx = Atx.CurrentAccessCampaign.Id;

                    foreach (GridViewRow gvr in grid.Rows)
                        if ((int)grid.DataKeys[gvr.DataItemIndex]["Id"] == idx)
                        {
                            grid.SelectedIndex = gvr.DataItemIndex;
                            break;
                        }
                }
                else
                {
                    grid.SelectedIndex = 0;
                    int selIdx = (int)grid.SelectedDataKey.Value;
                    Atx.SetCurrentAccessCampaign(selIdx);
                }
            }

            FormEditor.DataBind();
        }
        protected void GridListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (FormEditor.CurrentMode != FormViewMode.Edit)
                FormEditor.ChangeMode(FormViewMode.Edit);

            //match selected index to current object
            int selIdx = (int)grid.SelectedDataKey.Value;
            Atx.SetCurrentAccessCampaign(selIdx);

            FormEditor.DataBind();
        }
        protected void GridListing_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    ProductAccess moved = Atx.ProductAccessCampaigns.ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    ////set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
                    break;
            }

            if (FormEditor.CurrentMode != FormViewMode.Edit)
                FormEditor.ChangeMode(FormViewMode.Edit);
        }
        protected void GridListing_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            CustomValidator validation = (CustomValidator)grid.FindControl("CustomValidation");
            int idx = (int)grid.DataKeys[e.RowIndex].Value;
            ProductAccess selected = (ProductAccess)Atx.ProductAccessCampaigns.Find(idx);

            try
            {
                e.Cancel = ((selected == null) || (!Atx.ProductAccessCampaigns.DeleteFromCollection(idx)));

                //reset data - rebinding of grid will select the new current object
                Atx.SetCurrentAccessCampaign(0);
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
        protected void GridListing_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.DataBind();
        }


        #endregion

        #region Form Editor

        protected void GridSelections_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            grid.DataSource = Atx.CurrentAccessCampaign.ProductAccessProductRecords();
        }
        protected void GridSelections_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            GridViewRow gvr = e.Row;

            if (gvr != null && gvr.RowType == DataControlRowType.DataRow)
            {
                ProductAccessProduct p = (ProductAccessProduct)gvr.DataItem;

                string txt = string.Empty;
                Literal lit = (Literal)gvr.FindControl("litDescription");

                if (lit != null)
                {
                    switch(p.ProductContext)
                    {
                        case _Enums.ProductAccessProductContext.ticket:
                            txt = Utils.ParseHelper.StripHtmlTags(p.ShowTicketRecord.DisplayNameWithAttribsAndDescription);
                            break;
                        //case _Enums.ProductAccessProductContext.showdate:
                        //    txt = p.ShowTicketRecord
                        case _Enums.ProductAccessProductContext.merch:
                            txt = p.ParentMerchRecord.DisplayNameWithAttribs;
                            break;
                    }

                    lit.Text = txt;
                }
            }
        }
        protected void GridSelections_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;

            CustomValidator validation = (CustomValidator)grid.FindControl("CustomValidation");
            int idx = (int)grid.DataKeys[e.RowIndex].Value;
            ProductAccessProduct selected = (ProductAccessProduct)Atx.CurrentAccessCampaign.ProductAccessProductRecords().Find(idx);

            try
            {
                if (selected != null)
                {
                    string sql = "DELETE FROM [ProductAccessProduct] WHERE [Id] = @idx ";
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                    cmd.Parameters.Add("@idx", selected.Id, DbType.Int32);
                    SubSonic.DataService.ExecuteQuery(cmd);

                    Atx.CurrentAccessCampaign.ProductAccessProductRecords().Remove(selected);

                    //reset data - rebinding of grid will select the new current object
                    //Atx.SetCurrentAccessCampaign(0);

                    e.Cancel = false;

                    FormEditor.DataBind();
                }
                else
                    e.Cancel = true;
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
        }

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            if (cal.ID.ToLower().IndexOf("start") != -1)
                cal.DefaultValue = Utils.Constants._MinDate;
            else
                cal.DefaultValue = DateTime.MaxValue;
        }

        protected void FormEditor_DataBinding(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            ProductAccessCollection coll = new ProductAccessCollection();

            if (Atx.CurrentAccessCampaign != null)
                coll.Add(Atx.CurrentAccessCampaign);

            form.DataSource = coll;
        }
        protected void FormEditor_DataBound(object sender, EventArgs e)
        {
            FormView form = (FormView)sender;
            ProductAccess entity = (ProductAccess)form.DataItem;

            if (entity != null)
            {
                if (form.CurrentMode != FormViewMode.Edit)
                    form.ChangeMode(FormViewMode.Edit);

                bool HasActivationWindow = (Atx.CurrentAccessCampaign.ActivationWindowRecord != null);
                Panel pnlAddActivationWindow = (Panel)form.FindControl("pnlAddActivationWindow");
                Panel pnlEditActivationWindow = (Panel)form.FindControl("pnlEditActivationWindow");

                if (pnlAddActivationWindow != null)
                    pnlAddActivationWindow.Visible = (!HasActivationWindow);
                if (pnlEditActivationWindow != null)
                    pnlEditActivationWindow.Visible = HasActivationWindow;

                //activation window object
                if (HasActivationWindow)
                {
                    Literal litId = (Literal)form.FindControl("litId");
                    //CheckBox chkCodeActive = (CheckBox)form.FindControl("chkCodeActive");
                    //TextBox txtActivationCode = (TextBox)form.FindControl("txtActivationCode");
                    //WillCallWeb.Components.Util.CalendarClock codeStart = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockCodeStart");
                    //WillCallWeb.Components.Util.CalendarClock codeEnd = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockCodeEnd");
                    WillCallWeb.Components.Util.CalendarClock publicStart = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockPublicStart");
                    WillCallWeb.Components.Util.CalendarClock publicEnd = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockPublicEnd");

                    if (litId != null)
                        litId.Text = Atx.CurrentAccessCampaign.ActivationWindowRecord.Id.ToString();
                    //if (chkCodeActive != null)
                    //    chkCodeActive.Checked = Atx.CurrentAccessCampaign.ActivationWindowRecord.UseCode;
                    //if (txtActivationCode != null)
                    //    txtActivationCode.Text = Atx.CurrentAccessCampaign.ActivationWindowRecord.Code;
                    //if (codeStart != null)
                    //    codeStart.SelectedDate = Atx.CurrentAccessCampaign.ActivationWindowRecord.DateCodeStart;
                    //if (codeEnd != null)
                    //    codeEnd.SelectedDate = Atx.CurrentAccessCampaign.ActivationWindowRecord.DateCodeEnd;
                    if (publicStart != null)
                        publicStart.SelectedDate = Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicStart;
                    if (publicEnd != null)
                        publicEnd.SelectedDate = Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicEnd;
                }
            }
        }
        protected void FormEditor_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;

            CheckBox chkActive = (CheckBox)form.FindControl("chkActive");
            TextBox txtName = (TextBox)form.FindControl("txtName");

            if (chkActive != null && chkActive.Checked != Atx.CurrentAccessCampaign.IsActive)
                Atx.CurrentAccessCampaign.IsActive = chkActive.Checked;

            if (txtName != null && txtName.Text.Trim() != Atx.CurrentAccessCampaign.CampaignName)
                Atx.CurrentAccessCampaign.CampaignName = txtName.Text.Trim();


            //activation window object
            if (Atx.CurrentAccessCampaign.ActivationWindowRecord != null)
            {
                bool dirty = false;
                //CheckBox chkCodeActive = (CheckBox)form.FindControl("chkCodeActive");
                //WillCallWeb.Components.Util.CalendarClock codeStart = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockCodeStart");
                //WillCallWeb.Components.Util.CalendarClock codeEnd = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockCodeEnd");
                WillCallWeb.Components.Util.CalendarClock publicStart = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockPublicStart");
                WillCallWeb.Components.Util.CalendarClock publicEnd = (WillCallWeb.Components.Util.CalendarClock)form.FindControl("clockPublicEnd");

                //if (codeStart != null && codeStart.SelectedDate != Atx.CurrentAccessCampaign.ActivationWindowRecord.DateCodeStart)
                //{
                //    Atx.CurrentAccessCampaign.ActivationWindowRecord.DateCodeStart = codeStart.SelectedDate;
                //    dirty = true;
                //}
                //if (codeEnd != null && codeEnd.SelectedDate != Atx.CurrentAccessCampaign.ActivationWindowRecord.DateCodeEnd)
                //{
                //    Atx.CurrentAccessCampaign.ActivationWindowRecord.DateCodeEnd = codeEnd.SelectedDate;
                //    dirty = true;
                //}
                if (publicStart != null && publicStart.SelectedDate != Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicStart)
                {
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicStart = publicStart.SelectedDate;
                    dirty = true;
                }
                if (publicEnd != null && publicEnd.SelectedDate != Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicEnd)
                {
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicEnd = publicEnd.SelectedDate;
                    dirty = true;
                }

                if (dirty)
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.Save();
            }


            Atx.CurrentAccessCampaign.Save();

            //reset data
            //int index = (int)form.SelectedValue;
            //Atx.SetCurrentMerchRecord(Atx.CurrentMerchRecord.Id);

            GridListing.DataBind();
        }
        protected void FormEditor_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            FormView form = (FormView)sender;
            CustomValidator custom = (CustomValidator)form.FindControl("CustomValidation");

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "new":
                    //CreateNewPostPurchase(form);
                    //form.ChangeMode(FormViewMode.Edit);
                    break;

                case "resetcampaigncode":
                    Atx.CurrentAccessCampaign.CampaignCode = Utils.ParseHelper.GenerateRandomPassword(10);
                    Atx.CurrentAccessCampaign.Save();
                    GridListing.DataBind();
                    break;


                //activation window
                case "addactivation":
                    Atx.CurrentAccessCampaign.ActivationWindowRecord = new ActivationWindow();
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.DtStamp = DateTime.Now;
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.ApplicationId = _Config.APPLICATION_ID;
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.ActivationWindowContext = _Enums.ActivationWindowContext.ProductAccess;
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.TParentId = Atx.CurrentAccessCampaign.Id;
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.Code = Utils.ParseHelper.GenerateRandomPassword(7);
                    Atx.CurrentAccessCampaign.ActivationWindowRecord.Save();
                    GridListing.DataBind();
                    break;
                case "deleteactivation":
                    Atx.CurrentAccessCampaign.DeleteActivationWindowRecord();
                    GridListing.DataBind();
                    break;
                //case "resetactivationcode":
                //    Atx.CurrentAccessCampaign.ActivationWindowRecord.Code = Utils.ParseHelper.GenerateRandomPassword(7);
                //    Atx.CurrentAccessCampaign.ActivationWindowRecord.Save();
                //    GridListing.DataBind();
                //    break;
                case "addticket":
                    //validate selection - id can't be zero and must not exist
                    Choosers.Chooser_Ticket chooserT = (Choosers.Chooser_Ticket)form.FindControl("Chooser_Ticket1");
                    if (chooserT != null && chooserT.SelectedValue > 0)
                    {
                        int selectedIdx = chooserT.SelectedValue;
                        int exists = Atx.CurrentAccessCampaign.ProductAccessProductRecords().GetList().FindIndex(delegate(ProductAccessProduct match)
                        { return (match.ProductContext == _Enums.ProductAccessProductContext.ticket && match.TParentId == selectedIdx); });
                        if (exists > -1)
                        {
                            //throw error
                            custom.IsValid = false;
                            custom.ErrorMessage = "This selection has already been added.";
                        }
                        else
                        {
                            ProductAccessProduct p = new ProductAccessProduct();
                            p.DtStamp = DateTime.Now;
                            p.TProductAccessId = Atx.CurrentAccessCampaign.Id;
                            p.ProductContext = _Enums.ProductAccessProductContext.ticket;
                            p.TParentId = selectedIdx;
                            p.Save();

                            Atx.CurrentAccessCampaign.ProductAccessProductRecords().Add(p);

                            form.DataBind();
                        }
                    }
                    break;
                case "addmerch":
                    //validate selection - id can't be zero and must not exist
                    Choosers.Chooser_Merch chooserM = (Choosers.Chooser_Merch)form.FindControl("Chooser_Merch1");
                    if (chooserM != null && chooserM.SelectedValue > 0)
                    {
                        int selectedIdx = chooserM.SelectedValue;
                        int exists = Atx.CurrentAccessCampaign.ProductAccessProductRecords().GetList().FindIndex(delegate(ProductAccessProduct match)
                        { return (match.ProductContext == _Enums.ProductAccessProductContext.merch && match.TParentId == selectedIdx); });
                        if (exists > -1)
                        {
                            //throw error
                            custom.IsValid = false;
                            custom.ErrorMessage = "This selection has already been added.";
                        }
                        else
                        {
                            ProductAccessProduct p = new ProductAccessProduct();
                            p.DtStamp = DateTime.Now;
                            p.TProductAccessId = Atx.CurrentAccessCampaign.Id;
                            p.ProductContext = _Enums.ProductAccessProductContext.merch;
                            p.TParentId = selectedIdx;
                            p.Save();

                            Atx.CurrentAccessCampaign.ProductAccessProductRecords().Add(p);

                            form.DataBind();
                        }
                    }
                    break;
            }
        }

        protected void FormEditor_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;
            _errors.Clear();

            CustomValidator validation = (CustomValidator)form.FindControl("CustomValidation");
            TextBox tb = (TextBox)form.FindControl("txtName");
            Utils.Validation.ValidateRequiredField(_errors, "CampaignName", tb.Text);

            if (Utils.Validation.IncurredErrors(_errors, validation))
            {
                e.Cancel = true;
                return;
            }

            //try to insert
            try
            {
                ProductAccess newOne = Atx.ProductAccessCampaigns.AddToCollection(tb.Text.Trim());
                Atx.SetCurrentAccessCampaign(newOne.Id);
                GridListing.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _errors.Add(ex.Message);
            }

            if (Utils.Validation.IncurredErrors(_errors, validation))
            {
                e.Cancel = true;
                return;
            }

            form.ChangeMode(FormViewMode.Edit);
            GridListing.DataBind();
        }
        protected void FormEditor_ModeChanging(object sender, FormViewModeEventArgs e)
        {
            FormView form = (FormView)sender;
            form.ChangeMode(e.NewMode);
            if (e.CancelingEdit)
                form.DataBind();
        }
        #endregion

        protected void SqlAccess_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
}
}