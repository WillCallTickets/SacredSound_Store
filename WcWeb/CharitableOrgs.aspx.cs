using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb
{
    public partial class CharitableOrgs : BasePage
    {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rptEnt.DataBind();
        }

        protected void rptEnt_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (rpt.DataSource == null)
            {
                rpt.DataSource = _Lookits.CharityListings;
            }

            rpt.Visible = (_Lookits.CharityListings.Count > 0);
        }
        protected void rptEnt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            ListItemType lit = e.Item.ItemType;

            if (lit == ListItemType.Header)
            {
                Literal litHeader = (Literal)e.Item.FindControl("litHeader");
                if (litHeader != null)//also make sure we have a title to display
                {
                    litHeader.Text = string.Format("<div class=\"header\">{0}</div>", _Config._Message_Goodwill);
                }
            }
            else if (lit != ListItemType.Footer && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                CharitableListing listing = (CharitableListing)e.Item.DataItem;
                if (listing != null)
                {
                    Literal litStart = (Literal)e.Item.FindControl("litStart");
                    Literal litEnd = (Literal)e.Item.FindControl("litEnd");
                    if (listing.TopBilling_Effective && litStart != null && litEnd != null)
                    {
                        litStart.Text = "<div class=\"topbilling\">";
                        litEnd.Text = "</div>";
                    }

                    //name, short, writeup
                    Literal litName = (Literal)e.Item.FindControl("litName");
                    Literal litShort = (Literal)e.Item.FindControl("litShort");
                    Literal litWriteup = (Literal)e.Item.FindControl("litWriteup");

                    if (litName != null)
                    {
                        //if we have a link than use it
                        string link = (Utils.Validation.IsValidUrl(listing.CharitableOrgRecord.WebsiteUrl)) ? 
                            string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", listing.CharitableOrgRecord.WebsiteUrl.Trim(), listing.CharitableOrgRecord.Name_Displayable.Trim()) : 
                            listing.CharitableOrgRecord.Name_Displayable.Trim();

                        litName.Text = string.Format("<div class=\"name\">{0}</div>", link);
                    }
                    if (litShort != null && listing.CharitableOrgRecord.ShortDescription != null && listing.CharitableOrgRecord.ShortDescription.Trim().Length > 0)
                    {
                        litShort.Text = string.Format("<div class=\"short\">{0}</div>", listing.CharitableOrgRecord.ShortDescription.Trim());
                    }
                    if (litWriteup != null && listing.CharitableOrgRecord.Description != null && listing.CharitableOrgRecord.Description.Trim().Length > 0)
                    {
                        litWriteup.Text = string.Format("<div class=\"writeup\">{0}</div>", listing.CharitableOrgRecord.Description.Trim());
                    }

                    Literal litStartContainer = (Literal)e.Item.FindControl("litStartContainer");
                    Literal litEndContainer = (Literal)e.Item.FindControl("litEndContainer");
                    if ((litShort.Text.Trim().Length > 0 || litWriteup.Text.Trim().Length > 0) && litStartContainer != null && litEndContainer != null)
                    {
                        litStartContainer.Text = "<div class=\"container\">";
                        litEndContainer.Text = "</div>";
                    }
                }
            }
        }
       
       
}
}
