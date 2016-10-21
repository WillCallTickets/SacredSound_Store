using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

using WillCallWeb.StoreObjects;
using Wcss;
using Utils;

namespace WillCallWeb.Controls
{
    public partial class Cart : WillCallWeb.BaseControl, IPostBackEventHandler
    {
        #region Page Overhead
        
        List<WebControl> needsReg = null;
        protected void ShippingOptions_Merch_ShipRateChanged(object sender, EventArgs e)
        {
            BindCart();

            Cart_Totals1.UpdateTotals();
        }
        
        private void EventHandler_CartChanged(object sender, EventArgs e)
        {
            if (Ctx.CurrentCartException == null)
            {
                valCustom.IsValid = true;
                valCustom.ErrorMessage = "";
            }

            if (Ctx.Cart.IsOverMaxTransactionAllowed)
            {
                Ctx.CurrentCartException = Ctx.Cart.FormatMaxTransactionError();

                Response.Redirect("/Store/Cart_Edit.aspx");
            }

            //BindCart();
        }

        private void EventHandler_PromoMessage(object sender, WillCallWeb.StoreObjects.ShoppingCart.PromoMessageEventArgs e)
        {
            valCustom.IsValid = false;
            valCustom.ErrorMessage = e.Reason;
        }

        private void EventHandler_NoMerchShipMethodChosen(object sender, EventArgs e)
        {
            CustomShipMerch.IsValid = false;
            CustomShipMerch.ErrorMessage = "Please select a shipping method for your merchandise.";
        }

