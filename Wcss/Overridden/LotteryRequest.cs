using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class LotteryRequest
    {
        #region Properties

        private ShowTicket _ticket = null;
        protected ShowTicket ShowTicketRecord
        {
            get
            {
                if (_ticket == null)
                {
                    _ticket = this.LotteryRecord.ShowTicketRecord;
                    //_ticket.CopyFrom(this.LotteryRecord.ShowTicketRecord);
                }

                return _ticket;
            }
        }
        /// <summary>
        /// Takes into account if the object is approved, if the start and end dates are valid, if it has yet to be fullfilled 
        /// and if the status is valid(approved)
        /// </summary>
        [XmlAttribute("IsAvailalbleForFulfillment")]
        public bool IsAvailalbleForFulfillment
        {
            get { return this.ProcessStatus == _Enums.ProcessStatus.approved && this.LotteryRecord.IsAvailalbleForFulfillment && 
                this.FulfilledDate > DateTime.MaxValue; }
        }
        /// <summary>
        /// Let's us know the status of the lottery object. Pending, Approved, Denied and CancelledByUser.
        /// Note that the process date is also in use to record changes as well as user events to track the state of the request
        /// </summary>
        [XmlAttribute("ProcessStatus")]
        public _Enums.ProcessStatus ProcessStatus
        {
            get 
            {
                if (this.VcStatus == null)
                    return _Enums.ProcessStatus.pending;

                return (_Enums.ProcessStatus)Enum.Parse(typeof(_Enums.ProcessStatus), this.VcStatus, true);
            }
            set
            {
                this.VcStatus = value.ToString();
            }
        }

        [XmlAttribute("StatusDate")]
        public DateTime StatusDate
        {
            get { return (!this.DtStatus.HasValue) ? DateTime.MaxValue : this.DtStatus.Value; }
            set { this.DtStatus = value; }
        }
        /// <summary>
        /// refers to the date that the user fulfilled their request. Maxdate = neverBeenFulfilled
        /// </summary>
        [XmlAttribute("FulfilledDate")]
        public DateTime FulfilledDate
        {
            get { return (!this.DtFulfilled.HasValue) ? DateTime.MaxValue : this.DtFulfilled.Value; }
            set { this.DtFulfilled = value; }
        }
        /// <summary>
        /// refers to the quantity that the user requested - this may not necessarily be a user chosen value (may be set by admin)
        /// </summary>
        [XmlAttribute("QtyRequested")]
        public int QtyRequested
        {
            get { return this.IRequested; }
            set { this.IRequested = value; }
        }
        /// <summary>
        /// refers to the quantity that the user purchased - used to compare requests to actual purchases
        /// </summary>
        [XmlAttribute("QtyPurchased")]
        public int QtyPurchased
        {
            get { return this.IPurchased; }
            set { this.IPurchased = value; }
        }

        #endregion

        #region UserEvents

        public void RecordStatusChange(_Enums.ProcessStatus newStatus, string creatorName, string notes, string Ip)
        {
            //string oldStatus = this.ProcessStatus.ToString();

            //try
            //{
            //    this.ProcessStatus = newStatus;
            //    this.StatusDate = DateTime.Now;
            //    this.StatusBy = creatorName;
            //    this.StatusNotes = notes;
            //    this.StatusIP = Ip;
            //    Save???????

            //    UserEvent.NewUserEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, creatorName, this.UserId,
            //        this.UserName, _Enums.EventQContext.User, _Enums.EventQVerb.LotteryStatusUpdate, oldStatus,
            //        newStatus.ToString(), null, true);
            //}
            //catch (Exception ex)
            //{
            //    _Error.LogException(ex);
            //}
        }

        #endregion
    }
}
