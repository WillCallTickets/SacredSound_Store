using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using Wcss;
using SubSonic;
using WillCallWeb.StoreObjects;

namespace WillCallWeb
{
    /// <summary>
    /// Summary description for ConfirmationQuery
    /// </summary>
    public class ConfirmationQuery
    {
        public ConfirmationQuery() { }

        public static void InitConfirmationQuery(QueryCommand cmd, WebContext ctx, string lastFour)
        {
            cmd.AddParameter("@authId", ctx.SessionAuthorizeNet.Id, DbType.Int32);
            cmd.AddParameter("@aspnetUserId", ctx.SessionAuthorizeNet.UserId.ToString(), DbType.String);
            cmd.AddParameter("@customerId", ctx.SessionAuthorizeNet.CustomerId, DbType.Int32);
            cmd.AddParameter("@invoiceId", ctx.SessionAuthorizeNet.TInvoiceId, DbType.Int32);
            cmd.AddParameter("@processorId", ctx.SessionAuthorizeNet.ProcessorId, DbType.String);
            cmd.AddParameter("@amount", ctx.SessionAuthorizeNet.Amount, DbType.Decimal);
            cmd.AddParameter("@nameOnCard", ctx.SessionAuthorizeNet.NameOnCard, DbType.String);
            cmd.AddParameter("@lastFour", lastFour, DbType.String);
            cmd.AddParameter("@userIp", ctx.SessionAuthorizeNet.IpAddress, DbType.String);
            cmd.AddParameter("@zero", 0, DbType.Decimal);
            cmd.AddParameter("@singleQty", 1, DbType.Int32);
            cmd.AddParameter("@purchased", _Enums.PurchaseActions.Purchased.ToString(), DbType.String);

            string keys = string.Empty;
            if (ctx.MarketingProgramKey != null && ctx.MarketingProgramKey.Trim().Length > 0)
                keys = ctx.MarketingProgramKey.Trim();
            if (ctx.SalePromotionUnlock != null && ctx.SalePromotionUnlock.Trim().Length > 0)
                keys += string.Format("{0}sp={1}", (keys.Length > 0) ? "~" : string.Empty, ctx.SalePromotionUnlock.Trim());
            cmd.AddParameter("@marketingKeys", keys, DbType.String);
            cmd.AddParameter("@purchaseName", ctx.Cart.PurchaseName.ToUpper(), DbType.String);
        }

        public static void ConstructCartSql(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId, StringBuilder products, ref int counter, DateTime now)
        {
            sb.Append("BEGIN TRANSACTION ");

            //UPDATE INVOICE - by substracting from balance due - we may reveal errors
            sb.Append("UPDATE [Invoice] ");
            sb.Append("SET [vcProducts] = @products, [InvoiceStatus] = 'Paid', [mTotalPaid] = @amount, [mBalanceDue] = ([mBalanceDue] - @amount), [MarketingKeys] = @marketingKeys ");
            sb.Append("WHERE [Id] = @invoiceId  ");

            ConfirmationQuery.InsertSql_ProcessingFee(sb, ctx, cmd, google, uniqueId);
            ConfirmationQuery.InsertSql_Tickets(sb, ctx, cmd, google, uniqueId, products, ref counter);
            ConfirmationQuery.InsertSql_Merchandise(sb, ctx, cmd, google, uniqueId, products, ref counter);
            ConfirmationQuery.InsertSql_Promotions(sb, ctx, cmd, google, uniqueId, products, ref counter);
            ConfirmationQuery.InsertSql_Shipping(sb, ctx, cmd, google, uniqueId, ref counter);
            ConfirmationQuery.InsertSql_Donations(sb, ctx, cmd, google, uniqueId, products, ref counter);

            #region Complete Confirm Query

            cmd.AddParameter("@products", products.ToString(), DbType.String);

            //Ticket Items Only! If no ship method has been specified - ensure it is will call
            sb.AppendFormat("UPDATE [InvoiceItem] SET [ShippingMethod] = '{0}' WHERE [tInvoiceId] = @invoiceId ", ShipMethod.WillCall);
            sb.Append("AND [TShowTicketId] IS NOT NULL AND ([ShippingMethod] IS NULL OR ([ShippingMethod] IS NOT NULL AND LEN(RTRIM(LTRIM([ShippingMethod]))) = 0)) ");

            //INSERT A TRANSACTION
            sb.Append("INSERT [InvoiceTransaction] ([tInvoiceId], [UserId], [PerformedBy], [CustomerId], [TransType], ");
            sb.Append("[FundsType], [FundsProcessor], [ProcessorId], [mAmount], [NameOnCard], ");
            sb.Append("[LastFourDigits], [UserIp], [dtStamp]) ");
            sb.AppendFormat("VALUES (@invoiceId, '{0}', '{1}', @customerId, '{2}', ", ctx.SessionAuthorizeNet.UserId.ToString(),
                _Enums.PerformedByTypes.CustomerSite.ToString(), _Enums.TransTypes.Payment.ToString());
            sb.AppendFormat("'{0}', '{1}', @processorId, @amount, @nameOnCard, ", _Enums.FundsTypes.CreditCard.ToString(),
                _Enums.FundsProcessor.AuthorizeNet.ToString());
            sb.AppendFormat("@lastFour, @userIp, '{0}') ", now.ToString());

            #endregion

            ConfirmationQuery.InsertSql_RecordCredits(sb, ctx, cmd, google, uniqueId, now);

            sb.Append("COMMIT TRANSACTION ");

            //assign the query to the command
            cmd.CommandSql = sb.ToString();
        }

