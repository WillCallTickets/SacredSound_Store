using System;
using System.Collections.Generic;
using System.Data;

namespace Wcss.QueryRow
{
    [Serializable]
    public partial class InventoryDiscrep_TicketRow
    {
        /*
         * [ShowName],[ShowDate],[tShowId] as 'ShowId', [ShowTicketId],[Alloted],[Pending],[Sold],[Available],[Refunded], 
		[InStockHolding],[ii_Purchased] as 'Purchased_Actual',[ii_PurchasedThenRemoved] as 'Removed_Actual',[ii_NotYetPurchased] as 'NotYetPurchased_Actual', 
		[Sold_Disc],[Pending_Disc],[Refund_Disc],
		a.[Name] as 'AgeDescription', st.[bActive] as 'Active', st.[bSoldOut] as 'SoldOut', st.[bDosTicket] as 'DosTicket', st.[mPrice] as 'Price', 
		st.[mServiceCharge] as 'ServiceCharge', st.[Status]
         */
       
        private string _showName;
        private DateTime _showDate;
        private DateTime _onsale;
        private DateTime _offsale;
        private int _showTicketId;
        private int _showDateId;
        private int _showId;
        private int _allot;
        private int _pend;
        private int _sold;
        private int _avail;
        private int _refund;
        private int _purchasedActual;
        private int _removedActual;
        private int _notYetPurchasedActual;
        private bool _soldDisc;
        private bool _refundDisc;
        private string _ageName;
        private bool _isActive;
        private bool _isSoldOut;
        //private bool _isPrivateShow;
        private bool _isDosTicket;
        private decimal _price;
        private decimal _serviceCharge;
        private string _status;
        private string _salesDescription;
        private string _criteriaText;

        public string   ShowName { get { return _showName; } set { _showName = value; } }
        public DateTime ShowDate { get { return _showDate; } set { _showDate = value; } }
        public int      ShowId { get { return _showId; } set { _showId = value; } }
        public int      ShowDateId { get { return _showDateId; } set { _showDateId = value; } }
        public int      ShowTicketId { get { return _showTicketId; } set { _showTicketId = value; } }

        public int      Allot { get { return _allot; } set { _allot = value; } }
        public int      Pend { get { return _pend; } set { _pend = value; } }
        public int      Sold { get { return _sold; } set { _sold = value; } }
        public int      Avail { get { return _avail; } set { _avail = value; } }
        public int      Refund { get { return _refund; } set { _refund = value; } }

        public int      PurchasedActual { get { return _purchasedActual; } set { _purchasedActual = value; } }
        public int      RemovedActual { get { return _removedActual; } set { _removedActual = value; } }
        public int      NotYetPurchasedActual { get { return _notYetPurchasedActual; } set { _notYetPurchasedActual = value; } }
        public bool     SoldDisc { get { return _soldDisc; } set { _soldDisc = value; } }
        public bool     RefundDisc { get { return _refundDisc; } set { _refundDisc = value; } }

        public string   AgeName { get { return _ageName; } set { _ageName = value; } }
        public bool     IsActive { get { return _isActive; } set { _isActive = value; } }
        public DateTime OffSaleDate { get { return _offsale; } set { _offsale = value; } }
        public DateTime OnSaleDate { get { return _onsale; } set { _onsale = value; } }
        public bool     IsSoldOut { get { return _isSoldOut; } set { _isSoldOut = value; } }
        //public bool     IsPrivateShow { get { return _isPrivateShow; } set { _isPrivateShow = value; } }
        public bool     IsDosTicket { get { return _isDosTicket; } set { _isDosTicket = value; } }
        public decimal  Price { get { return _price; } set { _price = value; } }
        public decimal  ServiceCharge { get { return _serviceCharge; } set { _serviceCharge = value; } }
        public decimal  TotalPrice { get { return Price + ServiceCharge; } }
        public string   Status { get { return _status; } set { _status = value; } }
        public string   SalesDescription { get { return _salesDescription; } set { _salesDescription = value; } }
        public string   CriteriaText { get { return _criteriaText; } set { _criteriaText = value; } }

