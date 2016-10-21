using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlTypes;
using System.Web.UI.WebControls;

using Wcss.QueryRow;

namespace Wcss
{
    public partial class ShipmentBatch
    {
        [XmlAttribute("TicketListing_Event")]
        public List<string> TicketListing_Event
        {
            get
            {
                return Utils.ParseHelper.CsvSeparatedStringToList(this.CsvEventTix);
            }
            set
            {
                if (value.Count == 0)
                    this.CsvEventTix = null;
                else
                    this.CsvEventTix = Utils.ParseHelper.SplitListIntoString<string>(value, false);
            }
        }
        [XmlAttribute("TicketListing_Other")]
        public List<string> TicketListing_Other
        {
            get
            {
                return Utils.ParseHelper.CsvSeparatedStringToList(this.CsvOtherTix);
            }
            set
            {
                if (value.Count == 0)
                    this.CsvOtherTix = null;
                else
                    this.CsvOtherTix = Utils.ParseHelper.SplitListIntoString<string>(value, false);
            }
        }
        public List<string> TicketListing_All
        {
            get
            {
                List<string> list = new List<string>();
                list.AddRange(this.TicketListing_Event);
                list.AddRange(this.TicketListing_Other);
                return list;
            }
        }
        [XmlAttribute("ShippingMethodListing")]
        public List<string> ShippingMethodListing
        {
            get
            {
                return Utils.ParseHelper.CsvSeparatedStringToList(this.CsvMethods);
            }
            set
            {
                if (value.Count == 0)
                    this.CsvMethods = null;
                else
                    this.CsvMethods = Utils.ParseHelper.SplitListIntoString<string>(value, false);
            }
        }

        [XmlAttribute("EstimatedShipDate")]
        public DateTime EstimatedShipDate
        {
            get { return (this.DtEstShipDate.HasValue) ? this.DtEstShipDate.Value : DateTime.MaxValue; }
            set { this.DtEstShipDate = (value != DateTime.MaxValue && value != DateTime.MinValue) ? value : (DateTime?)null; }
        }

        public static string GenerateShipmentBatchId()
        {
            return Utils.ParseHelper.GenerateRandomPassword(7);
        }
        public static string GenerateShipmentBatchName(string selectedEventText)
        {
            selectedEventText = selectedEventText.Trim();
            return string.Format("{0}_{1}", DateTime.Now.ToString("yyMMddhhmmss"), (selectedEventText.Length > 200) ? selectedEventText.Substring(0,200).Trim() : selectedEventText);
        }

        #region BatchShipping Fulfillment

