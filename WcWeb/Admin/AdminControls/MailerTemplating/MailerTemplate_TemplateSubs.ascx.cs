using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    public partial class MailerTemplate_TemplateSubs : BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Atx.CurrentMailerTemplate == null)
                base.Redirect("/Admin/Mailers.aspx?p=select");

            if(!IsPostBack)
                listTemplate.DataBind();
        }

        protected MailerTemplateContent CurrentTemplateContent
        {
            get
            {
                if(listTemplate.DataKeys.Count == 0)
                   return null;

                if(listTemplate.SelectedIndex == -1)
                    listTemplate.SelectedIndex = 0;

                int idx = (int)listTemplate.SelectedValue;

                return (MailerTemplateContent)Atx.CurrentMailerTemplate.MailerTemplateContentRecords().Find(idx);
            }
        }
        //select
        protected void SqlMailerTemplateContentList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@templateId"].Value = Atx.CurrentMailerTemplate.Id;
        }
        protected void SqlMailerTemplateContentList_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            if (e.AffectedRows > 0 && listTemplate.SelectedIndex == -1)
            {
                listTemplate.SelectedIndex = 0;
            }
        }
        protected void listTemplate_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataList dl = (DataList)sender;

            //throw an event on the last item bound
            if (e.Item.ItemIndex == dl.DataKeys.Count -1)
            {
                listTemplate_DataBound(dl, new EventArgs());
            }
        }
        protected void listTemplate_DataBound(object sender, EventArgs e)
        {
            txtTemplate.DataBind();
        }
        protected void listTemplate_SelectionChanged(object sender, EventArgs e)
        {
            txtTemplate.DataBind();
        }
        protected void txtTemplate_DataBinding(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            MailerTemplateContent cont = CurrentTemplateContent;
            txt.Text = (cont != null) ? cont.Template : string.Empty;

            GridSubs.DataBind();
        }


        #region Subs GRID
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //iterate thru rows and save changes
            GridView grid = GridSubs;

            foreach (GridViewRow gvr in grid.Rows)
            {
                int idx = (int)grid.DataKeys[gvr.RowIndex].Value;
                if (idx > 0)
                {
                    MailerTemplateSubstitution mts = (MailerTemplateSubstitution)CurrentTemplateContent.MailerTemplateSubstitutionRecords().Find(idx);

                    if (mts != null)
                    {
                        string txtName = ((TextBox)gvr.FindControl("txtName")).Text.Trim();
                        string txtValue = ((TextBox)gvr.FindControl("txtValue")).Text.Trim();

                        if((!txtName.StartsWith("<")) || (!txtName.EndsWith(">")))
                        {
                            CustomValidation.IsValid = false;
                            CustomValidation.ErrorMessage = "TagName must begin with a &lt; and end with a &gt;";
                            return;
                        }

                        bool isDirty = false;
                        if (txtName != mts.TagName)
                        {
                            mts.TagName = txtName.ToUpper();
                            isDirty = true;
                        }
                        if (txtValue != mts.TagValue)
                        {
                            mts.TagValue = txtValue;
                            isDirty = true;
                        }

                        if (isDirty)
                        {
                            mts.Save();
                        }
                    }
                }
            }

            grid.DataBind();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            GridSubs.DataBind();
        }
        protected void GridSubs_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (this.CurrentTemplateContent != null)
            {
                MailerTemplateSubstitutionCollection coll = new MailerTemplateSubstitutionCollection();
                coll.AddRange(this.CurrentTemplateContent.MailerTemplateSubstitutionRecords());
                if (coll.Count > 1)
                    coll.Sort("TagName", true);

                MailerTemplateSubstitution blank = new MailerTemplateSubstitution();
                blank.TagName = string.Empty;
                blank.TagValue = string.Empty;

                coll.Add(blank);

                grid.DataSource = coll;
            }
        }
        protected void GridSubs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Grid_GenericBind(sender, e);
            GridView grid = (GridView)sender;
            int rowCount = ((MailerTemplateSubstitutionCollection)grid.DataSource).Count;

            //when editing - turn of the other rows buttons
            Button btnAddNew = (Button)e.Row.FindControl("btnAddNew");
            LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");

            if(btnAddNew != null)
                btnAddNew.Visible = (e.Row.RowIndex == rowCount - 1);
            if(btnDelete != null)
                btnDelete.Visible = (e.Row.RowIndex != (rowCount - 1));
        }
        protected void GridSubs_Deleting(object sender, GridViewDeleteEventArgs e)
        {
            //handled in row commands
        }
        protected void GridSubs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.Trim().ToLower();

            if (cmd == "addnew" || cmd == "delete")
            {
                MailerTemplateContent mtc =  this.CurrentTemplateContent;

                switch (cmd)
                {
                    case "delete":
                        int idx = int.Parse(e.CommandArgument.ToString());
                        MailerTemplateSubstitution delItem = (MailerTemplateSubstitution)mtc.MailerTemplateSubstitutionRecords().Find(idx);
                        if (delItem != null)
                        {
                            mtc.MailerTemplateSubstitutionRecords().Remove(delItem);
                            mtc.MailerTemplateSubstitutionRecords().SaveAll();
                        }
                        break;

                    case "addnew":
                        //get values
                        GridViewRow gvr = grid.Rows[grid.Rows.Count-1];
                        TextBox txtName = (TextBox)gvr.FindControl("txtName");
                        TextBox txtValue = (TextBox)gvr.FindControl("txtValue");

                        if (txtName != null && txtValue != null && txtName.Text.Trim().Length > 0)
                        {
                            string tagName = txtName.Text.Trim();
                            if((!tagName.StartsWith("<")) || (!tagName.EndsWith(">")))
                            {
                                CustomValidation.IsValid = false;
                                CustomValidation.ErrorMessage = "TagName must begin with a &lt; and end with a &gt;";
                                return;
                            }

                            MailerTemplateSubstitution sub = new MailerTemplateSubstitution();
                            sub.DtStamp = DateTime.Now;
                            sub.TMailerTemplateContentId = mtc.Id;
                            sub.TagName = tagName.ToUpper();
                            sub.TagValue = txtValue.Text.Trim();

                            mtc.MailerTemplateSubstitutionRecords().Add(sub);

                            //coll.SaveAll();
                            mtc.MailerTemplateSubstitutionRecords().SaveAll();
                        }

                        break;
                }

                
                //rebind
                grid.DataBind();
            }
        }



        #endregion
    }
}