using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;

using Wcss;
using WillCallWeb.StoreObjects;
using Utils.ExtensionMethods;

namespace WillCallWeb.Controls
{
    public partial class Listing_Ticket : WillCallWeb.BaseControl
    {
        protected Show _show;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (_show != null)
            {
                if (_show.IsAllowFacebookLike)
                {
                    if (Wcss._Config._FacebookIntegration_Like_Active || (this.Page.User != null && this.Page.User.Identity.IsAuthenticated && this.Page.User.IsInRole("Administrator")))
                    {                    
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(
                            this.Page, 
                            this.Page.GetType(),
                            "fb_integration", 
                            _Config.FB_RENDERINTEGRATION((_show != null) ? _show.Id : 0, (_show != null) ? _show.ShowUrl : string.Empty),
                            true);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (IsPostBack)
            
            //SHOW handle these situations:
            //show not announced yet - redirect to show listing
            //show not on sale yet - should show details of show
            //show within doors cutoff
            //show within dayOfShow context

            //sets show in admin context
            if (this.Page.ToString().IndexOf(".admin") != -1 && Atx != null && Atx.CurrentShowRecord != null)
            {
                _show = Atx.CurrentShowRecord;//in admin, who cares if it is null
            }
            else
            {
                if (Globals.ShowId == 0)//set show 
                    _show = null;
                else
                {
                    _show = Ctx.RetrieveShowRecordFromDatesByShowId(Globals.ShowId, true);
                }

                //now, determine if the show is in past and not contained in the current on sale shows
                //allow old valid shows to be listed - keep old links alive
                if (_show == null && Globals.ShowId != 0)
                {   
                    _show = Show.FetchByID(Globals.ShowId);

                    //if the old show is valid for display
                    if (_show != null && _show.ApplicationId == _Config.APPLICATION_ID && _show.IsDisplayable)
                    {
                        //CustomVal.IsValid = false;
                        //CustomVal.ErrorMessage = string.Format("Sorry, there are no tickets available for <br/>{0}", _show.Name);

                        //showset.Visible = false;
                        TicketSet.Visible = false;
                    }
                    else
                        _show = null;
                }
                
                //redirect shows not found AND INVALID shows
                if (_show == null && Globals.ShowId != 0)
                {
                    Globals.ShowId = 0;
                    base.Redirect("/Store/ChooseTicket.aspx");
                }
                //otherwise show a null show? - a null show goes to the latest listing 080807
            }

            if ((!IsPostBack) || base.IsPageAdminContext)
            {                
                ((BasePage)this.Page).FB_WRITEMETA(_show);
                BindControl();
            }
        }

        public void BindControl()
        {
            if (this.Page.ToString().IndexOf(".admin") != -1 && Atx != null && Atx.CurrentShowRecord != null)
            {
                _show = Atx.CurrentShowRecord;//in admin, who cares if it is null
            }

            BindSocial();//needs to come before binding description
            BindShowDescription();

            BindTicketListing();
        }

        #region Social 

        private void BindSocial()
        {
            litSocial_FB.DataBind();
        }
        protected void litSocial_DataBinding(object sender, EventArgs e)
        {
            if (_show != null)
            {
                Literal lit = (Literal)sender;
                if (lit.ID.ToLower().IndexOf("litsocial_fb") != -1)
                {
                    bool isAdminUser = this.Page.User != null && this.Page.User.Identity.IsAuthenticated && this.Page.User.IsInRole("Administrator");

                    if (_show.IsAllowFacebookLike)
                        if (isAdminUser || (_show.LastDate >= _Config.SHOWOFFSETDATE && Wcss._Config._FacebookIntegration_Like_Active))                        
                            lit.Text = _Config.FB_RENDERLIKECONTROL;
                }
            }
        }
       
        #endregion

        private void BindShowDescription()
        {
            if (_show != null)
            {
                bool drt = _show.IsDisplayRichText;

                //show the venue in different places depending on displaymode
                if(_Config._Site_Entity_Mode != _Enums.SiteEntityMode.Venue)
                    LiteralVenue.Text = string.Format("<div class=\"legend\">{0}</div>", 
                        Utils.ParseHelper.ParseCommasAndAmpersands(_show.DisplayVenue_Wrapped(true, false, true)));
                    
                    //(_Config._Site_Entity_Mode != _Enums.SiteEntityMode.Venue) ?
                    //Utils.ParseHelper.ParseCommasAndAmpersands(_show.DisplayVenue_Wrapped(false, true, true)) : //_show.DisplayVenue_JustifiedAndWrapped(false, true)
                    //Utils.ParseHelper.ParseCommasAndAmpersands(_show.DisplayVenue_Wrapped(false, true, true));

                LiteralShowTitle.Text = (_show.ShowTitle != null && _show.ShowTitle.Trim().Length > 0) ? 
                    string.Format("<div class=\"showtitle\">{0}</div>", Utils.ParseHelper.ParseCommasAndAmpersands(_show.ShowTitle.Trim())) : string.Empty;

                if (!drt)
                    LiteralShowTimes.Text = Utils.ParseHelper.ParseCommasAndAmpersands(_show.wc_DisplayShowTimes(false, true));

                LiteralShowStatus.Text = (_show.StatusText != null && _show.StatusText.Trim().Length > 0) ? 
                    string.Format("<span class=\"status\">{0}</span>", Utils.ParseHelper.ParseCommasAndAmpersands(_show.StatusText.Trim())) : string.Empty;


                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //drt
                if ((!drt) && _show.cartPromoter != null && _show.cartPromoter.Trim().Length > 0)
                {
                    sb.AppendFormat("<div class=\"promoter\">{0}</div>", _show.cartPromoter.Trim());
                    sb.AppendLine();
                }

                if (_show.TopText != null && _show.TopText.Trim().Length > 0)
                {
                    sb.AppendFormat("<div class=\"toptxt\">{0}</div>", _show.TopText.Trim());
                    sb.AppendLine();
                }

                if (!drt)
                {
                    sb.AppendFormat("<div class=\"actcontainer\">");

                    if (_show.OverrideActBilling)
                    {
                        sb.AppendFormat("<div class=\"actlist\">{0}</div>", (_show.ActBilling == null || _show.ActBilling.Trim().Length == 0) ?
                            string.Empty : _show.ActBilling.Trim());
                        sb.AppendLine();
                    }
                    else
                    {
                        string heads = _show.listHeadliners;
                        if (heads != null && heads.Trim().Length > 0)
                        {
                            sb.AppendFormat("<div class=\"mainact\">{0}</div>", heads);
                            sb.AppendLine();
                        }

                        string opens = _show.wc_DisplayOpeners;
                        if (opens != null && opens.Trim().Length > 0)
                        {
                            sb.AppendFormat("<span class=\"openerlist\">{0}</span>", opens.Trim());
                            sb.AppendLine();
                        }
                    }

                    sb.AppendFormat("</div>");
                    sb.AppendLine();
                }
                
                //writeup
                if (_show.BotText != null && _show.BotText.Trim().Length > 0)
                {
                    //be sure to check for texts that only have tags
                    //string stripped = Utils.ParseHelper.StripHtmlTags(_show.BotText);
                    string strip = _show.BotText.Trim();

                    //if (stripped.Trim().Length > 0)
                    if (strip.ToLower() != "<br />" && strip.ToLower() != "<br/>")
                    {
                        sb.AppendFormat("<div class=\"bottxt{0}\">{1}</div>", (drt) ? " richtxt" : string.Empty,
                            Utils.ParseHelper.ParseCommasAndAmpersands(strip));
                        sb.AppendLine();
                    }
                }

                //cleanup literal - steal text and put into description
                if (drt && litSocial_FB.Text.Trim().Length > 0)
                {
                    sb.Insert(0, litSocial_FB.Text);
                    litSocial_FB.Text = string.Empty;
                }

                LiteralDescription.Text = Utils.ParseHelper.ParseCommasAndAmpersands(sb.ToString().Trim());

                if (!drt)
                {
                    litShowImage.Text = (_Config.altDisplay && _show.ShowImageUrl != null && _show.ShowImageUrl.Trim().Length > 0) ?
                        string.Format("<img id=\"showimage\" name=\"showimage\" class=\"showimage\" src=\"{0}\">", _show.ShowImageUrl.Trim()) : string.Empty;
                    sb.AppendLine();
                }

                litDisplayNotes.Text = (_show.DisplayNotes != null && _show.DisplayNotes.Trim().Length > 0) ?                
                    string.Format("<div class=\"show-notes\">{0}</div>{1}", Utils.ParseHelper.ParseCommasAndAmpersands(_show.DisplayNotes.Trim()), Environment.NewLine) 
                    : string.Empty;
                litDisplayNotes.Text += AddShowLinks();
                
            }
        }

        private string AddShowLinks()
        {
            //show links
            if (_show.ShowLinkRecords().Count > 0 && (!_show.IsHideAutoGenerated))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                ShowLinkCollection internalColl = new ShowLinkCollection();
                ShowLinkCollection externalColl = _show.ShowLinkRecords().RemoteLinks_Active;

                //history shows should not link
                if (_show.LastDate >= DateTime.Now.Date)
                    internalColl.AddRange(_show.ShowLinkRecords().GetList().FindAll(delegate(ShowLink match)
                    {
                        if (match.IsActive && match.IsShowLink)
                        {
                            Show linkedShow = null;

                            try
                            {
                                linkedShow = Ctx.RetrieveShowRecordFromDatesByShowId(int.Parse(match.LinkUrl), true);

                                return linkedShow != null && linkedShow.IsActive && linkedShow.AnnounceDate < DateTime.Now && linkedShow.LastDate > DateTime.Now.Date;
                            }
                            catch (Exception) { }
                        }

                        return false;
                    }));

                if (internalColl.Count > 1)
                    internalColl.Sort("IDisplayOrder", true);

                if (internalColl.Count > 0 || externalColl.Count > 0)
                {
                    sb.AppendFormat("<div class=\"showlinkcontainer\">");

                    if (internalColl.Count > 0 && _Config._ShowLinks_Header != null && _Config._ShowLinks_Header.Length > 0)
                    {
                        sb.AppendFormat("<span class=\"header\">{0}</span>", _Config._ShowLinks_Header);
                        sb.AppendLine();
                    }

                    foreach (ShowLink sl in internalColl)
                    {
                        sb.AppendFormat("<span class=\"internal\">{0}</span>", sl.LinkUrl_Formatted(false));
                        sb.AppendLine();
                    }

                    foreach (ShowLink sl in externalColl)
                    {
                        sb.AppendFormat("<span class=\"external\">{0}</span>", sl.LinkUrl_Formatted(true));
                        sb.AppendLine();
                    }

                    sb.AppendFormat("</div>");
                    sb.AppendLine();
                }

                return sb.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Bind the list of tickets
        /// </summary>
        private void BindTicketListing()
        {
            TicketSet.Visible = false;
            rptTickets.Visible = false;

            if (_show != null && (!_show.IsHideAutoGenerated))
            {
                if (_show.DateOnSale > DateTime.Now)
                {
                    LiteralNoneAvailable.Text = string.Format("<div class=\"noneavailable\">Tickets on sale {0} <span class=\"timezone\">{1}</span></div>",
                        _show.DateOnSale.ToString("MM/dd/yyyy hh:mmtt"), System.TimeZoneInfo.Local.AbbreviateTimeZone());
                    return;
                }

                string userName = (this.Profile != null && (!this.Profile.IsAnonymous)) ? this.Profile.UserName : null;

                ShowTicketCollection coll = new ShowTicketCollection();
                coll.AddRange(_show.GetDisplayableTickets(_Enums.VendorTypes.online, Ctx.MarketingProgramKey, true, false).GetList().FindAll(
                    delegate(ShowTicket match)
                    {
                        return (match.IsAvailableForListing(_Enums.VendorTypes.online, Ctx.MarketingProgramKey, true, false, coll) == true) &&
                            ((bool)RequiredShowTicketPastPurchase.ScanPurchaseRequirements(userName, match).First == true && 
                            //scan product Access reqs
                            
                            (! match.IsOverrideSellout)
                            );
                    }));


                //externalTixUrl - in theory, if we are using this feature, we should never get to here. The redirect will happen from the show page.
                // I have plkaced here as well for people who may be sitting on a ticket page during publish or onsale
                string externalTixUrl = (_show.ExternalTixUrl != null && _show.ExternalTixUrl.Trim().Length > 0) ? _show.ExternalTixUrl.Trim() : null;
                
                if(!_show.IsSoldOut && externalTixUrl != null)
                {
                    string tickets = "Tickets";
                    LiteralNoneAvailable.Text = string.Format("<span class=\"buylink\"><a href=\"{0}\" target=\"_blank\" class=\"btntribe\" title=\"see tickets\">{1}</a></span>",
                        _show.ExternalTixUrl, tickets);
                    return;
                }

                //end externalTixUrl



                //handle overrides
                if (_show.IsSoldOut && _show.OverrideSelloutTickets.Count > 0)
                {
                    //coll2 will hold st's to add to coll
                    ShowTicketCollection coll2 = new ShowTicketCollection();

                    coll2.AddRange(_show.OverrideSelloutTickets.GetList().FindAll(
                    delegate(ShowTicket match)
                    {
                        return (match.IsAvailableForListing(_Enums.VendorTypes.online, Ctx.MarketingProgramKey, true, false, coll2) == true) &&
                            ((bool)RequiredShowTicketPastPurchase.ScanPurchaseRequirements(userName, match).First == true);
                    }));


                    //ensure we adding to collection only once
                    if (coll2.Count > 0)
                    {
                        foreach (ShowTicket st2 in coll2)
                        {
                            int existing = coll.GetList().FindIndex(delegate(ShowTicket match) { return (match.Id == st2.Id); });
                            if (existing == -1)
                                coll.Add(st2);
                        }
                    }
                }


                //handle productaccess
                if (userName != null)
                {
                    //determine if any of the tickets need productAccess
                    foreach (ProductAccess pa in _Lookits.ProductAccessors)
                    {
                        //active PAs only have been filtered

                        if (pa.IsActive && pa.IsWithinActivationPeriod(Ctx.MarketingProgramKey) && pa.OrderFlowUserList.Contains(userName.ToLower()))
                        {
                            //easiest to most intensive search
                            //match user name first
                            //match a ticket to this showdate
                            foreach (ProductAccessProduct pap in pa.ProductAccessProductRecords())
                            {
                                if (pap.ProductContext == _Enums.ProductAccessProductContext.ticket && pap.ShowTicketRecord != null && 
                                    pap.ShowTicketRecord.ShowDateRecord.ShowRecord.Id == _show.Id)
                                {
                                    int existing = coll.GetList().FindIndex(delegate(ShowTicket match) { return (match.Id == pap.ShowTicketRecord.Id); });

                                    //deal with qty when displaying ticket option - this way they can tell if they are over - and so can we
                                    if (existing == -1 && pap.ShowTicketRecord.IsAvailableForListing(_Enums.VendorTypes.online, Ctx.MarketingProgramKey, false, false, coll, pa, true))
                                        coll.Add(pap.ShowTicketRecord);
                                }
                            }
                        }
                    }
                }

                
                var sortedColl =
                    from showTick in coll
                    orderby showTick.IsPackage, showTick.ShowDateRecord.DateOfShow_ToSortBy, showTick.DisplayOrder
                    select showTick;

                if (sortedColl.Count() > 0)
                {
                    if (_show.ShowDateRecords().Count > 1)
                    {
                        //we need to display "no tickets" if the show is in the future (or today) and there are no tix available
                        //get a collection of showdates in the future or today
                        ShowDateCollection noTixForDates = new ShowDateCollection();

                        foreach (ShowDate sd in _show.ShowDateRecords())
                        {
                            if (sd.IsActive && sd.DateOfShow_ToSortBy >= _Config.SHOWOFFSETDATE)
                            {
                                int idx = coll.GetList().FindIndex(delegate(ShowTicket match) 
                                {
                                    return (

                                        (match.TShowDateId == sd.Id)

                                        ||

                                        ((match.IsPackage) && 
                                        match.LinkedShowTickets.GetList().FindIndex(delegate(ShowTicket matcher) { return matcher.TShowDateId == sd.Id; }) != -1)
                                        
                                        //|| 
 
                                        //(( ! match.IsPackage) && )
                                        
                                        ); 
                                });

                                if (idx == -1)
                                    noTixForDates.Add(sd);
                            }
                        }

                        if (noTixForDates.Count > 0)
                        {
                            litTicketHeader.Text += string.Format("<div class=\"notix\">");

                            noTixForDates.SortBy_DateToOrderBy();

                            foreach (ShowDate sd in noTixForDates)
                                litTicketHeader.Text += string.Format("<div class=\"notixdate\">Tickets for {0} are not available at this website at this time.</div>",
                                    sd.DateOfShow.ToString("ddd MMM d, hh:mmtt"));

                            litTicketHeader.Text += string.Format("</div>");
                        }
                    }


                    TicketSet.Visible = true;
                    rptTickets.Visible = true;

                    if (rptTickets.Visible)
                    {
                        _rowCounter = 0;
                        rptTickets.DataSource = sortedColl;
                        rptTickets.DataBind();
                    }
                }
                else
                {
                    string details = (_show.VenueRecord.WebsiteUrl != null && _show.VenueRecord.WebsiteUrl.Trim().Length > 0) ?
                        string.Format("<a href=\"{0}\">{1}</a>",
                            Utils.ParseHelper.FormatUrlFromString(_show.VenueRecord.WebsiteUrl, true, false), _show.VenueRecord.Name) :
                        string.Format("<a href=\"{0}\">{1}</a>",
                            Utils.ParseHelper.FormatUrlFromString(_Config._Site_Entity_HomePage, true, false), _Config._Site_Entity_Name);

                    if (_show.IsSoldOut || _show.IsVirtuallySoldOut)
                        LiteralNoneAvailable.Text = string.Format("<div class=\"noneavailable\">This show is sold out!</div>", details);
                    else
                        LiteralNoneAvailable.Text = string.Format("<div class=\"noneavailable\">Sorry, there are no tickets currently available at this website at this time.<br/>See {0} for the latest details.</div>",
                            details);
                }
            }
        }

        

        protected void rptTickets_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string result = null;
            int idx = int.Parse(e.CommandArgument.ToString());

            switch (e.CommandName.ToLower())
            {
                case "updtkt":
                    int qty = 1;
                    DropDownList ddl = (DropDownList)e.Item.FindControl("ddlQty");
                    if (ddl != null)
                    {
                        string selection = ddl.SelectedValue;
                        qty = (selection != null && Utils.Validation.IsInteger(selection)) ? int.Parse(selection) : 0;
                    }
                    
                    result = Ctx.Cart.SaleItem_AddUpdate(_Enums.InvoiceItemContext.ticket, idx, qty, this.Profile);

                    if (result.Trim().Length == 0)
                        base.Redirect("/Store/Cart_Edit.aspx");

                    //reset the drop down list
                    SaleItem_Ticket sit = (SaleItem_Ticket)Ctx.Cart.TicketItems.Find(delegate(SaleItem_Ticket match) { return (match.Ticket.Id == idx); } );
                    
                    //list is 1 based
                    ddl.SelectedIndex = (sit == null || sit.Quantity == 1) ? 0 : sit.Quantity - 1;

                    break;
            }

            if (result != null && result.Trim().Length > 0)
            {
                CustomVal.IsValid = false;
                CustomVal.ErrorMessage = result;
            }

            BindControl();
        }
        
        protected void ProcessDates(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                Repeater rpr = (Repeater)sender;
                ShowTicket item = (ShowTicket)e.Item.DataItem;
                Literal time = (Literal)e.Item.FindControl("LiteralTime");

                if (item != null)
                {
                    time.Text = string.Format("{0} DOORS{1}", item.DtDateOfShow.ToString("h:mmtt"),
                        (item.ShowDateRecord.ShowTime != null) ? string.Format(" / {0} SHOW", item.ShowDateRecord.ShowTime) : string.Empty);

                    Literal venue = (Literal)e.Item.FindControl("litVenue");
                    if (venue != null)
                        venue.Text = item.ShowDateRecord.ShowRecord.DisplayVenue_Wrapped(false, false, false);//.DisplayVenue_JustifiedAndWrapped(false);

                    Literal title = (Literal)e.Item.FindControl("LiteralDateTitle");
                    if (title != null && item.ShowDateRecord.ShowDateTitle != null && item.ShowDateRecord.ShowDateTitle.Trim().Length > 0)
                        title.Text = string.Format("<div class=\"title\">{0}</div>", item.ShowDateRecord.ShowDateTitle);

                    Literal status = (Literal)e.Item.FindControl("LiteralDateStatus");
                    if (status != null && item.ShowDateRecord.StatusText != null && item.ShowDateRecord.StatusText.Trim().Length > 0)
                        status.Text = string.Format("<div class=\"status\">{0}</div>", item.ShowDateRecord.StatusText);

                    Literal eventinfo = (Literal)e.Item.FindControl("LiteralEventInfo");
                    if (eventinfo != null)
                    {
                        if (item.ShowDateRecord.wc_CartHeadliner != null && item.ShowDateRecord.wc_CartHeadliner.Trim().Length > 0)
                            eventinfo.Text = string.Format("<div class=\"mainact\">{0}</div>", item.ShowDateRecord.wc_CartHeadliner.Trim());
                        if (item.ShowDateRecord.wc_CartOpeners != null && item.ShowDateRecord.wc_CartOpeners.Trim().Length > 0)
                            eventinfo.Text += string.Format("{0}", item.ShowDateRecord.wc_CartOpeners.Trim());
                    }
                }
            }
        }

        protected int _rowCounter = 0;
        protected void rptTickets_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                Repeater rpr = (Repeater)sender;
                ShowTicket item = (ShowTicket)e.Item.DataItem;

                if (item != null)
                {
                    HtmlGenericControl divAlt = (HtmlGenericControl)e.Item.FindControl("divAlternate");
                    if (divAlt != null)
                        divAlt.Attributes.Add("class", string.Format("listitem-wrap{0}", (_rowCounter % 2 != 0) ? " alternate" : string.Empty));

                    ShowTicketCollection coll = new ShowTicketCollection();
                    coll.Add(item);
                    if (item.IsPackage && (!item.IsCampingPass()))
                    {
                        coll.AddRange(item.LinkedShowTickets);
                        if (coll.Count > 1)
                            coll.Sort("DtDateOfShow", true);

                        Literal pkg = (Literal)e.Item.FindControl("LiteralPackage");
                        if (pkg != null)
                            pkg.Text += string.Format("<div class=\"pkginfo\"><span>{0} SHOW PASS</span></div>", coll.Count);
                    }

                    Literal sts = (Literal)e.Item.FindControl("LiteralStatus");
                    sts.Text = (item.Status != null && item.Status.Trim().Length > 0) ? string.Format("<div class=\"ticketstatus\">{0}</div>", item.Status.Trim()) : string.Empty;

                    if (item.IsCampingPass())
                    {
                        Literal camping = (Literal)e.Item.FindControl("litCamping");
                        if (camping != null)
                            camping.Text = string.Format("<div class=\"eventdate\">CAMPING - {0} - {1}</div>", item.AgeRecord.Name,
                                item.ShowDateRecord.ShowRecord.ShowEventPart);
                    }
                    else
                    {
                        Repeater packages = (Repeater)e.Item.FindControl("rptShowNames");
                        packages.DataSource = coll;
                        packages.DataBind();
                    }
                    
                    Literal pickupPost = (Literal)e.Item.FindControl("LiteralPickupAndPost");
                    if (pickupPost != null)
                    {
                        //Check for usa eligibility later in the process
                        if (_Config._Allow_HideShipMethod && (!item.HideShipMethod))
                        {
                            if (item.IsCurrentlyShippable)
                            {
                                if(item.IsShipSeparate && item.FlatMethod != null)
                                    pickupPost.Text = string.Format("this items ships for {0} via {1}.", 
                                        (item.FlatShip == 0) ? "FREE" : item.FlatShip.ToString("c"), item.FlatMethod);
                                else
                                    pickupPost.Text = "shipping options available at checkout";
                            }
                            else if (item.IsAllowWillCall)
                                pickupPost.Text = "<div><span class=\"willcall-note\">tickets to be picked up at venue's WillCall only</span></div>";
                            else
                                pickupPost.Text = string.Empty;

                            if (!item.IsShipSeparate)
                            {
                                if (!item.IsAllowWillCall)
                                    pickupPost.Text = pickupPost.Text.Insert(0, "<div><span class=\"willcall-note\">these tickets NOT available for WillCall</span></div>");
                            }
                        }

                        //POST PURCHASE TEXTS
                        PostPurchaseTextCollection postColl = new PostPurchaseTextCollection();
                        postColl.AddRange(item.PostPurchaseTextRecords().GetList()
                            .FindAll(delegate(PostPurchaseText match) { return (match.InProcessDescription != null && match.InProcessDescription.Trim().Length > 0); }));
                        if (postColl.Count > 0)
                        {
                            postColl.Sort("IDisplayOrder", true);
                            pickupPost.Text = "<div class=\"postpurchase\">";

                            foreach (PostPurchaseText pp in postColl)
                                pickupPost.Text += string.Format("<div class=\"pptext\">{0}</div>",
                                    System.Web.HttpUtility.HtmlEncode(pp.InProcessDescription.Trim()));

                            pickupPost.Text += "</div>";
                        }

                        //add a wrapper if we have added any text
                        if (pickupPost.Text.Trim().Length > 0)
                            pickupPost.Text = string.Format("<div class=\"itemoptions\">{0}</div>", pickupPost.Text);
                    }

                    Literal description = (Literal)e.Item.FindControl("LiteralDescription");
                    if (description != null)
                    {
                        if (item.SalesDescription != null && item.SalesDescription.Trim().Length > 0)
                            description.Text = string.Format("<div class=\"description\"><span>{0}</span></div>", item.SalesDescription.Trim());
                        if (item.CriteriaText != null && item.CriteriaText.Trim().Length > 0)
                            description.Text += string.Format("<div class=\"criteria\"><span>{0}</span></div>", item.CriteriaText.Trim());
                    }

                    //bundle information
                    Literal litBundle = (Literal)e.Item.FindControl("litBundle");
                    if (litBundle != null)
                    {
                        //list included and potential bundle items
                        litBundle.Text = MerchBundle.DisplayBundle_Listing(item, true, true, false, this.Page.ToString());
                    }

                    Literal notAvailable = (Literal)e.Item.FindControl("LiteralNotAvailable");
                    HtmlGenericControl allowPurchase = (HtmlGenericControl)e.Item.FindControl("AllowPurchase");
                    string hasPurchasedQuota = string.Empty;

                    if (notAvailable != null && allowPurchase != null)
                    {
                        string code = Ctx.MarketingProgramKey;
                        bool codeCanDisplay = (item.IsUnlockActive && item.UnlockCode == code && item.UnlockEndDate > DateTime.Now);
                        bool codeCanPurchase = (codeCanDisplay && item.UnlockDate < DateTime.Now);
                        bool hasPickupOptions = item.Allotment > 0 && (item.IsAllowWillCall || item.IsCurrentlyShippable);

                        string userName = (this.Profile != null && (!this.Profile.IsAnonymous)) ? this.Profile.UserName : null;
                        bool isAvailable = ((item.PublicOnsaleDate < DateTime.Now) || (codeCanPurchase)) && hasPickupOptions;

                        int productAccessAvailable = 0;

                        //if it is a regular item than use public onsaleDate, codecanpurchase and has pickup options - //if we are past the onsale date or code is valid and falls within date range
                        //if it is a productaccess item than use activation window
                        if ((!isAvailable) && userName != null)
                        {
                            //get list of applicable prodAccessors - activated, has user and has the ticket item
                            ProductAccessCollection paColl = new ProductAccessCollection();
                            paColl.AddRange(_Lookits.ProductAccessors.GetList().FindAll(delegate(ProductAccess match) 
                            { return (match.IsWithinActivationPeriod(Ctx.MarketingProgramKey) && match.OrderFlowUserList.Contains(userName.ToLower()) && match.HasTicketAccess(item.Id)); }));

                            if (paColl.Count > 0)
                            {   
                                ProductAccessUserCollection puColl = new ProductAccessUserCollection();
                                int maxAllowed = 0;

                                //get the user records related to the accessors
                                foreach (ProductAccess pa in paColl)
                                    puColl.AddRange(pa.ProductAccessUserRecords().GetList().FindAll(delegate(ProductAccessUser match) { return (match.UserName.ToLower() == userName.ToLower()); }));
                                
                                //aggregate the amount they are allowed to buy
                                foreach (ProductAccessUser pu in puColl)
                                    maxAllowed += pu.QuantityAllowed;

                                //if they are allowed any purchases - subtract any previous purchases from their total
                                if (maxAllowed > 0)
                                {
                                    int hasAlreadyPurchased = 0;
                                    hasAlreadyPurchased = ProductAccess.User_HasPurchasedPastAccess(userName, item.Id);

                                    if (hasAlreadyPurchased > 0 && maxAllowed == hasAlreadyPurchased)
                                        hasPurchasedQuota = "You have purchased your maximum allowed for this ticket.";

                                    productAccessAvailable = maxAllowed - hasAlreadyPurchased;
                                }
                            }

                            //update access variable
                            isAvailable = productAccessAvailable > 0;
                        }

                        
                        //then allow purchase
                        if (isAvailable)
                        {
                            //if we have a sellable item
                            allowPurchase.Visible = true;
                            notAvailable.Visible = false;

                            DropDownList ddl = (DropDownList)e.Item.FindControl("ddlQty");
                            LinkButton btnadd = (LinkButton)e.Item.FindControl("btnAdd");

                            if (ddl != null && ddl.Items.Count == 0 && item != null)
                            {
                                int currentQty = 0;
                                int max = 0;

                                //determine if we have this in our cart
                                WillCallWeb.StoreObjects.SaleItem_Ticket cartItem = Ctx.Cart.FindSaleItem_TicketById(item.Id);
                                currentQty = (cartItem == null) ? currentQty = 1 : cartItem.Quantity;

                                if (btnadd != null)
                                    btnadd.Text = (cartItem == null) ? "add to cart" : "update qty";


                                //this may get overridden by requirements
                                //product access will override and is the amount of total allowed - past purchases have already been taken into account
                                max = (productAccessAvailable > 0) ? productAccessAvailable : RequiredShowTicketPastPurchase.SetMaxAllowedBasedOnRequired(userName, item);

                                for (int i = 1; i <= max; i++)
                                    ddl.Items.Add(new ListItem(i.ToString()));

                                ddl.SelectedIndex = currentQty - 1;//zero-based
                            }
                        }
                        else
                        {
                            allowPurchase.Visible = false;
                            notAvailable.Visible = true;

                            if (hasPurchasedQuota.Trim().Length > 0)
                            {
                                notAvailable.Text = string.Format("<span class=\"sorry\">{0}</span>", hasPurchasedQuota);
                            }
                            else if (hasPickupOptions)
                            {
                                DateTime unlockDate = (codeCanDisplay) ? item.UnlockDate : item.PublicOnsaleDate;

                                notAvailable.Text = string.Format("<div>These tickets available:</div>{0}", unlockDate.ToString("ddd MM/dd/yyyy hh:mmtt"));
                            }
                            else
                                notAvailable.Text = "<span class=\"sorry\">Sorry, these tickets are not available for sale at this time.</span>";
                        }
                    }                    
                    
                    _rowCounter += 1;

                }
            }
        }
    }
}
