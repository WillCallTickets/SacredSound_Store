using System;

namespace Wcss
{
    public partial class SalePromotionAward
    {
        private Merch _merch;
        /// <summary>
        /// Lazy loaded object
        /// </summary>
        public Merch MerchRecord_Parent
        {
            get
            {
                if (_merch == null && this.TParentMerchId.HasValue)
                {
                    _merch = new Merch();
                    _merch.CopyFrom(this.MerchRecord);
                }

                return _merch;
            }
            set
            {
                _merch = null;
                this.TParentMerchId = value.Id;                
            }
        }
    }
}
