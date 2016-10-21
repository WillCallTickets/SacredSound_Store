using System;
using System.Text;
using System.Xml.Serialization;
using System.Web.Profile;
using System.Net.Mail;
using System.Xml;
using System.Collections.Generic;

using Utils.ExtensionMethods;

namespace Wcss
{
    public partial class AuthorizeNet
    {
        /// <summary>
        /// Authnet API specifies max 20 chars for key
        /// </summary>
        /// <param name="userKey"></param>
        /// <returns></returns>
        public static string CreateInvoiceKey()
        {
            string baseKey = Guid.NewGuid().ToString();
            baseKey = baseKey.Replace("-",string.Empty).Replace(" ",string.Empty).Trim().ToLower();

            string timeKey = string.Format("{0:x}", long.Parse(DateTime.Now.Ticks.ToString()));

            string key = string.Format("{0}{1}", baseKey.Substring(0,8), timeKey.Substring(timeKey.Length-8));
            
            //ensure we haven't gone over length - auth net only accepts 20 chars
            if(key.Length > 20)
                key = key.Substring(0,19);//key.Length-20);

            return key;
        }

        #region Confirmation email
        
        private void AddHeaderRow(StringBuilder table, bool includeServiceTitle, string itemTitle)
        {
            table.Append("<tr>");
            table.Append("<th>Total</th>");
            table.Append("<th>Qty</th>");
            table.Append("<th>Each</th>");
            table.AppendFormat("<th>{0}</th>", (includeServiceTitle) ? "Svc" : "&nbsp;");
            table.AppendFormat("<th class=\"description\" style=\"width:66%;\">{0}</th>", itemTitle);
            table.Append("</tr>");
        }

        private void AddItemRow(StringBuilder table, InvoiceItem item, string description)
        {
            table.Append("<tr>");
            table.AppendFormat("<td class=\"total\">{0}</td>", (item.LineItemTotal > 0) ? item.LineItemTotal.ToString("c") : "&nbsp;");
            table.AppendFormat("<td class=\"qty\">{0}</td>", item.Quantity.ToString());
            table.AppendFormat("<td class=\"each\">{0}</td>", (item.Price > 0) ? item.Price.ToString("c") : "&nbsp;");
            table.AppendFormat("<td class=\"svc\">{0}</td>", (item.ServiceCharge > 0) ? item.ServiceCharge.ToString() : "&nbsp;");
            table.AppendFormat("<td>{0}</td>", description);
            table.Append("</tr>");
        }

        private void AddDetailRow(StringBuilder table, string description)
        {
            if(description != null && description.Trim().Length > 0)
                table.AppendFormat("<tr><td colspan=\"4\">&nbsp;</td><td>{0}</td></tr>", description);
        }

        private void AddTotalRow(StringBuilder table, string amount, string title)
        {
            table.AppendFormat("<tr><td class=\"total\">{0}</td><td>&nbsp;</td><th align=\"left\" colspan=\"3\">{1}</th></tr>", amount, title);
        }

        private void AddSeparatorRow(StringBuilder sb, int cols)
        {
            AddSeparatorRow(sb, cols, false);
        }

        private void AddSeparatorRow(StringBuilder sb, int cols, bool useHR)
        {
            sb.AppendFormat("<tr><td colspan=\"{0}\">{1}</td></tr>", cols.ToString(), (useHR) ? "<hr style=\"width: 100%;\" />" : "<br />");
        }

        private void AddDashedSeparatorRow(StringBuilder table, int cols)
        {
            table.AppendFormat("<tr><td colspan=\"{0}\">-------------------------------------------------------</td></tr>", cols.ToString());
        }

        private void AddItemBundles(StringBuilder table, InvoiceItem item, int cols)
        {
            //item cannot be a bundle item or a selected bundle item
            if ((!item.IsBundle) && (!item.IsBundleSelection))
            {
                //get bundles within this order tied to this item
                List<InvoiceItem> bundles = new List<InvoiceItem>();
                bundles.AddRange(item.AssociatedBundles
                    .FindAll(delegate (InvoiceItem match) { return match.Quantity > 0 && match.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString(); } ));

                foreach(InvoiceItem bundle in bundles)
                {
                    //get any selections
                    List<InvoiceItem> selections = new List<InvoiceItem>();
                    selections.AddRange(bundle.AssociatedBundleSelections
                        .FindAll(delegate(InvoiceItem match) { return match.Quantity > 0 && match.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString(); } ));

                    if (selections.Count > 0)
                    {
                        //display the name of the bundle - with pricing, etc
                        //include description if present
                        AddItemRow(table, bundle, bundle.MainActName);
                        if (bundle.Description != null && bundle.Description.Trim().Length > 0)
                            AddDetailRow(table, bundle.Description.Trim());


                        //display any selections - with their qtys
                        string desc = string.Empty;
                        foreach (InvoiceItem selected in selections)
                        {
                            desc += string.Format("<div>{0} @ {1}</div>", selected.Quantity.ToString(), selected.MainActName);

                            //skip criteria for bundles and selections and gift certs             
                    
                            //include download codes for downloads, activationCodes and giftCertificates
                            if (selected.IsDeliverableByCode)
                            {
                                string codeLabel = (selected.IsGiftCertificateDelivery) ? "Gift" : (selected.IsDownloadDelivery) ? "Download" : "Activation";
                                desc += string.Format("<div>{0} code: {1}</div>", codeLabel, selected.DeliveryCode);
                            }

                            //include description - this may contain redemption information
                            if (selected.Description != null && selected.Description.Trim().Length > 0)
                                desc += string.Format("<div>{0}</div>", selected.Description);

                            //Post Purchase
                            InvoiceItemPostPurchaseTextCollection ppColl = new InvoiceItemPostPurchaseTextCollection();
                            ppColl.AddRange(selected.InvoiceItemPostPurchaseTextRecords());
                            if (ppColl.Count > 0)
                            {
                                ppColl.Sort("IDisplayOrder", true);

                                string descpp = string.Empty;
                                foreach (InvoiceItemPostPurchaseText pp in ppColl)
                                    descpp += string.Format("{0}<br /><br />", Utils.ParseHelper.LinksToHref(pp.PostText));

                                desc += string.Format("<div>{0}</div>", descpp);
                            }
                            //END PP
                        }

                        AddDetailRow(table, desc);

                        AddSeparatorRow(table, cols);
                    }
                }
            }
        }

        public void SendConfirmationEmail(string userWebInfo, string emailTo)
        {
            SendConfirmationEmail(userWebInfo, emailTo, false);
        }

