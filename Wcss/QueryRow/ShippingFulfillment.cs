using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace Wcss.QueryRow
{
    /// <summary>
    /// InvoiceShipment
    /// </summary>
    public class ShippingInvoiceShipmentRow
    {
        private int _id;
        private string _uniqueId;
        private DateTime _invoiceDate;
        private int _tInvoiceId;
        private string _refId;
        private int _tShipItemId;
        private bool _labelPrinted;
        private string _purchaseEmail;
        private string _company;
        private string _firstName;
        private string _lastName;
        private string _address1;
        private string _address2;
        private string _city;
        private string _state;
        private string _zip;
        private string _country;
        private string _phone;
        private DateTime _dtShipped;
        private bool _returnedToSender;
        private string _tracking;
        private string _packing;
        private string _packingAdditional;
        private decimal _shippingActual;
        private string _billingName;

        public int Id { get { return _id; } }
        public string UniqueId { get { return _uniqueId; } }
        public DateTime InvoiceDate { get { return _invoiceDate; } }
        public int InvoiceId { get { return _tInvoiceId; } }
        public string ReferenceId { get { return _refId; } }
        public int ShipItemId { get { return _tShipItemId; } }
        public bool LabelPrinted { get { return _labelPrinted; } }
        public string PurchaseEmail { get { return _purchaseEmail; } }
        public string Company { get { return _company; } }
        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public string Address1 { get { return _address1; } }
        public string Address2 { get { return _address2; } }
        public string City { get { return _city; } }
        public string State { get { return _state; } }
        public string Zip { get { return _zip; } }
        public string Country { get { return _country; } }
        public string Phone { get { return _phone; } }
        public DateTime DateShipped { get { return _dtShipped; } }
        public bool ReturnedToSender { get { return _returnedToSender; } }
        public string Tracking { get { return _tracking; } }//500
        public string PackingList { get { return _packing; } }//2000
        public string PackingAdditional { get { return _packingAdditional; } }//500
        public decimal ShippingActual { get { return decimal.Round(_shippingActual,2); } }
        public string BillingName { get { return _billingName; } }

        public string FirstNameLastName { get { return string.Format("{0} {1}", FirstName, LastName); } }
        public string LastNameFirst { get { return string.Format("{0}, {1}", Utils.ParseHelper.TitleCase(LastName), Utils.ParseHelper.TitleCase(FirstName)); } }

        public string AddressBlockFormatted
        {
            get
            {
                return string.Format("{0}{1}<br/>{2}<br/>{3}{4}<br/>{5}<br/>{6}", 
                    (Company.Trim().Length > 0) ? string.Format("{0}<br/>", Company) : string.Empty, 
                    FirstNameLastName,
                    Address1,
                    (Address2.Trim().Length > 0) ? string.Format("{0}<br/>", Address2) : string.Empty, 
                    //city/state
                    string.Format("{0}, {1}", City, State), 
                    //postal/country
                    string.Format("{0}, {1}", Zip, Country), 
                    //phone
                    Phone);
            }
        }

        public string AddressBlockFormatted_LastNameFirst
        {
            get
            {
                return string.Format("{0}{1}<br/>{2}<br/>{3}{4}<br/>{5}<br/>{6}",
                    (Company.Trim().Length > 0) ? string.Format("{0}<br/>", Company) : string.Empty,
                    LastNameFirst,
                    Address1,
                    (Address2.Trim().Length > 0) ? string.Format("{0}<br/>", Address2) : string.Empty,
                    //city/state
                    string.Format("{0}, {1}", City, State),
                    //postal/country
                    string.Format("{0}, {1}", Zip, Country),
                    //phone
                    Phone);
            }
        }

        public ShippingInvoiceShipmentRow(IDataReader dr) 
        {
            try
            {
                _id = (int)dr.GetValue(dr.GetOrdinal("Id"));
                _uniqueId = dr.GetValue(dr.GetOrdinal("UniqueId")).ToString();
                _invoiceDate = (DateTime)dr.GetValue(dr.GetOrdinal("InvoiceDate"));
                _tInvoiceId = (int)dr.GetValue(dr.GetOrdinal("tInvoiceId"));
                _refId = dr.GetValue(dr.GetOrdinal("ReferenceNumber")).ToString();
                _tShipItemId = (int)dr.GetValue(dr.GetOrdinal("tShipItemId"));
                _labelPrinted = (bool)dr.GetValue(dr.GetOrdinal("bLabelPrinted"));
                _purchaseEmail = dr.GetValue(dr.GetOrdinal("PurchaseEmail")).ToString();
                _company = dr.GetValue(dr.GetOrdinal("CompanyName")).ToString();
                _firstName = dr.GetValue(dr.GetOrdinal("FirstName")).ToString();
                _lastName = dr.GetValue(dr.GetOrdinal("LastName")).ToString();
                _address1 = dr.GetValue(dr.GetOrdinal("Address1")).ToString();
                _address2 = dr.GetValue(dr.GetOrdinal("Address2")).ToString();
                _city = dr.GetValue(dr.GetOrdinal("City")).ToString();
                _state = dr.GetValue(dr.GetOrdinal("StateProvince")).ToString();
                _zip = dr.GetValue(dr.GetOrdinal("PostalCode")).ToString();
                _country = dr.GetValue(dr.GetOrdinal("Country")).ToString();
                _phone = dr.GetValue(dr.GetOrdinal("Phone")).ToString();

                object o = dr.GetValue(dr.GetOrdinal("dtShipped"));
                _dtShipped = (o != null) ? (DateTime)o : DateTime.MaxValue;

                _returnedToSender = (bool)dr.GetValue(dr.GetOrdinal("bRTS"));

                _tracking = dr.GetValue(dr.GetOrdinal("TrackingInformation")).ToString();
                //_packingListIds = dr.GetValue(dr.GetOrdinal("PackingListIds")).ToString();
                _packing = dr.GetValue(dr.GetOrdinal("PackingList")).ToString();
                _packingAdditional = dr.GetValue(dr.GetOrdinal("PackingAdditional")).ToString();

                _shippingActual = (decimal)dr.GetValue(dr.GetOrdinal("mShippingActual"));
                _billingName = dr.GetValue(dr.GetOrdinal("BillingName")).ToString();
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }

    }
    /// <summary>
    /// Invoice
    /// </summary>
    public class ShippingInvoiceRow
    {
        private int _id;
        private DateTime _invoiceDate;
        private string _uniqueId;
        private string _lastNameFirst;
        private string _firstNameLastName;
        private string _purchaseEmail;        
        private int _tTicketShipItemId;
        private string _ticketShipMethod;

        //all are readonly
        public int Id { get { return _id; } }
        public DateTime InvoiceDate { get { return _invoiceDate; } }
        public string UniqueId { get { return _uniqueId; } }
        public string LastNameFirst { get { return _lastNameFirst; } }
        public string FirstNameLastName { get { return _firstNameLastName; } }
        public string PurchaseEmail { get { return _purchaseEmail; } }
        public int tTicketShipItemId { get { return _tTicketShipItemId; } }
        public string TicketShipMethod { get { return _ticketShipMethod; } }

        public ShippingInvoiceRow() { }
        public ShippingInvoiceRow(IDataReader dr) 
        {
            try
            {
                _id = (int)dr.GetValue(dr.GetOrdinal("Id"));
                _invoiceDate = (DateTime)dr.GetValue(dr.GetOrdinal("dtInvoiceDate"));
                _uniqueId = dr.GetValue(dr.GetOrdinal("UniqueId")).ToString();
                _lastNameFirst = dr.GetValue(dr.GetOrdinal("LastNameFirst")).ToString();
                _firstNameLastName = dr.GetValue(dr.GetOrdinal("FirstNameLastName")).ToString();
                _purchaseEmail = dr.GetValue(dr.GetOrdinal("PurchaseEmail")).ToString();
                _tTicketShipItemId = (int)dr.GetValue(dr.GetOrdinal("tTicketShipItemId"));
                _ticketShipMethod = dr.GetValue(dr.GetOrdinal("TicketShipMethod")).ToString();
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }
    /// <summary>
    /// InvoiceItem
    /// </summary>
    public class ShippingItemRow
    {
        private int _id;
        private int _tInvoiceId;
        private int _tTicketShipItemId;
        private int _tShowTicketId;
        private int _quantity;
        private string _purchaseName;
        private string _shipMethod;
        private DateTime _dateShipped;
        private string _ticketNumbers;

        public int Id { get { return _id; } }
        public string IdAndShowTicketId { get { return string.Format("{0}~{1}", Id.ToString(), tShowTicketId.ToString()); } }
        public int tInvoiceId { get { return _tInvoiceId; } }
        public int tTicketShipItemId { get { return _tTicketShipItemId; } }
        public int tShowTicketId { get { return _tShowTicketId; } }
        public int Quantity { get { return _quantity; } }
        public string PurchaseName { get { return _purchaseName; } }
        public string ShipMethod { get { return _shipMethod; } }
        public DateTime DateShipped { get { return _dateShipped; } }
        public string TicketNumbers { get { return _ticketNumbers; } }

        //public string TicketInfo { get { return string.Format("ID: {0} {1} + {2} {3} {4}", Id, Price.ToString("c"), ServiceCharge.ToString("n2"), AgeName, Description); } }

        public ShippingItemRow(IDataReader dr) 
        {
            try
            {
                _id = (int)dr.GetValue(dr.GetOrdinal("Id"));
                _tInvoiceId = (int)dr.GetValue(dr.GetOrdinal("tInvoiceId"));
                _tTicketShipItemId = (int)dr.GetValue(dr.GetOrdinal("tShipItemId"));
                _tShowTicketId = (int)dr.GetValue(dr.GetOrdinal("tShowTicketId"));
                _quantity = (int)dr.GetValue(dr.GetOrdinal("iQuantity"));
                _purchaseName = dr.GetValue(dr.GetOrdinal("PurchaseName")).ToString();
                _shipMethod = dr.GetValue(dr.GetOrdinal("ShippingMethod")).ToString();
                _ticketNumbers = dr.GetValue(dr.GetOrdinal("TicketNumbers")).ToString(); 
                object o = dr.GetValue(dr.GetOrdinal("dtShipped"));
                _dateShipped = (o != null && o.ToString().Trim().Length > 0) ? (DateTime)o : DateTime.MaxValue;
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }
    /// <summary>
    /// ShowTicket
    /// </summary>
    public class ShippingTicketRow
    {
        private int _id;
        private DateTime _dateOfShow;
        private string _criteria;
        private string _description;
        private decimal _price;
        private decimal _serviceCharge;
        private bool _allowShipping;
        private string _ageName;
        private string _showName;
        private int _orderQty;
        private int _itemQty;
        //private int _pkgBaseId = 0;

        public int Id { get { return _id; } }
        public DateTime DateOfShow { get { return _dateOfShow; } }
        public string Criteria { get { return _criteria; } }
        public string Description { get { return _description; } }
        public decimal Price { get { return _price; } }
        public decimal ServiceCharge { get { return _serviceCharge; } }
        public bool AllowShipping { get { return _allowShipping; } }
        public string AgeName { get { return _ageName; } }
        public string ShowName { get { return _showName; } }
        public int OrderQty { get { return _orderQty; } }
        public int ItemQty { get { return _itemQty; } }
        //public int PkgBaseId { get { return _pkgBaseId; } set { _pkgBaseId = value; } }

        //public string TicketInfo { get { return string.Format("ID: {0} {1} {2} {3} + {4} {5} {6} {7}", Id, DateOfShow.ToString("MM/dd/yyyy h:mmtt"), 
        //    ShowName, 
        //    Price.ToString("c"), ServiceCharge.ToString("n2"), AgeName, Description, Criteria); } }

        public string TicketInfo
        {
            get
            {
                return string.Format("ID: {0} {1}", Id, TicketInfo_Short);
            }
        }

        public string TicketInfo_Divs
        {
            get
            {
                return string.Format("<div>ID: {0}</div><div>{1}</div>", Id, TicketInfo_Short);
            }
        }

        public string TicketInfo_Short
        {
            get
            {
                string desc = string.Format("{0} {1}", Description, Criteria).Trim();

                return string.Format("{0} {1} {2} + {3} {4} {5}", 
                    ShowTicket.IsCampingPass(desc) ? "CAMPING" : DateOfShow.ToString("MM/dd/yyyy h:mmtt"),
                    ShowName,
                    Price.ToString("c"), ServiceCharge.ToString("n2"), AgeName, desc);
            }
        }

        public string QtyInfo { get { return string.Format("o{0}/t{1}", OrderQty.ToString(), ItemQty.ToString()); } }

        private string _packingInfo = null;
        public string PackingListInfo 
        {
            get
            {
                if (_packingInfo == null)
                {
                    string desc = string.Format("{0} {1}", Description, Criteria).Trim();

                    _packingInfo = string.Format("{0} {1} {2} {3}",
                        ShowTicket.IsCampingPass(desc) ? "CAMPING" : DateOfShow.ToString("MM/dd/yyyy h:mmtt"), 
                        ShowName, AgeName, desc).Trim();

                    if (_packingInfo.Length > 100)
                        _packingInfo = _packingInfo.Substring(0, 100).Trim();
                }

                return _packingInfo;

            }
        }
        public ShippingTicketRow(IDataReader dr) 
        {
            try
            {
                _id = (int)dr.GetValue(dr.GetOrdinal("Id"));
                _dateOfShow = (DateTime)dr.GetValue(dr.GetOrdinal("DateOfShow"));
                _criteria = dr.GetValue(dr.GetOrdinal("CriteriaText")).ToString();
                _description = dr.GetValue(dr.GetOrdinal("SalesDescription")).ToString();
                _price = (decimal)dr.GetValue(dr.GetOrdinal("mPrice"));
                _serviceCharge = (decimal)dr.GetValue(dr.GetOrdinal("mServiceCharge"));
                _allowShipping = (bool)dr.GetValue(dr.GetOrdinal("bAllowShipping"));
                _ageName = dr.GetValue(dr.GetOrdinal("AgeName")).ToString();
                _showName = dr.GetValue(dr.GetOrdinal("ShowName")).ToString();
                _orderQty = (int)dr.GetValue(dr.GetOrdinal("OrderQty"));
                _itemQty = (int)dr.GetValue(dr.GetOrdinal("ItemQty"));
            }
            catch(Exception ex)
            {
                _Error.LogException(ex);
            }
        }

        #region Method for fake construction of ticket rows

        
        public static List<Wcss.QueryRow.ShippingTicketRow> GetTicketRows(List<string> ticketList)
        {
            //PackageColorsAssigned.Clear();

            List<Wcss.QueryRow.ShippingTicketRow> list = new List<Wcss.QueryRow.ShippingTicketRow>();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendFormat("SELECT st.[Id], st.[dtDateOfShow] as 'DateOfShow', SUBSTRING(s.[Name], 22, LEN(s.[Name])) as 'ShowName', ");
            sb.AppendFormat("st.[CriteriaText], st.[SalesDescription], st.[mPrice], st.[mServiceCharge], ISNULL(st.[bAllowShipping],0) as 'bAllowShipping', ");
            sb.AppendFormat("a.[Name] as 'AgeName', -1 as 'OrderQty', -1 as 'ItemQty' ");
            sb.AppendFormat("FROM ");//[#tmpDistinctTicketList] t ");
            //sb.AppendFormat("LEFT OUTER JOIN [#tmpTicketAggs] ag ON t.[Id] = ag.[tShowTicketId], ");
            sb.AppendFormat("[ShowTicket] st LEFT OUTER JOIN [Age] a ON st.[tAgeId] = a.[Id], [Show] s ");
            sb.AppendFormat("WHERE	st.[Id] IN (@showticketlist)  AND st.[tShowId] = s.[Id] ");
            sb.AppendFormat("ORDER BY st.[dtDateOfShow] ");

            string showTicketList = Utils.ParseHelper.SplitListIntoString<string>(ticketList, false);

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@showTicketList", showTicketList, System.Data.DbType.String);

            using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
            {
                while (dr.Read())
                {
                    ShippingTicketRow row = new ShippingTicketRow(dr);
                    list.Add(row);
                }
            }

            //go thru the list and get the pkg ticket ids
            foreach (ShippingTicketRow str in list)
            {

            }

            return list;
        }

        #endregion
    }

    public class ShippingFulfillment
    {
        private readonly string _BATCHSORTMETHOD = "lastnamefirst";//"firstnamelastname";

        public enum SortMethod
        {
            LastNameFirst,
            //FirstNameLastName,
            PurchaseEmail,
            InvoiceDate
        }
        public enum FilterMethod
        {
            all,
            notshippedonly
        }

        #region Props

        private int _eventId;
        private List<int> _ticketIds = new List<int>();
        private List<string> _shipMethods = new List<string>();

        protected int EventId { get { return _eventId; } set { _eventId = value; } }
        protected List<int> TicketIds { get { return _ticketIds; } set { _ticketIds = value; } }
        protected List<string> ShipMethods { get { return _shipMethods; } set { _shipMethods = value; } }

        private List<ShippingTicketRow> _showTickets = new List<ShippingTicketRow>();
        private List<ShippingInvoiceShipmentRow> _invoiceShipments = new List<ShippingInvoiceShipmentRow>();
        private List<ShippingInvoiceRow> _shippingInvoices = new List<ShippingInvoiceRow>();
        private List<ShippingItemRow> _shippingItems = new List<ShippingItemRow>();

        /// <summary>
        /// This will be a list of all the tickets that were ordered that are shippable?
        /// </summary>
        public List<ShippingTicketRow> AllShippableShowTickets { get { return _showTickets; } set { _showTickets = value; } }
        public List<ShippingInvoiceRow> ShippingInvoices { get { return _shippingInvoices; } set { _shippingInvoices = value; } }
        public List<ShippingInvoiceShipmentRow> InvoiceShipments { get { return _invoiceShipments; } set { _invoiceShipments = value; } }
        public List<ShippingItemRow> ShippingItems { get { return _shippingItems; } set { _shippingItems = value; } }

        //public List<int> BasePackageIds = new List<int>();

        //public int GetBasePackageIndex(ShippingTicketRow row)
        //{
        //    return GetBasePackageIndex(row.PkgBaseId);
        //}
        //public int GetBasePackageIndex(int baseIdx)
        //{
        //    int idx = BasePackageIds.FindIndex(delegate(int match) { return (match == baseIdx); });
        //    return (idx != -1) ? idx + 1 : -1;
        //}

        #endregion

        #region Constructors and Public methods

        public static ShippingFulfillment GetShippingFulfillment(int eventId, string ticketIds, ShippingFulfillment.SortMethod sortMethod,
            ShippingFulfillment.FilterMethod filterMethod, int startRowIndex, int maximumRows)
        {
            return new ShippingFulfillment(eventId, ticketIds, sortMethod, filterMethod, startRowIndex, maximumRows);
        }
        public static int GetShippingFulfillment_Count(string ticketIds, ShippingFulfillment.FilterMethod filterMethod)
        {
            int count = 0;

            using (IDataReader dr = SPs.TxShippingFulfillmentItemsCount(ticketIds, filterMethod.ToString(), ShipMethod.WillCall).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }

        public static ShippingFulfillment GetBatchShipments(int eventId, int batchId, int maximumRows, int startRowIndex)
        {
            return new ShippingFulfillment(eventId, batchId, startRowIndex, maximumRows);
        }
        public static int GetBatchShipments_Count(int batchId)
        {
            int count = 0;

            using (IDataReader dr = SPs.TxShippingBatchListingCount(batchId).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }

         public ShippingFulfillment() { }
        /// <summary>
        /// Leave ship method up to client to highlight/differentiate
        /// </summary>
        /// <param name="showDate"></param>
        /// <param name="ticketIds">This should only be shippable tickets</param>
        /// <param name="shipMethods"></param>
        /// <param name="sortMethod"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        public ShippingFulfillment(int eventId, string ticketIds, ShippingFulfillment.SortMethod sortMethod, ShippingFulfillment.FilterMethod filterMethod, 
            int startRowIndex, int maximumRows)
        {
            EventId = eventId;

            //we only have one method for ticket shipping now - so ignore ship methods

            //todo: deal with startindex and max rows
            startRowIndex = 1;
            maximumRows = 200000;

            ShippingInvoices.Clear();
            ShippingItems.Clear();
            AllShippableShowTickets.Clear();
            InvoiceShipments.Clear();

            //fill invoices, items and tickets
            using (IDataReader dr = SPs.TxShippingFulfillmentItems(ticketIds, sortMethod.ToString(), filterMethod.ToString(), 
                ShipMethod.WillCall, startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    ShippingInvoiceRow row = new ShippingInvoiceRow(dr);
                    ShippingInvoices.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    ShippingItemRow row = new ShippingItemRow(dr);
                    ShippingItems.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    ShippingTicketRow row = new ShippingTicketRow(dr);
                    AllShippableShowTickets.Add(row);
                }
            }
        }

        /// <summary>
        /// This constructor is for Batch Listings
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="ticketIds"></param>
        /// <param name="sortMethod"></param>
        /// <param name="filterMethod"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        public ShippingFulfillment(int eventId, int batchId, int startRowIndex, int maximumRows)
        {
            EventId = eventId;

            //we only have one method for ticketshipping now - so ignore ship methods

            ShippingInvoices.Clear();
            ShippingItems.Clear();
            AllShippableShowTickets.Clear();
            InvoiceShipments.Clear();

            //fill invoices, items and tickets
            using (IDataReader dr = SPs.TxShippingBatchListing(batchId, _BATCHSORTMETHOD, startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    ShippingInvoiceShipmentRow row = new ShippingInvoiceShipmentRow(dr);
                    InvoiceShipments.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    ShippingItemRow row = new ShippingItemRow(dr);
                    ShippingItems.Add(row);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    ShippingTicketRow row = new ShippingTicketRow(dr);
                    AllShippableShowTickets.Add(row);
                }
            }
        }

        #endregion

    }
}