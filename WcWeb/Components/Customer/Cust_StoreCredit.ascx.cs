using System;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb.Components.Customer
{
    public partial class Cust_StoreCredit : WillCallWeb.BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatus.Text = string.Empty;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string input = txtCode.Text.Trim();
            if (input.Length > 0)
            {
                try
                {
                    //remove quotes, and brackets....
                    input = input.Replace("\"","").Replace("'", "").Replace("{", "").Replace("}", "").Replace(" ","");

                    //clean input - code can only contain letters, numbers and dashes
                    if (!Utils.Validation.IsValidGuid(input))
                        throw new Exception("The code you have entered is not valid. Please see the example.");

                    //ensure that the gift has not been redeemed - to ANYONE
                    decimal redeemed = WillCallWeb.StoreObjects.SaleItem_StoreCredit.RedeemGiftCertificate(Profile.UserName, input);
                    this.Profile.Save();

                    //log cust event
                    MembershipUser mem = Membership.GetUser(Profile.UserName);//use to get user id
                    UserEvent.RecordStoreCreditEvent(Profile.UserName, (Guid)mem.ProviderUserKey, Profile.UserName, redeemed, 
                        decimal.Parse(this.Profile.StoreCredit.ToString()), "redeemed", null);

                    lblStatus.Text = "<div class=\"credit-status\">Your gift certificate has been applied to your account!</div>";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
                catch (Exception ex)
                {
                    lblStatus.Text = string.Format("<div class=\"credit-status\">{0}</div>", ex.Message);
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }

            txtCode.Text = string.Empty;
            GridCredits.DataBind();
        }

        protected int _rowCounter = 0;
        protected void GridCredits_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            _rowCounter = grid.PageSize * grid.PageIndex;
        }
        protected void GridCredits_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //display row number
                _rowCounter += 1;
                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                GridViewRow gvr = e.Row;
                DataRowView drv = (DataRowView)gvr.DataItem;
               
                Literal litComment = (Literal)e.Row.FindControl("litComment");
                if (litComment != null)
                {
                    string comment = drv["Comment"].ToString();
                    if (comment.ToLower().IndexOf("invoiceid:") != -1)
                    {
                        string[] parts = comment.Split(':');
                        if (parts.Length == 2)
                            comment = string.Format("<a href=\"/Store/Confirmation.aspx?inv={0}\">view invoice</a>", parts[1].Trim());
                    }
                    litComment.Text = comment;
                }
            }
        }

        #region Sql Selecting
        protected void Sql_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = _Config.APPLICATION_ID;
        }
        #endregion
}
}
