using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Data;



/**************************************************************************
 * REFUNDING NOTES *
 *
 * We can do a test trans thru auth net on a transaction that was
 * originated in production - but this will not work for dev transactions.
 * It will not be able to find the correct trans id.
 * 
*************************************************************************/

namespace Wcss
{
    #region RefundListItem

    [Serializable]
    public class RefundListItem
    {
        private Guid _itemIdentifier;
        private int _tItemId = 0;
        private int _quantity = 1;
        private decimal _basePrice = 0;
        private decimal _service = 0;
        private decimal _lineTotal = 0;
        private _Enums.InvoiceItemContext _context;
        private string _description = string.Empty;
        private int _salePromotionId = 0;
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
        public string Description { get { return _description; } set { _description = value; } }
        public int SalePromotionId { get { return _salePromotionId; } set { _salePromotionId = value; } }
        public bool IsPromotionItem { get { return _salePromotionId > 0; } }
        public bool IsPackageTicket { get { return _isPackageTicket; } set { _isPackageTicket = value; } }

        public string DescriptionNoQuantity
        {
            get
            {
                int idx = Description.IndexOf("@");
                
                if (idx == -1)
                    return this.Description;

                return Description.Substring(idx+1).Trim();
            } 
        }

        /// <summary>
        /// Returns the base price and service fees for an item
        /// </summary>
        public decimal Each { get { return decimal.Round(BasePrice + Service, 2); } }

        /// <summary>
        /// Use to build object from a grid view row
        /// </summary>
        public RefundListItem(DataKey key)
        {
            this._itemIdentifier = (Guid)key["ItemIdentifier"];
            this.ItemId = (int)key["ItemId"];
            this.Quantity = (int)key["Quantity"];
            this.BasePrice = (decimal)key["BasePrice"];
            this.Service = (decimal)key["Service"];
            this.LineTotal = (decimal)key["LineTotal"];
            this.Context = (_Enums.InvoiceItemContext)Enum.Parse(typeof(_Enums.InvoiceItemContext), key["Context"].ToString(), true);
            this.Description = key["Description"].ToString();
            this.SalePromotionId = (int)key["SalePromotionId"];
            this.IsPackageTicket = (bool)key["IsPackageTicket"];
        }

        //for tickets and merch
        public RefundListItem(InvoiceItem ii)
        {
            this._itemIdentifier = Guid.NewGuid();
            this.ItemId = ii.Id;
            this.Quantity = ii.Quantity;

            this.BasePrice = (ii.Price == 0 && ii.IsTicketItem && ii.ShowTicketRecord.IsPackage) ? ii.ShowTicketRecord.Price : ii.Price;
            this.Service = (ii.Price == 0 && ii.IsTicketItem && ii.ShowTicketRecord.IsPackage) ? ii.ShowTicketRecord.ServiceCharge : ii.ServiceCharge;
            this.LineTotal = (ii.Price == 0 && ii.IsTicketItem && ii.ShowTicketRecord.IsPackage) ? ii.ShowTicketRecord.PerItemPrice * ii.Quantity : ii.LineItemTotal;

            //this.BasePrice = ii.Price;
            //this.Service = ii.ServiceCharge;
            //this.LineTotal = ii.LineItemTotal;
            this.Context = ii.Context;
            this.SalePromotionId = (ii.TSalePromotionId.HasValue) ? ii.TSalePromotionId.Value : 0;

            if (ii.IsTicketItem && ii.ShowTicketRecord.IsPackage)
            {
                this.IsPackageTicket = true;

                ShowTicketCollection coll = new ShowTicketCollection();
                coll.Add(ii.ShowTicketRecord);
                coll.AddRange(ii.ShowTicketRecord.LinkedShowTickets);
                if (coll.Count > 1)
                    coll.Sort("DtDateOfShow", true);

                this.Description = string.Format("<div><b>Package Ticket</b></div>");

                foreach (ShowTicket s in coll)
                {
                    this.Description += string.Format("<div>{0}</div>", 
                        Utils.ParseHelper.StripHtmlTags(s.DisplayNameWithAttribsAndDescription));
                }
            }
            else
            {
                this.Description = ii.LineItemDescription_CriteriaAndDescription(false);
            }

            if (ii.DateShipped < DateTime.Now)
                this.Description += string.Format("<div>*** shipped on *** {0}</div>", ii.DateShipped.ToString("MM/dd/yyyy hh:mmtt"));
        }
    }

    #endregion

    public class OrderRefund
    {
        #region Refund Processes

