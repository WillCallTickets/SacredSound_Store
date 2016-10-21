using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

//<%@ Register Namespace="SqlNetFrameworkWebControls" TagPrefix="SqlNetFrameworkWebControls" %>

namespace WillCallWeb.Admin.AdminControls.BulkShipping
{
    public partial class Shipping_ShipmentBatchView : BaseControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#srceditor", true);
        }

        #region New paging

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
        }
        public override void Dispose()
        {
            GooglePager1.GooglePagerChanged += new WillCallWeb.Components.Navigation.gglPager.GooglePagerChangedEvent(GooglePager1_GooglePagerChanged);
            base.Dispose();
        }
        protected void GooglePager1_GooglePagerChanged(object sender, WillCallWeb.Components.Navigation.gglPager.GooglePagerEventArgs e)
        {
            Atx.adminPageSize = e.NewPageSize;
            Listing.DataBind();
        }

        #endregion

        #region Page Overhead

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                Atx.CurrentShippingFulfillment = null;
                Atx.CurrentShipmentBatch = null;

                GooglePager1.PageSize = Atx.adminPageSize;
                GooglePager1.DataSetSize = 0;
                GooglePager1.DataBind();
                
            }
        }

        #endregion


        #region Calendar for ship date estimate

        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            cal.SelectedDate = (Atx.CurrentShipmentBatch != null) ? Atx.CurrentShipmentBatch.EstimatedShipDate : DateTime.Now;
        }

        #endregion

        #region Date & Ticket Selection

        /// <summary>
        /// set the date to start the dates in the list
        /// </summary>
        protected void SqlPreviousBatches_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@currentBatchId"].Value = (Atx.CurrentShipmentBatch != null) ? Atx.CurrentShipmentBatch.Id : 0;
        }

        /// <summary>
        /// bind to current batch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPreviousBatch_DataBound(object sender, EventArgs e)
        {
            DropDownList list = (DropDownList)sender;

            if (Atx.CurrentShipmentBatch != null && (list.SelectedIndex <= 0 || list.SelectedValue != Atx.CurrentShipmentBatch.Id.ToString()))
            {
                list.SelectedIndex = -1;

                ListItem li = list.Items.FindByValue(Atx.CurrentShipmentBatch.Id.ToString());
                if (li != null)
                    li.Selected = true;
                else
                    list.SelectedIndex = 0;
            }
            else if (Atx.CurrentShipmentBatch == null && list.SelectedIndex > 0)
            {
                list.SelectedIndex = -1;
                list.SelectedIndex = 0;
            }

            btnUndoBatch.Enabled = (Atx.CurrentShipmentBatch != null);
        }
        protected void ddlPreviousBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            int idx = int.Parse(ddl.SelectedValue);

            if (idx > 0)
            {
                Atx.CurrentShipmentBatch = new ShipmentBatch(idx);
                clockEstimate.SelectedDate = Atx.CurrentShipmentBatch.EstimatedShipDate;
            }
            else
                Atx.CurrentShipmentBatch = null;

            btnUndoBatch.Enabled = (Atx.CurrentShipmentBatch != null);
            BindListing();
        }
        /// <summary>
        /// handles sorting and ship method changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindListing();
        }

        #endregion

        private void BindListing()
        {   
            Listing.DataBind();
        }
        
        #region OnClick Changes

        protected void ChangeBatchAll(object sender, EventArgs e)
        {
            if (Atx.CurrentShipmentBatch != null)
            {
                Button btn = (Button)sender;
                bool isExecute = false;

                try
                {
                    if (btn.ID.IndexOf("ChangeDateAll") != -1)
                    {
                        SPs.TxShippingUpdateBatchListing(Atx.CurrentShipmentBatch.Id, clockEstimate.SelectedDate, null).Execute();

                        Atx.CurrentShipmentBatch.DtEstShipDate = clockEstimate.SelectedDate;

                        isExecute = true;
                    }
                    else if (btn.ID.IndexOf("ChangeActualAll") != -1)
                    {
                        string inputActual = txtActualShip.Text.Trim();

                        if (!Utils.Validation.IsDecimal(inputActual))
                            throw new Exception("Please enter a valid amount.");

                        decimal amt = decimal.Parse(inputActual);

                        SPs.TxShippingUpdateBatchListing(Atx.CurrentShipmentBatch.Id, null, amt).Execute();

                        isExecute = true;
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidator.IsValid = false;
                    CustomValidator.ErrorMessage = ex.Message;
                    return;
                }

                if (isExecute)
                {
                    txtActualShip.Text = string.Empty;
                    Atx.CurrentShippingFulfillment = null;
                    BindListing();
                }
            }
        }
        protected void ChangeBatchPage(object sender, EventArgs e)
        {
            if (Atx.CurrentShippingFulfillment != null)
            {
                Button btn = (Button)sender;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                SubSonic.QueryCommand qry = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);

                if (btn.ID.IndexOf("ChangeDatePage") != -1)
                {
                    DateTime newDate = clockEstimate.SelectedDate;
                    qry.Parameters.Add("@newDate", newDate, DbType.DateTime);

                    //do each invoiceshipment
                    foreach (Wcss.QueryRow.ShippingInvoiceShipmentRow row in Atx.CurrentShippingFulfillment.InvoiceShipments)
                    {
                        sb.AppendFormat("UPDATE [InvoiceShipment] SET [dtShipped] = @newDate WHERE [Id] = @invsId_{0}; ", row.Id.ToString());
                        qry.Parameters.Add(string.Format("@invsId_{0}", row.Id.ToString()), row.Id, DbType.Int32);

                        //the matching invoicebillship
                        sb.AppendFormat("UPDATE [InvoiceBillShip] SET [dtShipped] = @newDate WHERE [tInvoiceId] = @invoiceId_{0}; ", row.Id.ToString());
                        qry.Parameters.Add(string.Format("@invoiceId_{0}", row.Id.ToString()), row.InvoiceId, DbType.Int32);

                        //do the matching items
                        sb.Append("UPDATE [InvoiceItem] SET [dtShipped] = @newDate FROM [InvoiceItem] ii, [InvoiceShipmentItem] isi ");
                        sb.AppendFormat("WHERE isi.[tInvoiceShipmentId] = @invsId_{0} AND isi.[tInvoiceItemId] = ii.[Id]; ", row.Id.ToString());
                    }
                }
                else if (btn.ID.IndexOf("ChangeActualPage") != -1)
                {
                    string input = txtActualShip.Text.Trim();

                    if (Utils.Validation.IsDecimal(input))
                    {
                        qry.Parameters.Add("@newActual", decimal.Parse(input), DbType.Decimal);

                        //do each invoiceshipment
                        foreach (Wcss.QueryRow.ShippingInvoiceShipmentRow row in Atx.CurrentShippingFulfillment.InvoiceShipments)
                        {
                            sb.AppendFormat("UPDATE [InvoiceShipment] SET [mShippingActual] = @newActual WHERE [Id] = @invsId_{0}; ", row.Id.ToString());
                            qry.Parameters.Add(string.Format("@invsId_{0}", row.Id.ToString()), row.Id, DbType.Int32);
                        }
                    }
                }

                if (sb.Length == 0)
                    return;

                qry.CommandSql = sb.ToString();

                try
                {
                    SubSonic.DataService.ExecuteQuery(qry);
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                    CustomValidator.IsValid = false;
                    CustomValidator.ErrorMessage = ex.Message;
                    return;
                }

                txtActualShip.Text = string.Empty;
                Atx.CurrentShippingFulfillment = null;
                BindListing();
            }
        }

        #endregion

        #region ListView Control

        protected int _rowCounter = 0;

        protected void Listing_Init(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            GooglePager1.PageSize = Atx.adminPageSize;
        }
        protected void Listing_DataBinding(object sender, EventArgs e)
        {
            if (Atx.CurrentShipmentBatch != null)
            {
                ListView list = (ListView)sender;

                Atx.CurrentShippingFulfillment = Wcss.QueryRow.ShippingFulfillment.GetBatchShipments(
                    (Atx.CurrentShipmentBatch.EventId.HasValue) ? Atx.CurrentShipmentBatch.EventId.Value : 0,
                    Atx.CurrentShipmentBatch.Id, GooglePager1.PageSize, GooglePager1.StartRowIndex);

                if (Atx.CurrentShippingFulfillment != null)
                {
                    GooglePager1.DataSetSize = Wcss.QueryRow.ShippingFulfillment.GetBatchShipments_Count(Atx.CurrentShipmentBatch.Id);
                    list.DataSource = Atx.CurrentShippingFulfillment.InvoiceShipments;
                }

                GooglePager1.DataBind();

                _rowCounter = GooglePager1.StartRowIndex - 1;
            }
        }
        protected void Listing_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem viewItem = (ListViewDataItem)e.Item;
                Wcss.QueryRow.ShippingInvoiceShipmentRow entity = (Wcss.QueryRow.ShippingInvoiceShipmentRow)viewItem.DataItem;

                _rowCounter += 1;

                Literal rowCounter = (Literal)e.Item.FindControl("litRowNum");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                Button print = (Button)e.Item.FindControl("btnPrintRow");
                if (print != null && entity != null)
                    print.OnClientClick = string.Format("javascript:doPagePopup('/Admin/PrintPackList.aspx?ship={0}','false')", entity.Id);

                DataList dlItem = (DataList)e.Item.FindControl("dlItem");
                if (dlItem != null && entity != null)
                {
                    //get the invoiceId
                    int invoiceId = entity.InvoiceId;

                    //get the list from fulfillment
                    List<Wcss.QueryRow.ShippingItemRow> items = new List<Wcss.QueryRow.ShippingItemRow>();
                    //items.AddRange(Atx.CurrentShippingFulfillment.ShippingItems.FindAll(delegate(Wcss.QueryRow.ShippingItemRow match) { return (match.tInvoiceId == invoiceId); }));

                    items.AddRange(Atx.CurrentShippingFulfillment.ShippingItems.FindAll(delegate(Wcss.QueryRow.ShippingItemRow match) 
                    { 
                        return (match.tInvoiceId == invoiceId && match.tTicketShipItemId == entity.ShipItemId); 
                    }));

                    //this is done in the proc
                    //if(items.Count > 1)
                    //    items.Sort(delegate(Wcss.QueryRow.ShippingItemRow x, Wcss.QueryRow.ShippingItemRow y) { return (x..CompareTo(y.Id)); } );

                    dlItem.DataSource = items;
                    dlItem.DataBind();
                }
            }
        }
        protected void dlItem_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataList dl = (DataList)sender;

            if (e.Item.DataItem != null)
            {
                Wcss.QueryRow.ShippingItemRow itemRow = (Wcss.QueryRow.ShippingItemRow)e.Item.DataItem;

                if (itemRow != null)
                {
                    //get the matching ticket
                    Wcss.QueryRow.ShippingTicketRow ticketRow = Atx.CurrentShippingFulfillment.AllShippableShowTickets
                        .Find(delegate(Wcss.QueryRow.ShippingTicketRow match) { return (match.Id == itemRow.tShowTicketId); });

                    if (ticketRow != null)
                    {
                        Literal litInfo = (Literal)e.Item.FindControl("litInfo");
                        if (litInfo != null)
                            litInfo.Text = (ticketRow != null) ? ticketRow.TicketInfo : string.Empty;
                    }

                    TextBox txtTicketNumbers = (TextBox)e.Item.FindControl("txtTicketNumbers");
                    if (txtTicketNumbers != null)
                        txtTicketNumbers.Text = itemRow.TicketNumbers;
                }
            }
        }
        protected void Listing_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (Atx.CurrentShipmentBatch != null && Atx.CurrentShippingFulfillment != null)
            {
                ListView list = (ListView)sender;                

                string cmd = e.CommandName.ToLower();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                SubSonic.QueryCommand qry = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
                string fileAttachmentName = string.Empty;

                switch (cmd)
                {
                    case "savepage":
                    case "saverow":

                        #region Save Page

                        sb.Length = 0;

                        List<ListViewDataItem> invoiceShipmentsToProcess = new List<ListViewDataItem>();

                        if (cmd == "savepage")
                            invoiceShipmentsToProcess.AddRange(list.Items);
                        else
                        {
                            ListViewDataItem lvdi = (ListViewDataItem)e.Item;
                            invoiceShipmentsToProcess.Add(lvdi);
                        }

                        //loop through grid and save changes
                        foreach (ListViewDataItem lvi in invoiceShipmentsToProcess)
                        {
                            int invShipmentId = (int)list.DataKeys[lvi.DataItemIndex]["Id"];
                            Wcss.QueryRow.ShippingInvoiceShipmentRow entity = Atx.CurrentShippingFulfillment.InvoiceShipments
                                .Find(delegate(Wcss.QueryRow.ShippingInvoiceShipmentRow match) { return (match.Id == invShipmentId); });

                            //save tracking and shipactual
                            TextBox txtTracking = (TextBox)lvi.FindControl("txtTracking");
                            TextBox txtActual = (TextBox)lvi.FindControl("txtActual");
                            //CheckBox chkPrinted = (CheckBox)lvi.FindControl("chkPrinted");

                            if (txtTracking != null && txtActual != null && entity != null)
                            {
                                //validate input
                                string inputActual = txtActual.Text.Trim();
                                if (!Utils.Validation.IsDecimal(inputActual))
                                {
                                    CustomValidator.IsValid = false;
                                    CustomValidator.ErrorMessage = "Please enter a valid amount";
                                    sb.Length = 0;
                                    return;
                                }

                                sb.AppendFormat("UPDATE [InvoiceShipment] SET [TrackingInformation] = @track_{0} WHERE [Id] = @invsId_{0} AND [TrackingInformation] <> @track_{0}; ", entity.Id.ToString());
                                sb.AppendFormat("UPDATE [InvoiceShipment] SET [mShippingActual] = @actual_{0} WHERE [Id] = @invsId_{0} AND [mShippingActual] <> @actual_{0}; ", entity.Id.ToString());
                                //sb.AppendFormat("UPDATE [InvoiceShipment] SET [bLabelPrinted] = @isPrinted_{0} WHERE [Id] = @invsId_{0} AND [bLabelPrinted] <> @isPrinted_{0}; ", entity.Id.ToString());

                                qry.Parameters.Add(string.Format("@invsId_{0}", entity.Id.ToString()), entity.Id, DbType.Int32);
                                qry.Parameters.Add(string.Format("@track_{0}", entity.Id.ToString()), txtTracking.Text.Trim(), DbType.String);
                                qry.Parameters.Add(string.Format("@actual_{0}", entity.Id.ToString()), decimal.Parse(inputActual), DbType.Decimal);
                                //qry.Parameters.Add(string.Format("@isPrinted_{0}", entity.Id.ToString()), chkPrinted.Checked, DbType.Boolean);
                            }

                            //loop thru repeater Items
                            DataList dl = (DataList)lvi.FindControl("dlItem");
                            if (dl != null)
                            {
                                foreach (DataListItem dli in dl.Items)
                                {
                                    int itemIdx = (int)dl.DataKeys[dli.ItemIndex];

                                    Wcss.QueryRow.ShippingItemRow item = Atx.CurrentShippingFulfillment.ShippingItems
                                        .Find(delegate(Wcss.QueryRow.ShippingItemRow match) { return (match.Id == itemIdx); });

                                    //Wcss.QueryRow.ShippingItemRow item = (Wcss.QueryRow.ShippingItemRow)dli.DataItem;
                                    if (item != null)
                                    {
                                        TextBox txtTicketNumbers = (TextBox)dli.FindControl("txtTicketNumbers");
                                        if (txtTicketNumbers != null)
                                        {
                                            string inputNotes = txtTicketNumbers.Text.Trim();
                                            string oldNotes = item.TicketNumbers;

                                            if (oldNotes != inputNotes)
                                            {
                                                //store the physical ticket numbers for the invoiceitem in entity value
                                                //if we dont have an entityvalue - then create a new one
                                                sb.AppendFormat("IF NOT EXISTS (SELECT * FROM [EntityValue] ev ");
                                                sb.AppendFormat("WHERE ev.[vcTableRelation] = 'InvoiceItem' AND ev.[vcValueContext] = 'TicketNumbers' ");
                                                sb.AppendFormat("AND ev.[tParentId] = @itemId_{0}) BEGIN ", itemIdx.ToString());
                                                sb.AppendFormat("INSERT [EntityValue] ([dtCreated],[dtModified],[UserId],[iDisplayOrder],[vcContext],[vcTableRelation],");
                                                sb.AppendFormat("[tParentId],[vcValueContext],[vcValueType],[vcValue] ) ");
                                                sb.AppendFormat("VALUES (getDate(), getDate(), null, 0, null, 'InvoiceItem', @itemId_{0}, 'TicketNumbers', 'String', @notes_{0}) ", itemIdx.ToString());
                                                sb.AppendFormat("END ELSE BEGIN ");
                                                //otherwise update
                                                sb.AppendFormat("UPDATE [EntityValue] SET [vcValue] = @notes_{0}, [dtModified] = getDate() FROM [EntityValue] ev WHERE  ev.[vcTableRelation] = 'InvoiceItem' AND ", itemIdx.ToString());
                                                sb.AppendFormat("ev.[vcValueContext] = 'TicketNumbers' AND ev.[tParentId] = @itemId_{0} ", itemIdx.ToString());
                                                sb.AppendFormat("END ");

                                                qry.Parameters.Add(string.Format("@itemId_{0}", itemIdx.ToString()), itemIdx, DbType.Int32);
                                                qry.Parameters.Add(string.Format("@notes_{0}", itemIdx.ToString()), inputNotes, DbType.String);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        break;
                    case "printpage":
                        //have a cover page - report of items printed - summary?
                        break;
                    case "printall":
                        break;


                    case "csvpage":
                        sb.Length = 0;

                        fileAttachmentName = string.Format("attachment; filename=TicketList_Batch_Page{0}_{1}.csv", (GooglePager1.PageIndex + 1).ToString(),
                            Atx.CurrentShipmentBatch.Name).Replace("-", string.Empty).Replace("__", "_");

                        WorldshipRow.CSV_ProvideDownload(Atx.CurrentShippingFulfillment.InvoiceShipments, Atx.CurrentShippingFulfillment.ShippingItems, 
                            fileAttachmentName, null);
                        
                        break;
                    case "csvall":
                        sb.Length = 0;
                        
                        fileAttachmentName = string.Format("attachment; filename=TicketList_Batch_Entire_{0}.csv", 
                            Atx.CurrentShipmentBatch.Name).Replace("-", string.Empty).Replace("__", "_");

                        List<WorldshipRow> worldshipList = WorldshipRow.GetWorldshipExportList(Atx.CurrentShipmentBatch.Id, "all");//allow filter choice in future?

                        WorldshipRow.CSV_ProvideDownload(worldshipList, fileAttachmentName, null);

                        break;
                }

                if (sb.Length > 0)
                {
                    qry.CommandSql = sb.ToString();

                    try
                    {
                        SubSonic.DataService.ExecuteQuery(qry);
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        CustomValidator.IsValid = false;
                        CustomValidator.ErrorMessage = ex.Message;
                        return;
                    }

                    Atx.CurrentShippingFulfillment = null;
                    BindListing();
                }
            }
        }
        protected void btnUndoBatch_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (Atx.CurrentShipmentBatch != null)
            {
                ShipmentBatch.UndoBatch(Atx.CurrentShipmentBatch);
                Atx.CurrentShipmentBatch = null;
                Atx.TransactionProcessingVariables = "processing";
                base.Redirect("/Admin/ProcessingShipmentBatch.aspx?redir=batchview");
            }
        }
        protected void CSV_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string fileAttachmentName = string.Empty;

            if (Atx.CurrentShipmentBatch != null)
            {
                if (btn.CommandName == "csvall")
                {
                    fileAttachmentName = string.Format("attachment; filename=TicketList_Batch_Entire_{0}.csv",
                        Utils.ParseHelper.StripInvalidChars_Filename(Atx.CurrentShipmentBatch.Name));

                    List<WorldshipRow> worldshipList = WorldshipRow.GetWorldshipExportList(Atx.CurrentShipmentBatch.Id, "all");//allow filter choice in future?

                    WorldshipRow.CSV_ProvideDownload(worldshipList, fileAttachmentName, null);
                }
                else
                {
                    fileAttachmentName = string.Format("attachment; filename=TicketList_Batch_Page{0}_{1}.csv", (GooglePager1.PageIndex + 1).ToString(),
                        Utils.ParseHelper.StripInvalidChars_Filename(Atx.CurrentShipmentBatch.Name));

                    WorldshipRow.CSV_ProvideDownload(Atx.CurrentShippingFulfillment.InvoiceShipments, Atx.CurrentShippingFulfillment.ShippingItems,
                        fileAttachmentName, null);
                }
            }
        }

        #endregion
    }
}
//091029 - 424 lines