        /// <summary>
        /// Main logic for creating the batch. Sets up the sql stringbuilder and command for use with the method
        /// </summary>
        /// <param name="fill"></param>
        /// <param name="otherTicketIds"></param>
        /// <param name="list">we know that this list has an inner repeater</param>
        /// <returns></returns>
        public static ShipmentBatch CreateBatchFromFulfillment(ShippingFulfillment fill, string proposedBatchId, string selectedEventText, string description, int selectedEventId, 
            List<string> selectedEventTicketIds, List<string> selectedOtherTicketIds, DateTime estimatedShipDate, ListView list, Guid userId)
        {
            //this will hold the command to 
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            //create a new batch
            ShipmentBatch batch = ConstructBatch(proposedBatchId, selectedEventText, description, selectedEventId, 
                selectedEventTicketIds, selectedOtherTicketIds, estimatedShipDate);

            batch.ShippingMethodListing = LoopThruGrid(sb, cmd, list, fill, userId, batch);
            batch.Save();

            //if we still have remaing query
            if(sb.Length > 0)
                SubmitBatchShipment(cmd, sb, batch);

            //go back to the db to get a "refreshed" copy
           return new ShipmentBatch(batch.Id);
        }
        public static void UndoBatch(ShipmentBatch batch)
        {
            try
            {
                SPs.TxShippingBatchUndo(batch.Id).Execute();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw;
            }

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

            ////get all invoiceshipments in the batch
            //ShipmentBatchInvoiceShipmentCollection coll = new ShipmentBatchInvoiceShipmentCollection();
            //coll.AddRange(batch.ShipmentBatchInvoiceShipmentRecords());

            //foreach (ShipmentBatchInvoiceShipment batchShipment in coll)
            //{
            //    int invoiceIdx = batchShipment.InvoiceShipmentRecord.TInvoiceId;

            //    //update invoicebill ship
            //    //if the invoice contains no shippable merch....
            //    //sb.AppendFormat("IF NOT EXISTS (SELECT * FROM [InvoiceItem] ii WHERE ii.[tInvoiceId] = {0} AND ii.[vcContext] = '{1}' AND ii.[PurchaseAction] = '{2}' AND ii.[dtShipped] IS NOT NULL) ",
            //    //    invoiceIdx.ToString(), _Enums.InvoiceItemContext.merch.ToString(), _Enums.PurchaseActions.Purchased.ToString());
            //    //sb.AppendFormat("BEGIN UPDATE [InvoiceBillShip] SET [dtShipped] = null WHERE [tInvoiceId] = {0} END ",
            //    //    invoiceIdx.ToString());


            //    //Get the id of the shipitem
            //    int shipInvoiceItemId = (batchShipment.InvoiceShipmentRecord.TShipItemId.HasValue) ? batchShipment.InvoiceShipmentRecord.TShipItemId.Value : 0;

            //    //if not 0
            //    //update invoiceitem and reset any item with a shipitem id that matches or an id that matches
            //    if (shipInvoiceItemId > 0)
            //        sb.AppendFormat("UPDATE [InvoiceItem] SET [dtShipped] = null WHERE [tShipItemId] = {0} OR [Id] = {0} ", shipInvoiceItemId.ToString());
            //}

            ////finally delete the batch and let the cascade take care of InvoiceShipmentItems, InvoiceShipments, ShipmentBatch_InvoiceShipments
            //sb.AppendFormat("DELETE FROM [InvoiceShipment] WHERE [Id] IN (SELECT [tInvoiceShipmentId] as 'Id' FROM [ShipmentBatch_InvoiceShipment] WHERE [tShipmentBatchId] = {0} ) ", 
            //    batch.Id.ToString());

            ////now we can delete the batch
            //sb.AppendFormat("DELETE FROM [ShipmentBatch] WHERE [Id] = {0} ", batch.Id.ToString());

            //cmd.CommandSql = sb.ToString();

            //try
            //{
            //    //SubSonic.DataService.ExecuteQuery(cmd);

            //    //cmd.CommandSql = string.Empty;
            //    //cmd.Parameters.Clear();
            //    //sb.Length = 0;
            //}
            //catch (Exception ex)
            //{
            //    _Error.LogException(ex);
            //    throw;
            //}
        }

