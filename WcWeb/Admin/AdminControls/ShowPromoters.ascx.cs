using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class ShowPromoters : BaseControl
    {
        #region Collections and Page Objects

        protected JShowPromoterCollection OrderedCollection
        {
            get
            {
                JShowPromoterCollection coll = new JShowPromoterCollection();
                coll.AddRange(Atx.CurrentShowRecord.JShowPromoterRecords());
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);
                return coll;
            }
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

        protected void btnSales_Click(object sender, EventArgs e)
        {
            base.Redirect(string.Format("/Admin/Listings.aspx?p=tickets&showid={0}",
                (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.Id.ToString() : "0"));
        }

        #region GridView

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView1.DataSource = OrderedCollection;
            string[] keyNames = { "Id" };
            GridView1.DataKeyNames = keyNames;
        }
        
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;

            if (typ == DataControlRowType.DataRow)
            {
                JShowPromoter ent = (JShowPromoter)e.Row.DataItem;

                Literal litText = (Literal)e.Row.FindControl("litText");
                if (litText != null && ent != null)
                    litText.Text = string.Format("{0} {1} {2} {3}", ent.PreText, ent.PromoterName, ent.PromoterText, ent.PostText).Trim();
                
                LinkButton select = (LinkButton)e.Row.FindControl("btnSelect");
                LinkButton delete = (LinkButton)e.Row.FindControl("btnDelete");
                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

                if (delete != null && ent != null)
                    delete.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(ent.PromoterName));

                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);
                    down.Enabled = (e.Row.RowIndex < OrderedCollection.Count - 1);
                }
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.DataSource != null && OrderedCollection.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            FormView1.DataBind();
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            if (FormView1.CurrentMode != FormViewMode.Edit)
                FormView1.ChangeMode(FormViewMode.Edit);

            switch (cmd)
            {
                case "up":
                case "down":
                    JShowPromoter moved = Atx.CurrentShowRecord.JShowPromoterRecords().ReorderItem(int.Parse(e.CommandArgument.ToString()), cmd);
                    //set the index of the moved item
                    grid.SelectedIndex = moved.DisplayOrder;
                    grid.DataBind();
                    
                    break;
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //todo select idx from data key
            GridView grid = (GridView)sender;
            FormView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex].Value;

            e.Cancel = (!Atx.CurrentShowRecord.JShowPromoterRecords().DeleteFromCollection(idx));

            //reset show data
            int index = Atx.CurrentShowRecord.Id;
            Atx.SetCurrentShowRecord(index);

            if (!e.Cancel)
            {
                grid.SelectedIndex = e.RowIndex - 1;
                grid.DataBind();
            }
        }

        #endregion

        #region Details

        protected void FormView1_DataBinding(object sender, EventArgs e)
        {
            int idx = (GridView1.SelectedDataKey != null) ? (int)GridView1.SelectedDataKey["Id"] : 0;

            JShowPromoterCollection selected = new JShowPromoterCollection();
            JShowPromoter addPromo = (JShowPromoter)Atx.CurrentShowRecord.JShowPromoterRecords().Find(idx);
            if(addPromo != null)
                selected.Add(addPromo);

            FormView1.DataSource = selected;
            string[] keyNames = { "Id" };
            FormView1.DataKeyNames = keyNames;
        }
        protected void FormView1_DataBound(object sender, EventArgs e)
        {
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
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "cancel":
                    FormView1.ChangeMode(FormViewMode.Edit);
                    break;
                case "new":
                    Atx.CurrentPromoterId = 0;
                    break;
            }
        }
        protected void FormView1_ItemInserting(object sender, FormViewInsertEventArgs e)
        {
            FormView form = (FormView)sender;

            Editor_Promoter promo = (Editor_Promoter)form.FindControl("Editor_Promoter1");
            
            int promoIdx = -1;

            try
            {
                promoIdx = promo.SelectedIdx;

                if(promoIdx == 0)
                    throw new Exception("Please choose a promoter.");

                JShowPromoter existing = (JShowPromoter)OrderedCollection.GetList().Find(
                    delegate (JShowPromoter match) { return (match.PromoterRecord.Id == promoIdx); } );
                if (existing != null)
                    throw new Exception("This promoter already exists in the collection.");

                JShowPromoter added = Atx.CurrentShowRecord.JShowPromoterRecords()
                    .AddPromoterToCollection(Atx.CurrentShowRecord.Id, promoIdx);

                FormView1.ChangeMode(FormViewMode.Edit);

                GridView1.SelectedIndex = added.DisplayOrder;//set to new item
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
            }
        }
        protected void FormView1_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            FormView form = (FormView)sender;

            TextBox pre = (TextBox)form.FindControl("txtPreText");
            TextBox promoter = (TextBox)form.FindControl("txtPromoterText");
            TextBox post = (TextBox)form.FindControl("txtPostText");

            try
            {
                JShowPromoter update = (JShowPromoter)Atx.CurrentShowRecord.JShowPromoterRecords().Find((int)form.SelectedValue);
                if (update != null && pre != null && post != null)
                {
                    update.PreText = pre.Text.Trim();
                    update.PromoterText = promoter.Text.Trim();
                    update.PostText = post.Text.Trim();

                    update.Save();

                    //reset show data
                    int index = Atx.CurrentShowRecord.Id;
                    Atx.SetCurrentShowRecord(index);

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
                }
            }
        }

        #endregion
    }
}