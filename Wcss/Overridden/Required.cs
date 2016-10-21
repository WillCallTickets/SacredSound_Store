using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using SubSonic;

namespace Wcss
{
    public partial class Required
    {
        #region Table Properties

        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        /// <summary>
        /// If exclusive - the requirement can be the only item in the order. Ignored for shipping contexts. Default is false
        /// </summary>
        [XmlAttribute("IsExclusive")]
        public bool IsExclusive
        {
            get { return this.BExclusive; }
            set { this.BExclusive = value; }
        }
        [XmlAttribute("DateStart")]
        public DateTime DateStart
        {
            get { return (!this.DtStart.HasValue) ? System.Data.SqlTypes.SqlDateTime.MinValue.Value : this.DtStart.Value; }
            set 
            {
                this.DtStart = value; 
            }
        }
        //dtEndDate
        [XmlAttribute("DateEnd")]
        public DateTime DateEnd
        {
            get { return (!this.DtEnd.HasValue) ? System.Data.SqlTypes.SqlDateTime.MaxValue.Value : this.DtEnd.Value; }
            set 
            {
                this.DtEnd = value; 
            }
        }

        private bool _hasStarted { get { return this.DateStart < DateTime.Now; } }
        private bool _hasEnded { get { return this.DateEnd < DateTime.Now; } }
        public bool IsCurrentlyRunning(string unlockCode) 
        { 
          return this.IsActive && this.IsUnlocked(unlockCode) && 
              this._hasStarted && (!this._hasEnded);           
        }
        //TODO
        public bool IsUnlocked(string unlockCode)
        {
            return true;
            //if (this.IsPublicOffer)
            //    return true;

            //if (unlockCode != null && unlockCode.Trim().Length > 0 && unlockCode == this.UnlockCode)
            //    return true;

            //return false;
        }

        /// <summary>
        /// This gives us the context - merchshipping, ticketshipping, ticket, merch - of the required entity
        /// </summary>
        [XmlAttribute("RequiredContext")]
        public _Enums.RequirementContext RequiredContext 
        { 
            get 
            { 
                if(this.VcRequiredContext == null) 
                    return _Enums.RequirementContext.NA;

                return (_Enums.RequirementContext)Enum.Parse(typeof(_Enums.RequirementContext), this.VcRequiredContext, true); 
            } 
            set 
            {
                this.VcRequiredContext = value.ToString();
            } 
        }
        public bool Requires_NotApplicable { get { return this.RequiredContext == _Enums.RequirementContext.NA; } }
        public bool Requires_Merch { get { return this.RequiredContext == _Enums.RequirementContext.merch; } }
        public bool Requires_Ticket { get { return this.RequiredContext == _Enums.RequirementContext.ticket; } }
        public bool Requires_ShowDate { get { return this.RequiredContext == _Enums.RequirementContext.showdate; } }
        public bool Requires_Show { get { return this.RequiredContext == _Enums.RequirementContext.show; } }
        public bool Requires_MerchShipping { get { return this.RequiredContext == _Enums.RequirementContext.merchshipping; } }
        public bool Requires_TicketShipping { get { return this.RequiredContext == _Enums.RequirementContext.ticketshipping; } }
        
        public bool Requires_MinPurchase { get { return this.MinimumAmount > 0; } }
        public bool Requires_MinMerchPurchase { get { return this.RequiredContext == _Enums.RequirementContext.minmerchpurchase; } }
        public bool Requires_MinTicketPurchase { get { return this.RequiredContext == _Enums.RequirementContext.minticketpurchase; } }
        public bool Requires_MinTotalPurchase { get { return this.RequiredContext == _Enums.RequirementContext.mintotalpurchase; } }

        /// <summary>
        /// The listing of ids (could also be text values as in the case of shipping methods) to match on
        /// </summary>
        [XmlAttribute("IdxListing")]
        public string IdxListing
        {
            get { return this.VcIdx; }
            set 
            { 
                _reqIdx.Clear();
                this.VcIdx = value; 
            }
        }

        private List<string> _reqIdx = new List<string>();
        /// <summary>
        /// A list of the ids to match. Allows list methods to match ids
        /// </summary>
        [XmlAttribute("RequiredIdx")]
        public List<string> RequiredIdx
        {
            get
            {
                if (_reqIdx.Count == 0 && this.IdxListing != null)
                {
                    string[] s = this.IdxListing.Split(',');
                    _reqIdx = new List<string>();
                    _reqIdx.AddRange(s);
                }
                else if (_reqIdx.Count > 0 && this.IdxListing == null)
                    _reqIdx.Clear();

                return _reqIdx;
            }
            set
            {
                this.VcIdx = null;
                foreach (string s in value)
                    this.VcIdx += string.Format("{0},", s);

                if(this.VcIdx != null)
                    this.VcIdx = this.VcIdx.TrimEnd(',');
            }
        }
        /// <summary>
        /// Determines if the id is in the required collection. Quantity is not at issue.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool Id_IsInRequiredListing(string idx)
        {
            return Id_IsInRequiredListing(idx, int.MaxValue);
        }
        /// <summary>
        /// Determines if the id is in the required collection and has the necessary quantity.
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public bool Id_IsInRequiredListing(string idx, int qty)
        {
            return (RequiredIdx.Contains(idx) && qty >= RequiredQty);
        }

        //required name is varchar
        [XmlAttribute("RequiredQty")]
        public int RequiredQty
        {
            get { return this.IRequiredQty; }
            set { this.IRequiredQty = value; }
        }
        //mMinMerch
        [XmlAttribute("MinimumAmount")]
        public decimal MinimumAmount
        {
            get { return this.MMinAmount; }
            set { this.MMinAmount = value; }
        }

        //mMinMerch
        [XmlAttribute("MinimumMerchandisePurchase")]
        public decimal MinimumMerchandisePurchase
        {
            get { return (this.RequiredContext == _Enums.RequirementContext.minmerchpurchase) ? this.MinimumAmount : 0; }
        }
        //mMinTicket
        [XmlAttribute("MinimumTicketPurchase")]
        public decimal MinimumTicketPurchase
        {
            get { return (this.RequiredContext == _Enums.RequirementContext.minticketpurchase) ? this.MinimumAmount : 0; }
        }
        //mMinTotal
        [XmlAttribute("MinimumTotalPurchase")]
        public decimal MinimumTotalPurchase
        {
            get { return (this.RequiredContext == _Enums.RequirementContext.mintotalpurchase) ? this.MinimumAmount : 0; }
        }
        #endregion
    }
}
