using System;

namespace Wcss
{
    public partial class ShowTicket
    {
        public Age _agerecord;
        /// <summary>
        /// Lazy loaded object
        /// </summary>
        public Age AgeRecord
        {
            get
            {
                if (_agerecord == null)
                {
                    _agerecord = new Age();
                    _agerecord = (Age)_Lookits.Ages.Find(this.TAgeId);
                }

                return _agerecord;
            }
            set
            {
                _agerecord = value;
                this.TAgeId = value.Id;
            }
        }

        private Vendor _vendorRecord;
        /// <summary>
        /// Lazy loaded object
        /// </summary>
        public Vendor VendorRecord
        {
            get
            {
                if (_vendorRecord == null)
                {
                    _vendorRecord = new Vendor();
                    _vendorRecord = (Vendor)_Lookits.Vendors.Find(this.TVendorId);
                }

                return _vendorRecord;
            }
            set
            {
                _vendorRecord = value;
                this.TVendorId = value.Id;
            }
        }

        private ShowTicket _parentShowTicketRecord = null;
        /// <summary>
        /// only dos tickets can have parents
        /// </summary>
        public ShowTicket ParentShowTicketRecord
        {
            get
            {
                if (_parentShowTicketRecord == null && this.IsDosTicket && this.ShowTicketDosTicketRecordsFromShowTicket().Count > 0)
                {
                    _parentShowTicketRecord = new ShowTicket();
                    _parentShowTicketRecord.CopyFrom(this.ShowTicketDosTicketRecordsFromShowTicket()[0].ParentShowTicketRecord);
                }

                return _parentShowTicketRecord;
            }
        }

        private ShowTicket _dosShowTicketRecord = null;
        /// <summary>
        /// only parent tickets can have dos
        /// </summary>
        public ShowTicket DosShowTicketRecord
        {
            get
            {
                if (_dosShowTicketRecord == null && (!this.IsDosTicket) && this.ShowTicketDosTicketRecords().Count > 0)
                {
                    _dosShowTicketRecord = new ShowTicket();
                    _dosShowTicketRecord.CopyFrom(this.ShowTicketDosTicketRecords()[0].DosShowTicketRecord);
                }

                return _dosShowTicketRecord;
            }
        }
        
    }
    public partial class ShowTicketDosTicket
    {
        private ShowTicket _parentShowTicketRecord;
        /// <summary>
        /// Lazy loaded object
        /// </summary>
        public ShowTicket ParentShowTicketRecord
        {
            get
            {
                if (_parentShowTicketRecord == null)
                {
                    _parentShowTicketRecord = new ShowTicket();
                    _parentShowTicketRecord.CopyFrom(this.ShowTicketRecord);
                }

                return _parentShowTicketRecord;
            }
            set
            {
                _parentShowTicketRecord = value;
                this.ParentId = value.Id;
            }
        }

        private ShowTicket _dosShowTicketRecord;
        /// <summary>
        /// Lazy loaded object
        /// </summary>
        public ShowTicket DosShowTicketRecord
        {
            get
            {
                if (_dosShowTicketRecord == null)
                {
                    _dosShowTicketRecord = new ShowTicket();
                    _dosShowTicketRecord.CopyFrom(this.ShowTicketToDosIdRecord);
                }

                return _dosShowTicketRecord;
            }
            set
            {
                _dosShowTicketRecord = value;
                this.DosId = value.Id;
            }
        }
    }
}
