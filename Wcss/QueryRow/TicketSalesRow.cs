using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace Wcss.QueryRow
{
    [Serializable]
    public partial class TicketSalesRow
    {
        private int _parentInvoiceId;
        private string _uniqueInvoiceId;
        private int _itemId;
        private int _showTicketId;
        private int _shipId;
        private string _purchaseName;
        private string _pickupName;
        private string _nameOnCard;
        private string _lastFour;
        private string _email;
        private string _phoneBilling;
        private string _phoneShipping;
        private string _phoneProfile;
        private int _qty;
        private string _productName;
        private string _age;
        private string _notes;
        private bool _isReturned;
        private string _dateShipped;
        private string _shippingMethod;
        private string _shippingNotes;

        public int ParentInvoiceId { get { return _parentInvoiceId; } set { _parentInvoiceId = value; } }
        public string UniqueInvoiceId { get { return _uniqueInvoiceId; } set { _uniqueInvoiceId = value; } }
        public int ItemId { get { return _itemId; } set { _itemId = value; } }
        public int ShowTicketId { get { return _showTicketId; } set { _showTicketId = value; } }
        public int ShipId { get { return _shipId; } set { _shipId = value; } }
        public string PurchaseName { get { return _purchaseName; } set { _purchaseName = value; } }
        public string PickupName { get { return _pickupName; } set { _pickupName = value; } }
        public string NameOnCard { get { return _nameOnCard; } set { _nameOnCard = value; } }
        public string LastFour { get { return _lastFour; } set { _lastFour = value; } }
        public string Email { get { return _email; } set { _email = value;  } }
        public string PhoneBilling { get { return _phoneBilling; } set { _phoneBilling = value; } }
        public string PhoneShipping { get { return _phoneShipping; } set { _phoneShipping = value; } }
        public string PhoneProfile { get { return _phoneProfile; } set { _phoneProfile = value; } }
        public int Qty { get { return _qty; } set { _qty = value; } }
        public string ProductName { get { return _productName; } set { _productName = value; } }
        public string Age { get { return _age; } set { _age = value; } }
        public string Notes { get { return _notes; } set { _notes = value; } }
        public bool IsReturned { get { return _isReturned; } set { _isReturned = value; } }
        public string DateShipped { get { return _dateShipped; } set { _dateShipped = value; } }
        public string ShippingMethod { get { return _shippingMethod; } set { _shippingMethod = value; } }
        public string ShippingNotes { get { return _shippingNotes; } set { _shippingNotes = value; } }

        public TicketSalesRow(IDataReader dr)
        {
            ParentInvoiceId = (int)dr.GetValue(dr.GetOrdinal("ParentInvoiceId"));
            UniqueInvoiceId = dr.GetValue(dr.GetOrdinal("UniqueInvoiceId")).ToString();
            ItemId = (int)dr.GetValue(dr.GetOrdinal("ItemId"));
            ShowTicketId = (int)dr.GetValue(dr.GetOrdinal("ShowTicketId"));

            object shp = dr.GetValue(dr.GetOrdinal("ShipId"));
            ShipId = (shp != null && shp.ToString().Trim().Length > 0) ? (int)shp : 0;

            PurchaseName = dr.GetValue(dr.GetOrdinal("PurchaseName")).ToString();
            PickupName = dr.GetValue(dr.GetOrdinal("PickupName")).ToString();
            NameOnCard = dr.GetValue(dr.GetOrdinal("NameOnCard")).ToString();
            LastFour = dr.GetValue(dr.GetOrdinal("LastFour")).ToString();
            Email = dr.GetValue(dr.GetOrdinal("Email")).ToString();
            PhoneBilling = dr.GetValue(dr.GetOrdinal("PhoneBilling")).ToString();
            PhoneShipping = dr.GetValue(dr.GetOrdinal("PhoneShipping")).ToString();
            PhoneProfile = dr.GetValue(dr.GetOrdinal("PhoneProfile")).ToString();
            Qty = (int)dr.GetValue(dr.GetOrdinal("Qty"));
            ProductName = dr.GetValue(dr.GetOrdinal("ProductName")).ToString();
            Age = dr.GetValue(dr.GetOrdinal("Age")).ToString();
            Notes = dr.GetValue(dr.GetOrdinal("Notes")).ToString();
            IsReturned = (bool)dr.GetValue(dr.GetOrdinal("bRTS"));
            DateShipped = dr.GetValue(dr.GetOrdinal("DateShipped")).ToString();
            ShippingMethod = dr.GetValue(dr.GetOrdinal("ShippingMethod")).ToString();
            ShippingNotes = dr.GetValue(dr.GetOrdinal("ShippingNotes")).ToString();
        }

        #region Get Emails

        public static List<string> GetEmailOfTicketIdSales(int showDateId, string showTicketIds, 
            string shipContext, string purchaseContext, string sortContext)
        {
            if (shipContext == "Will_Call_Only")
                shipContext = ShipMethod.WillCall;

            if (showDateId > 0 && showTicketIds.Trim().Length > 0 && showTicketIds.Trim() != "0")
                showDateId = 0;
            
            bool emailOnly = true;

            List<string> list = new List<string>();

            using (IDataReader dr = SPs.TxGetTicketSales(showDateId, showTicketIds, ShipMethod.WillCall, sortContext, shipContext, purchaseContext, 
                emailOnly, 0, System.Data.SqlTypes.SqlInt32.MaxValue.Value).GetReader())
            {
                while (dr.Read())
                    list.Add(dr.GetValue(dr.GetOrdinal("PurchaseEmail")).ToString());

                dr.Close();
            }

            return list;
        }

        #endregion

        #region GetTicketSales

        public static List<TicketSalesRow> GetTicketIdSales(int showDateId, string showTicketIds, string shipContext, string purchaseContext,
            string sortContext, int startRowIndex, int maximumRows)
        {
            if (shipContext == "Will_Call_Only")
                shipContext = ShipMethod.WillCall;

            if (showDateId > 0 && showTicketIds.Trim().Length > 0 && showTicketIds.Trim() != "0")
                showDateId = 0;

            bool emailOnly = false;

            List<TicketSalesRow> list = new List<TicketSalesRow>();

            using (IDataReader dr = SPs.TxGetTicketSales(showDateId, showTicketIds, ShipMethod.WillCall, sortContext, shipContext, purchaseContext,
                emailOnly, startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    TicketSalesRow tsr = new TicketSalesRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            return list;
        }
        
        #endregion

        #region Count

        public static int GetTicketIdSalesCount(int showDateId, string showTicketIds, string purchaseContext, string shipContext, string sortContext)
        {

            if (showDateId > 0 && showTicketIds.Trim().Length > 0 && showTicketIds.Trim() != "0")
                showDateId = 0;

            int count = 0;

            using (IDataReader dr = SPs.TxGetTicketSalesCount(showDateId, showTicketIds, ShipMethod.WillCall, shipContext, purchaseContext).GetReader())
            {
                while (dr.Read())
                    count = (int)dr.GetValue(0);

                dr.Close();
            }

            return count;
        }

        #endregion
    }
}
