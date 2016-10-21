using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

using Wcss;

/*
 * <!--????? if we only ship part of the tickets - how does this show in the orders section - ie does shipping partial mark the whole order as shipped or not?? -->

<!-- Get the invoices that have matching items -->
<!-- show the matching items with selection and entry for ticket numbers in the order -->
<!--
--matching items
context would be ticket
shipitemid would not be null > 0
shippingmethod = @selection


--get invoices that are in that list - order by preference
--page size, page by context - session cookies

-->*/
namespace WillCallWeb.Admin.AdminControls.BulkShipping
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Shipping_FulfillmentCreate : BaseControl
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#srceditor", true);
        }

        #region Page Overhead

        private List<string> SelectedDate_TicketIds
        {
            get
            {
                List<string> idList = new List<string>();

                foreach(ListItem li in chkTickets.Items)
                if(li.Selected)
                    idList.Add(li.Value);

                return idList;
            }
        }

        private List<string> _otherTicketsSelected = null;
        protected List<string> OtherTicketsSelected
        {
            get
            {
                if (_otherTicketsSelected == null)
                    _otherTicketsSelected = new List<string>();

                return _otherTicketsSelected;
            }
            set
            {
                _otherTicketsSelected = value;
            }
        }

        private List<string> GetOtherTicketsSelected
        {
            get
            {
                List<string> list = new List<string>();
                foreach(ListItem li in this.chkOtherTickets.Items)
                    if(li.Selected)
                        list.Add(li.Value);

                return list;
            }
        }

        protected List<string> AllSelectedTickets
        {
            get
            {
                List<string> list = new List<string>();
                
                foreach(ListItem li in chkTickets.Items)
                    if(li.Selected)
                        list.Add(li.Value);
                
                foreach(ListItem li in chkOtherTickets.Items)
                    if(li.Selected)
                        list.Add(li.Value);

                return list;
            }
        }
        
        public Wcss.QueryRow.ShippingFulfillment.SortMethod SortBy
        {
            get
            {
                return (Wcss.QueryRow.ShippingFulfillment.SortMethod)Enum.Parse(typeof(Wcss.QueryRow.ShippingFulfillment.SortMethod), 
                    rblSort.SelectedValue.Trim(), true);
            }
        }
        public Wcss.QueryRow.ShippingFulfillment.FilterMethod FilterBy
        {
            get
            {
                return (Wcss.QueryRow.ShippingFulfillment.FilterMethod)Enum.Parse(typeof(Wcss.QueryRow.ShippingFulfillment.FilterMethod), 
                    rblFilter.SelectedValue.Trim(), true);
            }
        }

        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._otherTicketsSelected = (List<string>)ctlState[1];
            //this._methodsSelected = (List<string>)ctlState[2];
        }

        protected override object SaveControlState()
        {
            object[] ctlState = new object[2];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = OtherTicketsSelected;
            //ctlState[2] = MethodsSelected;
            return ctlState;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Page.RegisterRequiresControlState(this);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                Atx.CurrentShippingFulfillment = null;
            }
        }
       
        #endregion        



        #region Date & Ticket Selection
        
        /// <summary>
        /// set the date to start the dates in the list
        /// </summary>
        protected void SqlDates_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@DateBaseline"].Value = DateTime.Now.AddDays(-3).Date.AddHours(_Config.DayTurnoverTime);
        }
        protected void ddlDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedDate_TicketIds.Clear();
            OtherTicketsSelected.Clear();
            chkTickets.Items.Clear();
            chkOtherTickets.Items.Clear();
            chkTickets.SelectedIndex = -1;
            chkOtherTickets.SelectedIndex = -1;

            Atx.CurrentShippingFulfillment = null;
            //form binding handled by other controls in the binding chain
        }
        protected void ddlDates_DataBound(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if(ddl.SelectedIndex <= 0)
                Atx.CurrentShippingFulfillment = null;
        }

        protected void chkTickets_DataBound(object sender, EventArgs e)
        {
            CheckBoxList list = (CheckBoxList)sender;

            if (list.SelectedIndex == -1 && list.Items.Count > 0)//set default on first entry
            {
                list.Items[0].Selected = true;// = 0;
            }

            BindListing();
        }
        protected void chkTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get the selected ids from other tix and save
            //so that when the list is repopulated the appropriate ids can be checked
            if (SelectedDate_TicketIds.Count == 0)
            {
                OtherTicketsSelected.Clear();
                chkOtherTickets.SelectedIndex = -1;
            }

            BindListing();
        }

        protected void chkOtherTickets_DataBinding(object sender, EventArgs e)
        {
            if (Atx.CurrentShippingFulfillment != null)
            {
                CheckBoxList list = (CheckBoxList)sender;

                //list all other tickets not in the list above
                List<Wcss.QueryRow.ShippingTicketRow> allTix = new List<Wcss.QueryRow.ShippingTicketRow>();
                allTix.AddRange(Atx.CurrentShippingFulfillment.AllShippableShowTickets
                    .FindAll(delegate(Wcss.QueryRow.ShippingTicketRow match) { return (!SelectedDate_TicketIds.Contains(match.Id.ToString())); }));

                List<Wcss.QueryRow.ShippingTicketRow> sortedTix = new List<Wcss.QueryRow.ShippingTicketRow>();

                //now we should order the collection by base pkg id -> then ticket id
                if (allTix.Count > 0)
                {
                    allTix.Sort(new Utils.Reflector.CompareEntities<Wcss.QueryRow.ShippingTicketRow>(Utils.Reflector.Direction.Ascending, "Id"));
                }

                list.DataSource = allTix;
            }
        }
        /// <summary>
        /// iterate thru items and select those that are in control state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkOtherTickets_DataBound(object sender, EventArgs e)
        {
            CheckBoxList list = (CheckBoxList)sender;
            foreach (ListItem li in list.Items)
            {
                if(OtherTicketsSelected.Contains(li.Value))
                    li.Selected = true;
            }
        }
        protected void chkOtherTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList list = (CheckBoxList)sender;

            List<string> selections = new List<string>();
            foreach(ListItem li in list.Items)
                if(li.Selected)
                    selections.Add(li.Value);
            OtherTicketsSelected = selections;

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
        
        #region Calendar for ship date estimate
        
        protected void clock_Init(object sender, EventArgs e)
        {
            WillCallWeb.Components.Util.CalendarClock cal = (WillCallWeb.Components.Util.CalendarClock)sender;

            cal.SelectedDate = DateTime.Now;//.Parse(string.Format("{0}/1/{1}", DateTime.Now.Month, DateTime.Now.Year));
        }
        
        #endregion

        private void BindListing()
        {
            Listing.DataBind();
        }

        #region ListView Control

        protected void Listing_DataBinding(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;

            if (chkTickets.SelectedIndex != -1 && ddlDates.SelectedIndex > 0)
            {
                int dateIdx = int.Parse(ddlDates.SelectedValue);
                string ids = Utils.ParseHelper.SplitListIntoString<string>(this.SelectedDate_TicketIds);
                Atx.CurrentShippingFulfillment = new Wcss.QueryRow.ShippingFulfillment(dateIdx, ids, this.SortBy, this.FilterBy, 0, 100000);

                chkOtherTickets.DataSource = Atx.CurrentShippingFulfillment.AllShippableShowTickets;
                list.DataSource = Atx.CurrentShippingFulfillment.ShippingInvoices;
            }
            else
            {
                chkOtherTickets.DataSource = null;
                list.DataSource = null;
            }

            chkOtherTickets.DataBind();

            //update the ticket listings
            //match the results in full.Tickets and display counts with tickets
            if (Atx.CurrentShippingFulfillment != null)
            {
                foreach (Wcss.QueryRow.ShippingTicketRow str in Atx.CurrentShippingFulfillment.AllShippableShowTickets)
                {
                    if (str.OrderQty >= 0 && str.ItemQty >= 0)
                    {
                        string inventoryText = string.Format("({0}/{1}) ", str.OrderQty.ToString(), str.ItemQty.ToString());

                        //look for a match in the current tickets
                        ListItem match = chkTickets.Items.FindByValue(str.Id.ToString());

                        if (match != null)
                        {
                            string itemText = match.Text;

                            if (!itemText.StartsWith(inventoryText))
                                match.Text = string.Format("{0}{1}", inventoryText, itemText);
                        }
                        else
                        {
                            //look for a match in the other tickets
                            match = chkOtherTickets.Items.FindByValue(str.Id.ToString());

                            if (match != null)
                            {
                                string itemText = match.Text;

                                if (!itemText.StartsWith(inventoryText))
                                    match.Text = string.Format("{0}{1}", inventoryText, itemText);
                            }
                        }
                    }
                }


                ApplyItemDescription();
            }            
        }

        
        /// <summary>
        /// This will apply a color coding to the lists so that package tix relations can be easier to see
        /// </summary>
        protected void ApplyItemDescription()
        {
            //establish packages and matching color
            //save pkg/color pairs

            List<int> BasePackageIds = new List<int>();

            //loop thru tickets
            foreach (ListItem li in chkTickets.Items)
            {
                ShowTicket st = new ShowTicket(li.Value);

                if (st != null)
                {
                    //establish package ids
                    if (st.IsPackage)
                    {
                        int baseId = st.PackageBase.Id;

                        if (!BasePackageIds.Contains(baseId))
                            BasePackageIds.Add(baseId);
                    }


                    if (li.Text.ToLower().IndexOf("<div") == -1)//indicates description has already been applied
                    {
                        if (st.IsPackage)
                        {
                            int existingPkgNum = BasePackageIds.FindIndex(delegate(int match) { return (st.PackageBase.Id == match); });

                            existingPkgNum += 1;//remove zero base

                            li.Text += string.Format(" Package # {0}", existingPkgNum.ToString());
                        }

                        li.Text = string.Format("{0}<div style=\"padding:2px;border:solid #999 1px;\">{1}</div>", li.Text, st.TicketInfo_Short);

                    }
                }
            }

            //loop thru other tickets
            foreach (ListItem li in chkOtherTickets.Items)
            {
                ShowTicket st = new ShowTicket(li.Value);

                //establish package ids
                if (st != null)
                {
                    if (st.IsPackage)
                    {
                        int baseId = st.PackageBase.Id;

                        if (!BasePackageIds.Contains(baseId))
                            BasePackageIds.Add(baseId);
                    }

                    if (li.Text.ToLower().IndexOf("<div") == -1)//indicates description has already been applied
                    {
                        if (st.IsPackage)
                        {
                            int existingPkgNum = BasePackageIds.FindIndex(delegate(int match) { return (st.PackageBase.Id == match); });

                            existingPkgNum += 1;//remove zero base

                            li.Text += string.Format(" Package # {0}", existingPkgNum.ToString());
                        }

                        li.Text = string.Format("{0}<div style=\"padding:2px;border:solid #999 1px;\">{1}</div>", li.Text, st.TicketInfo_Short);
                    }
                }
            }
        }

        protected void Listing_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView list = (ListView)sender;
                ListViewDataItem viewItem = (ListViewDataItem)e.Item;
                Wcss.QueryRow.ShippingInvoiceRow invoiceRow = (Wcss.QueryRow.ShippingInvoiceRow)viewItem.DataItem;
               
                DataList dlItem = (DataList)e.Item.FindControl("dlItem");
                if (dlItem != null)
                {
                    //get the invoiceId
                    int invoiceId = invoiceRow.Id;
                    int shipId = invoiceRow.tTicketShipItemId;
                    
                    //get the list from fulfillment
                    List<Wcss.QueryRow.ShippingItemRow> items = new List<Wcss.QueryRow.ShippingItemRow>();
                    items.AddRange(Atx.CurrentShippingFulfillment.ShippingItems.FindAll(delegate(Wcss.QueryRow.ShippingItemRow match) 
                    { 
                        return (match.tInvoiceId == invoiceId && match.tTicketShipItemId == shipId); 
                    }));
                    
                    dlItem.DataSource = items;
                    dlItem.DataBind();
                }
            }
        }

        protected void dlItem_DataBound(object sender, DataListItemEventArgs e)
        {
            DataList dl = (DataList)sender;

            if (e.Item.DataItem != null)
            {
                Wcss.QueryRow.ShippingItemRow itemRow = (Wcss.QueryRow.ShippingItemRow)e.Item.DataItem;
                if (itemRow != null)
                {
                    bool hasShipped = (itemRow.DateShipped > DateTime.Parse("1/1/2000") && itemRow.DateShipped != DateTime.MaxValue);
                    //get the matching ticket
                    Wcss.QueryRow.ShippingTicketRow ticketRow = Atx.CurrentShippingFulfillment.AllShippableShowTickets.Find(delegate(Wcss.QueryRow.ShippingTicketRow match) { return (match.Id == itemRow.tShowTicketId); } );

                    CheckBox chkSlated = (CheckBox)e.Item.FindControl("chkSlated");
                    if (chkSlated != null)
                    {
                        //if it is in the selected tickets and has not been shipped yet.....
                        chkSlated.Checked = (AllSelectedTickets.Contains(itemRow.tShowTicketId.ToString()) && (!hasShipped));

                        System.Web.UI.HtmlControls.HtmlGenericControl rowDiv = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("rowDiv");
                        if (rowDiv != null)
                        {
                            //set visibility options
                            rowDiv.Attributes.Add("class", (hasShipped) ? "shipped" : (!chkSlated.Checked) ? "highlighted" : string.Empty);
                        }
                    }

                    Literal litInfo = (Literal)e.Item.FindControl("litInfo");
                    if (litInfo != null) 
                        litInfo.Text = (ticketRow != null) ? ticketRow.TicketInfo_Short : string.Empty;

                    Literal litShipping = (Literal)e.Item.FindControl("litShipping");
                    if (litShipping != null)
                        litShipping.Text = (hasShipped) ? string.Format("Shipped On: {0}", itemRow.DateShipped.ToString("MM/dd/yyyy hh:mmtt")) : string.Empty;
                }
            }
        }

        protected void Listing_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            ListView list = (ListView)sender;

            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "selectall":
                case "selectnone":
                    foreach (ListViewItem lvi in list.Items)
                    {
                        CheckBox chkSelect = (CheckBox)lvi.FindControl("chkSelect");
                        if(chkSelect != null)
                            chkSelect.Checked = (cmd == "selectall");
                    }
                    break;
                case "batch":
                    List<string> msgs = new List<string>();
                    //Validate inputs
                    //calendar clock must be a valid date - it is ok to be set to a value earlier than today - for old batches
                    if(clockEstimate == null || (!clockEstimate.HasSelection))
                        msgs.Add("Please select an estimated ship date.");

                    //onclick shows an alert box

                    //must have a date selected from the ddl
                    if(ddlDates.SelectedIndex <= 0 || ddlDates.SelectedValue == "0")
                        msgs.Add("Please select an event from the list.");

                    //we must have at least one ticket selection from the main event ticket list
                    if(chkTickets.SelectedIndex == -1)
                        msgs.Add("Please select a ticket from the tickets for the selected date.");

                    //we must have selections in the listview
                    int selection = -1;
                    foreach (ListViewItem lvi in Listing.Items)
                    {
                        if (lvi.ItemType == ListViewItemType.DataItem)
                        {
                            CheckBox chkSelect = (CheckBox)lvi.FindControl("chkSelect");

                            //foreach INVOICE row that is selected
                            if (chkSelect != null && chkSelect.Checked)
                            {
                                selection = 0;
                                break;
                            }
                        }
                    }

                    if(selection == -1)
                        msgs.Add("Please select items for shipment.");

                    if (msgs.Count > 0)
                    {
                        CustomValidator.IsValid = false;
                        foreach(string s in msgs)
                            CustomValidator.ErrorMessage += string.Format("<li>{0}</li>", s);
                        return;
                    }

                    //get userid
                    System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(Profile.UserName);
                    
                    //create invoice shipments for all selected invoices
                    try
                    {
                        ShipmentBatch batch = ShipmentBatch.CreateBatchFromFulfillment(Atx.CurrentShippingFulfillment,
                            ShipmentBatch.GenerateShipmentBatchId(), ddlDates.SelectedItem.Text, null,
                            int.Parse(ddlDates.SelectedValue), SelectedDate_TicketIds, OtherTicketsSelected,
                            clockEstimate.SelectedDate, Listing, (Guid)mem.ProviderUserKey);

                        //redirect to view shipments
                        Atx.CurrentShipmentBatch = batch;
                        Atx.TransactionProcessingVariables = "processing";
                        base.Redirect("/Admin/ProcessingShipmentBatch.aspx?redir=batchview");
                        //SELECTED DATES,ETC WILL CARRY OVER TO NEXT PAGE
                    }
                    catch (Exception ex)
                    {
                        CustomValidator.IsValid = false;
                        CustomValidator.ErrorMessage = ex.Message;
                    }
                    
                    break;
            }
        }

        #endregion


}
}