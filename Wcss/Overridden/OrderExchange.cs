using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;

namespace Wcss
{
    #region ExchangeListItem

    [Serializable]
    public class ExchangeListItem
    {
        private Guid _itemIdentifier;
        private int _tItemId = 0;
        private int _quantity = 1;
        private decimal _basePrice = 0;
        private decimal _service = 0;
        private decimal _lineTotal = 0;
        private _Enums.InvoiceItemContext _context;
        private string _description = string.Empty;
        private int _exchangeId = 0;
        private bool _isPackageTicket = false;

        public Guid ItemIdentifier { get { return _itemIdentifier; } }
        /// <summary>
        /// The Id of the invoice item
        /// </summary>
        public int ItemId { get { return _tItemId; } set { _tItemId = value; } }
        public int Quantity { get { return _quantity; } set { _quantity = value; } }
        public decimal BasePrice { get { return decimal.Round(_basePrice, 2); } set { _basePrice = value; } }
        public decimal Service { get { return decimal.Round(_service, 2); } set { _service = value; } }
        public decimal LineTotal { get { return decimal.Round(_lineTotal, 2); } set { _lineTotal = value; } }
        public _Enums.InvoiceItemContext Context { get { return _context; } set { _context = value; } }
        public string Description { get { return _description; } set { _description = value.Replace("'","''"); } }
        public int ExchangeId { get { return _exchangeId; } set { _exchangeId = value; } }
        public bool IsPackageTicket { get { return _isPackageTicket; } set { _isPackageTicket = value; } }

        public string DescriptionNoQuantity
        {
            get
            {
                int idx = Description.IndexOf("@");

                if (idx == -1)
                    return this.Description;

                return Description.Substring(idx + 1).Trim();
            }
        }

        /// <summary>
        /// Returns the base price and service fees for an item
        /// </summary>
        public decimal Each { get { return decimal.Round(BasePrice + Service, 2); } }

        /// <summary>
        /// Use to build object from a grid view row
        /// </summary>
        public ExchangeListItem(int exchangeId, DataKey key)
        {
            this._itemIdentifier = (Guid)key["ItemIdentifier"];
            this.ItemId = (int)key["ItemId"];
            this.Quantity = (int)key["Quantity"];
            this.BasePrice = (decimal)key["BasePrice"];
            this.Service = (decimal)key["Service"];
            this.LineTotal = (decimal)key["LineTotal"];
            this.Context = (_Enums.InvoiceItemContext)Enum.Parse(typeof(_Enums.InvoiceItemContext), key["Context"].ToString(), true);
            this.Description = key["Description"].ToString();
            this.ExchangeId = exchangeId;
            this.IsPackageTicket = (bool)key["IsPackageTicket"];
        }

        //for tickets and merch
        public ExchangeListItem(InvoiceItem ii)
        {
            this._itemIdentifier = Guid.NewGuid();
            this.ItemId = ii.Id;
            this.Quantity = ii.Quantity;

            //when the item might not contain the correct pricing....
            this.BasePrice = (ii.Price == 0 && ii.IsTicketItem && ii.ShowTicketRecord.IsPackage) ? ii.ShowTicketRecord.Price : ii.Price;
            this.Service = (ii.Price == 0 && ii.IsTicketItem && ii.ShowTicketRecord.IsPackage) ? ii.ShowTicketRecord.ServiceCharge : ii.ServiceCharge;
            this.LineTotal = (ii.Price == 0 && ii.IsTicketItem && ii.ShowTicketRecord.IsPackage) ? ii.ShowTicketRecord.PerItemPrice * ii.Quantity : ii.LineItemTotal;
            this.Context = ii.Context;

            if (ii.IsTicketItem && ii.ShowTicketRecord.IsPackage)
            {
                this.IsPackageTicket = true;

                ShowTicketCollection coll = new ShowTicketCollection();
                coll.Add(ii.ShowTicketRecord);
                coll.AddRange(ii.ShowTicketRecord.LinkedShowTickets);
                if (coll.Count > 1)
                    coll.Sort("DtDateOfShow", true);

                this.Description = string.Format("<div><b>Package Ticket</b></div>");

                foreach(ShowTicket s in coll)
                    this.Description += string.Format("<div>{0}</div>", Utils.ParseHelper.StripHtmlTags(s.DisplayNameWithAttribsAndDescription));
            }
            else
                this.Description = ii.LineItemDescription_CriteriaAndDescription(false);
        }
    }

