using System;
using System.Data;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Customer_SalesHistory : WillCallWeb.BaseControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                // show the user's details
                GridView1.DataBind();
            }
        }
        protected int _rowCounter = 0;

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            //page # * pagesize
            _rowCounter = grid.PageSize * grid.PageIndex;
        }
        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            //turn off select button - hide
            
            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;
        }
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //change to view confirmation
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                _rowCounter += 1;

                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                CustomerInvoiceRow entity = null;

                if(e.Row.DataItem != null)
                    entity = (CustomerInvoiceRow)e.Row.DataItem;

                if(entity != null)
                {
                    LinkButton viewConfirm = (LinkButton)e.Row.FindControl("linkViewConfirm");
                    if (viewConfirm != null)
                    {
                        viewConfirm.Text = (entity.InvoiceStatus != "Void") ? "view details" : "void";
                        viewConfirm.Enabled = (entity.InvoiceStatus != "Void");
                    }

                    Literal description = (Literal)e.Row.FindControl("LiteralDescription");
                    Invoice _invoice = Invoice.FetchByID(entity.InvoiceId);

                    if (_invoice != null && description != null)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();

                        sb.Append("<div class=\"customerinvoicerow\">");
                        
                        sb.Append(Invoice.InterpretProductDescription(this.Page.User.IsInRole("Administrator"), 
                            _invoice, Ctx.SaleTickets, Ctx.SaleMerch));
                        
                        sb.Append("</div>");

                        description.Text = sb.ToString();
                    }
                }
            }
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string cmd = e.CommandName.ToLower();
            string arg = e.CommandArgument.ToString();

            switch (cmd)
            {
                case "confirm":
                case "linkinvoice":
                    Ctx.OrderProcessingVariables = null;
                    Ctx.InvoiceId = int.Parse(arg.ToString());
                    base.Redirect(string.Format("/Store/Confirmation.aspx?inv={0}", arg));
                    break;
            }
        }
}
}
