using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace WillCallWeb.Components.Cart
{
    public partial class Charity : WillCallWeb.BaseControl
    {
        protected Wcss.CharitableListingCollection coll = new Wcss.CharitableListingCollection();

        private void EventHandler_CartChanged(object sender, EventArgs e)
        {
            if (Ctx.Cart.PreFeeTotal == 0)
                this.Controls.Clear();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            coll.CopyFrom(Wcss._Lookits.CharityListings);
            coll.GetList().RemoveAll(delegate(Wcss.CharitableListing match) { return (!match.IsAvailableForContribution); });

            //be able to notify the shopping cart so it can update totals
            Ctx.Cart.CartChanged += new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);
        }
        public override void Dispose()
        {
            //be able to notify the shopping cart so it can update totals
            Ctx.Cart.CartChanged -= new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);

            base.Dispose();
        }
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (Ctx.Cart != null && Ctx.Cart.HasItems && Wcss._Config._Donations_Active && coll.Count > 0)
            {
                rdoListings.DataBind();
                chkDonate.DataBind();
                ddlAmounts.DataBind();
                chkDonate.DataBind();
            }
            else
                this.Controls.Clear();
        }

        protected void rdoListings_DataBinding(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            //configure based on num of listings available for donation
            rdoListings.Visible = false;
            litCharitablelisting.Visible = false;
            divListing.Visible = false;

            string linkToGoodwill = string.Format("<a href=\"/CharitableOrgs.aspx\">more details</a>");

            //literal is based on num items
            if (coll.Count > 1)
            {
                divListing.Visible = true;
                litCharitablelisting.Visible = true;
                litCharitablelisting.Text = string.Format("&laquo; Help support selected charities &raquo; {0}", linkToGoodwill);

                rdoListings.Visible = true;
                rbl.DataSource = coll;
                rbl.DataTextField = "OrgName_Displayable";
                rbl.DataValueField = "Id";            
            }
            else if(coll.Count == 1)
            {
                litCharitablelisting.Visible = true;
                
                string webUrl = coll[0].CharitableOrgRecord.WebsiteUrl;
                if(webUrl != null && webUrl.Trim().Length > 0 && Utils.Validation.IsValidUrl(webUrl))
                    webUrl = string.Format("<a target=\"_blank\" href=\"{0}\">more details</a>", Utils.ParseHelper.FormatUrlFromString(webUrl.Trim()));
                else
                    webUrl = linkToGoodwill;

                litCharitablelisting.Text = string.Format("&laquo; Help support <span class=\"org\">{0}</span> &raquo; {1}", 
                    coll[0].CharitableOrgRecord.Name_Displayable, webUrl);
            }
        }
        protected void  rdoListings_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            //if the cart matches an entry....
            if (Ctx.Cart.CharityOrg != null && rbl.SelectedValue != Ctx.Cart.CharityOrg.Name_Displayable)
            {
                ListItem li = rbl.Items.FindByText(Ctx.Cart.CharityOrg.Name_Displayable);
                if (li != null)
                    li.Selected = true;
            }

            if(rbl.Items.Count > 0 && rbl.SelectedIndex == -1)
                rbl.SelectedIndex = 0;
        }
        protected void rdoListings_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;

            if (rbl.SelectedIndex != -1)
            {
                Wcss.CharitableListing listing = (Wcss.CharitableListing)coll.Find(int.Parse(rbl.SelectedValue));
                if (listing != null && listing.CharitableOrgRecord != null)
                    Ctx.Cart.CharityOrg = listing.CharitableOrgRecord;
            }
        }
        protected void chkDonate_DataBinding(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            //bind to the cart's donation value - if no match, ensure that amount gets cleared too
            chk.Checked = (Ctx.Cart.CharityAmount > 0);
        }
        protected void chkDonate_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            //if we are choosing - then get amount to set for donation
            //update the cart's donation amount - when 0, org is set to null
            if (chk.Checked)
            {
                if(Wcss._Config._CharityAmounts.Count == 1)
                    Ctx.Cart.CharityAmount = Wcss._Config._CharityAmounts[0];
                else
                    Ctx.Cart.CharityAmount = decimal.Parse(ddlAmounts.SelectedValue);

                AssignCharityOrg();
            }
            else
                Ctx.Cart.CharityAmount = 0;            

            //notify the cart that it has changed
            Ctx.Cart.OnCartChanged();

        }
        private void AssignCharityOrg()
        {
            //assign the charity org
            if (coll.Count == 1)
                Ctx.Cart.CharityOrg = coll[0].CharitableOrgRecord;
            else if (rdoListings.Visible && rdoListings.SelectedIndex != -1)
            {
                Wcss.CharitableListing listing = (Wcss.CharitableListing)coll.Find(int.Parse(rdoListings.SelectedValue));
                if (listing != null)
                    Ctx.Cart.CharityOrg = listing.CharitableOrgRecord;
            }
        }
        protected void ddlAmounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            //only update the cart if the the check box is checked!!! otherwise ignore as the checkbox checked will assign amount
            if (chkDonate.Checked && ddl.SelectedValue != Ctx.Cart.CharityAmount.ToString("n2"))
            {
                Ctx.Cart.CharityAmount = decimal.Parse(ddlAmounts.SelectedValue);
                AssignCharityOrg();

                //notify the cart that it has changed
                Ctx.Cart.OnCartChanged();
            }
        }
        protected void ddlAmounts_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ddl.Visible = false;
            litAmount.Visible = false;

            if (Wcss._Config._CharityAmounts.Count > 1)
            {
                ddl.Visible = true;
                if (ddl.Items.Count == 0)
                {
                    ddl.DataSource = Wcss._Config._CharityAmounts;
                    ddl.DataTextFormatString = "{0:n2}";
                }
            }
            else
            {
                litAmount.Visible = true;
                litAmount.Text = Wcss._Config._CharityAmounts[0].ToString("n2");
            }
        }
        protected void ddlAmounts_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (Ctx.Cart.CharityAmount > 0 && ddl.SelectedValue != Ctx.Cart.CharityAmount.ToString("n2"))
            {
                ddl.SelectedIndex = -1;
                if (Ctx.Cart.CharityAmount > 0)
                {
                    ListItem li = ddl.Items.FindByText(Ctx.Cart.CharityAmount.ToString("n2"));
                    if (li != null)
                        li.Selected = true;
                }
            }
        }
}
}