        /// <summary>
        /// Create a new ShipmentBatch object
        /// </summary>
        private static ShipmentBatch ConstructBatch(string proposedBatchId, string selectedEventText, string description, int selectedEventId, 
            List<string> selectedEventTicketIds, List<string> selectedOtherTicketIds, DateTime estimatedShipDate)
        {
            ShipmentBatch batch = new ShipmentBatch();

            batch.ApplicationId = _Config.APPLICATION_ID;
            batch.DtStamp = DateTime.Now;
            batch.BatchId = proposedBatchId;           
            batch.Name = ShipmentBatch.GenerateShipmentBatchName(selectedEventText);
            batch.Description = description;
            batch.EventId = selectedEventId;
            batch.TicketListing_Event = selectedEventTicketIds;
            batch.TicketListing_Other = selectedOtherTicketIds;

            //batch.ShippingMethodListing = string.Empty;
            batch.EstimatedShipDate = estimatedShipDate;

            try
            {
                batch.Save();
            }
            catch (Exception)
            {
                throw;
            }

            return batch;
        }
        /// <summary>
        /// Loop through the supplied grid
        /// </summary>
        private static List<string> LoopThruGrid(System.Text.StringBuilder sb, SubSonic.QueryCommand cmd, ListView list, ShippingFulfillment fill, Guid userId, ShipmentBatch batch)
        {
            int batchCounter = 0;//this will allow us to batch the sql
            List<string> methods = new List<string>();

            foreach (ListViewDataItem lvi in list.Items)
            {
                if (lvi.ItemType == ListViewItemType.DataItem)
                {
                    CheckBox chkSelect = (CheckBox)lvi.FindControl("chkSelect");

                    //foreach INVOICE row that is selected
                    if (chkSelect != null && chkSelect.Checked)
                    {
                        //get reference to invoice row item
                        int invoiceId = (int)list.DataKeys[lvi.DataItemIndex]["Id"];
                        int shipId = (int)list.DataKeys[lvi.DataItemIndex]["tTicketShipItemId"];

                        string method = ProcessRow(ref batchCounter, invoiceId, shipId, fill, lvi, sb, cmd, userId, batch);

                        if (method != null && method.Trim().Length > 0 && (!methods.Contains(method.Trim())))
                            methods.Add(method.Trim());
                    }
                }
            }

            return methods;
        }   
        /// <summary>
        /// Create sql statements to process each row
        /// returns ship method for this row
        /// </summary>
        private static string ProcessRow(ref int counter, int invoiceId, int shipId, ShippingFulfillment fill, ListViewDataItem lvi, System.Text.StringBuilder sb, SubSonic.QueryCommand cmd,
            Guid userId, ShipmentBatch batch)
        {
            Wcss.QueryRow.ShippingInvoiceRow invoiceRow = fill.ShippingInvoices.Find(delegate(ShippingInvoiceRow match) { return (match.Id == invoiceId && match.tTicketShipItemId == shipId); } );

            if(invoiceRow != null)
            {
                //string idx = invoiceId.ToString();
                //use the last 2 numbers of the invoice
                string idx = string.Format("{0}{1}", shipId.ToString(), invoiceId.ToString().Substring(invoiceId.ToString().Length - 3));
                string packingList = string.Empty;

                DataList dl = (DataList)lvi.FindControl("dlItem");
                //Verify that the invoice has shippable items before beginning sql
                if (dl != null)
                {
                    if (VerifyShippableTickets(dl))
                    {
                        //create invoiceshipment
                        sb.AppendFormat("DECLARE @invoiceshipId_{0} int; ", idx);
                        sb.AppendLine();
                        sb.AppendFormat("INSERT [InvoiceShipment] ([tInvoiceId],[UserId],[dtCreated],[ReferenceNumber],[vcContext],[TShipItemId],[bLabelPrinted], ");
                        sb.AppendFormat("[CompanyName],[FirstName],[LastName],[Address1],[Address2],[City],[StateProvince],[PostalCode],[Country],[Phone],[ShipMessage], ");
                        sb.AppendFormat("[dtShipped],[vcCarrier],[ShipMethod],[PackingList],[mWeightCalculated],[mWeightActual],[mShippingCharged],[dtStamp],[TrackingInformation],[PackingAdditional]) ");
                        sb.AppendLine();
                        sb.AppendFormat("SELECT @invoiceId_{0},@userId_{0},getDate(),@refNumber_{0},'ticket',@tShipItemId_{0},@bLabelPrinted_{0}, ", idx);
                        sb.AppendLine();
                        cmd.Parameters.Add(string.Format("@invoiceId_{0}", idx), invoiceRow.Id, DbType.Int32);
                        cmd.Parameters.Add(string.Format("@userId_{0}", idx), userId.ToString(), DbType.String);
                        //cmd.Parameters.Add(string.Format("@dtCreated_{0}", idx), DateTime.Now, DbType.DateTime);//done by getdate()
                        cmd.Parameters.Add(string.Format("@refNumber_{0}", idx), Guid.NewGuid().ToString(), DbType.String);
                        //cmd.Parameters.Add(string.Format("@vcContext_{0}", idx), "??", DbType.String);set to static string in sql
                        cmd.Parameters.Add(string.Format("@tShipItemId_{0}", idx), invoiceRow.tTicketShipItemId, DbType.Int32);
                        cmd.Parameters.Add(string.Format("@bLabelPrinted_{0}", idx), false, DbType.Boolean);
                        sb.AppendLine();

                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[CompanyName] ELSE ibs.[blCompany] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[FirstName] ELSE ibs.[blFirstName] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[LastName] ELSE ibs.[blLastName] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[Address1] ELSE ibs.[blAddress1] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ISNULL(ibs.[Address2],'') ELSE ISNULL(ibs.[blAddress2],'') end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[City] ELSE ibs.[blCity] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[StateProvince] ELSE ibs.[blStateProvince] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[PostalCode] ELSE ibs.[blPostalCode] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[Country] ELSE ibs.[blCountry] end, ");
                        sb.AppendFormat("CASE WHEN ibs.[bSameAsBilling] = 0 THEN ibs.[Phone] ELSE ibs.[blPhone] end, ");
                        sb.AppendFormat("ibs.[ShipMessage], ");
                        sb.AppendLine();
                        
                        //use empty string values for tracking and packing
                        //weight is same for calculated and actual
                        sb.AppendFormat("@estShipDate,@vcCarrier_{0},@shipMethod_{0},@packingList_{0},@mWeightCalc_{0},@mWeightCalc_{0},ii.[mLineItemTotal],getDate(),'','' ", idx);
                        sb.AppendLine();
                        //cmd.Parameters.Add(string.Format("@estShipDate_{0}", idx), batch.EstimatedShipDate, DbType.DateTime);
                        //cmd.Parameters.Add(string.Format("@status_{0}", idx), "??", DbType.String);//not used - all rows are null
                        cmd.Parameters.Add(string.Format("@vcCarrier_{0}", idx), ShipmentBatch.GetCarrierName(invoiceRow.TicketShipMethod), DbType.String);
                        cmd.Parameters.Add(string.Format("@shipMethod_{0}", idx), invoiceRow.TicketShipMethod, DbType.String);
                        //packing list to be filled below
                        cmd.Parameters.Add(string.Format("@mWeightCalc_{0}", idx), 0.5, DbType.Decimal);                        
                        //cmd.Parameters.Add(string.Format("@mShipCharge_{0}", idx), , DbType.Decimal);//done in join
                        //cmd.Parameters.Add(string.Format("@dtStamp_{0}", idx), DateTime.Now, DbType.DateTime);//not needed
                        sb.AppendLine();
                        sb.AppendFormat("FROM [InvoiceBillShip] ibs, [InvoiceItem] ii ");
                        sb.AppendLine();
                        sb.AppendFormat("WHERE ibs.[tInvoiceId] = @invoiceId_{0} AND ii.[Id] = @tShipItemId_{0}; ", idx);
                        sb.AppendLine();
                        sb.AppendFormat("SELECT @invoiceshipId_{0} = SCOPE_IDENTITY(); ", idx);
                        sb.AppendLine();

                        //update ship date for invoicebillship
                        sb.AppendFormat("UPDATE [InvoiceBillShip] SET [dtShipped] = @estShipDate WHERE [tInvoiceId] = @invoiceId_{0}; ", idx);
                        ////update the invoiceitem - the shipment item
                        sb.AppendFormat("UPDATE [InvoiceItem] SET [ShippingMethod] = @shipMethod_{0}, [bRTS] = 0, [dtShipped] = @estShipDate WHERE [Id] = {1}; ", 
                            idx, shipId.ToString());

                        //create join table row
                        sb.AppendFormat("INSERT [ShipmentBatch_InvoiceShipment] ([dtStamp],[tShipmentBatchId],[tInvoiceShipmentId]) ");
                        sb.AppendLine();
                        sb.AppendFormat("VALUES (getDate(),@batchId,@invoiceshipId_{0}); ", idx);
                        sb.AppendLine();

                         //loop thru items in the repeater and create itemshipmentitems and a packing list
                        foreach (DataListItem e in dl.Items)
                        {
                            CheckBox chkSlated = (CheckBox)e.FindControl("chkSlated");
                            if (chkSlated != null && chkSlated.Checked)
                            {
                                int itemIdx = (int)dl.DataKeys[e.ItemIndex];

                                ShippingItemRow sir = fill.ShippingItems.Find(delegate(ShippingItemRow match) { return (match.Id == itemIdx); } );
                                if (sir != null)
                                {
                                    string jdx = sir.Id.ToString();

                                    //create an invoiceshipmentitem
                                    sb.AppendFormat("INSERT [InvoiceShipmentItem] ([dtStamp],[tInvoiceShipmentId],[tInvoiceItemId],[iQuantity]) ");
                                    sb.AppendLine();
                                    sb.AppendFormat("VALUES (getDate(),@invoiceshipId_{0},@itemId_{1},@iQuantity_{1}); ", idx, jdx);
                                    cmd.Parameters.Add(string.Format("@itemId_{0}", jdx), sir.Id, DbType.Int32);
                                    cmd.Parameters.Add(string.Format("@iQuantity_{0}", jdx), sir.Quantity, DbType.Int32);
                                    sb.AppendLine();

                                    //find matching ticket item for packinglist info
                                    ShippingTicketRow tktRow = fill.AllShippableShowTickets.Find(delegate(ShippingTicketRow match) { return (match.Id == sir.tShowTicketId); } );
                                    if(tktRow != null)
                                        packingList += string.Format("{0} @ {1}~", sir.Quantity.ToString(), tktRow.PackingListInfo);

                                    //Update the invoiceitem - mark as shipped
                                    sb.AppendFormat("UPDATE [InvoiceItem] SET [ShippingMethod] = @shipMethod_{0}, [bRTS] = 0, [dtShipped] = @estShipDate WHERE [Id] = @itemId_{1}; ", 
                                        idx, jdx);
                                }
                            }
                        }
                        //end for loop

                        //set packing list
                        cmd.Parameters.Add(string.Format("@packingList_{0}", idx), packingList.TrimEnd('~'), DbType.String);

                         //increment the counter
                        counter++;

                        //set up mult batches to go on an interval
                        if (counter % 10 == 0)
                        {
                            ShipmentBatch.SubmitBatchShipment(cmd, sb, batch);
                        }

                    }//row has shippable items
                }//datalist is not null

                return invoiceRow.TicketShipMethod;
            }//invoiceRow is not null

            return null;
        }