        public InventoryDiscrep_TicketRow(IDataReader dr)
        {
            ShowName = dr.GetValue(dr.GetOrdinal("ShowName")).ToString();
            ShowDate = (DateTime)dr.GetValue(dr.GetOrdinal("ShowDate"));
            ShowId = (int)dr.GetValue(dr.GetOrdinal("ShowId"));
            ShowDateId = (int)dr.GetValue(dr.GetOrdinal("ShowDateId"));
            ShowTicketId = (int)dr.GetValue(dr.GetOrdinal("ShowTicketId"));

            Allot = (int)dr.GetValue(dr.GetOrdinal("Alloted"));
            Pend = (int)dr.GetValue(dr.GetOrdinal("Pending"));
            Sold = (int)dr.GetValue(dr.GetOrdinal("Sold"));
            Avail = (int)dr.GetValue(dr.GetOrdinal("Available"));
            Refund = (int)dr.GetValue(dr.GetOrdinal("Refunded"));

            PurchasedActual = (int)dr.GetValue(dr.GetOrdinal("Purchased_Actual"));
            RemovedActual = (int)dr.GetValue(dr.GetOrdinal("Removed_Actual"));
            NotYetPurchasedActual = (int)dr.GetValue(dr.GetOrdinal("NotYetPurchased_Actual"));

            object sdc = dr.GetValue(dr.GetOrdinal("Sold_Disc"));
            SoldDisc = (sdc == null || sdc.ToString().Trim().Length == 0 || sdc.ToString() == "1" || bool.Parse(sdc.ToString()) == true) ?
                true : false;
            
            object rdc = dr.GetValue(dr.GetOrdinal("Refund_Disc"));
            RefundDisc = (rdc == null || rdc.ToString().Trim().Length == 0 || rdc.ToString() == "1" || bool.Parse(rdc.ToString()) == true) ?
                true : false;
            
            AgeName = dr.GetValue(dr.GetOrdinal("AgeName")).ToString();

            object isa = dr.GetValue(dr.GetOrdinal("Active"));
            IsActive = (isa == null || isa.ToString().Trim().Length == 0 || isa.ToString() == "1" || bool.Parse(isa.ToString()) == true) ?
                true : false;
            
            object ons = dr.GetValue(dr.GetOrdinal("OnSaleDate"));
            OnSaleDate = (ons == null || ons.ToString().Trim().Length == 0) ? DateTime.MinValue : (DateTime)ons;

            object offs = dr.GetValue(dr.GetOrdinal("OffSaleDate"));
            OffSaleDate = (offs == null || offs.ToString().Trim().Length == 0) ? DateTime.MaxValue : (DateTime)offs;
            object iso = dr.GetValue(dr.GetOrdinal("SoldOut"));
            IsSoldOut = (iso == null || iso.ToString().Trim().Length == 0 || iso.ToString() == "1" || bool.Parse(iso.ToString()) == true) ?
                true : false;

            //object ips = dr.GetValue(dr.GetOrdinal("PrivateShow"));
            //IsPrivateShow = (ips == null || ips.ToString().Trim().Length == 0 || ips.ToString() == "1" || bool.Parse(ips.ToString()) == true) ?
            //    true : false;

            object dos = dr.GetValue(dr.GetOrdinal("DosTicket"));
            IsDosTicket = (dos == null || dos.ToString().Trim().Length == 0 || dos.ToString() == "1" || bool.Parse(dos.ToString()) == true) ?
                true : false;

            Price = (decimal)dr.GetValue(dr.GetOrdinal("Price"));
            ServiceCharge = (decimal)dr.GetValue(dr.GetOrdinal("ServiceCharge"));
            Status = dr.GetValue(dr.GetOrdinal("Status")).ToString();
            SalesDescription = dr.GetValue(dr.GetOrdinal("SalesDescription")).ToString();
            CriteriaText = dr.GetValue(dr.GetOrdinal("CriteriaText")).ToString();
        }

        public static List<InventoryDiscrep_TicketRow> GetInventoryDiscrepancies_Ticket(DateTime startDate, DateTime endDate, int startRowIndex, int maximumRows)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<InventoryDiscrep_TicketRow> list = new List<InventoryDiscrep_TicketRow>();

            using (IDataReader dr = SPs.TxGetInventoryTicketsDiscrepancies(_Config.APPLICATION_ID, startRowIndex, maximumRows, 
                startDate, endDate).GetReader())
            {
                while (dr.Read())
                {
                    InventoryDiscrep_TicketRow cpr = new InventoryDiscrep_TicketRow(dr);

                    list.Add(cpr);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetInventoryDiscrepancies_TicketCount(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            int count = 0;

            using (IDataReader dr = SPs.TxGetInventoryTicketsDiscrepanciesCount(_Config.APPLICATION_ID, startDate, endDate).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }
    }
}