        /// <summary>
        /// called from mass refunds in static methods
        /// </summary>
        public static string DoRefund(string proc, Invoice invoice,
            GridView grid, string discountDescription, string creatorName, string userIp, string emailLink)
        {
            return DoRefund(proc, invoice, grid, string.Empty, 0, discountDescription, creatorName, userIp, emailLink);
        }
        /// <summary>
        /// singular refund processing. When using check for existience of SYNCUSER in the return string. This will indiacte whether or not to 
        /// update the user to sync store credit
        /// </summary>
        public static string DoRefund(string processor, Invoice invoice,
            GridView grid, string checkNumber, decimal discountAmount, string discountDescription, string creatorName, string userIp, string emailLink)
        {
            //validate inputs
            if (processor == "Check" && checkNumber.Trim().Length == 0)
                throw new Exception("Please enter a check number for the refund.");
            if(discountAmount > 0 && discountDescription.Trim().Length == 0)
                throw new Exception("Discounts must include a description.");


            AuthorizeNet authorize;
            decimal amountToRefundTotal = 0;
            decimal amountToRefundCredit = 0;
            decimal amountToRefundAuthNet = 0;
            string checkText = null;

            //The refund choices
            //"CreditAndAuthNet", "AuthNet", "StoreCredit", "Check"

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@invoiceId", invoice.Id, DbType.Int32);
            cmd.Parameters.Add("@customerId", invoice.CustomerId, DbType.Int32);
            cmd.Parameters.Add("@userIp", invoice.CustomerId, DbType.String);

            System.Text.StringBuilder desc = new System.Text.StringBuilder();//this will hold description of items to refund - for auth net only
            System.Text.StringBuilder inserts = new System.Text.StringBuilder();//this will hold sql to perform after a successful refund            

            List<ListItem> itemsToRefund = new List<ListItem>();

            //
            #region VALIDATE THE REFUND PROCESS
            

            //be sure to use a positive amount for discounts
            amountToRefundTotal = (discountAmount > 0) ? Math.Abs(discountAmount) : GetInvoiceRefundables(invoice, grid, desc, cmd, inserts, itemsToRefund);
            if(amountToRefundTotal <= 0)
                throw new Exception("Please specify an amount to discount - or select items to discount.");
            
            //assign credit and authnet amounts
            switch(processor)
            {
                case "CreditAndAuthNet":
                    decimal amountPaidInCredit = invoice.StoreCreditPaymentsTotal;
                    
                    //if the amount paid with credit is greater than the refund requested,
                    //limit the refund to store credit in the amount requested
                    if(amountPaidInCredit > amountToRefundTotal)
                        amountToRefundCredit = amountToRefundTotal;
                    else
                    {
                        amountToRefundCredit = amountPaidInCredit;
                        amountToRefundAuthNet = amountToRefundTotal - amountPaidInCredit;                        
                    }
                    break;
                case "AuthNet":
                    amountToRefundAuthNet = amountToRefundTotal;
                     break;
                case "StoreCredit":
                case "Check":
                    amountToRefundCredit = amountToRefundTotal;
                    break;
            }

            //further validation
            if (amountToRefundTotal > invoice.NetPaid)
                throw new Exception("The amount entered and/or total of items to refund exceeeds the amount paid for this invoice.");
            //if order is todays order (within 24 hours) and method is authnet and the entire amount is not being refunded - no partials
            if (amountToRefundAuthNet > 0)
            {
                if(invoice.InvoiceDate.Date.AddDays(1) > DateTime.Now.Date && amountToRefundAuthNet != invoice.CreditCardPaymentsTotal)
                    throw new Exception("You must wait one day from the invoice date to do partial refunds on a credit card. Payment batch needs to have been submitted before a partial refund can be processed. Batches are generally processed just after midnight.");

                if(invoice.InvoiceDate.AddDays(118) < DateTime.Now)//use 118 to avoid discrepancies in time
                    throw new Exception("Invoice is past 120 days and cannot be refunded here. You will need to issue a refund amount below via company check or store credit(tba).");

                if(!invoice.CashewRecord.IsExpiryValid)
                    throw new Exception("The credit card used for this invoice has expired. You will need to issue a refund amount below via company check or store credit(tba).");
            }


            //set up description
            if (discountDescription != null && discountDescription.Trim().Length > 0)
                desc.Insert(0, string.Format("Reason for Refund/Credit : {0}~", discountDescription.Trim()));
            if (amountToRefundCredit > 0)
            {
                if (processor == "Check")
                {
                    checkText = string.Format("CheckNum: {0}", checkNumber.Replace("'", ""));
                    desc.Insert(0, string.Format("Check Refund: {0}~{1}~", amountToRefundCredit.ToString("c"), checkText));
                }
                else
                    desc.Insert(0, string.Format("Refund StoreCredit: {0}~", amountToRefundCredit.ToString("c")));
            }
            if (amountToRefundAuthNet > 0)
                desc.Insert(0, string.Format("Refund AuthNet: {0}~", amountToRefundAuthNet.ToString("c")));
            

            #endregion

            //update the invoice
            if (amountToRefundTotal > 0)
            {
                inserts.AppendFormat("UPDATE Invoice SET [mTotalRefunds] = [mTotalRefunds] + {0}, ", amountToRefundTotal);
                inserts.AppendFormat("[InvoiceStatus] = '{0}' ", (invoice.NetPaid == amountToRefundTotal) ?
                    _Enums.InvoiceStatii.Refunded.ToString() : _Enums.InvoiceStatii.PartiallyRefunded.ToString());
                inserts.Append("WHERE [Id] = @invoiceId; ");
            }


            //perform auth net refunds
            if (amountToRefundAuthNet > 0)
            {
                //do auth net refund
                authorize = AuthorizeNetRefund(invoice, creatorName, amountToRefundAuthNet, desc.ToString(), userIp);

                if (!authorize.IsAuthorized)
                {
                    desc.Length = 0;
                    inserts.Length = 0;

                    if (authorize.ResponseReasonText.ToLower().IndexOf("the referenced transaction does not meet the criteria for issuing a credit.") != -1)
                    {
                        authorize.ResponseReasonText += string.Format(" Reasons for this could include that a partial refund cannot be issued within 24 hours of original purchase.");
                    }

                    throw new Exception(string.Format("This transaction cannot be authorized: {0}", authorize.ResponseReasonText));
                }

                //insert transaction row
                DoRefundTransaction(inserts, invoice, authorize.ProcessorId, creatorName, _Enums.FundsTypes.CreditCard,
                    _Enums.FundsProcessor.AuthorizeNet, amountToRefundAuthNet, userIp);
            }

            //issue any credit refunds
            if (amountToRefundCredit > 0)
            {
                if (processor == "Check")
                {
                    DoRefundTransaction(inserts, invoice, checkText, creatorName,
                        _Enums.FundsTypes.CompanyCheck, _Enums.FundsProcessor.Internal, amountToRefundCredit, userIp);
                }
                else
                {
                    //!!!!!!!!!!!!!!!!!!!!
                    //THIS NEEDS TO BE LONGHAND AS WE NEED THE IDENTITY TO TIE TO THE STORE CREDIT
                    //!!!!!!!!!!!!!!!!!!!!

                    //insert a new store credit
                    //create a trans for store credit - get the trans id to record for redemption
                    inserts.Append("DECLARE @transIdx int; ");
                    inserts.Append("INSERT [InvoiceTransaction] ([tInvoiceId], [UserId], [PerformedBy], [CustomerId], [TransType], ");
                    inserts.Append("[FundsType], [FundsProcessor], [ProcessorId], [mAmount], ");
                    inserts.Append("[UserIp], [dtStamp]) ");

                    cmd.Parameters.Add("@appId", invoice.ApplicationId, DbType.Guid);
                    cmd.Parameters.Add("@usrId", invoice.UserId, DbType.Guid);
                    cmd.Parameters.Add("@amountToRefundCredit", Math.Abs(amountToRefundCredit), DbType.Decimal);

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
            }

            //if we are issuing a discount - add a discount invoiceitem to the invoice
            //itemized items refunded via auth net will show refunds from grid - so we can ignore those
            string note = string.Empty;
            if (amountToRefundCredit > 0)
            {
                if (checkText != null)
                    note = string.Format("Check Refund: {0} {1}", amountToRefundCredit.ToString("c"), checkText);
                else
                    note = string.Format("Credit Issued: {0}", amountToRefundCredit.ToString("c"));

                note.Insert(0, DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"));

                inserts.Append("INSERT [InvoiceItem] ([TInvoiceId], [PurchaseAction], [vcContext], [MainActName], [mPrice], [iQuantity]) ");
                inserts.AppendFormat("VALUES (@invoiceId, '{0}', '{1}', '{2}', 0, 1) ",
                    _Enums.PurchaseActions.Purchased.ToString(), _Enums.InvoiceItemContext.noteitem.ToString(),
                    note);
            }

            if (amountToRefundAuthNet > 0)
            {
                note = string.Format("{0} CreditCard Refund: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mmtt"), amountToRefundAuthNet.ToString("c"));

                inserts.Append("INSERT [InvoiceItem] ([TInvoiceId], [PurchaseAction], [vcContext], [MainActName], [mPrice], [iQuantity]) ");
                inserts.AppendFormat("VALUES (@invoiceId, '{0}', '{1}', '{2}', 0, 1) ",
                    _Enums.PurchaseActions.Purchased.ToString(), _Enums.InvoiceItemContext.noteitem.ToString(),
                    note);
            }
            
            //RUN THE PROCS
            //if all goes well - run the procs
            cmd.CommandSql = inserts.ToString();
            SubSonic.DataService.ExecuteScalar(cmd);

            string returnVal = "SUCCESS";


            string description = desc.ToString().Trim();
            if (description.Length > 1500)
                description = description.Remove(1499); 


            //Record events
            if (amountToRefundAuthNet > 0)
            {
                InvoiceEvent.NewInvoiceEvent(invoice.Id, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                    creatorName, invoice.UserId, invoice.AspnetUserRecord.UserName,
                    _Enums.EventQContext.Invoice, _Enums.EventQVerb.Refund,
                    null, description, string.Format("AuthNet Refund: {0}", amountToRefundAuthNet.ToString("c")), true);
            }
            if (amountToRefundCredit > 0)
            {
                if (processor == "Check")
                {
                    InvoiceEvent.NewInvoiceEvent(invoice.Id, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                        creatorName, invoice.UserId, invoice.AspnetUserRecord.UserName,
                        _Enums.EventQContext.Invoice, _Enums.EventQVerb.Refund,
                        null, description, string.Format("Check Refund: {0}", amountToRefundCredit.ToString("c")), true);
                }
                else
                {
                    //tell caller to sync user profile
                    returnVal += "~SYNCUSER";

                    InvoiceEvent.NewInvoiceEvent(invoice.Id, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                        creatorName, invoice.UserId, invoice.AspnetUserRecord.UserName,
                        _Enums.EventQContext.Invoice, _Enums.EventQVerb.Refund,
                        null, description, string.Format("Store Credit Issued: {0}", amountToRefundCredit.ToString("c")), true);
                }
            }

            //SEND EMAIL NOTIFICATION
            MailQueue.SendRefundInformation(invoice, itemsToRefund, amountToRefundCredit, amountToRefundAuthNet, discountAmount, discountDescription, (processor == "Check"), emailLink);

            //return a friendly message
            return returnVal;
        }

        #endregion

        #region Construct (SQL) Invoice, Items

        /// <summary>
        /// Cannot separately refund a service charge that is within a purchased ticket
        /// </summary>
        public static decimal GetInvoiceRefundables(Invoice invoice, GridView grid,
            StringBuilder desc, SubSonic.QueryCommand cmd, StringBuilder inserts, List<ListItem> itemsToRefund)
        {
            decimal amountToRefund = 0;
            int idx = 0;

            foreach (GridViewRow gvr in grid.Rows)
            {
                CheckBox selected = (CheckBox)gvr.FindControl("chkSelect");
                CheckBox service = (CheckBox)gvr.FindControl("chkService");
                
                RefundListItem rli = new RefundListItem(grid.DataKeys[gvr.RowIndex]);

                //if the row has been marked for refund and we are able to construct a refund item from the row information....
                if(selected.Checked && rli != null)
                {
                    #region Establish Parameters

                    InvoiceItem invoiceItem = (InvoiceItem)invoice.InvoiceItemRecords().Find(rli.ItemId);
                    
                    bool isPackage_BaseTicketItem = (rli.IsPackageTicket);// && invoiceItem.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower());

                    if (invoiceItem == null)
                        throw new Exception("Matching invoice item cannot be found.");

                    DropDownList qty = (DropDownList)gvr.FindControl("ddlQty");
                    int quantity = int.Parse(qty.SelectedValue);

                    //////////////////////////////////////
                    //determine service fees later on in this method!!!!!!!!!!!!!!!!!
                    //////////////////////////////////////

                    string lineDescription = string.Empty; 
                    decimal lineRefund = rli.BasePrice * quantity;
                    amountToRefund += lineRefund;

                    if (invoiceItem.TShowTicketId.HasValue)
                        cmd.Parameters.Add(string.Format("@showTicketId_{0}", idx), invoiceItem.TShowTicketId, DbType.Int32);
                    if (invoiceItem.TMerchId.HasValue)
                        cmd.Parameters.Add(string.Format("@merchId_{0}", idx), invoiceItem.TMerchId, DbType.Int32);
                    if (invoiceItem.TShowId.HasValue)
                        cmd.Parameters.Add(string.Format("@showId_{0}", idx), invoiceItem.TShowId, DbType.Int32);
                    if (invoiceItem.DtDateOfShow.HasValue)
                        cmd.Parameters.Add(string.Format("@showDate_{0}", idx), invoiceItem.DateOfShow, DbType.DateTime);
                    if (invoiceItem.AgeDescription != null && invoiceItem.AgeDescription.Trim().Length > 0)
                        cmd.Parameters.Add(string.Format("@ageDescription_{0}", idx), invoiceItem.AgeDescription);

                    cmd.Parameters.Add(string.Format("@mainActName_{0}", idx), invoiceItem.MainActName);
                    cmd.Parameters.Add(string.Format("@invoiceItemId_{0}", idx), rli.ItemId, DbType.Int32);
                    cmd.Parameters.Add(string.Format("@context_{0}", idx), rli.Context.ToString());

                    //useDescription is an html formatted description
                    string useDescription = rli.DescriptionNoQuantity.Replace("</div><div>", "~").Replace("<div>", string.Empty).Replace("</div>", string.Empty);
                    cmd.Parameters.Add(string.Format("@description_{0}", idx), useDescription);

                    cmd.Parameters.Add(string.Format("@basePrice_{0}", idx), rli.BasePrice, DbType.Decimal);
                    cmd.Parameters.Add(string.Format("@service_{0}", idx), rli.Service, DbType.Decimal);
                    cmd.Parameters.Add(string.Format("@newQuantity_{0}", idx), quantity, DbType.Int32);
                        
                    //TODO: determine if necessary
                    //if (invoiceItem.TShipItemId.HasValue)
                    //    cmd.Parameters.Add(string.Format("@shipItemId_{0}", idx), invoiceItem.TShipItemId, DbType.Int32);
                    //if (invoiceItem.TSalePromotionId.HasValue)
                    //    cmd.Parameters.Add(string.Format("@salePromotionId_{0}", idx), invoiceItem.TSalePromotionId, DbType.Int32);

                    #endregion End Params

                    switch (rli.Context)
                    {
                        #region Service Charges

                        case _Enums.InvoiceItemContext.servicecharge:
                            lineDescription = string.Format("{0} @ {1}~", quantity.ToString(), rli.DescriptionNoQuantity);

                            //determine quantity to refund - if the qty does not match the maxQty - be sure to adjust item correctly
                            if (quantity != rli.Quantity)
                            {
                                //create leftover items
                                inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TShowTicketId], [TShowId], [dtDateOfShow], ");
                                inserts.Append("[MainActName], [mPrice], [iQuantity], [PurchaseAction], [dtStamp]) ");
                                inserts.AppendFormat("VALUES (@invoiceId, @context_{0}, @showTicketId_{0}, @showId_{0}, @showDate_{0}, ", idx);
                                inserts.AppendFormat("@description_{0}, @basePrice_{0}, {1}, '{2}', (getDate()) ); ", 
                                    idx, (rli.Quantity - quantity), _Enums.PurchaseActions.Purchased.ToString());
                            }

                            //update existing item
                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}', ", _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                            inserts.AppendFormat("[iQuantity] = @newQuantity_{0} WHERE [Id] = @invoiceItemId_{0}; ", idx);
                            
                            break;

                        #endregion

                        #region Merchandise

                        case _Enums.InvoiceItemContext.merch:
                            lineDescription = string.Format("{0} @ {1}~", quantity.ToString(), rli.DescriptionNoQuantity);

                            //determine quantity to refund - if the qty does not match the maxQty - be sure to adjust item correctly
                            if (quantity != rli.Quantity)
                            {
                                //create leftover items
                                inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TMerchId], [PurchaseName], ");
                                inserts.Append("[dtDateOfShow], [AgeDescription], [MainActName], [Criteria], [Description], ");
                                inserts.Append("[mPrice], [mServiceCharge], [iQuantity], [PurchaseAction], [Notes], ");
                                inserts.Append("[PickupName], [dtShipped], [ShippingNotes], [ShippingMethod], [dtStamp]) ");
                                inserts.Append("SELECT ii.[tInvoiceId], ii.[vcContext], ii.[TMerchId], ii.[PurchaseName], ");
                                inserts.Append("ii.[dtDateOfShow], ii.[AgeDescription], ii.[MainActName], ii.[Criteria], ii.[Description], ");
                                inserts.AppendFormat("ii.[mPrice], ii.[mServiceCharge], {0}, ii.[PurchaseAction], ", (rli.Quantity - quantity));
                                inserts.Append("ii.[Notes], ii.[PickupName], ii.[dtShipped], ii.[ShippingNotes], ");
                                inserts.AppendFormat("ii.[ShippingMethod], (getDate()) FROM InvoiceItem ii WHERE ii.[Id] = @invoiceItemId_{0}; ", 
                                    idx);
                            }

                            //update existing item
                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}', ", _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                            inserts.AppendFormat("[iQuantity] = @newQuantity_{0} WHERE [Id] = @invoiceItemId_{0}; ", idx);
                            
                            //inventory
                            inserts.AppendFormat("UPDATE Merch SET [iSold] = [iSold] - @newQuantity_{0}, ", idx);
                            inserts.AppendFormat("[iRefunded] = [iRefunded] + @newQuantity_{0} WHERE [Id] = @merchId_{0}; ", idx);

                            break;

                        #endregion

                        #region Tickets

                        case _Enums.InvoiceItemContext.ticket:

                            //for packages - we need to create some more variables
                            //we cannot overlap on the indexes!
                            int j = 10000 + idx * 1500;//this should be enough of an offset to keep us safe

                            #region Leftover Items

                            //create leftover items  not being refunded (make a copy with a different qty that is purchased
                            if (quantity != rli.Quantity)
                            {
                                int diffQty = (rli.Quantity - quantity);

                                inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TShowTicketId], [TShowId], [PurchaseName], ");
                                inserts.Append("[dtDateOfShow], [AgeDescription], [MainActName], [Criteria], [Description], ");
                                inserts.Append("[mPrice], [mServiceCharge], [iQuantity], [PurchaseAction], [Notes], ");
                                inserts.Append("[PickupName], [dtShipped], [ShippingNotes], [ShippingMethod], [dtStamp]) ");

                                inserts.Append("SELECT ii.[tInvoiceId], ii.[vcContext], ii.[TShowTicketId], ii.[TShowId], ii.[PurchaseName], ");
                                inserts.Append("ii.[dtDateOfShow], ii.[AgeDescription], ii.[MainActName], ii.[Criteria], ii.[Description], ");
                                inserts.AppendFormat("ii.[mPrice], ii.[mServiceCharge], {0}, ii.[PurchaseAction], ", diffQty);
                                inserts.Append("ii.[Notes], ii.[PickupName], ii.[dtShipped], ii.[ShippingNotes], ");
                                inserts.AppendFormat("ii.[ShippingMethod], (getDate()) FROM InvoiceItem ii WHERE ii.[Id] = @invoiceItemId_{0}; ", idx);


                                //if it is a package - only the base item has been inserted...
                                //we need to insert other package tix
                                if (isPackage_BaseTicketItem)
                                {   
                                    foreach (ShowTicket s in invoiceItem.ShowTicketRecord.LinkedShowTickets)
                                    {
                                        InvoiceItem packageTicket = invoice.InvoiceItemRecords().GetList().Find(delegate(InvoiceItem match)
                                        {
                                            return (match.TShowTicketId == s.Id &&
                                            match.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower());
                                        });

                                        if (packageTicket != null)
                                        {
                                            cmd.Parameters.Add(string.Format("@invoiceItemId_{0}", j), packageTicket.Id, DbType.Int32);

                                            inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [TShowTicketId], [TShowId], [PurchaseName], ");
                                            inserts.Append("[dtDateOfShow], [AgeDescription], [MainActName], [Criteria], [Description], ");
                                            inserts.Append("[mPrice], [mServiceCharge], [iQuantity], [PurchaseAction], [Notes], ");
                                            inserts.Append("[PickupName], [dtShipped], [ShippingNotes], [ShippingMethod], [dtStamp]) ");

                                            inserts.Append("SELECT ii.[tInvoiceId], ii.[vcContext], ii.[TShowTicketId], ii.[TShowId], ii.[PurchaseName], ");
                                            inserts.Append("ii.[dtDateOfShow], ii.[AgeDescription], ii.[MainActName], ii.[Criteria], ii.[Description], ");
                                            inserts.AppendFormat("ii.[mPrice], ii.[mServiceCharge], {0}, ii.[PurchaseAction], ", diffQty);
                                            inserts.Append("ii.[Notes], ii.[PickupName], ii.[dtShipped], ii.[ShippingNotes], ");
                                            inserts.AppendFormat("ii.[ShippingMethod], (getDate()) FROM InvoiceItem ii WHERE ii.[Id] = @invoiceItemId_{0}; ", j);
                                        }

                                        j++;
                                    }
                                }
                            }
                            //end of leftovers
                            #endregion

