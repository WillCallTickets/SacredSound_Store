using System;

namespace Wcss
{
    public partial class SalePromotion
    {
        private ShowDate _requiredShowDateForPromotion = null;
        /// <summary>
        /// Lazy loaded object
        /// </summary>
        public ShowDate ShowDateRecord_RequiredForPromotion
        {
            get
            {
                if (_requiredShowDateForPromotion == null && this.TRequiredParentShowDateId.HasValue)
                {
                    _requiredShowDateForPromotion = new ShowDate();
                    _requiredShowDateForPromotion.CopyFrom(this.ShowDateRecord);
                }

                return _requiredShowDateForPromotion;
            }
            set
            {
                _requiredShowDateForPromotion = null;
                this.TRequiredParentShowDateId = value.Id;
            }
        }

        private ShowTicket _requiredshowticket = null;
        /// <summary>
        /// Lazy loaded object
        /// </summary>
        public ShowTicket ShowTicketRecord_RequiredForPromotion
        {
            get
            {
                if (_requiredshowticket == null && this.TRequiredParentShowTicketId.HasValue)
                {
                    _requiredshowticket = new ShowTicket();
                    _requiredshowticket.CopyFrom(this.ShowTicketToTRequiredParentShowTicketIdRecord);
                }

                return _requiredshowticket;
            }
            set
            {
                _requiredshowticket = null;
                this.TRequiredParentShowTicketId = value.Id;
            }
        }

    }
}
