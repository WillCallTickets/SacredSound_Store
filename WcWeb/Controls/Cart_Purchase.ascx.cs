using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Wcss;

/* This cart is used for post purchase */
//<script type="text/javascript" src="/JQueryUI/jquery-ui-1.7.2.custom.min.js"></script> - old includes
//<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.9/jquery-ui.min.js"></script>

namespace WillCallWeb.Controls
{
    public partial class Cart_Purchase : WillCallWeb.BaseControl, System.Web.SessionState.IRequiresSessionState
    {
        protected Invoice _invoice;
        protected string _email;
        protected bool _hasGiftCertToRedeem = false;
        protected bool _hasCreditItem = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Ctx.SessionAuthorizeNet != null)
            {
                _invoice = Ctx.SessionAuthorizeNet.InvoiceRecord;

                if (_invoice == null)
                {
                    Exception ex = new Exception("Session invoice could not be found at purchaseCart.ascx.");
                    _Error.LogException(ex);
                    throw ex;
                }

                _email = Ctx.SessionAuthorizeNet.Email;

                BindCart();
            }
            else
            {
                Exception ex = new Exception("Session authNet could not be found at purchaseCart.ascx.");
                _Error.LogException(ex);
                throw ex;
            }
        }

        #region Binding Overview

        public void BindCart()
        {
            BindDonations();
            BindTickets();
            BindTicketShipping();
            BindMerch();
            BindMerchShipping();
            BindPromo();
            BindRefunds();
        }

        protected void BindDonations()
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();
            coll.AddRange(_invoice.InvoiceItemRecords().GetList().FindAll(
                delegate(InvoiceItem match) { return (match.IsCharitableItem); }));

            donationPanel.Visible = coll.Count > 0;
            rptDonation.DataSource = coll;
            rptDonation.DataBind();
            rptDonation.Visible = donationPanel.Visible;
        }
        protected void BindTickets()
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();
            coll.AddRange(_invoice.InvoiceItemRecords().GetList().FindAll(
                delegate(InvoiceItem match) { return ((match.IsTicketItem && match.LineItemTotal > 0) || (match.IsTicketItem && match.IsPromotionItem)); }));
            coll.SortTicketItemsBy_DateToOrderBy();
            //if (coll.Count > 1)
            //    coll.Sort("DateOfShow_ToSortBy", true);

            tktPanel.Visible = (coll.Count > 0);
            rptTicketItems.DataSource = coll;
            rptTicketItems.DataBind();
            rptTicketItems.Visible = coll.Count > 0;
        }
        protected void BindTicketShipping()
        {
            //show ticket shipping
            if (_invoice.HasTicketShipmentItemsOtherThanWillCall)
            {
                rptTicketShipments.Visible = true;
                rptTicketShipments.DataSource = _invoice.TicketShipmentItems;
                rptTicketShipments.DataBind();
            }
            else
                rptTicketShipments.Visible = false;
        }
        protected void BindMerch()
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();
            coll.AddRange(_invoice.InvoiceItemRecords().GetList().FindAll(
                delegate(InvoiceItem match) { return ((!match.IsBundle) && (!match.IsBundleSelection) && match.IsMerchandiseItem); }));

            mrcPanel.Visible = coll.Count > 0;
            rptMerchItems.DataSource = coll;
            rptMerchItems.DataBind();
            rptMerchItems.Visible = mrcPanel.Visible;
        }
        protected void BindMerchShipping()
        {
            //show merch shipping
            if (_invoice.HasMerchandiseShipmentItems)
            {
                rptMerchShipments.Visible = true;
                rptMerchShipments.DataSource = _invoice.MerchandiseShipmentItems;
                rptMerchShipments.DataBind();
            }
            else
                rptMerchShipments.Visible = false;
        }
        protected void BindPromo()
        {
            //only show discounts with actual discounts
            InvoiceItemCollection promos = new InvoiceItemCollection();
            promos.AddRange(_invoice.InvoiceItemRecords().GetList().FindAll(
                delegate(InvoiceItem match) { return ((match.IsDiscountItem) && match.Price < 0); }));

            promoPanel.Visible = promos.Count > 0;
            rptPromo.DataSource = promos;
            rptPromo.DataBind();
            rptPromo.Visible = promoPanel.Visible;
        }
        protected void BindRefunds()
        {
            bool showRefundInfo = (_invoice.InvoiceStatus == _Enums.InvoiceStatii.PartiallyRefunded.ToString() ||
                _invoice.InvoiceStatus == _Enums.InvoiceStatii.Refunded.ToString());
            RefundInfo.Visible = showRefundInfo;

            if (showRefundInfo)
                GridRefunds.DataBind();
        }

        #endregion

        #region Shipping

        //handles both merch and ticket shipments
        protected void Shipment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            InvoiceItem item = (InvoiceItem)e.Item.DataItem;

            if (item != null)
            {
                bool itemRefunded = (item.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString());

                HtmlGenericControl divPurchase = (HtmlGenericControl)e.Item.FindControl("divPurchaseDesc");
                if (divPurchase != null)
                    divPurchase.Attributes.Add("class", (itemRefunded) ? "purchasedescriptionref item-container" : "item-container");

                HtmlGenericControl pricing = (HtmlGenericControl)e.Item.FindControl("ItemPriceInfo");

                Literal shipDate = (Literal)e.Item.FindControl("litShipDate");
                if (shipDate != null)
                {
                    if (item.DateShipped < DateTime.MaxValue)
                        shipDate.Text = string.Format("<span class=\"item-ship\">shipped on {0}</span>", item.DateShipped.ToString("MM/dd/yyyy"));
                    else if (_Config._DisplayEstimatedShipDates && item.DateOfShow < DateTime.MaxValue)
                        shipDate.Text = string.Format("<span class=\"item-ship\">(Item(s) in this shipment will ship on or about {0})</span>", item.DateOfShow.ToString("MM/dd/yyyy"));
                }

                Literal status = (Literal)e.Item.FindControl("LiteralItemStatus");

                if (status != null)
                {
                    if (itemRefunded)
                        status.Text = string.Format("<div class=\"refunditem\">This item has been refunded/exchanged and is no longer valid.</div>");
                    else if (item.Notes != null && item.Notes.ToLower().IndexOf("exchanged from:") != -1)
                    {
                        status.Text = "<div class=\"refunditem\">Item Exchange</div>";
                        pricing.Visible = false;
                    }
                    else if (item.MainActName.ToLower().IndexOf("ups standard") != -1)
                        status.Text = "<div class=\"shipthrucustoms\">***Purchaser will be responsible for any fees incurred transporting order from customs to destination***</div>";
                }

                Repeater rptContent = (Repeater)e.Item.FindControl("rptContents");

                if (rptContent != null)
                {
                    rptContent.DataSource = item.InvoiceRecord.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match) { return (match.TShipItemId == item.Id); });
                    rptContent.DataBind();
                }
            }
        }

        protected void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;
            InvoiceItem item = (InvoiceItem)e.Item.DataItem;

            if (item != null && item.IsBundleSelection)
            {
                Literal lit = (Literal)e.Item.FindControl("litBundleItem");
                if (lit != null)
                    lit.Text = "<span class=\"bundle-indent\">*</span>";
            }
        }

        #endregion

        #region Binding Modules

        protected void rptItem_Bind(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                Repeater rpr = (Repeater)sender;
                bool isTicket = (rpr.ID.ToLower().IndexOf("ticket") > 0);
                bool isMerch = (rpr.ID.ToLower().IndexOf("merch") > 0);
                InvoiceItem item = (InvoiceItem)e.Item.DataItem;

                if (item != null)
                {
                    if (item.IsGiftCertificateDelivery && item.MainActName.ToLower().IndexOf("store credit") != -1)
                        _hasCreditItem = true;

                    //deal with refunds and exchanges
                    ConfigureRefundExchange(item, e);

                    //the item is a ticket or a merch - get its bundled counterparts
                    DisplayInfo(item, e);
                }
            }
        }
        protected void rptPromo_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                Repeater rpr = (Repeater)sender;
                InvoiceItem item = (InvoiceItem)e.Item.DataItem;
                Panel pricing = (Panel)e.Item.FindControl("pnlPricing");

                if (item.IsGiftCertificateDelivery && item.MainActName.ToLower().IndexOf("storecredit") != -1)
                    _hasCreditItem = true;

                Literal promoDisplay = (Literal)e.Item.FindControl("litPromoDisplay");
                if (promoDisplay != null && item != null && item.Description != null && item.Description.Trim().Length > 0)
                    promoDisplay.Text = string.Format("<div class=\"promodisplaytext\">{0}</div>", item.Description);

                if (pricing != null)
                {
                    pricing.Visible = false;

                    if (item != null && item.LineItemTotal != 0)
                    {
                        Literal price = (Literal)e.Item.FindControl("litPrice");

                        if (price != null)
                        {
                            pricing.Visible = true;
                            decimal prc = item.LineItemTotal;
                            price.Text = (prc > 0) ? prc.ToString("c") : string.Format("  you saved {0} on your order", decimal.Negate(prc).ToString("c"));
                        }
                    }
                }
            }
        }
        protected void rptDonation_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                Repeater rpr = (Repeater)sender;
                InvoiceItem item = (InvoiceItem)e.Item.DataItem;

                bool itemRefunded = (item.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString());

                Literal status = (Literal)e.Item.FindControl("LiteralItemStatus");

                if (status != null && itemRefunded)
                    status.Text = string.Format("<div class=\"refunditem\">This item has been refunded/exchanged and is no longer valid.</div>");
            }
        }

        /// <summary>
        /// setups up certain info depending on the InvoiceItem's status. A refunded or exchanged item should clearly 
        /// note that status and allow for the container to be presented differently. Exchanged items do not show price
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e"></param>
        private void ConfigureRefundExchange(InvoiceItem item, RepeaterItemEventArgs e)
        {
            HtmlGenericControl divPurchase = (HtmlGenericControl)e.Item.FindControl("divPurchaseDesc");
            Literal itemStatus = (Literal)e.Item.FindControl("litItemStatus");

            if (divPurchase != null && itemStatus != null)
            {
                bool itemRefunded = (item.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString());

                divPurchase.Attributes.Add("class", (itemRefunded) ? "item-container item-refunded" : "item-container");

                if (itemRefunded)
                    itemStatus.Text = string.Format("<div class=\"refunditem\">This item has been refunded/exchanged and is no longer valid.</div>");
                else if (item.Notes != null && item.Notes.ToLower().IndexOf("exchanged from:") != -1)
                    itemStatus.Text = "<div class=\"refunditem\">Item Exchange</div>";
            }
        }

        private string GetDescription(InvoiceItem item)
        {
            string description = string.Empty;

            if (item.IsTicketItem && item.ShowTicketRecord.ShowDateRecord.ShowRecord.ShowTitle != null && item.ShowTicketRecord.ShowDateRecord.ShowRecord.ShowTitle.Trim().Length > 0)
                description += string.Format("<div class=\"showtitle\">{0}</div>", item.ShowTicketRecord.ShowDateRecord.ShowRecord.ShowTitle.Trim());

            if (item.Description != null && item.Description.Trim().Length > 0)
                description += string.Format("<div class=\"description\">{0}</div>", item.Description.Trim()); 
            
            if ((!item.IsBundle) && (!item.IsBundleSelection) && (!item.IsDeliverableByCode))
                if (item.Criteria != null && item.Criteria.Trim().Length > 0)
                    description += string.Format("<div class=\"criteria\">{0}</div>", item.Criteria.Trim());

            return description;
        }
        private string GetPostPurchaseText(InvoiceItem item)
        {
            string post = string.Empty;

            if (item.IsGiftCertificateDelivery && item.MainActName.ToLower().IndexOf("gift certificate") == -1)
            {
                InvoiceItemPostPurchaseTextCollection postColl = new InvoiceItemPostPurchaseTextCollection();
                postColl.AddRange(item.InvoiceItemPostPurchaseTextRecords());

                if (postColl.Count > 0)
                {
                    postColl.Sort("IDisplayOrder", true);

                    post = "<div class=\"postpurchase item-option\">";

                    foreach (InvoiceItemPostPurchaseText pp in postColl)
                    {
                        string pgName = this.Page.ToString();
                        post += string.Format("<div class=\"pptext\">{0}</div>",
                            (pgName == "ASP.store_printconfirm_aspx") ?
                                Utils.ParseHelper.LinksToHref(pp.PostText.Trim()) : pp.PostText.Trim());
                    }

                    post += "</div>";
                }
            }

            return post;
        }
        private string GetShowNameListing(int qty, ShowTicketCollection coll)
        {
            string listing = string.Empty;

            if (coll.Count > 0)
            {
                int i = 0;
                foreach (ShowTicket st in coll)
                {
                    listing += "<div class=\"eventdate\">";
                    listing += string.Format("<div class=\"datelist\">{0} @ {1}</div>", qty.ToString(), st.DateOfShow.ToString("ddd MMM dd yyyy"));

                    listing += string.Format("<span class=\"agestimes\">{0} <span class=\"nowrap\">DOORS {1}{2}</span></span>",
                        st.AgeRecord.Name,
                        st.DtDateOfShow.ToString("h:mmtt"),
                        (st.ShowDateRecord.ShowTime != null) ? string.Format(" / {0} SHOW", st.ShowDateRecord.ShowTime) : string.Empty);

                    listing += "</div>";

                    //showdatetitle
                    if (st.ShowDateRecord.ShowDateTitle != null && st.ShowDateRecord.ShowDateTitle.Trim().Length > 0)
                        listing += string.Format("<div class=\"showdatetitle\">{0}</div>", st.ShowDateRecord.ShowDateTitle);

                    //venue
                    listing += st.ShowDateRecord.ShowRecord.DisplayVenue_Wrapped(false, false, false);

                    //eventinfo
                    listing += "<div class=\"mainact\">";

                    string heads = st.ShowDateRecord.wc_CartHeadliner;
                    if (heads != null && heads.Trim().Length > 0)
                        listing += heads.Trim();

                    string opens = st.ShowDateRecord.wc_CartOpeners;
                    if (opens != null && opens.Trim().Length > 0)
                        listing += string.Format("<span class=\"openers\">{0}</span>", opens.Trim());

                    listing += "</div>";

                    //separator
                    if (i++ < (coll.Count - 1))
                        listing += "<div class=\"item-separator\">&nbsp;</div>";
                }
            }

            return listing;
        }
        private string GetPickupName(InvoiceItem item, bool isPackage)
        {
            string pickup = string.Empty;

            if (_Config._Allow_HideShipMethod && (!item.ShowTicketRecord.HideShipMethod))
            {
                if (item.ShippingMethod != null && item.ShippingMethod.Trim().Length > 0 && item.ShippingMethod != ShipMethod.WillCall)
                    pickup += string.Format("<div class=\"pickupname shipped item-option\" >ticket(s) to be shipped via {0}</div>",
                        item.ShippingMethod);
                else
                {
                    string pickerUpper = (item.PickupName != null && item.PickupName.Trim().Length > 0 && item.PickupName.Trim().ToLower() != "purchaser") ?
                            item.PickupName : item.PurchaseName;
                    pickup += string.Format("<div class=\"pickupname item-option\" >{0}Tickets {1}at venue's WillCall under: {2}</div>",
                        (isPackage) ? "All " : string.Empty, (isPackage) ? "in package " : string.Empty, pickerUpper);
                }
            }

            return pickup;
        }
        private string GetDownloadText(InvoiceItem item)
        {
            if (item.DeliveryCode == null)
                return string.Empty;

            //gifts only - not credit items
            bool isGift = (item.IsGiftCertificateDelivery && item.MainActName.ToLower().IndexOf("gift certificate") != -1);            

            string txt = string.Format("<div class=\"{0}\">{1}: <span>{2}</span></div>", 
                "download-panel", item.GetDeliveryCodeLabel(), item.DeliveryCode);
            
            //show additional info for gift certificates
            //do not show the modal box on the print page - or if the item has been refunded
            if (item.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower() 
                && isGift && this.Page.MasterPageFile.ToLower().IndexOf("templateprint") == -1)
            {
                //add a link and hidden fields for a dialog control
                txt += string.Format("<div><a href=\"/Store/Cart_GiftCertificate.aspx?sim={0}\" title=\"print or email your gift\" rel=\"{1}\" class=\"btntribe ov-trigger pore\" >",
                    item.Guid.ToString(), "#overlay-bundle");

                //any element can be used inside the trigger
                txt += string.Format("Print or Email Your Gift here</a><br/><br/>");


                txt += string.Format("<input type=\"hidden\" id=\"hidCode_{0}\" name=\"hidCode_{0}\" Value=\"{1}\" />",
                    item.Id.ToString(), item.DeliveryCode);
                txt += string.Format("<input type=\"hidden\" id=\"hidAmount_{0}\" name=\"hidAmount_{0}\" Value=\"{1}\" />",
                    item.Id.ToString(), item.LineItemTotal.ToString("c"));

                _hasGiftCertToRedeem = true;

                txt += "</div>";
            }

            return txt;

        }
        private void DisplayPricing(InvoiceItem item, RepeaterItemEventArgs e)
        {
            HtmlGenericControl pricing = (HtmlGenericControl)e.Item.FindControl("ItemPriceInfo");
            if (pricing != null)
            {
                if (item.Notes != null && item.Notes.ToLower().IndexOf("exchanged from:") != -1)
                    pricing.Visible = false;
                else
                {
                    if (item.IsMerchandiseItem && item.Price == 0)
                    {
                        pricing.Visible = false;
                        return;
                    }

                    pricing.InnerHtml = string.Format("<span class=\"itemtotal\">item total</span><span class=\"money\">{0}</span>",
                        item.LineItemTotal.ToString("c"));
                    pricing.InnerHtml += string.Format("<span class=\"label\">price each</span><span class=\"money\">{0}</span>",
                        item.PricePerItem.ToString("c"));

                    if (item.IsTicketItem)
                        pricing.InnerHtml += string.Format("<span class=\"labelsmall\"> ( {0} + {1} service fee ) </span>",
                            item.Price.ToString("c"), item.ServiceCharge.ToString("c"),
                            (item.Adjustment != 0) ? string.Format("{0} {1} adjustment ", (item.Adjustment > 0) ? "+" : "-", item.Adjustment.ToString("c")) : string.Empty);
                }
            }
        }
        private void DisplayItemBundles(InvoiceItem item, RepeaterItemEventArgs e)
        {
            InvoiceItemCollection coll = new InvoiceItemCollection();
            //get bundles
            coll.AddRange(item.AssociatedBundles
                .FindAll(delegate(InvoiceItem match) { return (match.PurchaseAction != _Enums.PurchaseActions.NotYetPurchased.ToString()); }));

            if (coll.Count > 0)
            {
                HtmlGenericControl bundleDiv = (HtmlGenericControl)e.Item.FindControl("ItemBundles");
                if (bundleDiv != null)
                {
                    //ignore sorting the bundles
                    //foreach bundle
                    foreach (InvoiceItem bundle in coll)
                    {
                        //don't add a separator
                        //bundleDiv.InnerHtml += "<div class=\"item-separator\">&nbsp;</div>";

                        //separte bundles
                        bundleDiv.InnerHtml += "<div class=\"bundle-purchase-container\">";

                        //show status
                        bool itemRefunded = (item.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString());

                        if (itemRefunded)
                            bundleDiv.InnerHtml += string.Format("<div class=\"refunditem\">This item has been refunded/exchanged and is no longer valid.</div>");
                        else if (item.Notes != null && item.Notes.ToLower().IndexOf("exchanged from:") != -1)
                            bundleDiv.InnerHtml += "<div class=\"refunditem\">Item Exchange</div>";

                        bundleDiv.InnerHtml += string.Format("<div class=\"bundle-title\">{0} @ {1}</div>",
                            bundle.Quantity.ToString(), bundle.MainActName);

                        string comment = bundle.MerchBundleRecord.Comment;
                        if (comment != null && comment.Trim().Length > 0)
                            bundleDiv.InnerHtml += string.Format("<div class=\"bundle-comment\">{0}</div>", comment);

                        //get/display selections
                        InvoiceItemCollection selections = new InvoiceItemCollection();
                        selections.AddRange(bundle.AssociatedBundleSelections);

                        if (selections.Count > 0)
                            bundleDiv.InnerHtml += "<div class=\"bundle-selection-container\">";

                        foreach (InvoiceItem selection in selections)
                        {
                            bool selectionRefunded = (selection.PurchaseAction != _Enums.PurchaseActions.Purchased.ToString());
                            if (selectionRefunded)
                                bundleDiv.InnerHtml += string.Format("<div class=\"refunditem\">This item has been refunded/exchanged and is no longer valid.</div>");
                            else if (item.Notes != null && item.Notes.ToLower().IndexOf("exchanged from:") != -1)
                                bundleDiv.InnerHtml += "<div class=\"refunditem\">Item Exchange</div>";

                            bundleDiv.InnerHtml += string.Format("<div class=\"bundle-selection\">{0}</div>",
                                selection.LineItemDescription_CriteriaAndDescription(false));

                            bundleDiv.InnerHtml += GetDownloadText(selection);
                        }
                        //endloop
                        if (selections.Count > 0)
                            bundleDiv.InnerHtml += "</div>";

                        //if price > 0 - show total                        
                        if (bundle.LineItemTotal > 0 && (bundle.Notes == null || bundle.Notes.ToLower().IndexOf("exchanged from:") == -1))
                        {
                            bundleDiv.InnerHtml += "<div class=\"pricepanel\">";
                            bundleDiv.InnerHtml += string.Format("<span class=\"itemtotal\">item total</span><span class=\"money\">{0}</span>",
                                bundle.LineItemTotal.ToString("c"));
                            bundleDiv.InnerHtml += string.Format("<span class=\"label\">price each</span><span class=\"money\">{0}</span>",
                                bundle.PricePerItem.ToString("c"));
                            bundleDiv.InnerHtml += "</div>";
                        }

                        //close off the bundle
                        bundleDiv.InnerHtml += "</div>";
                    }
                }
            }
        }

        private void DisplayInfo(InvoiceItem item, RepeaterItemEventArgs e)
        {
            if (item.IsTicketItem)
                DisplayAsTicketInfo(item, e);
            else if (item.IsMerchandiseItem && (!item.IsBundle) && (!item.IsBundleSelection))
                DisplayAsMerchInfo(item, e);
        }
        private void DisplayAsTicketInfo(InvoiceItem item, RepeaterItemEventArgs e)
        {
            if (item.IsTicketItem)
            {
                Literal litItemDetail = (Literal)e.Item.FindControl("litItemDetail");

                if (litItemDetail != null)
                {
                    ShowTicketCollection coll = new ShowTicketCollection();
                    coll.Add(item.ShowTicketRecord);

                    //pkg info
                    bool isPackage = item.ShowTicketRecord.IsPackage;
                    if (isPackage && (!item.ShowTicketRecord.IsCampingPass()))
                    {
                        coll.AddRange(item.ShowTicketRecord.LinkedShowTickets);
                        if (coll.Count > 1)
                            coll.Sort("DtDateOfShow", true);

                        litItemDetail.Text += string.Format("<div class=\"pkginfo\"> {0} SHOW PASS</div>", coll.Count.ToString());
                    }

                    ////ticket status
                    //if (item.ShowTicketRecord.Status != null && item.ShowTicketRecord.Status.Trim().Length > 0)
                    //    litItemDetail.Text += string.Format("<div class=\"ticketstatus\">{0}</div>", item.ShowTicketRecord.Status.Trim());

                    //description goes here
                    litItemDetail.Text += GetDescription(item);                   

                    //show names
                    Literal litShowNames = (Literal)e.Item.FindControl("litShowNames");
                    if (litShowNames != null)
                    {
                        if (item.ShowTicketRecord.IsCampingPass())
                        {
                            litShowNames.Text = string.Format("<div class=\"eventdate\"><div class=\"datelist\">{0} @ {1}</div></div>", 
                                item.Quantity.ToString(), item.MainActName);
                        }
                        else
                            litShowNames.Text = GetShowNameListing(item.Quantity, coll);
                    }


                    Literal litPickupPost = (Literal)e.Item.FindControl("litPickupPost");
                    if (litPickupPost != null)
                    {
                        //pickup
                        litPickupPost.Text += GetPickupName(item, isPackage);
                        //post purchase texts
                        litPickupPost.Text += GetPostPurchaseText(item);
                    }

                    DisplayPricing(item, e);

                    DisplayItemBundles(item, e);
                }
            }
        }
        private void DisplayAsMerchInfo(InvoiceItem item, RepeaterItemEventArgs e)
        {
            if (item.IsMerchandiseItem && (!item.IsBundle) && (!item.IsBundleSelection))
            {
                Literal litItemDetail = (Literal)e.Item.FindControl("litItemDetail");

                if (litItemDetail != null)
                {
                    litItemDetail.Text = GetDescription(item);
                    litItemDetail.Text += GetDownloadText(item);

                    //litItemDetail.Text += GetGiftCertificateDelivery(item);

                    litItemDetail.Text += GetPostPurchaseText(item);
                    DisplayPricing(item, e);
                    DisplayItemBundles(item, e);
                }
            }
        }

        #endregion

        #region GridRefunds

        protected void GridRefunds_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            InvoiceItemCollection coll = new InvoiceItemCollection();
            coll.AddRange(_invoice.InvoiceItemRecords().GetList().FindAll(
                delegate(InvoiceItem match)
                {
                    switch (match.Context)
                    {
                        case _Enums.InvoiceItemContext.discount:
                            return true;
                    }
                    return (match.PurchaseAction.ToLower() == _Enums.PurchaseActions.PurchasedThenRemoved.ToString().ToLower());
                }));

            if (coll.Count > 1)
                coll.Sort("Id", false);

            grid.DataSource = coll;
        }
        protected void GridRefunds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal description = (Literal)e.Row.FindControl("litDescription");
                InvoiceItem ii = (InvoiceItem)e.Row.DataItem;

                if (ii != null && description != null)
                    description.Text = ii.LineItemDescription_CriteriaAndDescription(false);
            }
        }

        #endregion
    }
}
//<!-- 110525 - 797 -->