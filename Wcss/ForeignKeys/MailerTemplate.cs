using System;

namespace Wcss
{
    public partial class MailerContent
    {
        private ShowEventCollection _showEventRecords;
        public ShowEventCollection ShowEventRecords
        {
            get
            {
                if (_showEventRecords == null)
                {
                    _showEventRecords = new ShowEventCollection().Where("TOwnerId", this.Id).OrderByAsc("IOrdinal").Load();
                }

                return _showEventRecords;
            }
            set
            {
                _showEventRecords = value;
            }
        }
    }
}