        #endregion 

        #region Helper funcs

        public static void SubmitBatchShipment(SubSonic.QueryCommand cmd, System.Text.StringBuilder sb, ShipmentBatch batch)
        {
            try
            {
                cmd.CommandSql = sb.ToString();
                cmd.Parameters.Add("@batchId", batch.Id, DbType.Int32);
                cmd.Parameters.Add("@estShipDate", batch.EstimatedShipDate, DbType.DateTime);

                SubSonic.DataService.ExecuteQuery(cmd);

                cmd.CommandSql = string.Empty;
                cmd.Parameters.Clear();
                sb.Length = 0;
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
                throw ex;
            }
        }

        private static bool VerifyShippableTickets(DataList dl)
        {
            foreach (DataListItem e in dl.Items)
            {
                CheckBox chkSlated = (CheckBox)e.FindControl("chkSlated");

                if (chkSlated != null && chkSlated.Checked)
                    return true;
            }

            return false;
        }

        public static string GetCarrierName(string shipMethod)
        {
            if (shipMethod != null)
            {
                shipMethod = shipMethod.ToLower();

                if (shipMethod.IndexOf("usps") != -1)
                    return _Enums.ShippingCarrier.USPS.ToString();
                else if (shipMethod.IndexOf("fedex") != -1)
                    return _Enums.ShippingCarrier.FEDEX.ToString();
                else if (shipMethod.IndexOf("ups") != -1)
                    return _Enums.ShippingCarrier.UPS.ToString();
            }

            return _Enums.ShippingCarrier.NOTSPECD.ToString();
        }

        #endregion
    }
}