        public static void ExecuteCartSql(QueryCommand cmd)
        {
            try
            {
                //12-12-13 *issue found
                // it is possible for the @products parameter to be too long
                // * determine if we can truncate safely?
                //   - where is vcProducts in use
                // * change default size
                //   - check against schema to see if param is too long

                //SOLUTION:
                // I have decided to make the vcProducts param stuopid long to avoid any crazy million-item orders, but there
                // is a limit to how far I should go. I have set the maxLength to 1500 (up from 300 - previous used max was 280)
                // So now we check for going over that amount. If the threshhold is hit, we don't allow the order.
                QueryParameter vp = cmd.Parameters.Find(delegate(QueryParameter match) { return (match.ParameterName == "@products"); });
                if (vp != null && vp.ParameterValue.ToString().Length > 0)
                {
                    int productLength = Wcss.Invoice.VcProductsColumn.MaxLength;
                    string val = vp.ParameterValue.ToString();
                    if (val.Length > productLength)
                    {
                        try
                        {
                            //Notify Admin
                            QueryParameter idx = cmd.Parameters.Find(delegate(QueryParameter match) { return (match.ParameterName == "@invoiceId"); });

                            //Log and send email
                            _Error.LogException(new ArgumentOutOfRangeException("products", 
                                string.Format("Information truncated in InvoiceId {1}.{0}Max product length exceeded for invoice.vcProducts.{0}{0}Original submission{0}{0}{2}",
                                    Environment.NewLine,
                                    idx.ParameterValue.ToString(),
                                    val)                                
                                ), true );

                        }
                        catch(Exception prod)
                        {   
                            _Error.LogException(new ArgumentOutOfRangeException("products",
                                string.Format("Information truncated in InvoiceId {1}.{0}Max product length exceeded for invoice.vcProducts.{0}{0}Original submission{0}{0}{2}",
                                    Environment.NewLine,
                                    "could not get invoiceId",
                                    val)
                                ), true);
                        }

                        vp.ParameterValue = val.Substring(0, productLength);
                    }
                }



                //ensure marketingKeys length                
                QueryParameter q = cmd.Parameters.Find(delegate(QueryParameter match) { return (match.ParameterName == "@marketingKeys"); } );
                if(q != null && q.ParameterValue.ToString().Length > 0)
                {
                    int mktLength = Wcss.Invoice.MarketingKeysColumn.MaxLength;
                    string val = q.ParameterValue.ToString();
                    if (val.Length > mktLength)
                        q.ParameterValue = val.Substring(0, mktLength);
                }




                DataService.ExecuteScalar(cmd);

                cmd = null;
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                _Error.LogException(sex);
                throw new Exception(string.Format("Confirmation Sql Error.\r\n{0}\r\n{1}", sex.Message, sex.StackTrace));
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("Confirmation Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        public static void RecordCoupons(WebContext ctx, string userName)
        {
            try
            {
                //if the cart had street teamer program promotions...
                System.Collections.Generic.List<SaleItem_Promotion> proms = new System.Collections.Generic.List<SaleItem_Promotion>();
                proms.AddRange(ctx.Cart.PromotionItems.FindAll(delegate(SaleItem_Promotion match) { return match.SalePromotion.Requires_PromotionCode; }));

                foreach (SaleItem_Promotion promo in proms)
                {
                    //get the matching coupon code
                    string matchingCode = promo.SalePromotion.CouponMatch(ctx.SalePromotion_CouponCodes, _Config._Coupon_IgnoreCase);

                    if (matchingCode != null && matchingCode.Length > 0)
                    {
                        //record the coupon's use
                        UserCouponRedemption.RecordRedemption(userName, promo.tSalePromotionId, matchingCode,
                            promo.Price, ctx.Cart.Total_NonDiscounted);
                    }
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw new Exception(string.Format("Confirmation Error.\r\n{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        //public static void EndGoogleAnalyticScript(StringBuilder google)
        //{
        //    //finish up google analytics
        //    google.AppendFormat("{0}{1}pageTracker._trackTrans(); ", Utils.Constants.NewLines(2), Utils.Constants.Tabs(2));
        //    google.AppendFormat("{0}{1}}} catch(err) {{}}{0}</script> ", Utils.Constants.NewLine, Utils.Constants.Tabs(1));
        //}

        #region INSERT PROCESSING FEE

        private static void InsertSql_ProcessingFee(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId)
        {
            if (ctx.Cart.ProcessingFee != null)
            {
                InvoiceFee item = ctx.Cart.ProcessingFee;
                cmd.AddParameter("@procMainActName", item.Description, DbType.String);
                cmd.AddParameter("@procId", item.Id.ToString(), DbType.String);
                cmd.AddParameter("@procPrice", item.Price, DbType.Decimal);

                //insert sql
                sb.Append(Checkout_Services.InsertInvoiceItem_ProcessingFee_Sql("@procMainActName", "@procPrice", "@singleQty", "@procId"));

                ConfirmationTracker.Analytics_AddTransItem(google, uniqueId, string.Format("processing_{0}", item.Id.ToString()),
                    item.Description, _Enums.InvoiceItemContext.processing.ToString(), item.Price, 1);
            }
        }
        #endregion

        #region INSERT TICKET ITEMS

        private static void InsertSql_Tickets(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId, StringBuilder products, ref int counter)
        {
            foreach (SaleItem_Ticket item in ctx.Cart.TicketItems)
            {
                products.Append(ShoppingCart.FormatItemProductListing(item));

                ConfirmationHelper.CreateInvoiceItemTicket(cmd, sb, products, item.Ticket, item, false, ref counter);

                //if we have a pckage ticket - include info for other tickets too
                if (item.Ticket.IsPackage && (!item.Ticket.IsCampingPass()))
                    foreach (ShowTicket st in item.Ticket.LinkedShowTickets)
                        ConfirmationHelper.CreateInvoiceItemTicket(cmd, sb, products, st, item, true, ref counter);

                ConfirmationTracker.Analytics_AddTransItem(google, uniqueId, string.Format("ticket_{0}", item.tShowTicketId.ToString()),
                    Utils.ParseHelper.HtmlEncode_Extended(Utils.ParseHelper.StripHtmlTags(item.Ticket.DisplayNameWithAttribsAndDescription)), 
                    _Enums.InvoiceItemContext.ticket.ToString(), item.PerItemPrice, item.Quantity);
            }
        }
        #endregion

        #region INSERT MERCHANDISE ITEMS

        private static void InsertSql_Merchandise(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId, StringBuilder products, ref int counter)
        {
            foreach (SaleItem_Merchandise item in ctx.Cart.MerchandiseItems)
            {
                products.Append(ShoppingCart.FormatItemProductListing(item));

                ConfirmationHelper.CreateInvoiceItemMerch(cmd, sb, products, item, item.Quantity, ref counter);

                string categs = string.Empty;
                Merch parentItem = (item.MerchItem.IsParent) ? item.MerchItem : item.MerchItem.ParentMerchRecord;
                foreach (MerchDivision div in parentItem.MerchDivisionCollection)
                    categs += string.Format("{0},", div.Name);

                ConfirmationTracker.Analytics_AddTransItem(google, uniqueId, string.Format("merch_{0}", item.tMerchId.ToString()),
                    Utils.ParseHelper.HtmlEncode_Extended(Utils.ParseHelper.StripHtmlTags(item.MerchItem.DisplayNameWithAttribs)),
                    Utils.ParseHelper.HtmlEncode_Extended(categs.TrimEnd(',')), item.Price, item.Quantity);
            }
        }
        #endregion

        #region INSERT PROMOTIONS

        private static void InsertSql_Promotions(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId, StringBuilder products, ref int counter)
        {
            foreach (SaleItem_Promotion item in ctx.Cart.PromotionItems)
            {
                string promoIdx = ConfirmationHelper.AddParam(cmd, string.Format("@promoIdx_{0}", counter), item.tSalePromotionId, DbType.Int32);
                string qty = ConfirmationHelper.AddParam(cmd, string.Format("@qty_{0}", counter), item.Quantity, 
                    DbType.Int32);

                //Note: I now realize - way after the fact - that this should have been put into the criteria column - but 
                // don't want to break anything else that may have been in the flow, etc
                //So lesson learned for next time
                string atts = (item.SalePromotion != null) ? item.SalePromotion.DisplayText : item.SelectedAwardIdsString;

                if (item.SalePromotion != null && item.SalePromotion.Requires_PromotionCode)
                    atts += string.Format(" (coupon code: {0})", item.GetDisplayableCouponCode());
                string desc = ConfirmationHelper.AddParam(cmd, string.Format("@desc_{0}", counter),
                    (atts != null) ? ((atts.Length > 300) ? atts.Substring(0, 299).Trim() : atts) : null, DbType.String);
                string price = ConfirmationHelper.AddParam(cmd, string.Format("@price_{0}", counter), item.Price, DbType.Decimal);
                string contxt = ConfirmationHelper.AddParam(cmd, string.Format("@ctx_{0}", counter),
                    (item.SalePromotion != null) ? item.SalePromotion.Context_Award.ToString() : "promotion", DbType.String);

                //we need to ensure that we have a selected product
                try
                {
                    if (item != null && item.HasProductSelections)
                    {
                        //http://blog.tech-cats.com/2008/07/using-select-in-with-subsonic.html
                        MerchCollection selectedColl = new SubSonic.Select()
                            .From(Merch.Schema)
                            .Where(Merch.Columns.Id)
                            .In(item.SelectedAwardIds)
                            .ExecuteAsCollection<MerchCollection>();

                        foreach (Merch merch in selectedColl)
                        {
                            string selMerchIdx = string.Format("@selIdx_{0}", counter);
                            string name = string.Format("@name_{0}", counter);

                            cmd.AddParameter(selMerchIdx, merch.Id, DbType.Int32);
                            cmd.AddParameter(name, merch.DisplayNameWithAttribs, DbType.String);

                            sb.AppendFormat("UPDATE Merch SET [iSold] = m.[iSold] + {0} FROM [Merch] m WHERE m.[Id] = {1} ",
                                (item.SalePromotion.AllowMultipleAwardSelections) ? "1" : qty, selMerchIdx);

                            //add to shipments
                            if (ctx.Cart.Shipments_Merch.Count > 0)
                            {
                                if (merch.DeliveryType == _Enums.DeliveryType.parcel)
                                {
                                    //hook it up to a shipment
                                    SaleItem_Shipping general = ctx.Cart.Shipments_Merch.Find(delegate(SaleItem_Shipping match) { return (match.IsGeneral); });
                                    if (general != null)
                                    {
                                        general.Items_Promo.Add(item);
                                    }
                                    else//add it to the first shipment to go out for this thing
                                    {
                                        System.Collections.Generic.List<SaleItem_Shipping> coll = new System.Collections.Generic.List<SaleItem_Shipping>();
                                        coll.AddRange(ctx.Cart.Shipments_Merch);
                                        if (coll.Count > 1)
                                            coll.Sort(new Utils.Reflector.CompareEntities<SaleItem_Shipping>(Utils.Reflector.Direction.Ascending, "ShipDate"));
                                        coll[0].Items_Promo.Add(item);
                                    }
                                }
                            }
                            else if (merch.DeliveryType == _Enums.DeliveryType.parcel)
                            {
                                //indicate we need to add a shipment 
                                //create new shipment in sql
                                //notify admin
                                _Error.LogException(new Exception("There is no shipment to attach to"));
                            }

                            //generate codes for each download, etc
                            string crite = null;
                            if(item.SalePromotion.AdditionalText != null && item.SalePromotion.AdditionalText.Trim().Length > 0)
                                crite = ConfirmationHelper.AddParam(cmd, string.Format("@crit_{0}", counter), item.SalePromotion.AdditionalText, DbType.String);

                            if ((merch.IsDownloadDelivery || merch.IsActivationCodeDelivery || merch.IsGiftCertificateDelivery) && item.Quantity > 1)
                            {
                                if (!item.SalePromotion.AllowMultipleAwardSelections)
                                {
                                    for (int i = 0; i < item.Quantity; i++)
                                    {
                                        //insert sql - quantity is one - one entry for each in qty    
                                        //only record price for the first item
                                        sb.Append(Checkout_Services.InsertInvoiceItem_PromotionItem_Sql(true, contxt,
                                            name, (i == 0) ? price : "@zero", "@singleQty", selMerchIdx, promoIdx, crite, desc, null));
                                    }
                                }
                                else
                                {
                                    sb.Append(Checkout_Services.InsertInvoiceItem_PromotionItem_Sql(true, contxt,
                                            name, price, "@singleQty", selMerchIdx, promoIdx, crite, desc, null));
                                }
                            }
                            else
                            {
                                //insert sql
                                sb.Append(Checkout_Services.InsertInvoiceItem_PromotionItem_Sql(true, contxt,
                                    name, price, qty, selMerchIdx, promoIdx, crite, desc, null));
                            }


                            counter++;
                        }
                    }
                    //else
                    //{
                    //    string selMerchIdx = string.Format("@selIdx_{0}", counter);
                    //    string name = string.Format("@name_{0}", counter);
                    //    cmd.AddParameter(selMerchIdx, null, DbType.Int32);
                    //    cmd.AddParameter(name, "no promotion selected", DbType.String);

                    //    //insert sql
                    //    sb.Append(Checkout_Services.InsertInvoiceItem_PromotionItem_Sql(false, _Enums.InvoiceItemContext.noteitem,
                    //        name, price, qty, selMerchIdx, promoIdx, null, desc, null));
                    //}

                    else if (item.SalePromotion.Context_Award == _Enums.InvoiceItemContext.discount ||
                        item.SalePromotion.Context_Award == _Enums.InvoiceItemContext.shippingmerch ||
                        item.SalePromotion.Context_Award == _Enums.InvoiceItemContext.shippingticket)
                    {
                        //TESTED: false
                        //insert sql
                        sb.Append(Checkout_Services.InsertInvoiceItem_PromotionItem_Sql(true, contxt,
                            desc, price, qty, null, promoIdx, null, null, null));
                    }

                    products.Append(ShoppingCart.FormatItemProductListing(item));

                    SalePromotion itm = item.SalePromotion;
                    //PLEASE NOTE USE OF ITEM AND ITM - one is a sale item - the other - the matched product
                    ConfirmationTracker.Analytics_AddTransItem(google, uniqueId, string.Format("promo_{0}", itm.Id.ToString()),
                        Utils.ParseHelper.HtmlEncode_Extended(itm.Name), _Enums.InvoiceItemContext.promotion.ToString(), item.Price, item.Quantity);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }

                counter++;
            }
        }
        #endregion

        #region INSERT SHIPPING ITEMS

        private static void InsertSql_Shipping(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId, ref int counter)
        {
            foreach (SaleItem_Shipping item in ctx.Cart.Shipments_All)
            {
                //do not include will call shipments!
                if (item.ShipMethod != ShipMethod.WillCall)
                {
                    string idx = string.Format("@idx_{0}", counter);
                    string ctxt = ConfirmationHelper.AddParam(cmd, string.Format("@ctxt_{0}", counter), item.ShipContext.ToString(), DbType.String);
                    //this will be ultimately ignored in the ticket shipping context
                    string expShipDate = ConfirmationHelper.AddParam(cmd, string.Format("@expShipDate_{0}", counter), item.ShipDate, DbType.DateTime);
                    string price = ConfirmationHelper.AddParam(cmd, string.Format("@price_{0}", counter), item.ShipCost, DbType.Decimal);
                    string method = ConfirmationHelper.AddParam(cmd, string.Format("@method_{0}", counter), item.ShipMethod, DbType.String);
                    //get the shipping method...insert method into invoice items - update existing items connected to this shipment

                    sb.AppendFormat("DECLARE {0} int ", idx);

                    //insert sql
                    sb.Append(Checkout_Services.InsertInvoiceItem_ShippingItem_Sql(ctxt,
                        method, price, "@singleQty", expShipDate));

                    sb.AppendFormat("SELECT {0} = SCOPE_IDENTITY() ", idx);

                    if (item.ShipContext == _Enums.InvoiceItemContext.shippingticket)
                    {
                        foreach (SaleItem_Ticket tkt in item.Items_Tickets)
                        {
                            sb.AppendFormat("UPDATE [InvoiceItem] SET [TShipItemId] = {0}, [ShippingMethod] = {1} WHERE [TInvoiceId] = @invoiceId AND [tShowTicketId] = {2} ",
                                idx.ToString(), method, tkt.tShowTicketId.ToString());

                            if (tkt.Ticket.IsPackage)
                                foreach (ShowTicket st in tkt.Ticket.LinkedShowTickets)
                                    sb.AppendFormat("UPDATE [InvoiceItem] SET [TShipItemId] = {0}, [ShippingMethod] = {1} WHERE [TInvoiceId] = @invoiceId AND [tShowTicketId] = {2} ",
                                        idx.ToString(), method, st.Id.ToString());
                        }
                    }
                    else if (item.ShipContext == _Enums.InvoiceItemContext.shippingmerch)
                    {
                        //loop thru saleitems that have parcel items
                        foreach (SaleItem_Merchandise merch in item.Items_Merch)
                        {
                            //if the merch is a parcel item - update
                            //be sure to get the id of the parent
                            if (merch.MerchItem.IsParcelDelivery)
                            {
                                //update any merch items that are merch, invoice matches - no salePromo and no criteria.parentitemid
                                sb.AppendFormat("UPDATE [InvoiceItem] SET [TShipItemId] = {0}, [ShippingMethod] = {1} WHERE [TInvoiceId] = @invoiceId AND [tMerchId] = {2} AND [tSalePromotionId] IS NULL ",
                                    idx.ToString(), method, merch.tMerchId.ToString());
                                // and no criteria.parentitemid
                                sb.AppendFormat("AND [PurchaseAction] = '{0}' AND [vcContext] = 'merch' AND (([Criteria] IS NULL) OR (CHARINDEX('{1}', [Criteria]) < 1)) ",
                                    _Enums.PurchaseActions.Purchased.ToString(), InvoiceItem.ParentItemIdConstant);
                            }

                            //update selections that are parcels
                            if (merch.MerchBundleSelections.Count > 0)
                            {
                                List<MerchBundle_Listing> selections_All = new List<MerchBundle_Listing>();
                                selections_All.AddRange(merch.MerchBundleSelections
                                    .FindAll(delegate(MerchBundle_Listing match) { return (!match.IsOptOut) && match.SelectedInventory.IsParcelDelivery; }));

                                //Just go thru parcel items
                                if (selections_All.Count > 0)
                                {
                                    if (selections_All.Count > 1)
                                        selections_All.Sort(delegate(MerchBundle_Listing x, MerchBundle_Listing y) { return (x.BundleId.CompareTo(y.BundleId)); });

                                    int currentBundleId = 0;

                                    for (int i = 0; i < selections_All.Count; i++)
                                    {
                                        string merchBundleId = string.Format("{0}{1}", InvoiceItem.MerchBundleIdConstant, selections_All[i].BundleId.ToString());

                                        //record each bundle
                                        if (currentBundleId != selections_All[i].BundleId)
                                        {
                                            sb.AppendFormat("UPDATE [InvoiceItem] SET [TShipItemId] = {0}, [ShippingMethod] = {1} WHERE [TInvoiceId] = @invoiceId ",
                                                idx.ToString(), method);
                                            sb.AppendFormat("AND [PurchaseAction] = '{0}' AND [vcContext] = 'bundle' AND ([Criteria] IS NOT NULL AND CHARINDEX('{1}', [Criteria]) >= 1) ",
                                                _Enums.PurchaseActions.Purchased.ToString(), merchBundleId);
                                        }

                                        //Record parcel selection
                                        sb.AppendFormat("UPDATE [InvoiceItem] SET [TShipItemId] = {0}, [ShippingMethod] = {1} WHERE [TInvoiceId] = @invoiceId AND [tMerchId] = {2} AND [tSalePromotionId] IS NULL ",
                                            idx.ToString(), method, selections_All[i].SelectedInventoryId.ToString());
                                        sb.AppendFormat("AND [PurchaseAction] = '{0}' AND [vcContext] = 'merch' AND ([Criteria] IS NOT NULL AND CHARINDEX('{1}', [Criteria]) >= 1) ",
                                            _Enums.PurchaseActions.Purchased.ToString(), merchBundleId);

                                        //update comparer
                                        currentBundleId = selections_All[i].BundleId;
                                    }
                                }
                            }
                        }

                        //also update any promotion items
                        foreach (SaleItem_Promotion promo in item.Items_Promo)
                        {
                            sb.AppendFormat("UPDATE [InvoiceItem] SET [TShipItemId] = {0}, [ShippingMethod] = {1} WHERE [TInvoiceId] = @invoiceId AND [tMerchId] IN ({2}) AND [tSalePromotionId] = {3} ",
                                idx, method, promo.SelectedAwardIdsString, promo.tSalePromotionId);
                        }
                    }

                    counter++;

                    //there is no real sku here - use context
                    string shippingmethod = (item.ShipMethod != null) ? item.ShipMethod : "NA";
                    ConfirmationTracker.Analytics_AddTransItem(google, uniqueId, string.Format("{0}_{1}", item.ShipContext.ToString(),
                        Utils.ParseHelper.HtmlEncode_Extended(shippingmethod)),
                        Utils.ParseHelper.HtmlEncode_Extended(shippingmethod), "Shipping", item.ShipCost, item.Quantity);
                }
            }
        }
        #endregion

        #region INSERT DONATION ITEMS

        private static void InsertSql_Donations(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId, StringBuilder products, ref int counter)
        {
            //keep this operation separate until it is all sussed out
            //handle donations
            if (ctx.Cart.CharityAmount > 0)
            {
                //do we have an id for the invoice at this point? - yes!
                //create an invoice item and insert into table
                //description gets the id to hold onto for later
                sb.Append("DECLARE @charityIdx int; ");

                //insert sql
                sb.Append(Checkout_Services.InsertInvoiceItem_DonationItem_Sql("@charityContext",
                    "@charityName", "@charityAmount", "@singleQty", "@orgIdxString"));

                sb.Append("SELECT @charityIdx = SCOPE_IDENTITY(); ");

                //insert a charitable contribution to track
                sb.Append("INSERT [CharitableContribution] ([dtStamp], [tInvoiceItemId], [tCharitableOrgId]) ");
                sb.Append("VALUES ((getDate()), @charityIdx, @orgIdx); ");

                cmd.AddParameter("@charityContext", _Enums.InvoiceItemContext.charity.ToString(), DbType.String);

                string charityName = (ctx.Cart.CharityOrg != null) ? ctx.Cart.CharityOrg.Name_Displayable : "Charity";
                cmd.AddParameter("@charityName", charityName, DbType.String);
                cmd.AddParameter("@charityAmount", ctx.Cart.CharityAmount, DbType.Decimal);

                int orgIdx = (ctx.Cart.CharityOrg != null) ? ctx.Cart.CharityOrg.Id : 0;
                cmd.AddParameter("@orgIdx", orgIdx, DbType.Int32);
                cmd.AddParameter("@orgIdxString", orgIdx.ToString(), DbType.String);

                products.Append(ShoppingCart.FormatItemProductListing(_Enums.InvoiceItemContext.charity, orgIdx, 1));

                ConfirmationTracker.Analytics_AddTransItem(google, uniqueId, string.Format("charity_{0}", orgIdx.ToString()),
                   Utils.ParseHelper.HtmlEncode_Extended(charityName),
                   _Enums.InvoiceItemContext.charity.ToString(), ctx.Cart.CharityAmount, 1);
            }
        }
        #endregion

        #region RECORD CREDITS

        private static void InsertSql_RecordCredits(StringBuilder sb, WebContext ctx, QueryCommand cmd, StringBuilder google, string uniqueId, DateTime now)
        {
            //this is where we actually apply the 
            //make sure we dont over credit - or spend credit that was over the invoice balance due
            if (ctx.Cart.StoreCreditToApply > 0)
            {
                decimal pos = Math.Abs(ctx.Cart.StoreCreditToApply);
                decimal neg = pos * -1;
                cmd.Parameters.Add("@creditPOS", pos, DbType.Decimal);
                cmd.Parameters.Add("@creditNEG", neg, DbType.Decimal);

                //NOTE: we do not have to worry about admin profile here because admin cannot go thru order flow for another customer
                //update profile credit - after all has gone well

                //update the invoice to reflect what was paid by store credit
                sb.Append("UPDATE [Invoice] SET [mTotalPaid] = [mTotalPaid] + @creditPOS WHERE [Id] = @invoiceId; ");

                //create a trans for store credit - get the trans id to record for redemption
                sb.Append("DECLARE @transIdx int; ");
                sb.Append("INSERT [InvoiceTransaction] ([tInvoiceId], [UserId], [PerformedBy], [CustomerId], [TransType], ");
                sb.Append("[FundsType], [FundsProcessor], [ProcessorId], [mAmount], ");
                sb.Append("[UserIp], [dtStamp]) ");
                //invoiceId, userId, performer, custId, transaction type
                sb.AppendFormat("VALUES (@invoiceId, '{0}', '{1}', @customerId, '{2}', ", ctx.SessionAuthorizeNet.UserId.ToString(),
                    _Enums.PerformedByTypes.CustomerSite.ToString(), _Enums.TransTypes.Payment.ToString());
                //funds, processor, procId, amount, nameonCard
                sb.AppendFormat("'{0}', '{1}', '111', @creditPOS, ", _Enums.FundsTypes.StoreCredit.ToString(),
                    _Enums.FundsProcessor.Internal.ToString());
                //last4 digits, ip, dtStamp
                sb.AppendFormat("@userIp, '{0}') ", now.ToString());

                sb.AppendFormat("SET @transIdx = SCOPE_IDENTITY(); ");

                //create a redemption for the store credit
                sb.Append("INSERT [StoreCredit] ([dtStamp], [ApplicationId], [mAmount], [tInvoiceTransactionId], [Comment], [UserId]) ");
                cmd.Parameters.Add("@creditcomment", string.Format("InvoiceId: {0}", ctx.SessionInvoice.Id.ToString()), DbType.String);
                if (cmd.Parameters.FindIndex(delegate(SubSonic.QueryParameter match) { return (match.ParameterName == "@appId"); }) == -1)
                    cmd.AddParameter("@appId", _Config.APPLICATION_ID.ToString(), DbType.String);
                sb.AppendFormat("VALUES ('{0}', @appId, @creditNEG, @transIdx, @creditcomment, @aspnetUserId) ", now.ToString());

                //google analytics
                google.AppendFormat("{0}{1}", Utils.Constants.NewLines(2), Utils.Constants.Tabs(2));
                google.AppendFormat("pageTracker._addItem(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"); ",
                    uniqueId,
                    "StoreCredit",
                    "StoreCredit",
                    "StoreCredit",
                    neg.ToString("n2"),
                    "1");
            }
        }
        #endregion
    }
}