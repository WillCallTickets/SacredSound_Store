using System;

namespace Wcss
{
    public partial class InvoiceEvent
    {
        public static InvoiceEvent NewInvoiceEvent(int invoiceId, DateTime dateToProcess, DateTime dateProcessed, _Enums.EventQStatus status, 
            string creatorName, Guid affectedUserId, string affectedUserName, _Enums.EventQContext context, _Enums.EventQVerb verb, 
            string oldValue, string newValue, string description, bool saveToDb)
        {
            EventQ eventQ = EventQ.LogEvent(dateToProcess, dateProcessed, status, creatorName, affectedUserId, affectedUserName, 
                context, verb, oldValue, newValue, description);

            //log to eventQ as processed
            InvoiceEvent evt = new InvoiceEvent();
            evt.TEventQId = eventQ.Id;
            evt.TInvoiceId = invoiceId;
            evt.DtStamp = DateTime.Now;

            if(saveToDb)
                evt.Save();

            return evt;
        }
        /// <summary>
        /// This uses the unique id to find the invoice
        /// </summary>
        public static InvoiceEvent NewInvoiceEvent(string uniqueId, DateTime dateToProcess, DateTime dateProcessed, _Enums.EventQStatus status, 
            string creatorName, Guid affectedUserId, string affectedUserName, _Enums.EventQContext context, _Enums.EventQVerb verb, 
            string oldValue, string newValue, string description, bool saveToDb)
        {
            Invoice i = new Invoice();
            i.LoadAndCloseReader(Invoice.FetchByParameter("UniqueId", uniqueId));

            if (i != null && i.Id > 1)
            {
                EventQ eventQ = EventQ.LogEvent(dateToProcess, dateProcessed, status, creatorName, affectedUserId, affectedUserName,
                    context, verb, oldValue, newValue, description);

                //log to eventQ as processed
                InvoiceEvent evt = new InvoiceEvent();
                evt.TEventQId = eventQ.Id;
                evt.TInvoiceId = i.Id;
                evt.DtStamp = DateTime.Now;

                if (saveToDb)
                    evt.Save();

                return evt;
            }

            return null;
        }
    }
}