    #endregion

    public class OrderExchange
    {
        #region Construct (SQL) Invoice, Items

        /// <summary>
        /// Cannot separately refund a service charge that is within a purchased ticket
        /// </summary>
        public static decimal ExchangeItems(Invoice invoice, GridView grid, SubSonic.QueryCommand cmd, StringBuilder inserts, 
            List<ListItem> itemsToExchange, bool issueCredits)
        {
            decimal creditsToIssue = 0;

            foreach (GridViewRow gvr in grid.Rows)
            {
                CheckBox selected = (CheckBox)gvr.FindControl("chkSelect");

                int idx = (int)grid.DataKeys[gvr.RowIndex]["ItemId"];

                if(selected.Checked)
                {
                    DropDownList exc = (DropDownList)gvr.FindControl("ddlExchange");
                    int exchangeSelection = int.Parse(exc.SelectedValue);

                    if (exchangeSelection == 0)
                        throw new Exception("Please select an item for exchange.");

                    ExchangeListItem rli = new ExchangeListItem(exchangeSelection, grid.DataKeys[gvr.RowIndex]);

                    DropDownList qty = (DropDownList)gvr.FindControl("ddlQty");
                    int quantity = int.Parse(qty.SelectedValue);

                    RadioButtonList radio = (RadioButtonList)gvr.FindControl("rdoReason");
                    string reason = radio.SelectedValue;

                    bool isDamaged = (reason.ToLower() == "damaged");

                    if (reason.Trim().ToLower() == "other")
                    {
                        TextBox other = (TextBox)gvr.FindControl("txtOther");
                        reason += string.Format("- {0}", other.Text.Trim());
                    }

                    reason = reason.Insert(0, "Reason: ");

                    InvoiceItem invoiceItem = (InvoiceItem)invoice.InvoiceItemRecords().Find(rli.ItemId);
                    if (invoiceItem == null)
                        throw new Exception("InvoiceItem could not be found");

                    cmd.Parameters.Add(string.Format("@invoiceItemId_{0}", idx), rli.ItemId, DbType.Int32);
                    cmd.Parameters.Add(string.Format("@context_{0}", idx), rli.Context.ToString());
                    cmd.Parameters.Add(string.Format("@basePrice_{0}", idx), rli.BasePrice, DbType.Decimal);
                    cmd.Parameters.Add(string.Format("@service_{0}", idx), rli.Service, DbType.Decimal);
                    cmd.Parameters.Add(string.Format("@each_{0}", idx), rli.Each, DbType.Decimal);
                    cmd.Parameters.Add(string.Format("@lineTotal_{0}", idx), rli.LineTotal, DbType.Decimal);
                    cmd.Parameters.Add(string.Format("@description_{0}", idx), rli.Description);
                    cmd.Parameters.Add(string.Format("@exchangeId_{0}", idx), rli.ExchangeId, DbType.Int32);
                    cmd.Parameters.Add(string.Format("@reason_{0}", idx), reason);
                    cmd.Parameters.Add(string.Format("@oldQuantity_{0}", idx), rli.Quantity, DbType.Int32);
                    cmd.Parameters.Add(string.Format("@newQuantity_{0}", idx), quantity, DbType.Int32);

                    //inventory updates
                    switch (rli.Context)
                    {
                        #region Ticket

                        case _Enums.InvoiceItemContext.ticket:

                            ShowTicket tikExchange = ShowTicket.FetchByID(rli.ExchangeId);

                            if (tikExchange.ShowDateRecord.ShowRecord.ApplicationId != _Config.APPLICATION_ID)
                                throw new Exception("The ticket chosen does not belong to the application");
                            
                            //oldItem - reason qty@ newItem - discard the tags in the old item
                            itemsToExchange.Add(new ListItem(string.Format("{0} - {1}", invoiceItem.LineItemDescription_CriteriaAndDescription(false), reason),
                                string.Format("{0}@ {1}", quantity, Utils.ParseHelper.StripHtmlTags(tikExchange.DisplayNameWithAttribsAndDescription))));

                            //if there have been no errors so far
                            inserts.Append("IF (LEN(@result) = 0) BEGIN ");

                            //check inventory first!!! 
                            inserts.AppendFormat("UPDATE ShowTicket SET [iSold] = [iSold] + @newQuantity_{0} ", idx);
                            inserts.AppendFormat("WHERE [Id] = @exchangeId_{0} AND [iAvailable] >= @newQuantity_{0} ", idx);

                            //continue only if available inventory
                            inserts.Append("IF (@@ROWCOUNT > 0) BEGIN ");

                            #region Setup Params

                            //exchange
                            cmd.AddParameter(string.Format("@originalTicketId_{0}", idx), invoiceItem.TShowTicketId, DbType.Int32);
                            cmd.AddParameter(string.Format("@newShowId_{0}", idx), tikExchange.TShowId, DbType.Int32);
                            cmd.AddParameter(string.Format("@shipId_{0}", idx), invoiceItem.TShipItemId, DbType.Int32);
                            cmd.AddParameter(string.Format("@purchaseName_{0}", idx), invoiceItem.PurchaseName, DbType.String);
                            cmd.AddParameter(string.Format("@newShowDate_{0}", idx), tikExchange.DateOfShow, DbType.DateTime);
                            cmd.AddParameter(string.Format("@newAgeDescription_{0}", idx), tikExchange.AgeDescription, DbType.String);
                            cmd.AddParameter(string.Format("@newMainActName_{0}", idx), tikExchange.ShowDateRecord.ShowRecord.ShowNamePart, DbType.String);
                            //hold onto the tags here
                            cmd.AddParameter(string.Format("@exchangeDisplayName_{0}", idx), 
                                tikExchange.DisplayNameWithAttribsAndDescription, DbType.String);
                            cmd.AddParameter(string.Format("@newCriteria_{0}", idx), tikExchange.CriteriaText, DbType.String);
                            cmd.AddParameter(string.Format("@newDescription_{0}", idx), tikExchange.SalesDescription, DbType.String);
                            cmd.AddParameter(string.Format("@newPrice_{0}", idx), tikExchange.Price, DbType.Decimal);
                            cmd.AddParameter(string.Format("@newService_{0}", idx), tikExchange.ServiceCharge, DbType.Decimal);
                            string oldTikNotes = string.Format("EXCHANGED TO: {0}: {1} {2} {3}", tikExchange.Id,
                                Utils.ParseHelper.StripHtmlTags(tikExchange.DisplayNameWithAttribsAndDescription), reason, invoiceItem.Notes);
                            cmd.AddParameter(string.Format("@oldNotes_{0}", idx), oldTikNotes, DbType.String);
                            string tikNotes = string.Format("EXCHANGED FROM: {0}: {1} {2} {3}", invoiceItem.TShowTicketId,
                                Utils.ParseHelper.StripHtmlTags(invoiceItem.ShowTicketRecord.DisplayNameWithAttribsAndDescription), reason, invoiceItem.Notes);
                            cmd.AddParameter(string.Format("@newNotes_{0}", idx), tikNotes, DbType.String);

                            cmd.AddParameter(string.Format("@pickup_{0}", idx), invoiceItem.PickupName, DbType.String);
                            //cmd.AddParameter(string.Format("@shipNotes_{0}", idx), invoiceItem.ShippingNotes);
                            cmd.AddParameter(string.Format("@shipMethod_{0}", idx), invoiceItem.ShippingMethod, DbType.String);

                            #endregion

                            if (quantity != rli.Quantity)
                            {
                                //create new items - leftovers, if any - copy invoice item except qty
                                inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TShowTicketId], [TShowId], [TShipItemId], [PurchaseName], ");
                                inserts.Append("[dtDateOfShow], [AgeDescription], [MainActName], [Criteria], [Description], ");
                                inserts.Append("[mPrice], [mServiceCharge], [iQuantity], [PurchaseAction], [Notes], ");
                                inserts.Append("[PickupName], [dtShipped], [ShippingNotes], [ShippingMethod], [dtStamp]) ");
                                inserts.Append("SELECT ii.[TInvoiceId], ii.[vcContext], ii.[TShowTicketId], ii.[TShowId], ii.[TShipItemId], ii.[PurchaseName], ");
                                inserts.Append("ii.[dtDateOfShow], ii.[AgeDescription], ii.[MainActName], ii.[Criteria], ii.[Description], ii.[mPrice], ii.[mServiceCharge], ");
                                inserts.AppendFormat("{0}, ", rli.Quantity - quantity);
                                inserts.Append("ii.[PurchaseAction], ii.[Notes], ii.[PickupName], ii.[dtShipped], ii.[ShippingNotes], ");
                                inserts.AppendFormat("ii.[ShippingMethod], (getDate()) FROM InvoiceItem ii WHERE ii.[Id] = @invoiceItemId_{0} ", idx);
                            }

                            decimal priceDifferenceTicket = 0;
                            if (issueCredits)
                            {
                                priceDifferenceTicket = rli.Each - (tikExchange.Price + tikExchange.ServiceCharge);//(@each_{0} - (@newPrice_{0} + @newService_{0}))
                                if (priceDifferenceTicket > 0)
                                    creditsToIssue += priceDifferenceTicket * quantity;
                            }

                            //always record below zero adjustments
                            cmd.AddParameter(string.Format("@adjustmentT_{0}", idx), 
                                (priceDifferenceTicket < 0 || ((!issueCredits) && priceDifferenceTicket > 0)) ? priceDifferenceTicket : 0, DbType.Decimal);

                            //insert exchange tickets - update inventories - verify availability - keep price same as original
                            inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TShowTicketId], [TShowId], [TShipItemId], [PurchaseName], ");
                            inserts.Append("[dtDateOfShow], [AgeDescription], [MainActName], [Criteria], [Description], ");
                            inserts.Append("[mPrice], [mServiceCharge], [mAdjustment], [iQuantity], [PurchaseAction], [Notes], ");
                            inserts.Append("[PickupName], [ShippingMethod], [dtStamp]) ");
                            inserts.AppendFormat("VALUES (@invoiceId, @context_{0}, @exchangeId_{0}, @newShowId_{0}, @shipId_{0}, @purchaseName_{0}, ", idx);
                            inserts.AppendFormat("@newShowDate_{0}, @newAgeDescription_{0}, @newMainActName_{0}, @newCriteria_{0}, @newDescription_{0}, ", idx);
                            //@each - (@newPrice_{0} + @newService_{0} equates to old PricePerItem - new PricePerItem
                            inserts.AppendFormat("@newPrice_{0}, @newService_{0}, @adjustmentT_{0}, @newQuantity_{0}, '{1}', @newNotes_{0}, ",
                                idx, _Enums.PurchaseActions.Purchased.ToString());
                            inserts.AppendFormat("@pickup_{0}, @shipMethod_{0}, (getDate()) ) ", idx);

                            //update invoice items - mark as removed, mark notes with exchange, record reason
                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{1}', [TShipItemId] = null, [iQuantity] = @newQuantity_{0}, ",
                                    idx, _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                            inserts.AppendFormat("[MainActName] = 'EXCHANGED: ' + [MainActName], [Notes] = @oldNotes_{0} WHERE [Id] = @invoiceItemId_{0} ", idx);

                            //inventory original
                            inserts.AppendFormat("UPDATE ShowTicket SET [iSold] = [iSold] - @newQuantity_{0}, ", idx);
                            inserts.AppendFormat("[iRefunded] = [iRefunded] + @newQuantity_{0} WHERE [Id] = @originalTicketId_{0} ", idx);

                            //continue branching
                            inserts.AppendFormat("END ELSE BEGIN SET @result = @result + ' Exchange Item ({1}: ' + @exchangeDisplayName_{0} + ') not available ' END ",
                                idx, tikExchange.Id);

                            inserts.Append("END ");

                            break;

                        #endregion

                        #region Merch

                        case _Enums.InvoiceItemContext.merch:

                            Merch merchExchange = Merch.FetchByID(rli.ExchangeId);
                            itemsToExchange.Add(new ListItem(string.Format("{0} - {1}", invoiceItem.LineItemDescription_CriteriaAndDescription(false), reason),
                                string.Format("{0}@ {1}", quantity, merchExchange.DisplayNameWithAttribs)));

                            inserts.Append("IF (LEN(@result) = 0) BEGIN ");

                            //check inventory first!!!
                            inserts.AppendFormat("UPDATE Merch SET [iSold] = [iSold] + {0} WHERE [Id] = {1} AND [iAvailable] >= {0} ", 
                                quantity, rli.ExchangeId);

                            //continue only if available inventory
                            inserts.Append("IF (@@ROWCOUNT > 0) BEGIN ");

                            cmd.AddParameter(string.Format("@originalMerchId_{0}", idx), invoiceItem.TMerchId, DbType.Int32);
                            cmd.AddParameter(string.Format("@shipId_{0}", idx), invoiceItem.TShipItemId, DbType.Int32);
                            cmd.AddParameter(string.Format("@purchaseName_{0}", idx), invoiceItem.PurchaseName, DbType.String);
                            cmd.AddParameter(string.Format("@newMainActName_{0}", idx), merchExchange.DisplayNameWithAttribs, DbType.String);
                            //cmd.AddParameter(string.Format("@newCriteria_{0}", idx), merchExchange.ShortText, DbType.String);
                            cmd.AddParameter(string.Format("@newCriteria_{0}", idx), null, DbType.String);
                            cmd.AddParameter(string.Format("@newDescription_{0}", idx), merchExchange.Description, DbType.String);
                            cmd.AddParameter(string.Format("@newPrice_{0}", idx), merchExchange.Price_Effective, DbType.Decimal);
                            
                            string oldMerchNotes = string.Format("EXCHANGED TO: {0}: {1} {2}", merchExchange.Id,
                                merchExchange.DisplayNameWithAttribs, reason);
                            cmd.AddParameter(string.Format("@oldNotes_{0}", idx), oldMerchNotes, DbType.String);

                            string merchNotes = string.Format("EXCHANGED FROM: {0}: {1} {2}", invoiceItem.TMerchId,
                                invoiceItem.MerchRecord.DisplayNameWithAttribs, reason);

                            cmd.AddParameter(string.Format("@newNotes_{0}", idx), merchNotes, DbType.String);

                            cmd.AddParameter(string.Format("@pickup_{0}", idx), invoiceItem.PickupName, DbType.String);
                            cmd.AddParameter(string.Format("@shipNotes_{0}", idx), invoiceItem.ShippingNotes, DbType.String);
                            cmd.AddParameter(string.Format("@shipMethod_{0}", idx), invoiceItem.ShippingMethod, DbType.String);

                            if (quantity != rli.Quantity)
                            {
                                //create new items - leftovers, if any - copy invoice item except qty
                                inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TMerchId], [TShipItemId], [PurchaseName], ");
                                inserts.Append("[MainActName], [Criteria], [Description], ");
                                inserts.Append("[mPrice], [mServiceCharge], [iQuantity], [PurchaseAction], [Notes], ");
                                inserts.Append("[PickupName], [dtShipped], [ShippingNotes], [ShippingMethod], [dtStamp]) ");
                                inserts.Append("SELECT ii.[TInvoiceId], ii.[vcContext], ii.[TMerchId], ii.[TShipItemId], ii.[PurchaseName], ");
                                inserts.Append("ii.[MainActName], ii.[Criteria], ii.[Description], ii.[mPrice], ii.[mServiceCharge], ");
                                inserts.AppendFormat("{0}, ", rli.Quantity - quantity);
                                inserts.Append("ii.[PurchaseAction], ii.[Notes], ii.[PickupName], ii.[dtShipped], ii.[ShippingNotes], ");
                                inserts.AppendFormat("ii.[ShippingMethod], (getDate()) FROM InvoiceItem ii WHERE ii.[Id] = @invoiceItemId_{0} ", idx);
                            }

                            decimal priceDifferenceMerch = 0;
                            if (issueCredits)
                            {
                                priceDifferenceMerch = rli.Each - merchExchange.Price_Effective;//@each_{0} - @newPrice_{0})
                                if (priceDifferenceMerch > 0)
                                    creditsToIssue += priceDifferenceMerch * quantity;
                            }

                            //always record below zero adjustments
                            cmd.AddParameter(string.Format("@adjustmentM_{0}", idx), 
                                (priceDifferenceMerch < 0 || ((!issueCredits) && priceDifferenceMerch > 0)) ? priceDifferenceMerch : 0, DbType.Decimal);

                            //insert exchange merch - update inventories - verify availability - keep price same as original
                            inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TMerchId], [TShipItemId], [PurchaseName], ");
                            inserts.Append("[MainActName], [Criteria], [Description], ");
                            inserts.Append("[mPrice], [mAdjustment], [iQuantity], [PurchaseAction], [Notes], ");
                            inserts.Append("[PickupName], [ShippingNotes], [ShippingMethod], [dtStamp]) ");
                            inserts.AppendFormat("VALUES (@invoiceId, @context_{0}, @exchangeId_{0}, @shipId_{0}, @purchaseName_{0}, ", idx);
                            inserts.AppendFormat("@newMainActName_{0}, @newCriteria_{0}, @newDescription_{0}, ", idx);
                            inserts.AppendFormat("@newPrice_{0}, @adjustmentM_{0}, @newQuantity_{0}, '{1}', @newNotes_{0}, ",
                                idx, _Enums.PurchaseActions.Purchased.ToString());
                            inserts.AppendFormat("@pickup_{0}, @shipNotes_{0}, @shipMethod_{0}, (getDate()) ) ", idx);