        protected override void Render(HtmlTextWriter writer)
        {
            foreach (WebControl cont in needsReg)
                this.Page.ClientScript.RegisterForEventValidation(cont.UniqueID);

            base.Render(writer);
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            needsReg = new List<WebControl>();
            Ctx.CurrentCartItems = string.Empty;

            //display merchandise shipping options
            shipping.Visible = this.Page.ToString() == "ASP.store_shipping_aspx" && Ctx.Cart.HasMerchandiseItems_Shippable && _Config._Shipping_Merch_Active;

            //display ticketing shipping options
            ticketshipping.Visible = (this.Page.ToString() == "ASP.store_shipping_aspx" && Ctx.Cart.HasTicketItems_CurrentlyShippable);

            StoreEvent.NoMerchShipMethodChosen += new StoreEvent.NoMerchShipMethodChosenEvent(EventHandler_NoMerchShipMethodChosen);
            Ctx.Cart.CartChanged += new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);
            Ctx.Cart.PromoMessage += new WillCallWeb.StoreObjects.ShoppingCart.PromoMessageEvent(EventHandler_PromoMessage);

            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("mouseToHourglass"))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("<script type=\"text/javascript\">\r\n");
                sb.Append("\tfunction mouseToHourglass() {\r\n");
                //sb.Append("\t\talert('m To h');\r\n");
                sb.Append("\t\tdocument.body.style.cursor = 'wait';\r\n");
                sb.Append("\t}\r\n");
                sb.Append("</script>\r\n");

                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "mouseToHourglass", sb.ToString());
            }

            chkShipMultiple.Attributes.Add("onclick", "mouseToHourglass()");
        }

        public override void Dispose()
        {
            Ctx.Cart.PromoMessage -= new WillCallWeb.StoreObjects.ShoppingCart.PromoMessageEvent(EventHandler_PromoMessage);
            Ctx.Cart.CartChanged -= new WillCallWeb.StoreObjects.ShoppingCart.CartChangedEvent(EventHandler_CartChanged);
            StoreEvent.NoMerchShipMethodChosen -= new StoreEvent.NoMerchShipMethodChosenEvent(EventHandler_NoMerchShipMethodChosen);
            base.Dispose();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Ctx.Cart.IsOverMaxTransactionAllowed && this.Page.ToString().ToLower() != "asp.store_cart_edit_aspx")
            {
                Response.Redirect("/Store/Cart_Edit.aspx");
            }

            if (Ctx.Cart.IsOverMaxTransactionAllowed && (Ctx.CurrentCartException == null || Ctx.CurrentCartException.Trim().Length == 0))
                Ctx.CurrentCartException = Ctx.Cart.FormatMaxTransactionError();

            if (Ctx.CurrentCartException != null && Ctx.CurrentCartException.Trim().Length > 0)
            {
                valCustom.IsValid = false;
                valCustom.ErrorMessage = string.Format("{0}", Ctx.CurrentCartException);

                Ctx.CurrentCartException = null;
            }

            
            BindThisControl();
            
        }

        protected void BindThisControl()
        {
            if (!IsPostBack)
                BindTicketShipping();

            BindCart();

            if ((!IsPostBack) && this.Page.ToString().ToLower() != "asp.store_shipping_aspx" && this.Page.ToString().ToLower() != "asp.store_confirmation_aspx")
            {
                Ctx.Cart.Shipments_Merch.Clear();
                Ctx.Cart.Shipments_Tickets.Clear();
            }
        }

        protected void BindTicketShipping()
        {
            rdoTixShipRates.DataBind();
        }

        /// <summary>
        /// Keep in mind that the TermCart control follows this pattern of loading saleItems
        /// </summary>
        public void BindCart()
        {
            Ctx.CurrentCartItems = string.Empty;
            bool displayItemTable = (this.Page.ToString().IndexOf("store_cart_edit_aspx") == -1);

            //item list holds items for small cart
            List<SaleItem_Base> itemlist = new List<SaleItem_Base>();

            //check here to make sure we still have items in stock
            //items may have changed in publishing, etc
            List<SaleItem_Ticket> listTickets = new List<SaleItem_Ticket>();
            listTickets.AddRange(Ctx.Cart.TicketItems.FindAll(
                delegate(SaleItem_Ticket match) { return (match.Ticket != null); } ));
            if(listTickets.Count > 1)
                listTickets.Sort(new Reflector.CompareEntities<SaleItem_Ticket>(Reflector.Direction.Ascending, "ItemShowDate"));
            
            List<SaleItem_Merchandise> sortedByMerch = new List<SaleItem_Merchandise>();
            List<SaleItem_Merchandise> listmerch = new List<SaleItem_Merchandise>();

            listmerch.AddRange(Ctx.Cart.MerchandiseItems.FindAll(
                delegate(SaleItem_Merchandise match) { return (match.MerchItem != null); }));
            //sort list so that like items are together
            if (listmerch.Count > 0)
            {
                var sortedColl =
                    from listItem in listmerch
                    select listItem;

                sortedByMerch.AddRange(sortedColl.OrderBy(x => x.MerchItem.TParentListing)
                    .ThenBy(x => (x.MerchItem.DisplayNameWithAttribs.ToLower())));
            }


            if (listTickets.Count != Ctx.Cart.TicketItems.Count || sortedByMerch.Count != Ctx.Cart.MerchandiseItems.Count)
            {
                valCustom.IsValid = false;
                valCustom.ErrorMessage = string.Format("We apologize. Due to an inventory change, some items were removed from your cart.");
            }

            tktPanel.Visible = ((!displayItemTable) && Ctx.Cart.HasTicketItems);
            if (tktPanel.Visible)
            {
                rptTickets.DataSource = listTickets;
                rptTickets.DataBind();
                rptTickets.Visible = Ctx.Cart.HasTicketItems;
            }
            else if (Ctx.Cart.HasTicketItems)
            {
                foreach(SaleItem_Ticket sit in listTickets)
                    itemlist.Add((SaleItem_Base)sit);
            }           


            //list backordered, flat and separate items first
            List<SaleItem_Merchandise> sortedByInventoryStatus = new List<SaleItem_Merchandise>();
            sortedByInventoryStatus.AddRange(sortedByMerch.FindAll(delegate(SaleItem_Merchandise match) 
                { return (match.MerchItem.IsShipSeparate || match.MerchItem.IsFlatShip || match.MerchItem.IsBackordered); } ));
            sortedByInventoryStatus.AddRange(sortedByMerch.FindAll(delegate(SaleItem_Merchandise match)
                { return ((! match.MerchItem.IsShipSeparate) && (! match.MerchItem.IsFlatShip) && (! match.MerchItem.IsBackordered)); }));

            if (sortedByInventoryStatus.Count != sortedByMerch.Count)
                throw new Exception("Item lists do not match");

            mrcPanel.Visible = ((!displayItemTable) && Ctx.Cart.HasMerchandiseItems);
            if (mrcPanel.Visible)
            {
                rptMerch.DataSource = sortedByInventoryStatus;
                rptMerch.DataBind();
                rptMerch.Visible = mrcPanel.Visible;
            }
            else if (Ctx.Cart.HasMerchandiseItems)
            {
                foreach (SaleItem_Merchandise sim in sortedByInventoryStatus)
                    itemlist.Add((SaleItem_Base)sim);
            }
            

            SaleItem_Shipping generalMerch = Ctx.Cart.Shipments_Merch.Find(delegate(SaleItem_Shipping match) 
                { return (match.ShipContext == _Enums.InvoiceItemContext.shippingmerch && match.IsGeneral); } );

            divMultiple.Visible = (((generalMerch != null && generalMerch.Items_Merch.Count > 1 && Ctx.Cart.HasBackorderedMerch) || Ctx.Cart.IsShipMultiple_Merch));

            if (divMultiple.Visible)
                chkShipMultiple.Checked = Ctx.Cart.IsShipMultiple_Merch;

            cartTable.Visible = (itemlist.Count > 0);
            if (cartTable.Visible)
            {
                rptItems.DataSource = itemlist;
                rptItems.DataBind();
                rptItems.Visible = cartTable.Visible;
            }
        }

        #endregion

        #region Small Cart

        protected void rptItems_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (rpt.DataSource != null)
            {
                List<SaleItem_Base> list = (List<SaleItem_Base>)rpt.DataSource;
                List<SaleItem_Listeditem> coll = new List<SaleItem_Listeditem>();
                //string description = string.Empty;

                foreach (SaleItem_Base sib in list)
                {
                    string description = string.Empty;
                    SaleItem_Merchandise sim = sib as SaleItem_Merchandise;
                    SaleItem_Ticket sit = sib as SaleItem_Ticket;
                    List<MerchBundle> bundles = new List<MerchBundle>();

                    //add the item
                    if (sim != null)
                    {
                        description = string.Format("<div>{0}</div>", sim.MerchItem.DisplayNameWithAttribs);

                        coll.Add(new SaleItem_Listeditem(_Enums.InvoiceItemContext.merch, sim.tMerchId, sim.TTL_Extended, sim.TTL,
                            sim.Price.ToString("n2"), sim.Quantity.ToString(), sim.LineTotal.ToString("c"), description));
                        
                        bundles.AddRange(sim.MerchItem.ParentMerchRecord.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
                    }
                    else if (sit != null)
                    {
                        //handle packages

                        //pkg info
                        bool isPackage = sit.Ticket.IsPackage;
                        if (isPackage && (!sit.Ticket.IsCampingPass()))
                        {
                            int cnt = sit.Ticket.LinkedShowTickets.Count + 1;

                            description += string.Format("<div> {0} SHOW PASS</div>", cnt.ToString());
                        }

                        //ticket status
                        //if (sit.Ticket.Status != null && sit.Ticket.Status.Trim().Length > 0)
                        //    description += string.Format("<div>{0}</div>", sit.Ticket.Status.Trim());


                        if (sit.Ticket.IsCampingPass())
                        {
                            description += string.Format("<div>CAMPING - {0} - {1}</div>", sit.Ticket.AgeRecord.Name,
                               sit.Ticket.ShowDateRecord.ShowRecord.ShowEventPart);
                        }
                        else
                        {
                            foreach (string s in sit.Ticket.TicketDateNameList)
                                description += string.Format("<div>{0}</div>", s);
                        }
                        //add descriptions
                        if (sit.Ticket.SalesDescription_Derived.Trim().Length > 0)
                            description += string.Format("<span>{0}</span>", sit.Ticket.SalesDescription_Derived);
                        if (sit.Ticket.CriteriaText_Derived.Trim().Length > 0)
                            description += string.Format("<span>{0}</span>", sit.Ticket.CriteriaText_Derived);
                        //add pickup/shipping options
                        description += ConstructPickupOptions(sit);
                        
                        string priceEach = sit.Price.ToString("n2");
                        if (sit.ServiceFee > 0)
                            priceEach += string.Format(" + {0}svc", sit.ServiceFee.ToString("n2"));

                        coll.Add(new SaleItem_Listeditem(_Enums.InvoiceItemContext.ticket, sit.tShowTicketId, sit.TTL_Extended, sit.TTL, 
                            priceEach, sit.Quantity.ToString(), sit.LineTotal.ToString("c"), description));

                        bundles.AddRange(sit.Ticket.MerchBundleRecords().Get_MerchBundleRecords_RunningAndAvailable());
                    }
                    
                    //loop thru any bundles                    
                    if (bundles.Count > 0)
                        foreach (MerchBundle bundle in bundles)
                            coll.Add(GetBundleText(sib, bundle));
                }

                rpt.DataSource = coll;
            }
        }
        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                SaleItem_Listeditem listItem = (SaleItem_Listeditem)e.Item.DataItem;

                Literal litDesc = (Literal)e.Item.FindControl("litDescription");

                if (litDesc != null && listItem.Description.Trim().Length > 0)
                    litDesc.Text = listItem.Description.Trim();

                Literal litTimer = (Literal)e.Item.FindControl("litTimer");
                if (litTimer != null && listItem.Context == _Enums.InvoiceItemContext.ticket)
                {
                    //add timing to the description(isTicket && timer != null && _Config._Display_CountdownTimer)
                    litTimer.Text = ConstructTimer(listItem.ItemProductId, listItem.IsExtended, listItem.Ttl);
                }
            }
        }
        private SaleItem_Listeditem GetBundleText(SaleItem_Base saleItemBase, MerchBundle bundle)
        {
            string description = null;

            bool isOnlyOneAvailableSelection = bundle.HasOnlyOneAvailableSelection;
            List<MerchBundle_Listing> selections = saleItemBase.GetValidMerchBundleListings_Selected(bundle.Id);

            int maxSelectionsAllowed = Ctx.Cart.GetMaxPossibleSelectionsAllowedForBundle(saleItemBase, bundle.Id);
            int qtySelected = WillCallWeb.StoreObjects.SaleItem_Services.GetQtySelected(selections);

            //display the title
            description += string.Format("<div class=\"bn-title\">* {0}</div>", bundle.TitleEncoded);

            //format container
            string innerDescription = string.Empty;

            //you have selected n out of n for this bundle
            //dont show for onlyoneavailable
            if (!isOnlyOneAvailableSelection)
            {
                bool isfull = (qtySelected >= maxSelectionsAllowed);
                
                innerDescription += string.Format("<div class=\"bn-status{2}\">You have selected {0} out of {1} selections for this bundle.</div>",
                    qtySelected.ToString(), maxSelectionsAllowed.ToString(),
                    (!isfull) ? " non-qual" : string.Empty);

                //only set this to false if not fulfilled
                //allows for setting once based on one item
                //if (!isfull)
                //    Ctx.Cart.BundleSelectionsFulfilled = false;
            }

            //show selections
            foreach (MerchBundle_Listing listing in selections)
                if (listing.SelectedInventory != null)
                    innerDescription += string.Format("<div class=\"bn-select\">{0} @ {1}</div>", 
                        listing.Quantity.ToString(), listing.SelectedInventory.DisplayNameWithAttribs);
            
            if (innerDescription.Trim().Length > 0)
                description += string.Format("<div class=\"bn-container\">{0}</div>", innerDescription);


            string lineTotal = null;
            string price = null;            

            //quantity is how many have qualified - charge instances
            int chargeInstances = 0;
            decimal line = saleItemBase.GetIndividualBundlePrice(bundle, out chargeInstances);
            string quantity = (chargeInstances > 0) ? chargeInstances.ToString() : null;

            if(chargeInstances > 0 && bundle.OffersOptout)
            {
                lineTotal = line.ToString("c");
                price = bundle.Price.ToString("n2");
            }

            return new SaleItem_Listeditem(_Enums.InvoiceItemContext.merch, 0, saleItemBase.TTL_Extended, saleItemBase.TTL,
                price, quantity, lineTotal, description);
        }
       
        #endregion

        #region Commands

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            string[] args = eventArgument.Split('~');
            string command = args[0];
            int idx = (args.Length > 1 && Utils.Validation.IsInteger((string)args[1])) ? int.Parse(args[1]) : 0;
            string result = string.Empty;

            switch (command.ToLower())
            {   
                case "addtime":
                    if (_Config._Display_CountdownTimer)
                    {
                        SaleItem_Ticket add = Ctx.Cart.FindSaleItem_TicketById(idx);
                        if (!add.TTL_Expired)
                            add.AddMoreTimeToLive();
                        else//this is just in case and should never happen as the update to the class will remove the link
                        {
                            result = "This ticket has expired.";
                            Response.Redirect("/Store/Cart_Edit.aspx");
                        }
                    }
                    break;
                case "addmrctime":
                    if (_Config._Display_CountdownTimer)
                    {
                        SaleItem_Merchandise add = Ctx.Cart.FindSaleItem_MerchandiseById(idx);
                        if (!add.TTL_Expired)
                            add.AddMoreTimeToLive();
                        else//this is just in case and should never happen as the update to the class will remove the link
                        {
                            result = "This item has expired.";
                            Response.Redirect("/Store/Cart_Edit.aspx");
                        }
                    }
                    break;
                case "changepickup":
                    if (_Config._Allow_3rdPartyPurchase)
                    {
                        SaleItem_Ticket tik = Ctx.Cart.FindSaleItem_TicketById(idx);

                        if (tik != null)
                        {
                            //TODO: change to first and last name - both are required
                            string first = Request[string.Format("PkpFirst{0}", idx)];
                            string last = Request[string.Format("PkpLast{0}", idx)];

                            if (first != null && last != null)
                            {
                                //verify input
                                if ((first.Trim().Length > 0 && last.Trim().Length == 0) || (last.Trim().Length > 0 && first.Trim().Length == 0))
                                    result = string.Format("{0} {1}<br/>Both first name and last are required for a pickup name change.", 
                                        tik.ItemShowDate.ToString("ddd MM/dd/yyyy hh:mmtt"), 
                                        tik.Ticket.ShowDateRecord.ShowRecord.ShowNamePart);
                                else//if we have inputs that are empty or have value
                                {
                                    string newPickup = string.Format("{0}{1}{2}", last.Trim().ToUpper().Replace(',', '_'), (last.Trim().Length > 0) ? ", " : string.Empty,
                                        first.Trim().ToUpper().Replace(',', '_')).Trim();//comma adds a space

                                    if (newPickup != tik.PickupName)
                                        tik.PickupName = (newPickup.Length > 0) ? newPickup : null;

                                    BindCart();
                                }
                            }
                        }
                    }
                    break;
                case "edit":
                    Response.Redirect("/Store/Cart_Edit.aspx");
                    break;
                case "rebindcart":
                    Ctx.Cart.OnCartChanged();
                    break;
            }

            //todo handle result from above
            if (result.Length > 0)
            {
                valCustom.IsValid = false;
                valCustom.ErrorMessage = result;
            }
           
            BindCart();

        }
        protected void ProcessCommand(object source, RepeaterCommandEventArgs e)
        {
            DropDownList ddl = (DropDownList)e.Item.FindControl("ddlQty");
            CustomValidator rowValidator = (CustomValidator)e.Item.FindControl("RowValidator");
            int qty = 0;

            if (ddl != null)
            {
                string selection = ddl.SelectedValue;
                qty = (selection != null && Utils.Validation.IsInteger(selection)) ? int.Parse(selection) : 0;
            }

            ProcessCommand(e.CommandName, int.Parse(e.CommandArgument.ToString()), qty, ddl, rowValidator);
        }
        private void ProcessCommand(string command, int idx, int qty, DropDownList ddl, CustomValidator rowValidator)
        {
            string result = null;
            int originalItemQty = 0;

            switch (command.ToLower())
            {
                case "clearcart":
                    Ctx.Cart.ClearCart();
                    BindCart();
                    return;
                case "updtkt":
                    SaleItem_Ticket sit = (SaleItem_Ticket)Ctx.Cart.TicketItems.Find(delegate(SaleItem_Ticket match) { return (match.tShowTicketId == idx); });
                    if (sit != null)
                    {
                        originalItemQty = sit.Quantity;

                        if (qty > 0)
                            result = Ctx.Cart.SaleItem_AddUpdate(_Enums.InvoiceItemContext.ticket, idx, qty, this.Profile);
                        else
                            //result = Ctx.Cart.SaleItem_AddUpdate(_Enums.InvoiceItemContext.ticket, idx, 0);
                            result = Ctx.Cart.SaleItem_Remove(_Enums.InvoiceItemContext.ticket, idx);
                    }
                    break;
                case "updmrc":
                    SaleItem_Merchandise mit = 
                        (SaleItem_Merchandise)Ctx.Cart.MerchandiseItems.Find(delegate(SaleItem_Merchandise match) { return (match.tMerchId == idx); });
                    if (mit != null)
                    {
                        originalItemQty = mit.Quantity;

                        if (qty > 0)
                        {
                            //result = Ctx.Cart.Update(idx, qty, "merch");
                            result = Ctx.Cart.SaleItem_AddUpdate(_Enums.InvoiceItemContext.merch, idx, qty, this.Profile);
                        }
                        else
                            //result = Ctx.Cart.RemoveItem(idx, "merch");
                            result = Ctx.Cart.SaleItem_Remove(_Enums.InvoiceItemContext.merch, idx);
                    }
                    break;
            }

            if (Ctx.Cart.ItemCount == 0 && (this.Page.ToString().ToLower() == "asp.store_checkout_aspx" ||  this.Page.ToString().ToLower() == "asp.store_shipping_aspx"))
                base.Redirect("/Store/Cart_Edit.aspx");

            if (result != null && result.Trim().Length > 0 && rowValidator != null)
            {
                //reset the drop down list
                ddl.SelectedIndex = originalItemQty;

                rowValidator.IsValid = false;
                rowValidator.ErrorMessage = result;

                return;
            }

            BindCart();
        }
        #endregion

        #region Binding
        
        protected void ProcessShowNames(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                ShowTicket _itm = (ShowTicket)e.Item.DataItem;

                Literal time = (Literal)e.Item.FindControl("LiteralTime");
                if (_itm != null && time != null)
                    time.Text = string.Format("<span class=\"agestimes\">{0} <span class=\"nowrap\">DOORS {1}{2}</span></span>", 
                        _itm.AgeRecord.Name,
                        _itm.DtDateOfShow.ToString("h:mmtt"),
                        (_itm.ShowDateRecord.ShowTime != null) ? string.Format(" / {0} SHOW", _itm.ShowDateRecord.ShowTime) : string.Empty);

                Literal venue = (Literal)e.Item.FindControl("litVenue");
                if (venue != null && _itm != null)
                    venue.Text = _itm.ShowDateRecord.ShowRecord.DisplayVenue_Wrapped(false, false, false);// DisplayVenue_JustifiedAndWrapped(false);

                Literal eventinfo = (Literal)e.Item.FindControl("LiteralEventInfo");
                if (eventinfo != null)
                {
                    eventinfo.Text = "<div class=\"mainact\">";

                    string heads = _itm.ShowDateRecord.wc_CartHeadliner;
                    if (heads != null && heads.Trim().Length > 0)
                        eventinfo.Text += heads.Trim();

                    string opens = _itm.ShowDateRecord.wc_CartOpeners;
                    if (opens != null && opens.Trim().Length > 0)
                        eventinfo.Text += string.Format("<span class=\"openers\">{0}</span>", opens.Trim());

                    eventinfo.Text += "</div>";
                }
            }
        }
        protected void ProcessBind(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                Repeater rpr = (Repeater)sender;
                bool isMerch = (rpr.ID.ToLower().IndexOf("merch") > 0);
                bool isTicket = (rpr.ID.ToLower().IndexOf("ticket") > 0);

                DropDownList ddl = (DropDownList)e.Item.FindControl("ddlQty");
                
                Literal pickup = (Literal)e.Item.FindControl("LiteralPickup");
                LinkButton add = (LinkButton)e.Item.FindControl("btnAdd");

                if (add != null)
                    needsReg.Add(add);
                
                object item = e.Item.DataItem;
                int objectId = 0;
                DateTime ttl = DateTime.MaxValue;
                bool extended = false;
                string parentId = string.Empty;
                string userName = (this.Profile != null && (!this.Profile.IsAnonymous)) ? this.Profile.UserName : null;


                if (ddl != null && ddl.Items.Count == 0 && item != null)
                {
                    int currentQty = 0;
                    int max = 0;
                    //WillCallWeb.StoreObjects.SaleItem_Base _ITEM = null;

                    if (isMerch)
                    {
                        WillCallWeb.StoreObjects.SaleItem_Merchandise _itm = (WillCallWeb.StoreObjects.SaleItem_Merchandise)item;

                        ttl = _itm.TTL;
                        extended = _itm.TTL_Extended;
                        objectId = _itm.tMerchId;
                        parentId = "mrc";
                        currentQty = _itm.Quantity;
                        //max = _itm.MerchItem.MaxQuantityPerOrder;
                        //this may get overridden by requirements                        
                        max = RequiredMerch.SetMaxAllowedBasedOnRequired(userName, _itm.MerchItem.ParentMerchRecord);

                        if (_itm.MerchItem.SpecialInstructions != null)
                        {
                            Literal special = (Literal)e.Item.FindControl("litSpecialInstructions");
                            special.Text = string.Format("<div class=\"special\">{0}</div>", _itm.MerchItem.SpecialInstructions);
                        }

                        //we need to track that we only display bundles for one item
                        //this should be the first item added!
                        //compare with the parent id
                        if (_itm.MerchItem.TParentListing.HasValue)
                        {
                            //how are items listed in cart - what order?
                            //only allow first occurrence of a merch parent to show bundle                            
                            //if the item index is the first index of the same parent id
                            List<SaleItem_Merchandise> rptList = new List<SaleItem_Merchandise>();
                            rptList.AddRange((List<SaleItem_Merchandise>)rpr.DataSource);
                            int firstOrd = rptList.FindIndex(delegate(SaleItem_Merchandise match) { return (match.MerchItem.TParentListing == _itm.MerchItem.TParentListing.Value); });

                            if (firstOrd == e.Item.ItemIndex)
                                DisplayBundles(_itm, e);
                        }
                    }
                    else if (isTicket)
                    {
                        WillCallWeb.StoreObjects.SaleItem_Ticket _itm = (WillCallWeb.StoreObjects.SaleItem_Ticket)item;

                        ttl = _itm.TTL;
                        extended = _itm.TTL_Extended;
                        objectId = _itm.tShowTicketId;
                        parentId = _itm.Ticket.ShowDateRecord.TShowId.ToString();
                        currentQty = _itm.Quantity;
                        //this may get overridden by requirements                        
                        max = RequiredShowTicketPastPurchase.SetMaxAllowedBasedOnRequired(userName, _itm.Ticket);

                        Literal sts = (Literal)e.Item.FindControl("LiteralStatus");
                        sts.Text = (_itm.Ticket.Status != null && _itm.Ticket.Status.Trim().Length > 0) ?
                            string.Format("<div class=\"ticketstatus\">{0}</div>", _itm.Ticket.Status.Trim()) : string.Empty;

                        ShowTicketCollection coll = new ShowTicketCollection();
                        coll.Add(_itm.Ticket);
                        if (_itm.Ticket.IsPackage && (!_itm.Ticket.IsCampingPass()))
                        {
                            coll.AddRange(_itm.Ticket.LinkedShowTickets);
                            if (coll.Count > 1)
                                coll.Sort("DtDateOfShow", true);

                            Literal pkg = (Literal)e.Item.FindControl("LiteralPackage");
                            if (pkg != null)
                                pkg.Text += string.Format("<div class=\"pkginfo\">{0} SHOW PASS</div>", coll.Count);
                        }

                        if (_itm.Ticket.IsCampingPass())
                        {
                            Literal camping = (Literal)e.Item.FindControl("litCamping");
                            if (camping != null)
                                camping.Text = string.Format("<div class=\"eventdate\">CAMPING - {0} - {1}</div>", _itm.Ticket.AgeRecord.Name, 
                                    _itm.Ticket.ShowDateRecord.ShowRecord.ShowEventPart);
                        }
                        else
                        {
                            Repeater packages = (Repeater)e.Item.FindControl("rptShowNames");
                            packages.DataSource = coll;
                            packages.DataBind();
                        }

                        if (_Config._Allow_HideShipMethod && (!_itm.Ticket.HideShipMethod))
                        {
                            Literal pickupOptions = (Literal)e.Item.FindControl("LiteralPickupOptions");
                            if (pickupOptions != null)
                                pickupOptions.Text = ConstructPickupOptions(_itm);
                        }

                        Literal description = (Literal)e.Item.FindControl("LiteralDescription");
                        if (description != null)
                        {
                            if (_itm.Ticket.ShowDateRecord.ShowRecord.ShowTitle != null && _itm.Ticket.ShowDateRecord.ShowRecord.ShowTitle.Trim().Length > 0)
                                description.Text = string.Format("<div class=\"showtitle\">{0}</div>", _itm.Ticket.ShowDateRecord.ShowRecord.ShowTitle.Trim());
                            if (_itm.Ticket.SalesDescription != null && _itm.Ticket.SalesDescription.Trim().Length > 0)
                                description.Text += string.Format("<div class=\"description\">{0}</div>", _itm.Ticket.SalesDescription.Trim());
                            if (_itm.Ticket.CriteriaText != null && _itm.Ticket.CriteriaText.Trim().Length > 0)
                                description.Text += string.Format("<div class=\"criteria\">{0}</div>", _itm.Ticket.CriteriaText.Trim());
                        }

                        //POST PURCHASE TEXT
                        if (this.Page.ToString().ToLower().IndexOf("shipping_aspx") != -1)
                        {
                            PostPurchaseTextCollection postColl = new PostPurchaseTextCollection();
                            postColl.AddRange(_itm.Ticket.PostPurchaseTextRecords().GetList()
                                .FindAll(delegate(PostPurchaseText match) { return (match.InProcessDescription != null && match.InProcessDescription.Trim().Length > 0); }));
                            if (postColl.Count > 0)
                            {
                                Literal postPurchase = (Literal)e.Item.FindControl("litPostPurchaseText");
                                if (postPurchase != null)
                                {
                                    postColl.Sort("IDisplayOrder", true);
                                    postPurchase.Text = "<div class=\"postpurchase\">";

                                    foreach (PostPurchaseText pp in postColl)
                                        postPurchase.Text += string.Format("<div class=\"pptext\">{0}</div>",
                                            System.Web.HttpUtility.HtmlEncode(pp.InProcessDescription.Trim()));

                                    postPurchase.Text += "</div>";
                                }
                            }
                        }

                        //add in bundles
                        DisplayBundles(_itm, e);
                    }                    

                    //set quantity
                    for (int i = 0; i <= max; i++)
                        ddl.Items.Add(new ListItem(i.ToString()));

                    ddl.SelectedIndex = currentQty ;
                }

                //workout postback validation for 3rd party and timer
                Literal timer = (Literal)e.Item.FindControl("LiteralTimer");
                if (isTicket && timer != null)
                {
                    timer.Text = ConstructTimer(objectId, extended, ttl);
                }

                //only do this for tickets
                if (isTicket && pickup != null && _Config._Allow_3rdPartyPurchase && objectId > 0)
                {
                    int depth = 0;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    WillCallWeb.StoreObjects.SaleItem_Ticket _itm = (WillCallWeb.StoreObjects.SaleItem_Ticket)item;
                    
                    //TODO: change to first and last - both required
                    string pickupName = (_itm.PickupName.Trim().Length > 0) ? _itm.PickupName.Trim().ToUpper() : "PURCHASER"; 
                    
                    //only allow changes to be made in the edit cart - not on checkout - not on confirmation
                    bool showTextBox = (this.Page.ToString().ToLower() == "asp.store_cart_edit_aspx");// (Auth == null && EditMode && Ctx.CurrentShowID == 0);
                    string linkText = (showTextBox) ? "changepickup" : "edit"; 
                    
                    sb.AppendFormat("{1}<div id=\"Pkp{2}\" class=\"pickupname\">{0}", Constants.NewLines(1), Constants.Tabs(++depth), objectId);

                    if (showTextBox)
                    {
                        string first = string.Empty;
                        string last = string.Empty;

                        //note: because the only error we are catching is to see if both inputs are being used
                        //we can have an indicator that reponds to that situation
                        string reqFirst = Request[string.Format("PkpFirst{0}", objectId)];
                        string reqLast = Request[string.Format("PkpLast{0}", objectId)];

                        //both values = null indicates first entry to page
                        //so if they aren't null and one or the other is empty - use request values 
                        //otherwise, use the pickupName
                        if (reqFirst != null && reqLast != null && ((reqFirst.Trim().Length == 0 && reqLast.Trim().Length > 0) ||
                            (reqLast.Trim().Length == 0 && reqFirst.Trim().Length > 0)))
                        {
                            sb.AppendFormat("{0}{1}<span class=\"validator\" style=\"color: Red; font-size: 2em;\">*</span>", Constants.NewLines(1), Constants.Tabs(depth + 1));

                            first = reqFirst.Trim();
                            last = reqLast.Trim();
                        }
                        else if (pickupName != "PURCHASER")
                        {
                            sb.AppendFormat("{0}{1}", Constants.NewLines(1), Constants.Tabs(depth + 1));

                            string[] parts = pickupName.Split(',');

                            //last is first!
                            if (parts.Length > 0) last = parts[0].Trim().ToUpper();
                            if (parts.Length > 1) first = parts[1].Trim().ToUpper();
                        }

                        sb.AppendFormat("Tickets to be picked up by:");
                        sb.AppendFormat("<div>First Name: <input type=\"text\" id=\"PkpFirst{2}\" name=\"PkpFirst{2}\" maxlength=\"50\" value=\"{3}\" {4}/></div>{0}",
                            Constants.NewLines(1), Constants.Tabs(depth), objectId, first, (showTextBox) ? string.Empty : "READONLY");
                        sb.AppendFormat("<span>Last Name: <input type=\"text\" id=\"PkpLast{2}\" name=\"PkpLast{2}\" maxlength=\"50\" value=\"{3}\" {4}/></span>{0}",
                            Constants.NewLines(1), Constants.Tabs(depth), objectId, last, (showTextBox) ? string.Empty : "READONLY");

                        string href = Page.ClientScript.GetPostBackClientHyperlink(this, string.Format("{0}~{1}", linkText, objectId)).Replace("'", "&#39;");

                        sb.AppendFormat("{1}&nbsp;<a href=\"{2}\">{3}</a>{0}", Constants.NewLines(1), Constants.Tabs(depth + 1), href, "save name");
                    }
                    else
                        sb.AppendFormat("{1}Tickets to be picked up by: {2}{0}",
                            Constants.NewLines(1), Constants.Tabs(depth + 1), pickupName);

                    sb.AppendFormat("{1}</div>{0}", Constants.NewLines(1), Constants.Tabs(depth--));

                    pickup.Text = sb.ToString();
                }
            }
        }
        private string ConstructPickupOptions(SaleItem_Ticket _itm)
        {
            string txt = string.Empty;

            if (_Config._Shipping_Tickets_Active)
            {
                if (_itm.Ticket.IsCurrentlyShippable)
                {
                    if (_itm.Ticket.IsShipSeparate && _itm.Ticket.FlatMethod != null)
                    {
                        txt = string.Format("this items ships for {0} via {1}.",
                            (_itm.Ticket.FlatShip == 0) ? "FREE" : _itm.Ticket.FlatShip.ToString("c"), _itm.Ticket.FlatMethod);
                    }
                    else
                    {
                        //decide what to show based on current page in flow - note that no option is shown for ship page
                        //else
                        if (this.Page.ToString().ToLower() == "asp.store_checkout_aspx")
                        {
                            txt = "shipping options available on next page";
                        }
                        else if (this.Page.ToString().ToLower() == "asp.store_shipping_aspx")
                        {
                            //find the correlating ship method
                            string method = ShipMethod.WillCall;
                            foreach (SaleItem_Shipping ship in Ctx.Cart.Shipments_Tickets)
                            {
                                if (ship.Items_Tickets.Contains(_itm))
                                {
                                    method = ship.ShipMethod;
                                    break;
                                }
                            }

                            txt = string.Format("Current shipping method: <b>{0}</b>", method);
                        }
                        else
                        {
                            txt = "shipping options available at checkout";
                        }

                        if (!_itm.Ticket.IsAllowWillCall)
                            txt = txt.Insert(0, "<div><b style=\"color: red;\">these tickets NOT available for WillCall</b></div>");
                    }
                }
                else if (_itm.Ticket.IsAllowWillCall)
                    txt = "<div><b style=\"color: red;\">tickets to be picked up at venue's WillCall only</b></div>";


                if (txt.Trim().Length > 0)
                    txt = string.Format("<div class=\"pickupoptions\">{0}</div>", txt);

            }

            return txt;
        }
        private string ConstructTimer(int tShowTicketId, bool isExtended, DateTime ttl)
        {
            if (_Config._Display_CountdownTimer && tShowTicketId > 0)
            {
                int depth = 0;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                string itemName = tShowTicketId.ToString();

                sb.AppendFormat("{1}<div id=\"tmrcnt{2}\" class=\"TimerContainer\">{0}", Constants.NewLines(1), Constants.Tabs(depth), itemName);

                //COUNTDOWN CLOCK
                //this sets up a div to be written by the JS
                sb.AppendFormat("{1}<span id=\"tmr{2}\" class=\"TimerClock\"></span>{0}", Constants.NewLine, Constants.Tabs(depth), itemName);

                //FUNCTIONS FOR TIMER
                if (!isExtended)
                {
                    sb.AppendFormat("{1}<span id=\"tmrfnc{2}\" class=\"TimerFunction\">{0}", Constants.NewLines(1), Constants.Tabs(++depth), itemName);

                    //Page.GetPostBackEventReference or Page.GetPostBackClientHyperlink methods (these methods are practically identical, 
                    //the only difference is that GetPostBackClientHyperlink adds "javascript:" prefix in the beginning of the returned string).

                    string href = Page.ClientScript.GetPostBackClientHyperlink(this,
                        string.Format("addtime~{0}", tShowTicketId)).Replace("'", "&#39;");

                    sb.AppendFormat("{1}<a href=\"{2}\" class=\"btntribe\">add more time</a>{0}", Constants.NewLines(1), Constants.Tabs(++depth), href);

                    sb.AppendFormat("{1}</span>{0}", Constants.NewLines(1), Constants.Tabs(--depth));
                }

                Ctx.CurrentCartItems += string.Format("{0},{1}~", itemName, ttl);

                sb.AppendFormat("{1}</div>{0}", Constants.NewLines(1), Constants.Tabs(--depth));

                return sb.ToString();
            }

            return string.Empty;
        }

        private void DisplayBundles(SaleItem_Base item, RepeaterItemEventArgs e)
        {
            //add in bundles
            Panel pnlBundle = (Panel)e.Item.FindControl("pnlBundle");
            if (pnlBundle != null)
            {
                WillCallWeb.Components.Cart.Bundle_Listing listing =
                    (WillCallWeb.Components.Cart.Bundle_Listing)LoadControl(@"\Components\Cart\Bundle_Listing.ascx");
                listing.SaleItem = item;
                pnlBundle.Controls.Add(listing);
            }
        }
        
        #endregion

        #region Shipping

        protected void chkShipMultiple_CheckChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            //***if this round trip process is taking too long - place logic for toggling cart in processing page
            Ctx.Cart.IsShipMultiple_Merch = chk.Checked;//check text asks to split shipments
            
            base.Redirect("ProcessingShipping.aspx");
        }

        #region Tickets

        private List<ListItem> _list_Tix = new List<ListItem>();

        //combine list visibility with literal visibility
        protected void rdoTixShipRates_DataBinding(object sender, EventArgs e)
        {
            if (ticketshipping.Visible && Ctx.Cart.HasTicketItems_CurrentlyShippable)
            {
                //verify ship address
                if (Ctx.SessionInvoice != null && Ctx.SessionInvoice.InvoiceBillShip != null)
                {
                    //a dummy list to use ups's address verification
                    List<ListItem> lst = new List<ListItem>();

                    string address = Ctx.SessionInvoice.WorkingShippingAddress;
                    string country = Ctx.SessionInvoice.WorkingCountry.Trim();
                    if (country.ToLower() == "usa")
                        country = "us";
                    string zip = Ctx.SessionInvoice.WorkingZip;
                    string state = Ctx.SessionInvoice.WorkingState;

                    //we consider tickets to be of a fixed weight regardless of # of tix
                    decimal weight = 1;

                    if (country.Length > 0 && zip.Length > 0 && state.Length > 0 && weight > 0 && _Config._Shipping_Tickets_Active)
                    {
                        try
                        {
                            //use this list to verify UPS shippability
                            //itemlist not needed for ticket shipping
                            //will throw error and stop flow if there is an exception - bad address, etc
                            lst = ecommercemax_shipping.GetShipRates(0, address, country, zip, state, weight, true, null);

                            //if the lst has no count - no valid methods were returned
                            //will call is the only option
                            //- not available indicates to list that it should be disabled

                            //only make a list if we have items to ship in a bulk/batch
                            if (Ctx.Cart.TicketItems_ShipsInGeneralBatch.Count > 0)
                            {
                                bool HasWillCallOption = false;

                                //do not add will call if we are purchasing options for previous tix
                                //also, if list contains an item that cant ship - then no will call option                                
                                if (Ctx.Cart.TicketItems_NotAvailableForWillCall.Count > 0)
                                {
                                    //ensure that shipment is cont us
                                    if (Utils.Shipping.IsContinentalUsShipment(country, state))
                                    {
                                        ListItem noWillCall = new ListItem(string.Format("{0} - not available", ShipMethod.WillCall), string.Empty);
                                        noWillCall.Enabled = false;
                                        noWillCall.Selected = false;
                                        _list_Tix.Add(noWillCall);
                                    }
                                }
                                else
                                {
                                    HasWillCallOption = true;
                                    _list_Tix.Add(new ListItem(string.Format("{0} - $0.00", ShipMethod.WillCall),
                                        string.Format("{0}~{1}", ShipMethod.WillCall, "0")));
                                }

                                //this ensures we have at least a will call option
                                if (!HasWillCallOption && (!_Config._Shipping_AllowTicketsToPoBox && Utils.Shipping.IsPoBoxAddress(address)))
                                    throw new Exception("Sorry, tickets cannot be shipped to PO Boxes.");



                                //if we are trying to ship to continental US
                                //no need to tell customer exactly how it is getting there
                                if (Utils.Shipping.IsContinentalUsShipment(country, state))
                                {
                                    _list_Tix.Add(new ListItem(string.Format("{0} - {1}{2}",
                                       _Config._Shipping_Tickets_DefaultMethod, _Config._Shipping_Tickets_FixedAmount.ToString("c"),
                                       //if the address is not valid - a po box - mark as not available
                                       (! _Config._Shipping_AllowTicketsToPoBox && Utils.Shipping.IsPoBoxAddress(address)) ? 
                                       " - not available - cannot ship to PO Boxes" : string.Empty),
                                       string.Format("{0}~{1}", _Config._Shipping_Tickets_DefaultMethod, _Config._Shipping_Tickets_FixedAmount)));
                                }
                                else if (_list_Tix.Count == 0)
                                    throw new Exception("Tickets may only be shipped within the continental United States.");
                            }
                        }
                        catch (System.Threading.ThreadAbortException) { }
                        catch (Exception ex)
                        {
                            Ctx.CurrentCartException = ex.Message;
                            base.Redirect("/Store/Checkout.aspx");
                        }

                        //SHIPPING ADDRESS HAS BEEN VERIFIED AT THIS POINT!!

                        //show and list any separate shipments
                        int separate = Ctx.Cart.TicketItems.FindIndex(delegate(SaleItem_Ticket match) { return (match.Ticket.IsCurrentlyShippable && match.Ticket.IsShipSeparate); });

                        if (separate != -1 && Ctx.Cart.Shipment_Tickets_Main != null && Ctx.Cart.Shipment_Tickets_Main.Items_Tickets.Count > 0)
                        {
                            litTicketShipping.Text = "<div class=\"ticketship-options\">Your cart includes tickets that do not include shipping in the ticket price.</div>";
                            litTicketShipping.Text += "<div class=\"ticketship-options\">Please select a ship method for tickets that do not include shipping in the price.</div>";
                        }
                        else if (separate != -1)
                            litTicketShipping.Text = "<div class=\"ticketship-options\">Your cart includes tickets that include shipping in the ticket price.</div>";

                        //fill list with shipping choices
                        RadioButtonList list = (RadioButtonList)sender;

                        if (Ctx.Cart.Shipment_Tickets_Main != null && Ctx.Cart.Shipment_Tickets_Main.Items_Tickets.Count > 0)
                        {
                            list.Visible = true;
                            list.DataTextField = "Text";
                            list.DataValueField = "Value";
                            list.DataSource = _list_Tix;                            
                        }
                        else
                        {
                            _list_Tix.Clear();
                            list.Visible = false;
                        }

                    }
                }
            }
        }
        protected void rdoTixShipRates_DataBound(object sender, EventArgs e)
        {
            if (ticketshipping.Visible)
            {
                RadioButtonList list = (RadioButtonList)sender;

                //spruce up the list for display
                foreach (ListItem li in list.Items)
                {
                    if (li.Text.ToLower().IndexOf("- not available") != -1)
                    {
                        li.Enabled = false;
                    }
                }

                SaleItem_Shipping ship = Ctx.Cart.Shipment_Tickets_Main;
                if (ship != null)
                {
                    int idx = ship.Items_Tickets.FindIndex(delegate(SaleItem_Ticket match) { return (!match.Ticket.IsAllowWillCall); } );
                    if (ship.ShipMethod == Wcss.ShipMethod.WillCall && idx != -1)
                    {
                        //then assign the default method
                        ship.ShipCost = _Config._Shipping_Tickets_FixedAmount;
                        ship.ShipMethod = _Config._Shipping_Tickets_DefaultMethod;
                    }

                    string toFind = string.Format("{0}~{1}", ship.ShipMethod, ship.ShipCost.ToString());

                    ListItem li = list.Items.FindByValue(toFind);
                    if (li != null)
                    {
                        list.SelectedIndex = -1;
                        li.Selected = true;
                    }
                }                    
            }
        }
        protected void rdoTixShipRates_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList list = (RadioButtonList)sender;

            if (list.Items.Count > 0)
            {
                if (list.SelectedIndex == -1 && list.Items[0].Enabled)
                    list.SelectedIndex = 0;

                AssignTicketShippingToCart(list);
            }
        }
        protected void AssignTicketShippingToCart(RadioButtonList listControl)
        {
            if (Ctx.SessionInvoice != null)
            {   
                string[] args = listControl.SelectedItem.Value.Split('~');

                string method = args[0];
                decimal cost = decimal.Parse(args[1]);

                Ctx.Cart.Shipments_Tickets.Clear();
                Ctx.Cart.Shipment_Tickets_Main.ShipMethod = method;
                Ctx.Cart.Shipment_Tickets_Main.ShipCost = cost;

                BindCart();
            }
        }
        #endregion

        #endregion
}
}
//110711 - 1074