                            //2 flows to handle - 1 with servce fees included - the other without
                            //service Charges included
                            if (service.Checked)
                            {
                                //update existing items - new quantity = quantity selected
                                inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}', ", _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                                inserts.AppendFormat("[iQuantity] = @newQuantity_{0} WHERE [Id] = @invoiceItemId_{0}; ", idx);

                                //update the description
                                lineDescription = string.Format("{0} @ {1} (svc fees incl)~", quantity.ToString(), rli.DescriptionNoQuantity);
                                lineRefund += rli.Service * quantity;//tack on the service fee

                                ///////////////////////////////////////////////
                                //update the amount to refund
                                ///////////////////////////////////////////////
                                amountToRefund += rli.Service * quantity;//tack on the service fee

                                //Dont bother checking for the base item
                                if (isPackage_BaseTicketItem)
                                {
                                    foreach (ShowTicket s in invoiceItem.ShowTicketRecord.LinkedShowTickets)
                                    {
                                        InvoiceItem packageTicket = invoice.InvoiceItemRecords().GetList().Find(delegate(InvoiceItem match)
                                        {
                                            return (match.TShowTicketId == s.Id &&
                                                match.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower());
                                        });

                                        if (packageTicket != null)
                                        {
                                            cmd.Parameters.Add(string.Format("@invoiceItemId_{0}", j), packageTicket.Id, DbType.Int32);

                                            //update existing items
                                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}', ", _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                                            inserts.AppendFormat("[iQuantity] = @newQuantity_{0} WHERE [Id] = @invoiceItemId_{1}; ", idx, j);

                                            //update the description
                                            lineDescription = string.Format("{0} @ {1} (svc fees incl)~", quantity.ToString(), rli.DescriptionNoQuantity);
                                            //lineRefund += (invoiceItem.Price + invoiceItem.Adjustment) * quantity;//tack on the service fee

                                            ///////////////////////////////////////////////
                                            //update the amount to refund - service fee handled in the base item!!! do not update here
                                            ///////////////////////////////////////////////
                                        }

                                        j++;
                                    }
                                }
                            }
                            else
                            {
                                //update existing items
                                inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}', ", 
                                    _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                                inserts.AppendFormat("[iQuantity] = @newQuantity_{0}, [mServiceCharge] = 0, ", idx);
                                inserts.Append("[MainActName] = [MainActName] + ' (svc fees not incl)' ");
                                inserts.AppendFormat("WHERE [Id] = @invoiceItemId_{0}; ", idx);

                                //insert service charge items
                                inserts.Append("INSERT INTO InvoiceItem ([tInvoiceId], [vcContext], [tShowTicketId], [tShowId], ");
                                inserts.Append("[dtDateOfShow], [AgeDescription], [MainActName], [mPrice], ");
                                inserts.Append("[iQuantity], [PurchaseAction], [dtStamp]) ");
                                inserts.AppendFormat("VALUES (@invoiceId, '{1}', @showTicketId_{0}, @showId_{0}, ",
                                    idx, _Enums.InvoiceItemContext.servicecharge.ToString());

                                string addPackages = string.Empty;
                                inserts.AppendFormat("@showDate_{0}, @ageDescription_{0}, 'SERVICE FEE FOR: {1}' + @mainActName_{0} + @addPackages_{0}, ",
                                    idx, (isPackage_BaseTicketItem) ? "PACKAGE - " : string.Empty);


                                //service is price for servicecharge
                                inserts.AppendFormat(" @service_{0}, @newQuantity_{0}, '{1}', (getDate()) ); ", 
                                    idx, _Enums.PurchaseActions.Purchased.ToString());

                                //update the description -- follow flow
                                //lineDescription = string.Format("{0} @ {1} (svc fees not incl)~", quantity, useDescription);

                                //bool specifyPackage = false;

                                if (isPackage_BaseTicketItem)
                                {
                                    //specifyPackage = true;
                                    //lineDescription = string.Format("PKG: ", quantity.ToString(), useDescription);

                                    foreach (ShowTicket s in invoiceItem.ShowTicketRecord.LinkedShowTickets)
                                    {
                                        InvoiceItem packageTicket = invoice.InvoiceItemRecords().GetList().Find(delegate(InvoiceItem match)
                                        {
                                            return (match.TShowTicketId == s.Id &&
                                                match.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower());
                                        });

                                        if (packageTicket != null)
                                        {
                                            cmd.Parameters.Add(string.Format("@invoiceItemId_{0}", j), packageTicket.Id, DbType.Int32);

                                            addPackages += string.Format("~{0}", packageTicket.MainActName);

                                            //update existing items
                                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}', ",
                                                _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                                            inserts.AppendFormat("[iQuantity] = @newQuantity_{0}, [mServiceCharge] = 0, ", idx);
                                            inserts.Append("[MainActName] = [MainActName] + ' (svc fees not incl)' ");
                                            inserts.AppendFormat("WHERE [Id] = @invoiceItemId_{0}; ", j);

                                            //service charge items are handled by baseitem
                                        }

                                        j++;
                                    }
                                }

                                cmd.Parameters.Add(string.Format("@addPackages_{0}", idx), addPackages);

                                lineDescription = string.Format("{0} @ {1} (svc fees not incl)~", quantity.ToString(), useDescription);
                            }

                            //inventory
                            inserts.AppendFormat("UPDATE ShowTicket SET [iSold] = [iSold] - @newQuantity_{0}, ", idx);
                            inserts.AppendFormat("[iRefunded] = [iRefunded] + @newQuantity_{0} WHERE [Id] = @showTicketId_{0}; ", idx);

                            //inventory for packaged tickets
                            if (rli.IsPackageTicket)
                            {
                                foreach (ShowTicket s in invoiceItem.ShowTicketRecord.LinkedShowTickets)
                                {
                                    InvoiceItem packageTicket = invoice.InvoiceItemRecords().GetList().Find(delegate(InvoiceItem match)
                                    {
                                        return (match.TShowTicketId == s.Id &&
                                            match.PurchaseAction.ToLower() == _Enums.PurchaseActions.Purchased.ToString().ToLower());
                                    });

                                    if (packageTicket != null)
                                    {
                                        cmd.Parameters.Add(string.Format("@showTicketId_{0}", j), packageTicket.TShowTicketId, DbType.Int32);

                                        inserts.AppendFormat("UPDATE ShowTicket SET [iSold] = [iSold] - @newQuantity_{0}, ", idx);
                                        inserts.AppendFormat("[iRefunded] = [iRefunded] + @newQuantity_{0} ", idx);
                                        inserts.AppendFormat("WHERE [Id] = @showTicketId_{0}; ", j);
                                    }

                                    j++;
                                }
                            }

                            break;

                        #endregion

                        #region DONATIONS
                        case _Enums.InvoiceItemContext.charity:

                            //these items are always of quantity 1
                            lineDescription = string.Format("{0} @ Donation {1}~", quantity.ToString(), rli.DescriptionNoQuantity);

                            //update existing item
                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}' ", _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                            inserts.AppendFormat("WHERE [Id] = @invoiceItemId_{0}; ", idx);

                            break;
                        #endregion

                        #region Bundles, Promotional Discounts & Shipping

                        //ticket and merch promotions are handled in their context above
                        case _Enums.InvoiceItemContext.processing:
                        case _Enums.InvoiceItemContext.shippingmerch:
                        case _Enums.InvoiceItemContext.shippingticket:
                        case _Enums.InvoiceItemContext.discount:
                        case _Enums.InvoiceItemContext.bundle:
                            
                            lineDescription = string.Format("{0} @ {1}~", quantity.ToString(), rli.DescriptionNoQuantity);

                            //update existing item
                            inserts.AppendFormat("UPDATE InvoiceItem SET [PurchaseAction] = '{0}' ", _Enums.PurchaseActions.PurchasedThenRemoved.ToString());
                            inserts.AppendFormat("WHERE [Id] = @invoiceItemId_{0}; ", idx);

                            break;

                        #endregion
                    }

                    desc.Append(lineDescription);
                    itemsToRefund.Add(new ListItem(lineDescription.TrimEnd('~'), lineRefund.ToString()));

                    idx++;
                }
            }

            return decimal.Round(amountToRefund, 2);
        }

        #endregion

        #region Contruct (SQL) AuthorizeNet

        public static AuthorizeNet AuthorizeNetRefund(Invoice invoice, string creatorName, decimal amountToReturn,
            string refundDescription, string userIp)
        {
            AuthorizeNet auth = new AuthorizeNet();

            AuthorizeNetCollection coll = new AuthorizeNetCollection();
            coll.AddRange(invoice.AuthorizeNetRecords().GetList().FindAll(
                delegate(AuthorizeNet match)
                { return (match.IsAuthorized && match.TransactionType.ToLower() == "auth_capture"); }));
            if (coll.Count > 1)
                coll.Sort("Id", true);

            if (coll.Count == 0)
                throw new Exception("Could not find original transaction");

            AuthorizeNet originalTransaction = coll[0];
            string transactionId = originalTransaction.ProcessorId;

            auth.ApplicationId = invoice.ApplicationId;
            auth.DtStamp = DateTime.Now;
            auth.IsAuthorized = false;
            auth.InvoiceRecord = invoice;
            auth.IpAddress = userIp;
            auth.UserId = invoice.UserId;
            auth.Email = invoice.AspnetUserRecord.UserName;
            if (refundDescription.Length > 1000)
                refundDescription = string.Format("{0}...", refundDescription.Substring(0, 975));
            auth.Description = string.Format("REFUND:~{0} total~{1}", amountToReturn.ToString("c"), refundDescription);

            //Initiate the refund
            AuthorizeNet.Purchaser p = new AuthorizeNet.Purchaser(originalTransaction.Email, originalTransaction.FirstName, originalTransaction.LastName, originalTransaction.Company,
                originalTransaction.BillingAddress, string.Empty, originalTransaction.City, originalTransaction.State, originalTransaction.Zip, originalTransaction.Country, originalTransaction.Phone);

            decimal AmountPaidViaAuthNet = 0;
            List<InvoiceTransaction> AuthNetTrans = new List<InvoiceTransaction>();
            AuthNetTrans.AddRange(invoice.CreditCardTransactions.GetList()
                .FindAll(delegate(InvoiceTransaction match) { return (match.FundsProcessor.ToLower() == "authorizenet" && match.TransType.ToLower() == "payment"); }));
            foreach (InvoiceTransaction trans in AuthNetTrans)
                AmountPaidViaAuthNet += trans.Amount;


            //if (invoice.InvoiceDate.Date == DateTime.Now.Date && invoice.NetPaid == amountToReturn)
            if (invoice.InvoiceDate.Date == DateTime.Now.Date && AmountPaidViaAuthNet == amountToReturn)
                auth.VoidPaymentData(p, invoice, amountToReturn, transactionId);
            else
                auth.RefundPaymentData(p, invoice, amountToReturn, transactionId);

            auth.LogTransactionInfoBeforeSending(auth.PostData.ToString(), auth.Description, true);

            Utils.HttpTrans t = new Utils.HttpTrans();

            auth.response = new AuthorizeNet.AuthorizeNetResponse(t.Post(_Config._AuthorizeNetPaymentUrl, auth.PostData.ToString()), 
                refundDescription, true, auth.IpAddress);

            auth.IsAuthorized = auth.response.IsAuthorized;

            auth.LogResponse(refundDescription);

            //log attempt to event Q
            InvoiceEvent.NewInvoiceEvent(invoice.Id, DateTime.Now, DateTime.Now,
                ((auth.IsAuthorized) ? _Enums.EventQStatus.Success : _Enums.EventQStatus.Failed),
                creatorName, invoice.UserId, auth.Email, _Enums.EventQContext.Invoice, _Enums.EventQVerb.Refund,
                string.Format("TransId: {0}", transactionId), string.Format("Amount: {0}", amountToReturn.ToString("c")),
                auth.Description, true);

            auth.Save();

            return auth;
        }

        #endregion

        #region Notifications

        private static void RefundErrorNotification(AuthorizeNet auth, string reasonForError)
        {
            string custEmail = (auth.Email != null) ? auth.Email : "email unknown";
            string amount = auth.Amount.ToString("c");
            string description = (auth.Description != null) ? auth.Description : "no description";
            DateTime attemptedDate = DateTime.Now;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName);

            mail.To.Add(new MailAddress(_Config._CustomerService_Email, _Config._CustomerService_FromName));

            if (_Config._CC_DeveloperEmail.Trim().Length > 0)
                mail.Bcc.Add(new MailAddress(_Config._CC_DeveloperEmail));

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{0}{0}", Utils.Constants.NewLine);
            sb.AppendFormat("Date: {1}{0}", Utils.Constants.NewLine, attemptedDate.ToString());
            sb.AppendFormat("Customer: {1}{0}", Utils.Constants.NewLine, custEmail);
            sb.AppendFormat("Amount To Refund: {1}{0}", Utils.Constants.NewLine, amount);
            sb.AppendFormat("Description: {1}{0}{0}", Utils.Constants.NewLine, description);
            sb.AppendFormat("Error: {1}{0}{0}", Utils.Constants.NewLine, reasonForError);

            mail.Body = sb.ToString();
            mail.Subject = "Refund Error Notification";

            try
            {
                SmtpClient client = new SmtpClient();
                client.Send(mail);
            }
            catch (Exception ex)
            {
                _Error.LogException(ex, true);
            }
        }

        #endregion

        #region Refund Transaction

        /// <summary>
        /// inserts an InvoiceTransaction row
        /// </summary>
        private static void DoRefundTransaction(StringBuilder inserts, Invoice invoice, string processorId, string creatorName, 
            _Enums.FundsTypes fundsType, _Enums.FundsProcessor fundsProcessor, decimal amountToRefund, string userIp)
        {
            //issue a transaction paid by check
            inserts.Append("INSERT INTO InvoiceTransaction ([ProcessorId],[TInvoiceId],[PerformedBy],[Admin],[UserId],[CustomerId],[TransType],");
            inserts.Append("[FundsType],[FundsProcessor],[mAmount],[UserIp],[dtStamp]) ");
            inserts.AppendFormat("VALUES ('{11}',{0},'{1}','{2}','{3}',{4},'{5}','{6}','{7}',{8},'{9}','{10}') ",
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
                DateTime.Now.ToString(),
                processorId);
        }

        #endregion

    }
}
