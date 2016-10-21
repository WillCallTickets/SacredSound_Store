using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Menu_Admin : BaseControl
    {
        protected override void OnLoad(EventArgs e)
        {
            litMenu.DataBind();         
        }
        
        protected void litMenu_DataBinding(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //int depth = 0;
            string page = this.Page.ToString().ToLower();
            
            sb.AppendLine("<div id=\"accordion\" class=\"rounded\">");

            //publish
            if (this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("ContentEditor"))
            {
                //this button is handled with jquery - see JQueryUI/stuHover.js for handlers
                sb.AppendLine(string.Format("<a id=\"publishMenuButton\" class=\"publish\" >Publish</a>", ""));
            }

            //ticket manifests
            bool isTicketRequest = (this.Request.QueryString.ToString().ToLower().IndexOf("p=merch") == -1);
            if (this.Page.User.IsInRole("Manifester") || this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Listings.aspx?p=tix&showid=0\">Ticket Manifest</a>",
                    (page == "asp.admin_listings_aspx" && isTicketRequest) ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                //sb.AppendLine("<a href=\"/Admin/Listings.aspx?p=tickets&showid=0\">manifest old</a>");
                sb.AppendLine("<a href=\"/Admin/Listings.aspx?p=tix&showid=0\">manifests</a>");
                sb.AppendLine("</div>");
            }

            //customers
            if (this.Page.User.IsInRole("OrderFiller") || this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/CustomerEditor.aspx\">Customers</a>", 
                    (page == "asp.admin_customereditor_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\"><a href=\"/Admin/CustomerEditor.aspx?p=customerpicker\">select</a></div>");
            }

            //sales
            if (this.Page.User.IsInRole("OrderFiller") || this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Orders.aspx\">Sales</a>", 
                    (page == "asp.admin_orders_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/Orders.aspx?p=recentorders\">orders</a>");
                sb.AppendLine("<a href=\"/Admin/Orders.aspx?p=shiplist\">shipments</a>");
                sb.AppendLine("</div>");
            }

            //reports
            if (this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("ReportViewer") || this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Reports.aspx?p=allsales\">Reports</a>", 
                    (page == "asp.admin_reports_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=allsales\">all sales</a>");
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=merchdetail\">category breakdown</a>"); 
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=counts\">date overview</a>");
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=tickets\">date breakdown</a>");
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=tix\">tix inventory</a>");
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=merch\">merch inventory</a>");
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=bundle\">bundle report</a>");
                sb.AppendLine("<a href=\"/Admin/Reports.aspx?p=period\">svc fee breakdown</a>");                
                sb.AppendLine("</div>");
            }

            //mailers
            if (this.Page.User.IsInRole("MassMailer") || this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Mailers.aspx\">Mailers</a>", 
                    (page == "asp.admin_mailers_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/Mailers.aspx?p=select\">templates</a>");
                sb.AppendLine("<a href=\"/Admin/Mailers.aspx?p=edit\">edit mailer</a>");
                sb.AppendLine("<a href=\"/Admin/Mailers.aspx?p=send\">send mailer</a>");
                sb.AppendLine("<a href=\"/Admin/Mailers.aspx?p=subscription\">subscriptions</a>");
                sb.AppendLine("<a href=\"/Admin/Mailers.aspx?p=customer\">customer email</a>");
                sb.AppendLine("<a href=\"/Admin/Mailers.aspx?p=list\">customer lists</a>");
                sb.AppendLine("</div>");
            }

            //batch shipping
            if (this.Page.User.IsInRole("OrderFiller") || this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Shipping_Tickets.aspx?p=batchview\">Batch Shipping</a>", 
                    (page == "asp.admin_shipping_tickets_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/Shipping_Tickets.aspx?p=fill\">Create Batch</a>");
                sb.AppendLine("<a href=\"/Admin/Shipping_Tickets.aspx?p=batchview\">View Batches</a>");
                sb.AppendLine("</div>");
            }

            //shows
            if (this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("ContentEditor"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/ShowEditor.aspx\">Shows</a>", 
                    (page == "asp.admin_showeditor_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx\">select/create</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=details\">show details</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=showdate\">dates</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=acts\">acts</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=tickets\">tickets</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=postp\">post purchase</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=reqs\">ticket reqs</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=promoter\">promoters</a>");
                sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=showlinks\">links</a>");

                //if (this.Page.User.IsInRole("_Master"))
                //    sb.AppendLine("<a href=\"/Admin/ShowEditor.aspx?p=bundle&ctx=show\">bundles</a>");

                sb.AppendFormat("<a href=\"/Admin/Listings.aspx?p=tickets&showid={0}\">sales</a>",
                    (Atx.CurrentShowRecord != null) ? Atx.CurrentShowRecord.Id.ToString() : "0");
                
                sb.AppendLine("</div>");
            }

            //merch
            if (this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("ContentEditor"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/MerchEditor.aspx\">Merch</a>", 
                    ((page.IndexOf("asp.admin_mercheditor", StringComparison.OrdinalIgnoreCase) != -1)
                    || (page.IndexOf("asp.admin_downloadeditor", StringComparison.OrdinalIgnoreCase) != -1))
                    ? " current" : ""));

                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/MerchEditor.aspx?p=legacypicker\">alt picker</a>");
                sb.AppendLine("<a href=\"/Admin/DownloadEditor.aspx?p=downloads\">downloads</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=color\">colors</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=size\">sizes</a>"); 
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=mjcorder\">organization</a>");                
                sb.AppendLine("<a href=\"/Admin/MerchEditor.aspx?p=fetord\">featured order</a>");
                sb.AppendLine("<a href=\"/Admin/MerchEditor.aspx?p=merchover18\">over 18 items</a>");
                sb.AppendLine("</div>");
            }

            //product access
            if (this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("ContentEditor"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/ProductAccess.aspx\">Product Access</a>",
                    ((page.IndexOf("asp.admin_productaccess", StringComparison.OrdinalIgnoreCase) != -1)) ? " current" : ""));

                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/ProductAccess.aspx?p=campaign\">Campaign Listing</a>");
                sb.AppendFormat("<a href=\"{0}\">Campaign Users</a>", (Atx.ProductAccessCampaigns.Count > 0) ? "/Admin/ProductAccess.aspx?p=usr" : string.Empty);
                sb.AppendFormat("<a href=\"{0}\">Campaign Mailer</a>", (Atx.ProductAccessCampaigns.Count > 0) ? "/Admin/ProductAccess.aspx?p=mlr" : string.Empty);
                sb.AppendLine();
                sb.AppendLine("</div>");
            }

            //scaffold
            if (this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Scaffold.aspx\">Scaffold</a>",
                    (page == "asp.admin_scaffold_aspx") ? " current" : ""));

                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/Scaffold.aspx?p=cartdeals\">deals</a>");
                sb.AppendLine("</div>");
            }

            //settings
            if (this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("ContentEditor"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Settings.aspx\">Settings</a>",
                    (page == "asp.admin_settings_aspx" || page == "asp.admin_errorviewer_listing_aspx" || page == "asp.admin_searches_aspx") ? " current" : ""));

                sb.AppendLine("<div class=\"pane\">");
                if (this.Page.User.IsInRole("Super"))
                {
                    sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=AddNew\">add new</a>");
                    sb.AppendLine("<a href=\"/Admin/ErrorViewer/Listing.aspx\">errors</a>");
                    sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=Admin\">admin</a>");
                    sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=fb_integration\">fb integration</a>");
                }
                sb.AppendLine("<a href=\"/Admin/Searches.aspx\">searches</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=Default\">defaults</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=email\">email</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=images\">images</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=downloads\">downloads</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=flow\">order flow</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=pagemsg\">page messages</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=service\">service charges</a>");
                sb.AppendLine("<a href=\"/Admin/Settings.aspx?p=ship\">shipping</a>");
                sb.AppendLine("</div>");
            }

            //promotions
            if (this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super") || this.Page.User.IsInRole("ContentEditor"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/PromotionEditor.aspx\">Promotions</a>", 
                    (page == "asp.admin_promotioneditor_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/PromotionEditor.aspx?p=banner\">banners</a>");
                sb.AppendLine("<a href=\"/Admin/PromotionEditor.aspx?p=order\">order banners</a>");
                sb.AppendLine("<a href=\"/Admin/PromotionEditor.aspx?p=promo\">promotions</a>");
                sb.AppendLine("</div>");

                if (Wcss._Config._Site_Entity_Name.ToLower() != "sts9")
                {
                    //header images
                    sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/HeaderImageEditor.aspx\">Header Images</a>",
                        (page == "asp.admin_headerimageeditor_aspx") ? " current" : ""));
                    sb.AppendLine("<div class=\"pane\">");
                    sb.AppendLine("<a href=\"/Admin/HeaderImageEditor.aspx?p=header\">Images</a>");
                    sb.AppendLine("</div>");
                }
            }

            //charities
            //if (this.Page.User.IsInRole("ContentEditor") || this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super"))
            //{
            //    sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/CharitableListings.aspx\">Charities</a>", 
            //        (page == "asp.admin_charitablelistings_aspx") ? " current" : ""));
            //    sb.AppendLine("<div class=\"pane\"></div>");
            //}

            //editors
            if (this.Page.User.IsInRole("ContentEditor") || this.Page.User.IsInRole("Administrator") || this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/EntityEditor.aspx\">Editors</a>", 
                    (page == "asp.admin_entityeditor_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\">");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=act\">acts</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=age\">ages</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=charity\">charities</a>");
                //sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=emp\">employees</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=faq\">faq</a>");
                //sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=gen\">genres</a>");

                if (this.Page.User.IsInRole("Super"))
                    sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=invoicefee\">invoice fees</a>");

                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=mjcorder\">merch organization</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=color\">merch colors</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=size\">merch sizes</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=promoter\">promoters</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=rule\">sale rules</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=chrg\">service fees</a>");
                sb.AppendLine("<a href=\"/Admin/EntityEditor.aspx?p=venue\">venues</a>");
                sb.AppendLine("</div>");
            }

            //charges
            if (this.Page.User.IsInRole("Super"))
            {
                sb.AppendLine(string.Format("<a class=\"accord{0}\" href=\"/Admin/Charges.aspx?p=Statement\">Charges</a>",
                    (page == "asp.admin_charges_aspx") ? " current" : ""));
                sb.AppendLine("<div class=\"pane\" style=\"margin:0;padding:0;\"></div>");
            }

            /*keep this for adding new items
            sb.AppendLine("<h2>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            sb.AppendLine("<div class=\"pane\"><a href=\"\"></a></div>");
            */

            sb.AppendLine("</div>");

            //sb.AppendLine("</div>");

            litMenu.Text = sb.ToString();
        }
}
}