                            //update invoice items - mark as removed, mark notes with exchange, record reason
                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{1}', [TShipItemId] = null, [iQuantity] = @newQuantity_{0}, ",
                                    idx, _Enums.PurchaseActions.PurchasedThenRemoved.ToString());

                            //change context if damaged - unique to merch returns
                            if (isDamaged)
                                inserts.AppendFormat("[vcContext] = '{0}', ", _Enums.InvoiceItemContext.damaged.ToString());

                            inserts.AppendFormat("[MainActName] = 'EXCHANGED: ' + [MainActName], [Notes] = @oldNotes_{0} WHERE [Id] = @invoiceItemId_{0}; ", idx);

                            //inventory original
                            if (isDamaged)
                            {
                                //also record a history
                                inserts.Append("INSERT HistoryInventory ([dtStamp], [UserId], [tMerchId], [iCurrentlyAllotted], [dtAdjusted], ");
                                inserts.Append("[iAdjustment], [vcContext]) ");
                                inserts.Append("SELECT ((getDate())), @userId, m.[Id], m.[iAllotment], ((getDate())), ");
                                inserts.AppendFormat("(-1)*@newQuantity_{0}, N'{1}' ", idx, _Enums.HistoryInventoryContext.Damage);
                                inserts.AppendFormat("FROM [Merch] m WHERE m.[Id] = @originalMerchId_{0} ", idx);

                                //these are no longer sold and get marked as damaged - which takes them out of inventory
                                //if this happens to make inventory negative - so be it - it means there is an issue
                                inserts.AppendFormat("UPDATE Merch SET [iSold] = [iSold] - @newQuantity_{0}, ", idx);
                                inserts.AppendFormat("[iDamaged] = [iDamaged] + @newQuantity_{0} WHERE [Id] = @originalMerchId_{0} ", idx);
                            }
                            else
                            {
                                //these are no longer sold and go back into inventory
                                inserts.AppendFormat("UPDATE Merch SET [iSold] = [iSold] - @newQuantity_{0} ", idx);
                                inserts.AppendFormat("WHERE [Id] = @originalMerchId_{0} ", idx);
                            }

