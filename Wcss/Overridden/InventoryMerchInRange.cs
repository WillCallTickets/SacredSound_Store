using System;
using System.Collections.Generic;
using System.Data;

namespace Wcss
{
    [Serializable]
    public partial class InventoryMerchInRange
    {
        private int _merchId;
        private int _parentId;
        private string _style;
        private string _color;
        private string _size;
        private bool _isActive;
        private bool _isTaxable;
        private bool _isFeatured;
        private bool _isSoldOut;
        private decimal _price;
        private bool _useSalePrice;
        private decimal _salePrice;
        private string _vcDeliveryType;
        private decimal _weight;
        private int _allot;
        private int _dmg;
        private int _pend;
        private int _sold;
        private int _avail;
        private int _refund;
        private int _salesPend;
        private int _salesSold;
        private int _salesRefund;


        public int MerchId { get { return _merchId; } set { _merchId = value; } }
        public int ParentId { get { return _parentId; } set { _parentId = value; } }
        public string Style { get { return _style; } set { _style = value; } }
        public string Color { get { return _color; } set { _color = value; } }
        public string Size { get { return _size; } set { _size = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        public bool IsTaxable { get { return _isTaxable; } set { _isTaxable = value; } }
        public bool IsFeatured { get { return _isFeatured; } set { _isFeatured = value; } }
        public bool IsSoldOut { get { return _isSoldOut; } set { _isSoldOut = value; } }
        public decimal Price { get { return _price; } set { _price = value; } }
        public bool UseSalePrice { get { return _useSalePrice; } set { _useSalePrice = value; } }
        public decimal SalePrice { get { return _salePrice; } set { _salePrice = value; } }
        public string VcDeliveryType { get { return _vcDeliveryType; } set { _vcDeliveryType = value; } }
        public _Enums.DeliveryType DeliveryType
        {
            get
            {
                if (this.VcDeliveryType == null) return _Enums.DeliveryType.parcel;
                return (_Enums.DeliveryType)Enum.Parse(typeof(_Enums.DeliveryType), this.VcDeliveryType, true);
            }
        }

        public decimal Weight { get { return _weight; } set { _weight = value; } }
        public int Allot { get { return _allot; } set { _allot = value; } }
        public int Dmg { get { return _dmg; } set { _dmg = value; } }
        public int Pend { get { return _pend; } set { _pend = value; } }
        public int Sold { get { return _sold; } set { _sold = value; } }
        public int Avail { get { return _avail; } set { _avail = value; } }
        public int Refund { get { return _refund; } set { _refund = value; } }
        public int SalesPend { get { return _salesPend; } set { _salesPend = value; } }
        public int SalesSold { get { return _salesSold; } set { _salesSold = value; } }
        public int SalesRefund { get { return _salesRefund; } set { _salesRefund = value; } }

        public InventoryMerchInRange(IDataReader dr)
        {
            MerchId = (int)dr.GetValue(dr.GetOrdinal("MerchId"));
            ParentId = (int)dr.GetValue(dr.GetOrdinal("ParentId"));
            Style = dr.GetValue(dr.GetOrdinal("Style")).ToString();
            Color = dr.GetValue(dr.GetOrdinal("Color")).ToString();
            Size = dr.GetValue(dr.GetOrdinal("Size")).ToString();

            IsActive = (bool)dr.GetValue(dr.GetOrdinal("IsActive"));
            IsTaxable = (bool)dr.GetValue(dr.GetOrdinal("IsTaxable"));
            IsFeatured = (bool)dr.GetValue(dr.GetOrdinal("IsFeatured"));
            IsSoldOut = (bool)dr.GetValue(dr.GetOrdinal("IsSoldOut"));

            Price = (decimal)dr.GetValue(dr.GetOrdinal("mPrice"));
            UseSalePrice = (bool)dr.GetValue(dr.GetOrdinal("bUseSalePrice"));
            SalePrice = (decimal)dr.GetValue(dr.GetOrdinal("mSalePrice"));
            Weight = (decimal)dr.GetValue(dr.GetOrdinal("mWeight"));
            
            Allot = (int)dr.GetValue(dr.GetOrdinal("Allot"));
            Dmg = (int)dr.GetValue(dr.GetOrdinal("Dmg"));
            Pend = (int)dr.GetValue(dr.GetOrdinal("Pend"));
            Sold = (int)dr.GetValue(dr.GetOrdinal("Sold"));
            Avail  = (int)dr.GetValue(dr.GetOrdinal("Avail"));

            Refund = (int)dr.GetValue(dr.GetOrdinal("Refund"));
            SalesPend = (int)dr.GetValue(dr.GetOrdinal("SalesPend"));
            SalesSold = (int)dr.GetValue(dr.GetOrdinal("SalesSold"));
            SalesRefund = (int)dr.GetValue(dr.GetOrdinal("SalesRefund"));
        }

        /// <summary>
        /// Dates are unnecessary - we are just getting theinventory items
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="style"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="activeStatus"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        public static List<InventoryMerchInRange> GetMerchInventoryInRange(int parentId, string style, string color, string size, 
            string activeStatus, int startRowIndex, int maximumRows)
        {
            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<InventoryMerchInRange> list = new List<InventoryMerchInRange>();

            using (IDataReader dr = SPs.TxGetMerchInventoryInRange(_Config.APPLICATION_ID, parentId, style, color, size, 
                activeStatus, startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    InventoryMerchInRange cpr = new InventoryMerchInRange(dr);

                    list.Add(cpr);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetMerchInventoryInRange_Count(int parentId, string style, string color, string size, string activeStatus)
        {
            if (style != null && style.ToLower().IndexOf("all") != -1)
                style = string.Empty;
            if (color != null && color.ToLower().IndexOf("all") != -1)
                color = string.Empty;
            if (size != null && size.ToLower().IndexOf("all") != -1)
                size = string.Empty;

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            int count = 0;

            using (IDataReader dr = SPs.TxGetMerchInventoryInRangeCount(_Config.APPLICATION_ID, parentId, style, color, size, activeStatus).GetReader())
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
