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
    public partial class CampaignMailer : BaseControl
    {
        List<string> _errors = new List<string>();

        #region Page Overhead
        
        protected override void OnLoad(EventArgs e)
        {
            if (Atx.ProductAccessCampaigns.Count == 0 || Atx.CurrentAccessCampaign == null)
                base.Redirect("/Admin/ProductAccess.aspx?p=campaign");

            if (!IsPostBack)
            {
                GridListing.DataBind();
            }

           btnCampaignList.Enabled = (Atx.CurrentAccessCampaign != null);
        }

        protected void btnCampaignList_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            base.Redirect("/Admin/ProductAccess.aspx?p=campaign");
        }
        protected void btnCampaignUser_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            base.Redirect("/Admin/ProductAccess.aspx?p=usr");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //if there are no users selected - notify
            List<int> selected = this.GridUsers_GetSelectedRows;
            if (selected.Count == 0)
            {
                Custom.IsValid = false;
                Custom.ErrorMessage = "Please select users from the grid to send the mailer to.";
            }
            else 
            {
                WillCallWeb.Admin.AdminControls.Creators.CustomerMailer camp = this.CustomerMailer1;
                if (camp.ControlLetter == null)
                {
                    Custom.IsValid = false;
                    Custom.ErrorMessage = "Please select a mailier template.";
                    return;
                }

                int count = 0;
                DateTime dateToProcess = DateTime.Now;

                //get emailtemplate
                string mappedFile = Server.MapPath(camp.ControlLetter.HtmlVersion);
                string file = Utils.FileLoader.FileToString(mappedFile);

                //loop thru selected users and setup params for email
                //put into queue and deliver
                foreach (GridViewRow gvr in GridUsers.Rows)
                {
                    int idx = gvr.DataItemIndex;
                    int dKey = (int)GridUsers.DataKeys[idx]["Id"];

                    if (selected.Contains(dKey))
                    {
                        //send email to user

                        //get user data from dataitem - productAccessUser
                        //e.[UserName], ISNULL(au.[UserId],null) as [UserId], e.[iQuantityAllowed]
                        string userName = GridUsers.DataKeys[idx]["UserName"].ToString();
                        //string userId = GridUsers.DataKeys[idx]["UserId"];
                        int quota = (int)GridUsers.DataKeys[idx]["iQuantityAllowed"];
                        //Guid userGuid;

                        //if (userId != null && userId.Trim().Length > 0)
                        //    userGuid = new Guid(userId);

                        if (quota > 0)
                        {
                            //create replacement dictionary
                            //EmailAddress,Quota,StartDate,EndDate
                            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();
                            dict.Add("<PARAM>EMAILADDRESS</PARAM>", userName);
                            dict.Add("<PARAM>QUOTA</PARAM>", quota.ToString());

                            if(Atx.CurrentAccessCampaign.ActivationWindowRecord != null)
                            {
                                dict.Add("<PARAM>STARTDATE</PARAM>", Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicStart.ToString("MM/dd/yyyy hh:mmtt"));
                                dict.Add("<PARAM>ENDDATE</PARAM>", Atx.CurrentAccessCampaign.ActivationWindowRecord.DatePublicEnd.ToString("MM/dd/yyyy hh:mmtt"));
                            }

                            MailQueue.SendEmail(_Config._CustomerService_Email, _Config._CustomerService_FromName, userName, null, null,
                                camp.ControlLetter.Subject, file, null, dict, true, camp.ControlLetter.Name);

                            count += 1;
                        }
                    }
                }

                if (count > 0)
                {
                    lblStatus.Text = string.Format("{0} Email{1} ha{2} been sent", count, (count > 1) ? "s" : string.Empty, (count > 1) ? "ve" : "s");
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Visible = true;
                }
            }
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
            }
        }
        protected void GridListing_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
            {
                if (Atx.CurrentAccessCampaign == null)
                {
                    grid.SelectedIndex = 0;
                    int selIdx = (int)grid.SelectedDataKey.Value;
                    Atx.SetCurrentAccessCampaign(selIdx);
                }
                else
                {
                    int idx = Atx.CurrentAccessCampaign.Id;
                    foreach (GridViewRow gvr in grid.Rows)
                    {
                        if ((int)grid.DataKeys[gvr.DataItemIndex]["Id"] == idx)
                        {
                            grid.SelectedIndex = gvr.DataItemIndex;
                            break;
                        }
                    }
                }
            }

            //FormEditor.DataBind();
            GridUsers.DataBind();
        }
        protected void GridListing_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            //if (FormEditor.CurrentMode != FormViewMode.Edit)
            //    FormEditor.ChangeMode(FormViewMode.Edit);

            //match selected index to current object
            int selIdx = (int)grid.SelectedDataKey.Value;
            Atx.SetCurrentAccessCampaign(selIdx);

            //FormEditor.DataBind();
            //GridUsers_ClearSelectedRows();
            //GridUsers.DataBind();
        }
        protected void GridListing_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            //if (FormEditor.CurrentMode != FormViewMode.Edit)
            //    FormEditor.ChangeMode(FormViewMode.Edit);
        }

        #endregion       

        #region Grid Users
        //http://www.4guysfromrolla.com/articles/053106-1.aspx

        protected void GridCmd_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;            
            string id = btn.ID;

            //GridView grid = GridUsers;
            CustomValidator custom = null;
            bool clearSelections = false;

            int counter = 0;
            _errors.Clear();
            List<int> selectedIds = null;

            //inputs
            //string qty = txtQty.Text.Trim();
            //int inpQty = 0;
            //string referral = txtReferral.Text.Trim();
            //string instructions = txtInstructions.Text.Trim();

        }

        protected List<int> GridUsers_GetSelectedRows
        {
            get
            {
                List<int> list = new List<int>();

                foreach (GridViewRow gvr in GridUsers.Rows)
                    if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                        list.Add((int)GridUsers.DataKeys[gvr.DataItemIndex]["Id"]);

                return list;
            }
        }
        protected void GridUsers_ClearSelectedRows()
        {
            ((CheckBox)GridUsers.HeaderRow.FindControl("chkMaster")).Checked = false;
            foreach (GridViewRow gvr in GridUsers.Rows)
                if(((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    ((CheckBox)gvr.FindControl("chkSelect")).Checked = false;
        }
        protected void GridUsers_ResetSelectedRows(List<int> selectedIds)
        {
            foreach (GridViewRow gvr in GridUsers.Rows)
                if(selectedIds.Contains((int)GridUsers.DataKeys[gvr.DataItemIndex]["Id"]))
                    ((CheckBox)gvr.FindControl("chkSelect")).Checked = true;
        }

        protected void GridUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            GridViewRow gvr = e.Row;

            if (gvr.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)gvr.DataItem;
                object userId = drv["UserId"];

                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                if (chkSelect != null)
                {

                }

                CheckBox chkRegistered = (CheckBox)gvr.FindControl("chkRegistered");
                if (chkRegistered != null)
                    chkRegistered.Checked = userId.ToString().Length > 0;
            }
        }

        #endregion
        
        protected void SqlAccess_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
}
}