                            //continue branching
                            inserts.AppendFormat("END ELSE BEGIN SET @result = @result + ' Exchange Item ({1}: ' + @newMainActName_{0} + ') not available ' END ",
                                idx, merchExchange.Id);

                            inserts.Append("END ");

                            break;

                        #endregion
                    }
                }
            }

            return creditsToIssue;
        }

        #endregion

        #region Exchange Processes

        public static string DoExchange(System.Web.Profile.ProfileBase userProfile, Invoice invoice, GridView grid, string creatorName, string userIp, bool issueStoreCreditForDifference)
        {
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            cmd.AddParameter("@appId", invoice.ApplicationId, DbType.Guid);
            cmd.AddParameter("@invoiceId", invoice.Id, DbType.Int32);

            List<ListItem> itemsToExchange = new List<ListItem>();
            System.Text.StringBuilder inserts = new System.Text.StringBuilder();//this will hold sql to perform after a successful exchange

            inserts.Append("DECLARE @result varchar(2000); SET @result = ''; ");
            inserts.AppendFormat("DECLARE @userId uniqueidentifier; SELECT @userId = u.[UserId] FROM [Aspnet_Users] u WHERE u.[ApplicationId] = @appId AND u.[LoweredUserName] = N'{0}'; ",
                System.Web.HttpContext.Current.User.Identity.Name);
           
            //gather info
            decimal creditsToIssue = ExchangeItems(invoice, grid, cmd, inserts, itemsToExchange, issueStoreCreditForDifference);

            inserts.Append("SELECT @result; ");

            //check to see if our results are good - then do other logging chores
            cmd.CommandSql = inserts.ToString();
            string result = SubSonic.DataService.ExecuteScalar(cmd).ToString();

            if (result.Trim().Length > 0)
                throw new Exception(result);

            //be sure to include a transaction
            inserts.Length = 0;
            cmd.Parameters.Clear();


            if (creditsToIssue > 0 && issueStoreCreditForDifference)
            {
                //@invo--iceId,@customerId,@userIp
                //insert a new store credit
                //create a trans for store credit - get the trans id to record for redemption
                inserts.Append("DECLARE @transIdx int; ");
                inserts.Append("INSERT [InvoiceTransaction] ([tInvoiceId], [UserId], [PerformedBy], [CustomerId], [TransType], ");
                inserts.Append("[FundsType], [FundsProcessor], [ProcessorId], [mAmount], ");
                inserts.Append("[UserIp], [dtStamp]) ");

                cmd.Parameters.Add("@appId", invoice.ApplicationId, DbType.Guid);
                cmd.Parameters.Add("@invoiceId", invoice.Id, DbType.Int32);
                cmd.Parameters.Add("@usrId", invoice.UserId, DbType.Guid);
                cmd.Parameters.Add("@customerId", invoice.CustomerId, DbType.Int32);
                cmd.Parameters.Add("@userIP", userIp, DbType.String);
                cmd.Parameters.Add("@amountToRefundCredit", creditsToIssue, DbType.Decimal);

                //invoiceId, userId, performer, custId, transaction type
                inserts.AppendFormat("VALUES (@invoiceId, @usrId, '{0}', @customerId, '{1}', ", 
                    _Enums.PerformedByTypes.AdminSite.ToString(), _Enums.TransTypes.Refund.ToString());
                //funds, processor, procId, amount, nameonCard
                inserts.AppendFormat("'{0}', '{1}', '111', @amountToRefundCredit, ", 
                    _Enums.FundsTypes.StoreCredit.ToString(), _Enums.FundsProcessor.Internal.ToString());
                //last4 digits, ip, dtStamp
                inserts.AppendFormat("@userIp, '{0}') ", DateTime.Now.ToString());

                inserts.AppendFormat("SET @transIdx = SCOPE_IDENTITY(); ");                    

                //create a redemption for the store credit
                inserts.Append("INSERT [StoreCredit] ([dtStamp], [ApplicationId], [mAmount], [tInvoiceTransactionId], [Comment], [UserId]) ");
                cmd.Parameters.Add("@creditcomment", string.Format("Refund to InvoiceId: {0}", invoice.Id.ToString()), DbType.String);
                inserts.AppendFormat("VALUES ('{0}', @appId, @amountToRefundCredit, @transIdx, @creditcomment, @usrId) ", DateTime.Now.ToString());
            }
            else
                DoExchangeTransaction(inserts, invoice, "exchange", creatorName, _Enums.FundsTypes.StoreCredit,
                    _Enums.FundsProcessor.Internal, 0, userIp);

            cmd.CommandSql = inserts.ToString();
            SubSonic.DataService.ExecuteScalar(cmd);
    
            //Exchange event
            string oldItems = string.Empty;
            string newItems = string.Empty;
            foreach (ListItem li in itemsToExchange)
            {
                oldItems += string.Format("{0}~", li.Text);
                newItems += string.Format("{0}~", li.Value);
            }

            if (oldItems.Length > 500)
                oldItems = oldItems.Substring(0,490);

            if (newItems.Length > 2000)
                newItems = newItems.Substring(0, 1990);

            string userName = invoice.AspnetUserRecord.UserName;

            InvoiceEvent.NewInvoiceEvent(invoice.Id, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                creatorName, invoice.UserId, userName,
                _Enums.EventQContext.Invoice, _Enums.EventQVerb.Exchange, oldItems.TrimEnd('~'), "ITEMS EXCHANGED", newItems.TrimEnd('~'), true);
            
            //send an email notification
            MailQueue.SendExchangeInformation(invoice, itemsToExchange);

            
            //IF NECESSARY - UPDATE THE PROFILE
            if (creditsToIssue > 0 && issueStoreCreditForDifference)
            {
                string userCredit = userProfile.GetPropertyValue("StoreCredit").ToString();
                if (userCredit == null || userCredit.Trim().Length == 0)
                    userCredit = "0";

                decimal credit = decimal.Parse(userCredit);
                float newCredit = (float)(credit + creditsToIssue);
                userProfile.SetPropertyValue("StoreCredit", newCredit);
                userProfile.Save();

                UserEvent.RecordStoreCreditEvent(creatorName, invoice.UserId, userName, creditsToIssue, credit + creditsToIssue,
                    "exchange amount refunded to store credit", invoice);

                return "SUCCESS~SYNCUSER";
            }

            //return a friendly message
            return "SUCCESS";
        }

        #endregion

        #region Exchange Transaction

        private static void DoExchangeTransaction(StringBuilder inserts, Invoice invoice, string processorId, string creatorName, 
            _Enums.FundsTypes fundsType, _Enums.FundsProcessor fundsProcessor, decimal amountToRefund, string userIp)
        {
            //issue a transaction paid by check
            inserts.Append("INSERT INTO InvoiceTransaction ([ProcessorId],[TInvoiceId],[PerformedBy],[Admin],[UserId],[CustomerId],[TransType],");
            inserts.Append("[FundsType],[FundsProcessor],[mAmount],[UserIp],[dtStamp]) ");
            inserts.AppendFormat("VALUES (0,{0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}',{8},'{9}','{10}') ",
                invoice.Id,
                _Enums.PerformedByTypes.AdminSite.ToString(),
                creatorName,
                invoice.UserId.ToString(),
                invoice.CustomerId,
                _Enums.TransTypes.Refund.ToString(),
                fundsType.ToString(),
                fundsProcessor.ToString(),
                amountToRefund,
                userIp,
                DateTime.Now.ToString());
        }

        #endregion
    }
}
