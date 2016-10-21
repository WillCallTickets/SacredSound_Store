using System;
using System.Collections.Generic;
using System.Data;

namespace Wcss.QueryRow
{
    public class InventoryCheck
    {
        private int _allotment = 0;
        private int _damaged = 0;
        private int _pending = 0;
        private int _sold = 0;
        private int _available = 0;
        private int _refunded = 0;
        
        public int Allotment { get { return _allotment; } set { _allotment = value; } }
        public int Damaged { get { return _damaged; } set { _damaged = value; } }
        public int Pending { get { return _pending; } set { _pending = value; } }
        public int Sold { get { return _sold; } set { _sold = value; } }
        public int Available { get { return _available; } set { _available = value; } }
        public int Refunded { get { return _refunded; } set { _refunded = value; } }

        private int _idx = 0;
        private string _description = string.Empty;
        private bool _isParent = false;
        public int Idx { get { return _idx; } }
        public string Description { get { return _description; } }
        public bool IsParent { get { return _isParent; } }
        private _Enums.InventoryCheck_Context _context = _Enums.InventoryCheck_Context.Merch;
        public _Enums.InventoryCheck_Context Context { get { return _context; } }

        /// <summary>
        /// This will report for parents or a single child
        /// </summary>
        /// <param name="m"></param>
        public InventoryCheck(Merch m)
        {
            _idx = m.Id;
            _description = m.DisplayNameWithAttribs;
            _isParent = m.IsParent;
            _context = _Enums.InventoryCheck_Context.Merch;

            RunInventoryCheck();
        }
        /// <summary>
        /// This will report for tickets - packages report as a linked entity - one reports for all
        /// </summary>
        /// <param name="m"></param>
        public InventoryCheck(ShowTicket m)
        {
            _idx = m.Id;
            _description = Utils.ParseHelper.StripHtmlTags(m.DisplayNameWithAttribsAndDescription);
            _isParent = false;
            _context = _Enums.InventoryCheck_Context.Ticket;

            RunInventoryCheck();
        }
        /// <summary>
        /// This will report for a salepromotion - decides merch or ticket
        /// </summary>
        /// <param name="m"></param>
        public InventoryCheck(SalePromotion m)
        {
            _idx = m.Id;
            _description = m.DisplayNameWithAttribs;
            _isParent = false;
            _context = (m.IsMerchPromotion) ? _Enums.InventoryCheck_Context.MerchPromo : _Enums.InventoryCheck_Context.TicketPromo;

            RunInventoryCheck();
        }

        private void RunInventoryCheck()
        {
            try
            {
                using (IDataReader dr = SPs.TxInventoryByContextOnId(this.Context.ToString(), this.Idx).GetReader())
                {
                    while (dr.Read())
                    {
                        Allotment = (int)dr.GetValue(dr.GetOrdinal("Allotment"));
                        Damaged = (int)dr.GetValue(dr.GetOrdinal("Damaged"));
                        Pending = (int)dr.GetValue(dr.GetOrdinal("Pending"));
                        Sold = (int)dr.GetValue(dr.GetOrdinal("Sold"));
                        Available = (int)dr.GetValue(dr.GetOrdinal("Available"));
                        Refunded = (int)dr.GetValue(dr.GetOrdinal("Refunded"));
                    }

                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }
}