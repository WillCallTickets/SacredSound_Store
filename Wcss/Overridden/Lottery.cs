using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class Lottery
    {
        #region Properties

        [XmlAttribute("IsActive_Signup")]
        public bool IsActive_Signup
        {
            get { return this.BActiveSignup; }
            set { this.BActiveSignup = value; }
        }
        /// <summary>
        /// refers to the date that the user is allowed to signup a request. Maxdate = not available
        /// </summary>
        [XmlAttribute("SignupStartDate")]
        public DateTime SignupStartDate
        {
            get { return (!this.DtSignupStart.HasValue) ? DateTime.MaxValue : this.DtSignupStart.Value; }
            set { this.DtSignupStart = value; }
        }
        /// <summary>
        /// refers to the date that the user is allowed to signup a request. Maxdate = neverBeenFulfilled
        /// </summary>
        [XmlAttribute("SignupEndDate")]
        public DateTime SignupEndDate
        {
            get { return (!this.DtSignupEnd.HasValue) ? DateTime.MaxValue : this.DtSignupEnd.Value; }
            set { this.DtSignupEnd = value; }
        }
        [XmlAttribute("IsActive_Fulfillment")]
        public bool IsActive_Fulfillment
        {
            get { return this.BActiveFulfillment; }
            set { this.BActiveFulfillment = value; }
        }
        /// <summary>
        /// refers to the date that the user is allowed to fulfill a request. Maxdate = not available
        /// </summary>
        [XmlAttribute("FulfillStartDate")]
        public DateTime FulfillStartDate
        {
            get { return (!this.DtFulfillStart.HasValue) ? DateTime.MaxValue : this.DtFulfillStart.Value; }
            set { this.DtFulfillStart = value; }
        }
        /// <summary>
        /// refers to the date that the user is allowed to fulfill a request. Maxdate = neverBeenFulfilled
        /// </summary>
        [XmlAttribute("FulfillEndDate")]
        public DateTime FulfillEndDate
        {
            get { return (!this.DtFulfillEnd.HasValue) ? DateTime.MaxValue : this.DtFulfillEnd.Value; }
            set { this.DtFulfillEnd = value; }
        }
        /// <summary>
        /// if this number is greater than 0, than it will establish an automatic qty selected for the purchase.
        /// ie if we have a small # of tix, but would rather get rid of them in 2's in stead of single orders...
        /// </summary>
        [XmlAttribute("EstablishQty")]
        public int EstablishQty
        {
            get { return this.IEstablishQty; }
            set { this.IEstablishQty = value; }
        }

        /// <summary>
        /// Takes into account if the object is active and if the start and end dates are valid
        /// </summary>
        [XmlAttribute("IsAvailalbleForSignup")]
        public bool IsAvailalbleForSignup
        {
            get { return this.ShowTicketRecord.IsAvailableForListing(_Enums.VendorTypes.online, null, true, false, null); }// && this.IsActive_Signup && this.SignupStartDate < DateTime.Now && this.SignupEndDate > DateTime.Now; }
        }
        /// <summary>
        /// Takes into account if the object is active, if the start and end dates are valid, if it has yet to be fullfilled 
        /// and if the status is valid(approved)
        /// </summary>
        [XmlAttribute("IsAvailalbleForFulfillment")]
        public bool IsAvailalbleForFulfillment
        {
            get { return this.ShowTicketRecord.IsAvailableForListing(_Enums.VendorTypes.online, null, false, true, null); } // && this.IsActive_Fulfillment && this.FulfillStartDate < DateTime.Now && this.FulfillEndDate > DateTime.Now; }
        }

        #endregion

        #region Methods

        //choose approved
            //marks non-approved as denied
        //

        //public void RecordStatusChange(_Enums.ProcessStatus newStatus, string creatorName, string notes, string Ip)
        //{
        //    string oldStatus = this.ProcessStatus.ToString();

        //    try
        //    {
        //        this.ProcessStatus = newStatus;
        //        this.StatusDate = DateTime.Now;
        //        this.StatusBy = creatorName;
        //        this.StatusNotes = notes;
        //        this.StatusIP = Ip;
        //        Save;???????

        //        UserEvent.NewUserEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, creatorName, this.UserId,
        //            this.UserName, _Enums.EventQContext.User, _Enums.EventQVerb.LotteryStatusUpdate, oldStatus,
        //            newStatus.ToString(), null, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        _Error.LogException(ex);
        //    }
        //}

        #endregion
    }
}