        public void SendConfirmationEmail(string userWebInfo, string emailTo, bool checkForRefunds)
        {
            Invoice invoice = this.InvoiceRecord;

            #region Construct Email message item

            System.Collections.Specialized.ListDictionary dict = new System.Collections.Specialized.ListDictionary();

            //
            dict.Add("<% InvoiceDate %>", invoice.InvoiceDate.ToString("ddd MM/dd/yyyy hh:mmtt"));
            dict.Add("<% InvoiceNumber %>", invoice.UniqueId);
            dict.Add("<% TransactionId %>", this.ProcessorId);

            //billingaddress & info
            string billing = string.Format("{0}{1}", this.Email.Replace("@", " @ "), Environment.NewLine);
            billing += string.Format("{0} {1}{2}", this.FirstName, this.LastName, Environment.NewLine);
            billing += string.Format("{0}{1}", this.BillingAddress, Environment.NewLine);//shipstreet1
            billing += string.Format("{0}, {1} {2} {3}{4}", this.City, this.State, this.Zip, this.Country, Environment.NewLine);//shipcity, state

            dict.Add("<% BillingInfo %>", billing);


            //shippingaddress
            string shipping = string.Empty;
            //if we have tickets to ship or merch to ship - then get details
            bool requiresShipping = invoice.ShippingItems.Count > 0;

            InvoiceBillShip ship = this.InvoiceRecord.InvoiceBillShip;
            if (ship != null && requiresShipping)
            {
                shipping = string.Format("</p><p><b>=== Shipping Info ===</b><br/>");

                if (!ship.SameAsBilling)
                {
                    if (ship.CompanyName.Trim().Length > 0)
                        shipping += string.Format("{0}{1}", ship.CompanyName.Trim(), Environment.NewLine);//company
                    shipping += string.Format("{0} {1}{2}", ship.FirstName, ship.LastName, Environment.NewLine);//shipname
                    shipping += string.Format("{0}{1}", ship.Address1, Environment.NewLine);//shipstreet1
                    if (ship.Address2.Trim().Length > 0)
                        shipping += string.Format("{0}{1}", ship.Address2.Trim(), Environment.NewLine);//shipstreet2
                    shipping += string.Format("{0}, {1} {2} {3}{4}{4}", ship.City, ship.StateProvince, ship.PostalCode, ship.Country, Environment.NewLine);//ship zip country
                }
                else
                    shipping += string.Format("Same as billing{0}", Environment.NewLine);
            }

            dict.Add("<% ShippingInfo %>", shipping);

            int maxCols = 5;

            System.Text.StringBuilder purchaseTable = new System.Text.StringBuilder();

            purchaseTable.AppendFormat("<table border=\"0\" cellspacing=\"5\" cellpadding=\"0\" style=\"width:95%;\">");

            InvoiceItemCollection itemColl = new InvoiceItemCollection();

            #region tickets

            
            itemColl.Clear();            
            itemColl.AddRange(invoice.TicketItems.GetList().FindAll(delegate(InvoiceItem match) { return (match.IsTicketItem && match.LineItemTotal > 0); }));

            itemColl.SortTicketItemsBy_DateToOrderBy();
            //if (itemColl.Count > 1)
            //    itemColl.Sort("DateOfShow_ToSortBy", true);

            itemColl.AddRange(invoice.PromotionItems.GetList().FindAll(delegate(InvoiceItem match) { return (match.TShowTicketId.HasValue); }));

            if (itemColl.Count > 0)
            {
                AddHeaderRow(purchaseTable, true, "Tickets");
                
                foreach (InvoiceItem item in itemColl)
                {
                    string dsc = string.Empty;

                    //add pkg info
                    bool isPackage = item.ShowTicketRecord.IsPackage;
                    if (isPackage && (!item.ShowTicketRecord.IsCampingPass()))
                    {
                        int cnt = item.ShowTicketRecord.LinkedShowTickets.Count + 1;

                        dsc += string.Format("<div> {0} SHOW PASS</div>", cnt.ToString());
                        dsc += string.Format("<div>{0} {1} - {2}</div>", item.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"), item.AgeDescription, item.MainActName);

                        foreach (ShowTicket stl in item.ShowTicketRecord.LinkedShowTickets)
                        {
                            InvoiceItem pkgItem = invoice.TicketItems.GetList().Find(delegate(InvoiceItem match) { return (match.TShowTicketId == stl.Id); });
                            if (pkgItem != null)
                            {
                                dsc += string.Format("<div>{0} {1} - {2}</div>",
                                    pkgItem.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"), pkgItem.AgeDescription, pkgItem.MainActName);
                            }
                        }
                    }
                    else
                    {
                        if (item.ShowTicketRecord.IsCampingPass())
                            dsc = string.Format("{0} {1}", item.AgeDescription, item.MainActName);
                        else
                            dsc = string.Format("{0} {1} - {2}", item.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"), item.AgeDescription, item.MainActName);
                    }

                    //AddItemRow(purchaseTable, item, 
                    //    string.Format("{0} {1} - {2}", item.DateOfShow.ToString("ddd MM/dd/yyyy hh:mmtt"), item.AgeDescription, item.MainActName));

                    AddItemRow(purchaseTable, item, dsc);


                    AddDetailRow(purchaseTable, item.Description);
                    AddDetailRow(purchaseTable, item.Criteria);                    

                    //Post Purchase
                    InvoiceItemPostPurchaseTextCollection ppColl = new InvoiceItemPostPurchaseTextCollection();
                    ppColl.AddRange(item.InvoiceItemPostPurchaseTextRecords());
                    if (ppColl.Count > 0)
                    {
                        ppColl.Sort("IDisplayOrder", true);

                        string desc = string.Empty;
                        foreach (InvoiceItemPostPurchaseText pp in ppColl)
                            desc += string.Format("{0}<br /><br />", Utils.ParseHelper.LinksToHref(pp.PostText));
                        AddDetailRow(purchaseTable, desc);
                    }
                    //END PP

                    //ship method
                    if ((!_Config._Allow_HideShipMethod) || (_Config._Allow_HideShipMethod && (!item.ShowTicketRecord.HideShipMethod)))
                    {
                        if (item.ShippingMethod == null || item.ShippingMethod.Trim().Length == 0 || item.ShippingMethod == ShipMethod.WillCall)
                        {
                            string pickupName = (item.PickupName != null && item.PickupName.Trim().Length > 0) ? item.PickupName : item.PurchaseName;
                            AddDetailRow(purchaseTable, string.Format("Ticket(s) at venue's WillCall for: {0}", pickupName));
                        }
                        else
                            AddDetailRow(purchaseTable, string.Format("Ticket(s) will be shipped via: {0}", item.ShippingMethod));
                    }

                    AddItemBundles(purchaseTable, item, maxCols);
                }

                AddSeparatorRow(purchaseTable, maxCols);
            }

            #endregion

            #region merch

            itemColl.Clear();
            itemColl.AddRange(invoice.MerchItems.GetList().FindAll(delegate(InvoiceItem match) { return (!match.IsBundleSelection); }));
            itemColl.AddRange(invoice.PromotionItems.GetList().FindAll(delegate(InvoiceItem match) { return (match.TMerchId.HasValue); }));

            if (itemColl.Count > 0)
            {
                AddHeaderRow(purchaseTable, false, "Merchandise");

                foreach (InvoiceItem item in itemColl)
                {
                    AddItemRow(purchaseTable, item, item.MainActName);
                    
                    //skip criteria for bundles and selections and gift certs, downloads and unique code
                    if ((!item.IsBundle) && (!item.IsBundleSelection) && (!item.IsDeliverableByCode))
                        AddDetailRow(purchaseTable, item.Criteria);

                    //include download codes for downloads, uniqueCodes and giftCertificates
                    else if (item.IsDeliverableByCode)
                    {
                        string codeLabel = (item.IsGiftCertificateDelivery) ? "Gift" : (item.IsDownloadDelivery) ? "Download" : "Activation";                        
                        AddDetailRow(purchaseTable, string.Format("{0} code: {1}", codeLabel, item.DeliveryCode));
                    }
                    
                    AddDetailRow(purchaseTable, item.Description); 

                    //Post Purchase
                    InvoiceItemPostPurchaseTextCollection ppColl = new InvoiceItemPostPurchaseTextCollection();
                    ppColl.AddRange(item.InvoiceItemPostPurchaseTextRecords());
                    if (ppColl.Count > 0)
                    {
                        ppColl.Sort("IDisplayOrder", true);

                        string desc = string.Empty;
                        foreach (InvoiceItemPostPurchaseText pp in ppColl)
                            desc += string.Format("{0}<br /><br />", Utils.ParseHelper.LinksToHref(pp.PostText, true));

                        AddDetailRow(purchaseTable, desc);
                    }
                    //END PP

                    AddItemBundles(purchaseTable, item, maxCols);
                }
                
                AddSeparatorRow(purchaseTable, maxCols);
            }

            #endregion

            #region merch shipments

            //////////////////////////
            //MERCH SHIPMENTS
            //////////////////////////
            //dont show unless we have multiple shipments - single shipments will show detail in totals
            //create its own collection to be used later
            InvoiceItemCollection merchShipments = new InvoiceItemCollection();
            merchShipments.AddRange(invoice.MerchandiseShipmentItems);

            if (merchShipments.Count > 1)
            {
                AddSeparatorRow(purchaseTable, maxCols);
                AddHeaderRow(purchaseTable, false, "Merchandise Shipment Detail");

                foreach (InvoiceItem item in merchShipments)
                {
                    AddItemRow(purchaseTable, item,
                        string.Format("{0}{1}", item.MainActName, 
                            (_Config._DisplayEstimatedShipDates && invoice.MerchandiseShipmentItems[0].DateOfShow < DateTime.MaxValue) ?
                                string.Format(" Item(s) in this shipment will ship on or about {0}", invoice.MerchandiseShipmentItems[0].DateOfShow.ToString("MM/dd/yyyy")) : string.Empty)
                        );
                    
                    //TODO not sure if this is the correct collection
                    InvoiceItemCollection shipmentItems = new InvoiceItemCollection();
                    shipmentItems.AddRange(item.ChildInvoiceItemRecords().GetList()
                        .FindAll(delegate(InvoiceItem match) { return (match.PurchaseAction == _Enums.PurchaseActions.Purchased.ToString()); } ));

                    if (shipmentItems.Count > 0)
                    {
                        string desc = string.Empty;
                        foreach (InvoiceItem shipItem in shipmentItems)
                            desc += string.Format("<div class=\"subitem\">{0}</div>", shipItem.MainActName);

                        AddDetailRow(purchaseTable, desc);
                    }

                }

                AddSeparatorRow(purchaseTable, maxCols);
            }

            #endregion

            #region donations

            //DONATIONS
            if (invoice.HasCharitableItems)
            {
                AddSeparatorRow(purchaseTable, maxCols, true);
                AddHeaderRow(purchaseTable, false, "Donations");

                foreach (InvoiceItem item in invoice.CharitableItems)
                    AddItemRow(purchaseTable, item, item.MainActName);

                AddSeparatorRow(purchaseTable, maxCols, true);
            }

            #endregion

            #region discounts

            if (invoice.PromotionalDiscountTotal > 0)
            {
                AddSeparatorRow(purchaseTable, maxCols, true);
                AddHeaderRow(purchaseTable, false, "Discounts");

                foreach (InvoiceItem item in invoice.PromotionItems)
                {
                    if (item.LineItemTotal < 0)//only record actual discounts
                    {
                        AddItemRow(purchaseTable, item, item.MainActName);
                        
                        AddDetailRow(purchaseTable, item.Criteria);
                        AddDetailRow(purchaseTable, item.Description);
                    }
                }

                AddSeparatorRow(purchaseTable, maxCols, true);
            }

            #endregion

            AddSeparatorRow(purchaseTable, maxCols);

            #region REFUNDS??
            //to keep it simple - just list the items that were refunded
            //if specified (not specified in normal order flow) then we look for any refunds and list them
            if (checkForRefunds)
            {
                itemColl.Clear();
                itemColl.AddRange(invoice.InvoiceItemRecords().GetList().FindAll(delegate(InvoiceItem match) { return (match.HasBeenRemoved); }));

                if (itemColl.Count > 0)
                {
                    purchaseTable.AppendFormat("<tr><td colspan=\"{0}\">REFUNDED/CREDITED-----</td></tr>", maxCols.ToString());

                    foreach (InvoiceItem item in itemColl)
                    {
                        AddItemRow(purchaseTable, item, item.MainActName);

                        if ((!item.IsCharitableItem) && (!item.IsProcessingFee) && (!item.IsBundle) && (!item.IsBundleSelection))
                            AddDetailRow(purchaseTable, item.Criteria);

                        AddDetailRow(purchaseTable, item.Description);
                    }
                    
                    AddDashedSeparatorRow(purchaseTable, maxCols);
                    AddSeparatorRow(purchaseTable, maxCols);
                }
            }
            #endregion


            //Totals here
            AddDashedSeparatorRow(purchaseTable, maxCols);

            AddTotalRow(purchaseTable, invoice.Total_TicketsAndMerch.ToString(), "Item Subtotal");

            itemColl.Clear();
            itemColl.AddRange(invoice.TicketShipmentItems);

            if (invoice.TicketShipping > 0)
            {
                string desc = "Ticket Shipping";

                if (invoice.TicketShipmentItems.Count == 1)
                    desc += string.Format(" - {0}", invoice.TicketShipmentItems[0].MainActName);
                
                foreach (InvoiceItem itm in itemColl)
                {
                    if (itm.MainActName != null && itm.MainActName.ToLower().IndexOf("ups standard") != -1)
                    {
                        desc += " ***Purchaser will be responsible for any fees incurred transporting order from customs to destination";
                        break;//only list this notice once!
                    }
                }

                AddTotalRow(purchaseTable, invoice.TicketShipping.ToString(), desc);
            }


            if (invoice.MerchandiseShipping > 0)
            {
                string desc = "Merchandise Shipping";

                if (invoice.MerchandiseShipmentItems.Count == 1)
                {
                    desc += string.Format(" - {0}", invoice.MerchandiseShipmentItems[0].MainActName);

                    if (_Config._DisplayEstimatedShipDates && invoice.MerchandiseShipmentItems[0].DateOfShow < DateTime.MaxValue)
                        desc += string.Format(" Item(s) in this shipment will ship on or about {0}", invoice.MerchandiseShipmentItems[0].DateOfShow.ToString("MM/dd/yyyy"));
                }
                foreach (InvoiceItem itm in merchShipments)
                {
                    if (itm.MainActName != null && itm.MainActName.ToLower().IndexOf("ups standard") != -1)
                    {
                        desc += " ***Purchaser will be responsible for any fees incurred transporting order from customs to destination";
                        break;//only list this notice once!
                    }
                }

                AddTotalRow(purchaseTable, invoice.MerchandiseShipping.ToString(), desc);
            }

            AddTotalRow(purchaseTable, invoice.ProcessingCharge.ToString(), "Processing");

            if (invoice.PromotionalDiscountTotal > 0)
                AddTotalRow(purchaseTable, invoice.PromotionalDiscountTotal.ToString(), "Discounts");

            AddDashedSeparatorRow(purchaseTable, maxCols);

            if (invoice.StoreCreditTotal > 0)
            {
                //add subtotal
                AddTotalRow(purchaseTable, invoice.NetPaid.ToString("c"), "Subtotal");
                //add credit line
                AddTotalRow(purchaseTable, invoice.StoreCreditTotal.ToString("c"), "Credit Applied");
                //add amount due
                AddTotalRow(purchaseTable, ((decimal)invoice.NetPaid - invoice.StoreCreditTotal).ToString("c"), "Paid");
            }
            else
                AddTotalRow(purchaseTable, invoice.NetPaid.ToString("c"), "Total");

            AddSeparatorRow(purchaseTable, maxCols);


            if ( _Config._Merchant_ChargeStatement_Descriptor.HasValueLength() && this.Amount > 0)
                purchaseTable.AppendFormat("<tr><td colspan=\"{0}\"><br /><p>*Note: You will see a charge from {1} on your statement.</p></td></tr>",
                    maxCols.ToString(), Wcss._Config._Merchant_ChargeStatement_Descriptor);

            purchaseTable.Append("</table>");


            dict.Add("<% PurchaseTable %>", purchaseTable.ToString());

            //userwebinfo
            dict.Add("<% UserWebInfo %>", userWebInfo);

            //cusomterserviceemail 
            dict.Add("<% CustomerServiceEmail %>", _Config._CustomerService_Email);

            //ticket shipping terms
            System.Text.StringBuilder terms = new StringBuilder();

            if(invoice.HasTicketShipmentItemsOtherThanWillCall)
            {
                terms.Append("<p><b>=================================</b><br/><br/>");
                terms.Append(_Config._ShippingTerms_Tickets);
                terms.Append("</p>");
            }

            if(invoice.HasGiftDeliverableItems)
            {
                if(terms.Length > 0)
                    terms.Append("<br /><br/>");
                terms.Append("<p><b>=================================</b><br/><br/>");
                terms.Append(_Config._GiftTerms);
                terms.AppendLine();

                if(invoice.GiftDeliverableItems.GetList()
                    .FindIndex(delegate(InvoiceItem match) { return (match.MainActName.ToLower().IndexOf("gift certificate") != -1); }) != -1)
                {
                    terms.Append(_Config._GiftDelivery);
                    terms.AppendLine();
                }

                if (invoice.GiftDeliverableItems.GetList()
                    .FindIndex(delegate(InvoiceItem match) { return (match.MainActName.ToLower().IndexOf("store credit") != -1); }) != -1)
                {
                    terms.Append(_Config._GiftRedemptionInstructions);
                    terms.Append("</p>");
                }
            }

            if (invoice.HasDownloadDeliveryItems)
            {
                if (terms.Length > 0)
                    terms.Append("<br /><br/>");
                terms.Append("<p><b>=================================</b><br/><br/>");
                terms.Append(_Config._DownloadInstructions_1320);
                terms.AppendLine();
            }

            dict.Add("<% TicketShippingTerms %>", terms.ToString());

            #endregion

            MailMessage mail = new MailMessage();

            try
            {
                mail.From = new MailAddress(_Config._Confirmation_Email, _Config._Confirmation_FromName);

                MailAddress emailAddress = new MailAddress((_Config._RefundTestMode) ? _Config._Admin_EmailAddress : emailTo, 
                    string.Format("{0} {1}", this.FirstName, this.LastName));

                mail.To.Add(emailAddress);

                if (_Config._Confirmation_CCSales.Trim().Length > 0)
                    mail.CC.Add(new MailAddress(_Config._Confirmation_CCSales.Trim()));

                mail.Subject = "Your Purchase Confirmation";
                mail.IsBodyHtml = false;

                //always send html email - at this point
                string template = string.Format("/{0}/MailTemplates/SiteTemplates/PurchaseConfirmationEmail.html", _Config._VirtualResourceDir);
                string mappedFile = (System.Web.HttpContext.Current != null) ?
                    System.Web.HttpContext.Current.Server.MapPath(template) :
                    string.Format("{0}\\WcWeb{1}", _Config._MappedRootDirectory, template);

                string htmlTemplate = Utils.FileLoader.FileToString(mappedFile);
                htmlTemplate = Utils.ParseHelper.DoReplacements(htmlTemplate, dict, true);

                mail.Body = htmlTemplate;
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Send(mail);
                
            }
            catch (Exception ex)
            {
                _Error.LogException(ex, true);
            }
        }

        #endregion

        #region Wrapped Properties

        [XmlAttribute("IsAuthorized")]
        public bool IsAuthorized
        {
            get
            {
                if (!this.BAuthorized.HasValue) return false;
                return this.BAuthorized.Value;
            }
            set { this.BAuthorized = value; }
        }
        [XmlAttribute("IsMD5Match")]
        public bool IsMD5Match
        {
            get
            {
                if (!this.BMd5Match.HasValue) return false;
                return this.BMd5Match.Value;
            }
            set { this.BMd5Match = value; }
        }
        [XmlAttribute("Amount")]
        public decimal Amount
        {
            get
            {
                if (!this.MAmount.HasValue) this.MAmount = 0;
                return decimal.Round(this.MAmount.Value, 2);
            }
            set
            {
                if (value == decimal.MinValue) this.MAmount = null;
                else this.MAmount = decimal.Round(value, 2);
            }
        }
        [XmlAttribute("TaxAmount")]
        public decimal TaxAmount
        {
            get
            {
                if (!this.MTaxAmount.HasValue) this.MTaxAmount = 0;
                return decimal.Round(this.MTaxAmount.Value, 2);
            }
            set
            {
                if (value == decimal.MinValue) this.MTaxAmount = null;
                else this.MTaxAmount = decimal.Round(value, 2);
            }
        }
        [XmlAttribute("FreightAmount")]
        public decimal FreightAmount
        {
            get
            {
                if (!this.MFreightAmount.HasValue) this.MFreightAmount = 0;
                return decimal.Round(this.MFreightAmount.Value, 2);
            }
            set
            {
                if (value == decimal.MinValue) this.MFreightAmount = null;
                else this.MFreightAmount = decimal.Round(value, 2);
            }
        }

        public int ResponseCode
        {
            get
            {
                if (!this.IResponseCode.HasValue) return 0;
                return this.IResponseCode.Value;
            }
            set
            {
                if (value == decimal.MinValue) this.IResponseCode = null;
                else this.IResponseCode = value;
            }
        }

        public int ResponseReasonCode
        {
            get
            {
                if (!this.IResponseReasonCode.HasValue) return 0;
                return this.IResponseReasonCode.Value;
            }
            set
            {
                if (value == decimal.MinValue) this.IResponseReasonCode = null;
                else this.IResponseReasonCode = value;
            }
        }

        public int DupeSeconds
        {
            get
            {
                if (!this.IDupeSeconds.HasValue) return 0;
                return this.IDupeSeconds.Value;
            }
            set
            {
                if (value == decimal.MinValue) this.IDupeSeconds = null;
                else this.IDupeSeconds = value;
            }
        }
        #endregion

        #region Send Transaction

        internal StringBuilder PostData = new StringBuilder();
        //private string lockObj = "";
        internal string _isDelimData = "TRUE";
        internal string _delimChar = ",";
        internal string _method = "CC";
        internal string _transactionType = "AUTH_CAPTURE";
        internal string _cardNum;
        internal string _expMonth;
        internal string _expYear;
        internal string _securityCode;

        public AuthorizeNetResponse response = null;

        private static string AddUrlParam(string name, object val)
        {
            return string.Format("{0}={1}&", name, System.Web.HttpUtility.UrlEncode(val.ToString().Replace(',', ' ')));
        }

        internal void VoidPaymentData(Purchaser purchaser, Invoice invoice, decimal amountToReturn, string transactionId)
        {
            _transactionType = "VOID";

            if (invoice.CashewRecord == null)
                throw new Exception("There is no credit card associated with this invoice.");

            SetRefundData(purchaser, invoice, invoice.CashewRecord.Name, invoice.CashewRecord.LastFour, string.Empty, string.Empty,
                amountToReturn.ToString(), transactionId);
        }

        internal void RefundPaymentData(Purchaser purchaser, Invoice invoice, decimal amountToReturn, string transactionId)
        {
            _transactionType = "CREDIT";

            if (invoice.CashewRecord == null)
                throw new Exception("There is no credit card associated with this invoice.");

            if (invoice.CashewRecord.LastFour == "-1" || invoice.CashewRecord.LastFour == "0000")
                throw new Exception("The credit card associated with this invoice has expired or is past the time-limit of allowed to refunds.");

            SetRefundData(purchaser, invoice, invoice.CashewRecord.Name, invoice.CashewRecord.LastFour, invoice.CashewRecord.Month, invoice.CashewRecord.Year,
                amountToReturn.ToString(), transactionId);
        }

        private void SetRefundData(Purchaser purchaser, Invoice invoice, string nameOnCard, string cardNumber, string expMonth, string expYear, 
            string amountToRefund, string previousTransactionId)
        {
            //reset postData
            PostData.Length = 0;

            PostData.Append(AddUrlParam("x_First_Name", purchaser.FirstName));
            PostData.Append(AddUrlParam("x_Last_Name", purchaser.LastName));
            //set up overhead for trans
            PostData.Append(AddUrlParam("x_Login", _Config._AuthorizeNetLogin));
            PostData.Append(AddUrlParam("x_Tran_Key", _Config._AuthorizeNetTxKey));
            PostData.Append(AddUrlParam("x_Test_Request", _Config._AuthorizeNetTestMode));
            PostData.Append(AddUrlParam("x_Delim_Data", _isDelimData));
            PostData.Append(AddUrlParam("x_Delim_Char", _delimChar));
            PostData.Append(AddUrlParam("x_Duplicate_Window", _Config._AuthorizeNetDuplicateSeconds));
            PostData.Append(AddUrlParam( "x_Version","3.1"));//TODO add to config - current authnetversion
            PostData.Append(AddUrlParam("x_Customer_IP", this.IpAddress));
            PostData.Append(AddUrlParam("x_Trans_Id", previousTransactionId));
            PostData.Append(AddUrlParam("x_Description", (Description.Length > 255) ?
                string.Format("{0}...", Description.Remove(251, Description.Length - 251)) : Description));
            PostData.Append(AddUrlParam("x_Cust_Id", invoice.CustomerId));
            PostData.Append(AddUrlParam("x_Email", this.Email));
            PostData.Append(AddUrlParam("x_Email_Customer", "FALSE"));//Todo add to config - emailrefundfrom payment processor
            PostData.Append(AddUrlParam("x_Invoice_Num", invoice.UniqueId));

            PostData.Append(AddUrlParam("x_Amount", amountToRefund));
            PostData.Append(AddUrlParam("x_Currency_Code", "USD"));
            PostData.Append(AddUrlParam("x_Method", _method));
            PostData.Append(AddUrlParam("x_Type", _transactionType));//test

            PostData.Append(AddUrlParam("x_Card_Num", cardNumber));
            PostData.Append(AddUrlParam("x_Exp_Date", string.Format("{0}{1}", expMonth, expYear)));
        }

        public void SendPayment(string itemDescription)
        {
            Utils.HttpTrans t = new Utils.HttpTrans();

            this.LogTransactionInfoBeforeSending(PostData.ToString());

            //TODO: make this a diff val....
            if (_Config._ShuntAuthNet)
            {
                //this.response = new AuthorizeNetResponse(this.InvoiceRecord, false, this.IpAddress);
                this.response = new AuthorizeNetResponse(CreateAuthResponse(_Config._AuthNetTestResult, true, this.InvoiceRecord, false), 
                    this.IpAddress);
            }
            else
            {
                //if we have a zero amount for the transaction due to store credit being used - than no need to 
                //call into the auth net service
                if(this.Amount > 0)
                    this.response = new AuthorizeNetResponse(t.Post(_Config._AuthorizeNetPaymentUrl, PostData.ToString()),
                        itemDescription, false, this.IpAddress);
                else
                {
                    this.response = new AuthorizeNetResponse(CreateAuthResponse(true, false, this.InvoiceRecord, false), 
                        itemDescription, false, this.IpAddress);
                }
            }

            this.IsAuthorized = this.response.IsAuthorized;

            //logs to the db
            this.LogResponse(itemDescription);
        }


        public string CreateAuthResponse(bool isAuthd, bool isTest, Invoice invoice, bool isRefund)
        {
            //response code,subcode,reason code, reason text,approval code, avs code, return md5
            System.Text.StringBuilder response = new StringBuilder();
            response.AppendFormat("{0},{1},{2},{3},{4},{5},", (isAuthd) ? "1" : "0", "1", "1", "1", "1", "yy");

            //ProcessorId,InvoiceNumber,Description,Amount,Method,TransactionType
            response.AppendFormat("{0},{1},{2},{3},{4},{5},", (isTest) ? DateTime.Now.Ticks.ToString().Substring(0, 9) : "0",
                invoice.UniqueId, (isTest) ? "test trans" : string.Empty, this.Amount.ToString(), "CC", (isRefund) ? "credit" : "auth_capture");

            //CustomerID,CardholderFirstName,CardholderLastName,Company,BillingAddress,City,State,Zip,Country,Phone,22?,Email
            response.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},,{10},",
                invoice.CustomerId, this._purchaser.FirstName, this._purchaser.LastName, string.Empty,
                this._purchaser.Address1, this._purchaser.City, this._purchaser.State,
                this._purchaser.PostalCode, this._purchaser.Country, this._purchaser.Phone,
                this._purchaser.UserName);

            //ShipToFirstName,ShipToLastName,ShipToCompany,ShipToAddress,ShipToCity,ShipToState,ShipToZip,ShipToCountry
            InvoiceBillShip bs = invoice.InvoiceBillShip;

            if (bs != null && !bs.SameAsBilling)
            {
                string shippingAddie = string.Format("{0} {1}", bs.Address1, bs.Address2).Trim();
                shippingAddie = (shippingAddie.Length > 60) ? shippingAddie.Substring(0, 50) : shippingAddie;

                response.AppendFormat("{0},{1},{2},{3},{4},{5},{6},{7},", bs.FirstName, bs.LastName,
                    bs.CompanyName, shippingAddie, bs.City, bs.StateProvince, bs.PostalCode, bs.Country);
            }
            else
                response.AppendFormat(",,,,,,,,");

            //tax amount,33?,freight amount,35,36,md5,cardcode response
            response.AppendFormat("{0},,{1},,,1,", "0", this.FreightAmount);//i.ShippingAndHandling);

            return response.ToString();
        }
        
        public class Purchaser
        {
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CompanyName { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string Phone { get; set; }

            public Purchaser(string userName, string firstName, string lastName, string companyName,
                string address1, string address2, string city, string state, string postalCode, string country, 
                string phone)
            {
                this.UserName = userName;
                this.FirstName = firstName;
                this.LastName = lastName;
                this.CompanyName = companyName;
                this.Address1 = address1;
                this.Address2 = address2;
                this.City = city;
                this.State = state;
                this.PostalCode = postalCode;
                this.Country = country;
                this.Phone = phone;
            }

            public Purchaser(ProfileBase profile)
            {
                System.Web.Profile.ProfileGroupBase contact = profile.GetProfileGroup("ContactInfo");

                this.UserName = profile.UserName;
                this.FirstName = profile.GetPropertyValue("FirstName").ToString();
                this.LastName = profile.GetPropertyValue("LastName").ToString();
                this.CompanyName = contact.GetPropertyValue("CompanyName").ToString();
                this.Address1 = contact.GetPropertyValue("Address1").ToString();
                this.Address2 = contact.GetPropertyValue("Address2").ToString();
                this.City = contact.GetPropertyValue("City").ToString();
                this.State = contact.GetPropertyValue("State").ToString();
                this.PostalCode = contact.GetPropertyValue("PostalCode").ToString();
                this.Country = contact.GetPropertyValue("Country").ToString();
                this.Phone = contact.GetPropertyValue("Phone").ToString();
            }
        }

        //ProfileBase _profile;
        Purchaser _purchaser = null;
        public void SetAuthData(Invoice invoice, System.Web.Profile.ProfileBase profile, 
            string firstName, string lastName, string address1, string address2, string city, string state, string postalCode, string country, string phone,
            string cardName, string cardNumber, string expMonth, string expYear,
            string securityCode, decimal amount, string ipAddress)
        {
            SetAuthData(invoice, new Purchaser(profile.UserName, firstName, lastName, string.Empty, address1, address2, city, state, postalCode, country, phone),
                cardName, cardNumber, expMonth, expYear, securityCode,
                amount, ipAddress, string.Empty);
        }
        public void SetAuthData(Invoice invoice, System.Web.Profile.ProfileBase profile, string cardName, string cardNumber, string expMonth, string expYear,
            string securityCode, decimal amount, string ipAddress)
        {
            SetAuthData(invoice, new Purchaser(profile), 
                cardName, cardNumber, expMonth, expYear, securityCode, 
                amount, ipAddress, string.Empty);
        }
        private void SetAuthData(Invoice invoice, Purchaser purchaser,
            string cardName, string cardNumber, string expMonth, string expYear, string securityCode, 
            decimal amount, string ipAddress, string previousTransactionId)
        {
            _purchaser = purchaser;
            this.InvoiceRecord = invoice;

            this.NameOnCard = cardName;
            _cardNum = cardNumber;
            _expMonth = expMonth;
            _expYear = expYear;
            this.IpAddress = ipAddress;
            _securityCode = securityCode;

            this.Amount = amount;

            //todo: add company info - shipping
            PostData.Length = 0;
            PostData.Append(AddUrlParam("x_Login", _Config._AuthorizeNetLogin));
            
            //PostData.Append(AddUrlParam("x_Password", _Config.AuthorizeNetPassword));
            PostData.Append(AddUrlParam("x_Tran_Key", _Config._AuthorizeNetTxKey));
            PostData.Append(AddUrlParam("x_Test_Request", _Config._AuthorizeNetTestMode));

            PostData.Append(AddUrlParam("x_Delim_Data", _isDelimData));
            PostData.Append(AddUrlParam("x_Delim_Char", _delimChar));
            PostData.Append(AddUrlParam("x_Duplicate_Window", _Config._AuthorizeNetDuplicateSeconds));

            PostData.Append(AddUrlParam( "x_Company", purchaser.CompanyName));
            PostData.Append(AddUrlParam("x_First_Name", purchaser.FirstName));
            PostData.Append(AddUrlParam("x_Last_Name", purchaser.LastName));

            string billingAddie = string.Format("{0} {1}", purchaser.Address1, purchaser.Address2).Trim();
            billingAddie = (billingAddie.Length > 60) ? billingAddie.Substring(0, 50) : billingAddie;
            PostData.Append(AddUrlParam("x_Address", billingAddie));

            PostData.Append(AddUrlParam("x_City", purchaser.City));
            PostData.Append(AddUrlParam("x_State", purchaser.State));
            PostData.Append(AddUrlParam("x_Zip", purchaser.PostalCode));
            PostData.Append(AddUrlParam("x_Country", purchaser.Country));
            PostData.Append(AddUrlParam("x_Phone", purchaser.Phone));


            InvoiceBillShip bs = this.InvoiceRecord.InvoiceBillShip;

            if (bs != null && !bs.SameAsBilling)
            {
                PostData.Append(AddUrlParam("x_Ship_To_First_Name", bs.FirstName));
                PostData.Append(AddUrlParam("x_Ship_To_Last_Name", bs.LastName));
                PostData.Append(AddUrlParam("x_Ship_To_Company", bs.CompanyName));

                string shippingAddie = string.Format("{0} {1}", bs.Address1, bs.Address2).Trim();
                shippingAddie = (shippingAddie.Length > 60) ? shippingAddie.Substring(0, 50) : shippingAddie;
                PostData.Append(AddUrlParam("x_Ship_To_Address", shippingAddie));

                PostData.Append(AddUrlParam("x_Ship_To_City", bs.City));
                PostData.Append(AddUrlParam("x_Ship_To_State", bs.StateProvince));
                PostData.Append(AddUrlParam("x_Ship_To_Zip", bs.PostalCode));
                PostData.Append(AddUrlParam("x_Ship_To_Country", bs.Country));
            }

            PostData.Append(AddUrlParam("x_Freight", this.FreightAmount));

            PostData.Append(AddUrlParam("x_Customer_IP", this.IpAddress));

            PostData.Append(AddUrlParam("x_Email", purchaser.UserName));
            PostData.Append(AddUrlParam("x_Email_Customer", "FALSE"));//whether a confirmation email should be sent to user

            PostData.Append(AddUrlParam("x_Invoice_Num", this.InvoiceRecord.UniqueId));
            
            
            //prepare description - we are allowed 255 chars
            string desc = (Description.Length > 255) ? string.Format("{0}...", Description.Remove(251, Description.Length - 251)) : Description;
            desc = desc.Replace("'", "&#39;");
            PostData.Append(AddUrlParam("x_Description", desc));


            PostData.Append(AddUrlParam("x_Amount", this.Amount));
            PostData.Append(AddUrlParam("x_Currency_Code", "USD"));//
            PostData.Append(AddUrlParam("x_Method", _method));
            PostData.Append(AddUrlParam("x_Type", _transactionType));

            PostData.Append(AddUrlParam("x_Card_Num", _cardNum));
            PostData.Append(AddUrlParam("x_Exp_Date", string.Format("{0}{1}", _expMonth, _expYear)));
            PostData.Append(AddUrlParam("x_Card_Code", _securityCode));
            PostData.Append(AddUrlParam("x_Trans_Id", previousTransactionId));//used for credits and voids and prior_auth_capture

            PostData.Append(AddUrlParam( "x_Tax", 0));//TODO: get tax of invoice
            PostData.Append(AddUrlParam( "x_Cust_Id", this.InvoiceRecord.CustomerId));//not used here - will only work if existing customer as data has not yet been commited

            #region Unused Parameters
            //PostData.Append(AddUrlParam( "x_Version", "3.1")); //not needed - set in merchant account interface upon upgrade
            //PostData.Append(AddUrlParam( "x_Encap_Char",	);
            //PostData.Append(AddUrlParam( "x_Fax",				);//not used
            //
            //PostData.Append(AddUrlParam( "x_Customer_Tax_Id", );//not used
            //PostData.Append(AddUrlParam( "x_Merchant_Email",	"");//an email address to send a copy of merchant confrim email
            //PostData.Append(AddUrlParam( "x_Recurring_Billing", "NO");//YES, NO
            //PostData.Append(AddUrlParam( "x_Bank_Aba_Code",	routing number);
            //PostData.Append(AddUrlParam( "x_Bank_Acct_Num",	account number);
            //PostData.Append(AddUrlParam( "x_Bank_Acct_Type",	);//CHECKING, BUSINESSCHECKING, SAVINGS
            //PostData.Append(AddUrlParam( "x_Bank_Name",		);
            //PostData.Append(AddUrlParam( "x_Bank_Acct_Name",	);//customer's acct name
            //PostData.Append(AddUrlParam( "x_Bank_Echeck_Type", );//CCD, PPD, TEL, WEB
            //PostData.Append(AddUrlParam( "x_Auth_Code",		);//for capture_only
            //PostData.Append(AddUrlParam( "x_Authentication_Indicator", );//for cardholder auth programs
            //PostData.Append(AddUrlParam( "x_Cardholder_Authentication_Value", );//for cardholder auth programs
            //PostData.Append(AddUrlParam( "x_Po_Num",			);
            //
            //PostData.Append(AddUrlParam( "x_Tax_Exempt",		);
            //PostData.Append(AddUrlParam( "x_Duty",			);	
            #endregion

        }

        #endregion

        #region Logging
        public void LogTransactionInfoBeforeSending(string data)
        {
            LogTransactionInfoBeforeSending(data, string.Empty, false);
        }
        public void LogTransactionInfoBeforeSending(string data, string itemDescription, bool IsRefund)
        {
            System.IO.FileStream fs = null;
            System.IO.StreamWriter sw = null;

            try
            {
                //lock (lockObj)
                //{
                    string fileName = string.Format("AuthorizeNetLog_{0}{1}.log", DateTime.Now.ToString("MM_dd_yyyy"), (IsRefund) ? "_Refunds" : string.Empty);
                    string fullPath = string.Format("{0}\\{1}", _Config._AuthorizeNetLogPath, fileName);

                    string mappedPath = string.Empty;
                    if (System.Web.HttpContext.Current != null)
                        mappedPath = System.Web.HttpContext.Current.Server.MapPath(fullPath);
                    else
                        mappedPath = string.Format(@"{0}{1}", _Config._MappedRootDirectory, fullPath.Replace("/", @"\")).Replace(@"\\", @"\");

                    fs = new System.IO.FileStream(mappedPath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                    sw = new System.IO.StreamWriter(fs);

                    string postData = string.Empty;
                    string lastFour = string.Empty;
                    if (data != null && data.Trim().Length > 0)
                    {
                        postData = data;

                        lastFour = data.Substring(data.IndexOf("&x_Exp_Date=") - 4, 4);
                    }
                    else
                    {
                        postData = "data is empty";
                    }

                    string custInfo = string.Empty;
                    if (!IsRefund)
                    {
                        custInfo = (this._purchaser != null) ? string.Format("{0}, {1} {2} {3} ip: {4} NameOnCard: {5} LastFour: {6}",
                            _purchaser.LastName, _purchaser.FirstName,
                            _purchaser.UserName, _purchaser.Phone, this.IpAddress, this.NameOnCard, lastFour) : "customer is empty";
                    }

                    //string post = ParseCardNumFromLogData(postData);

                    string post = IncludeSafeDataOnly(postData);
                    post = FormatPostDataForLog(post);

                    sw.Write(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\r\n");

                    sw.WriteLine(string.Format("Data Sent To AuthorizeNet:\t{0}\r\n{1}\r\n{2}\r\n{3}",
                        DateTime.Now.ToString(), post, custInfo, itemDescription.Replace("~","\r\n")));

                    sw.Write("--\r\n");
                //}
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            if (sw != null) sw.Close();
            if (fs != null) fs.Close();
        }

        //chop out the main cc nums
        private string ParseCardNumFromLogData(string post)
        {
            int idxStart = post.IndexOf("&x_Card_Num=") + 12;//length of string to find
            int idxEnd = post.IndexOf("&x_Exp_Date=") - 4;//just keep last four digits

            //take the substring from 0 to start and from end to length of post
            string ret = string.Format("{0}x{1}", post.Substring(0,idxStart), post.Substring(idxEnd));
            return ret;
        }

        private string IncludeSafeDataOnly(string post)
        {
            //no longer want to log raw cc data
            int idxStart1 = post.IndexOf("&x_Test_Request=");
            int idxStop1 = post.IndexOf("&x_Card_Num=");
            int idxStart2 = post.IndexOf("&x_Trans_Id=");

            return string.Format("{0}{1}", post.Substring(idxStart1, idxStop1 - idxStart1), post.Substring(idxStart2));
        }

        /// <summary>
        /// This adds line breaks to the post data
        /// </summary>
        private string FormatPostDataForLog(string posted)
        {
            StringBuilder sb = new StringBuilder();

            //place a new line every so many chars
            int lineLength = 110;
            int startIdx = 0;

            while (startIdx < posted.Length)
            {
                int indice = -1;
                if (startIdx + lineLength < posted.Length)
                    indice = posted.IndexOf("&", startIdx + lineLength);

                if (indice == -1)
                    indice = posted.Length;

                try
                {
                    sb.AppendFormat("{0}{1}", posted.Substring(startIdx, indice - startIdx), Environment.NewLine);
                }
                catch (Exception ex)
                {
                    //we HAVE to return a log of something
                    _Error.LogException(ex, true);
                    return posted;
                }

                startIdx = indice;

            }

            return sb.ToString();

        }

        internal void LogResponse(string description)
        {
            if (this.response != null && this.response.CompleteResponse != null && this.response.CompleteResponse.Trim().Length > 0)
            {
                this.ResponseCode = int.Parse(response.ResponseCode);
                this.ResponseSubcode = (response.ResponseSubcode.Trim().Length > 0) ? response.ResponseSubcode : null;
                this.ResponseReasonCode = (response.ResponseReasonCode.Trim().Length > 0) ? int.Parse(response.ResponseReasonCode) : int.MinValue;
                this.ResponseReasonText = (response.ResponseReasonText.Trim().Length > 0) ? response.ResponseReasonText : null;
                this.ApprovalCode = (response.ApprovalCode.Trim().Length > 0) ? response.ApprovalCode : null;
                this.AVSResultCode = (response.AVSResultCode.Trim().Length > 0) ? response.AVSResultCode : null;
                this.CardCodeResponseCode = (response.CardCodeResponseCode != null && response.CardCodeResponseCode.Trim().Length > 0) ? 
                    response.CardCodeResponseCode : null;
                this.IsMD5Match = response.MD5Result;
                this.DupeSeconds = _Config._AuthorizeNetDuplicateSeconds;
                this.CustomerId = (response.CustomerID.Trim().Length > 0) ? int.Parse(response.CustomerID) : 0;

                this.ProcessorId = response.ProcessorId;
                this.InvoiceNumber = response.InvoiceNumber;

                string itemInfo = string.Empty;

                if(description != null && description.Trim().Length > 0)
                    itemInfo = System.Text.RegularExpressions.Regex.Replace(description, @"\s+", " ").Replace(@"\", "-").Replace("/", "-").Replace("'", "").Trim();

                this.Description = (itemInfo.Length > 1000) ? string.Format("{0}...", itemInfo.Substring(0, 990)) : itemInfo;

                this.Amount = decimal.Parse(response.Amount);
                this.Method = response.Method;
                this.TransactionType = response.TransactionType;

                this.FirstName = response.CardholderFirstName;
                this.LastName = response.CardholderLastName;
                this.Company = (response.Company.Trim().Length > 0) ? response.Company : null;
                this.BillingAddress = response.BillingAddress;
                this.City = response.City;
                this.State = response.State;
                this.Zip = response.Zip;
                this.Country = response.Country;
                this.Phone = response.Phone;
                this.Email = response.Email;

                this.ShipToFirstName = (response.ShipToFirstName.Trim().Length > 0) ? response.ShipToFirstName : null;
                this.ShipToLastName = (response.ShipToLastName.Trim().Length > 0) ? response.ShipToLastName : null;
                this.ShipToCompany = (response.ShipToCompany.Trim().Length > 0) ? response.ShipToCompany : null;
                this.ShipToAddress = (response.ShipToAddress.Trim().Length > 0) ? response.ShipToAddress : null;
                this.ShipToCity = (response.ShipToCity.Trim().Length > 0) ? response.ShipToCity : null;
                this.ShipToState = (response.ShipToState.Trim().Length > 0) ? response.ShipToState : null;
                this.ShipToZip = (response.ShipToZip.Trim().Length > 0) ? response.ShipToZip : null;
                this.ShipToCountry = (response.ShipToCountry.Trim().Length > 0) ? response.ShipToCountry : null;

                this.TaxAmount = (response.TaxAmount.Trim().Length > 0) ? decimal.Parse(response.TaxAmount) : decimal.MinValue;
                this.FreightAmount = (response.FreightAmount.Trim().Length > 0) ? decimal.Parse(response.FreightAmount) : decimal.MinValue;
            }
            else
            {
                this.ResponseReasonText = response.RawAuthMessage;
            }
        }

        #endregion

        #region AuthorizeNet Response
        /// <summary>
        /// Holds information on the response from AuthorizeNet
        /// </summary>
        [Serializable()]
        public class AuthorizeNetResponse
        {
            public static string lockObj = "";

            public bool IsAuthorized;
            public bool MD5Result;
            public string RawAuthMessage;
            public string CompleteResponse;


            /// <summary>
            /// this is for testing - the shunt routine
            /// </summary>
            /// <param name="c"></param>
            /// <param name="i"></param>
            /// <param name="IsRefund"></param>
            /// <param name="ipAddress"></param>
            //public AuthorizeNetResponse(Invoice i, bool IsRefund, string ipAddress)
            public AuthorizeNetResponse(string response, string ipAddress)
            {
                string[] parsed = response.Split(',');

                ParseInfo(parsed);

                if(!this.IsAuthorized)
                    RawAuthMessage = "testing mode is declining transactions";

                //if (!_Config._AuthNetTestResult) RawAuthMessage = "testing mode is declining transactions";
            }

            public AuthorizeNetResponse(string responseString, string itemDescription, bool IsRefund, string ipAddress)
            {
                IsAuthorized = false;
                CompleteResponse = responseString;

                if (CompleteResponse != null && CompleteResponse.Trim().Length > 0)
                {
                    string[] parsed = CompleteResponse.Split(',');

                    ParseInfo(parsed);

                    //Determine authorization
                    if (this.ResponseCode == "1")
                        IsAuthorized = true;
                    else
                    {
                        if (this.ResponseCode != null && this.ResponseCode == "3" && this.ResponseReasonCode != null && this.ResponseReasonCode == "11")
                        {
                            RawAuthMessage = "DUPLICATE TRANSACTION. If you have recently made a purchase, please wait a few minutes and try again.";
                        }
                        else
                            RawAuthMessage = ResponseReasonText;
                    }

                    MD5Result = true;
                    decimal submitAmount = 0;
                    bool isAmount = decimal.TryParse(this.Amount, out submitAmount);

                    if (isAmount && (submitAmount > 0))
                    {
                        string md5 = string.Format("{0}{1}{2}{3}", _Config._AuthorizeNetMD5HashValue, _Config._AuthorizeNetLogin, this.ProcessorId,
                            (this.Amount == null) ? "0.00" : decimal.Round(decimal.Parse(this.Amount), 2).ToString());
                        string checkMd5 = Utils.Crypt.CreateMD5(md5).ToUpper();

                        if (this.ReturnMd5 != checkMd5)
                        {
                            _Error.LogException(new Exception(string.Format("***************\r\n{0}\tBad Md5 value.\r\nInvoiceGuid: {1}\tEmail: {2}\tIP: {3}\r\n**************",
                                DateTime.Now.ToString(), this.InvoiceNumber, this.Email, ipAddress)));

                            MD5Result = false;
                        }
                    }

                    LogAuthorizeNetResponse(null, itemDescription, IsRefund);
                }
                else
                {
                    LogAuthorizeNetResponse("No response from AuthorizeNet", itemDescription, IsRefund);

                    RawAuthMessage = "An error occurred connecting to our payment processor.  We apologize for the inconvenience, please try your order again later.";
                }
            }

            #region Logging
            public void LogAuthorizeNetResponse(string errorString, string itemDescription, bool IsRefund)
            {
                System.IO.FileStream fs = null;
                System.IO.StreamWriter sw = null;

                try
                {
                    lock (lockObj)
                    {
                        string fileName = string.Format("AuthorizeNetLog_{0}{1}.log", DateTime.Now.ToString("MM_dd_yyyy"), (IsRefund) ? "_Refunds" : string.Empty);
                        string fullPath = string.Format("{0}\\{1}", _Config._AuthorizeNetLogPath, fileName);

                        //string mappedPath = System.Web.HttpContext.Current.Server.MapPath(fullPath);
                        string mappedPath = string.Empty;
                        if (System.Web.HttpContext.Current != null)
                            mappedPath = System.Web.HttpContext.Current.Server.MapPath(fullPath);
                        else
                            mappedPath = string.Format(@"{0}{1}", _Config._MappedRootDirectory, 
                                fullPath.Replace("/", @"\")).Replace(@"\\", @"\");

                        fs = new System.IO.FileStream(mappedPath, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                        sw = new System.IO.StreamWriter(fs);


                        if (errorString != null)
                        {
                            sw.WriteLine(string.Format("{0}, {1}", DateTime.Now.ToString(), errorString));
                        }
                        else
                        {
                            string line1 = string.Format("OrderNum: {0}\r\n", this.InvoiceNumber);

                            string procId = this.ProcessorId.Trim();

                            string procIdTrunc = (procId.Length >= 4) ?
                                string.Format("x{0}", procId.Substring(procId.Length - 4, 4)) :
                                (procId.Length == 0) ? "NA" : string.Format("x{0}", procId);
                            
                            //2-14-2015 removed logging of approval code and truncated procId
                            string line2 = string.Format("ProcId: {0}\tAVS Result: {1}\tCode Response: {2}\r\n{3}\tReason: {4}\r\nAmount: ${5}\r\n",
                                procIdTrunc, AVSResultCode, CardCodeResponseCode,//note response code is normally blank
                                (ResponseCode == "1") ? "APPROVED" : (ResponseCode == "2") ? "DECLINED" : (ResponseCode == "3") ? "ERROR" : "UNKNOWN",
                                ResponseReasonText, this.Amount);

                            string cust = string.Empty;
                            string items = string.Empty;

                            if (!IsRefund)
                                cust = string.Format("{0} {1}, {2}\r\n", this.Email, this.CardholderLastName, this.CardholderFirstName);

                            sw.WriteLine(string.Format("Data Returned:\t{0}\t{1}{2}{3}", DateTime.Now.ToString(),
                                line1, line2, cust));
                        }

                        sw.Write("<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<\r\n");
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }

                if (sw != null) sw.Close();
                if (fs != null) fs.Close();
            }
            #endregion

            #region Parsing
            private void ParseInfo(string[] response)
            {
                //zero based!
                ResponseCode        = response[0];//1 Response Code
                ResponseSubcode     = response[1];//2 Response Subcode
                ResponseReasonCode  = response[2];//3 Response Reason Code
                ResponseReasonText  = response[3];//4 Response Reason Text
                ApprovalCode        = response[4];//5 ApprovalCode
                AVSResultCode       = response[5];//6 AVS Result Code

                ProcessorId         = response[6];//7 TransactionId
                InvoiceNumber       = response[7];//8 InvoiceNumber
                Description         = response[8];//9 Description
                Amount              = response[9];//10 Amount
                Method              = response[10];//11 Method
                TransactionType     = response[11];//12 Transaction Type

                CustomerID          = response[12];//13 CustomerID
                CardholderFirstName = response[13];//14 First
                CardholderLastName  = response[14];//15 Last
                Company             = response[15];//16 Company
                BillingAddress      = response[16];//17 Billing Address
                City                = response[17];//18 City
                State               = response[18];//19 State
                Zip                 = response[19];//20 Zip
                Country             = response[20];//21 Country
                Phone               = response[21];//22 Phone
                                    //23 Fax
                Email               = response[23];//24 Email

                ShipToFirstName     = response[24];//25 ShipFirst
                ShipToLastName      = response[25];//26 ShipLast
                ShipToCompany       = response[26];//27 ShipCompany
                ShipToAddress       = response[27];//28 ShipAddress
                ShipToCity          = response[28];//29 ShipCity
                ShipToState         = response[29];//30 ShipState
                ShipToZip           = response[30];//31 ShipZip
                ShipToCountry       = response[31];//32 ShipCountry

                TaxAmount           = response[32];//33 Tax
                                    //34 Duty
                FreightAmount       = response[34];//35 Freight
                                    //36 Tax Exempt Flag
                                    //37 PO Number
                ReturnMd5           = response[37];//38 MD5 hash
                if(response.Length > 38)
                    CardCodeResponseCode = response[38];//39 CC Response Code 
                                    //40 Cardholder Authentication Verification Value 
                                    //41-68 reserved for future
                                    //69- echo of merchant defined fields
            }
            #endregion

            #region Properties
            internal string InvoiceNumber;			//20
            internal string ProcessorId;			//10 - TransactionId (name changed to match existing schema)	
            internal string Amount;					//15
            internal string TaxAmount;				//15
            internal string FreightAmount;			//10
            internal string Description;			//255 - rebuild for insert into db

            internal string Method;					//CC or ECHECK
            internal string TransactionType;		//AUTH_CApture, AUTH_ONLY, CAPTURE_ONLY, CREDIT, VOID, PRIOR_AUTH_CAPTURE

            internal string ResponseCode;			//int 1-app, 2-dec, 3-err, 4-being held for review
            internal string ResponseSubcode;		//
            internal string ResponseReasonCode;		//int 1 - safe length of 10
            internal string ResponseReasonText;		//string - 255
            internal string ApprovalCode;			//6 digits
            internal string AVSResultCode;			//10
            internal string CardCodeResponseCode;	//10
            internal string ReturnMd5;				//32?

            internal string CustomerID;				//20
            internal string Email;					//255
            internal string CardholderFirstName;	//50
            internal string CardholderLastName;		//50
            internal string Company;				//50
            internal string BillingAddress;			//60
            internal string City;					//40
            internal string State;					//40 - 2 digit state code or full name
            internal string Zip;					//20
            internal string Country;				//60 - 2 digit country code or full name (english spelling)
            internal string Phone;					//25

            internal string ShipToFirstName;		//50
            internal string ShipToLastName;			//50
            internal string ShipToCompany;			//50
            internal string ShipToAddress;			//60
            internal string ShipToCity;				//40
            internal string ShipToState;			//40
            internal string ShipToZip;				//20
            internal string ShipToCountry;			//60
            #endregion
        }
        #endregion
    }
}
//110524 - 1439