using System;

namespace Wcss
{
    public partial class UserEvent
    {
        
        /// <summary>
        /// use this when user is not logged in but we have the email address - when we can't provide a userid
        /// </summary>
        /// <param name="email">the users email</param>
        /// <param name="dateToProcess">usa a past value to log</param>
        /// <param name="dateProcessed">inert value if logging</param>
        /// <param name="status">enumerated status</param>
        /// <param name="creatorName"></param>
        /// <param name="context"></param>
        /// <param name="verb"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="description"></param>
        /// <param name="saveToDb"></param>
        /// <returns></returns>
        public static UserEvent NewUserEvent(string email, DateTime dateToProcess, DateTime dateProcessed, _Enums.EventQStatus status,
            string creatorName, _Enums.EventQContext context, _Enums.EventQVerb verb, string oldValue, string newValue,
            string description, bool saveToDb)
        {
            //TODO: log somewhere else?
            //if we can't find the user - log to error log
            AspnetUser usr = AspnetUser.GetUserByUserName(email);

            if (usr != null)
                return NewUserEvent(dateToProcess, dateProcessed, status, creatorName, usr.UserId, email, 
                    context, verb, oldValue, newValue, description, saveToDb);
            else
            {
                EventQ.LogEvent(dateToProcess, dateProcessed, _Enums.EventQStatus.UserNotFound, creatorName, Guid.Empty, email,
                    context, verb, oldValue, newValue, description);

                return null;
            }
        }

        public static UserEvent RecordStoreCreditEvent(string creatorName, Guid affectedUserId, string affectedUserName, 
            decimal amountToAdjust, decimal newBalance, string added_removed_synced, Invoice invoice)
        {
            string oldValue = added_removed_synced;//(amountToAdjust > 0) ? "added" : "removed";
            string invoiceDescription = (invoice != null) ? string.Format("Invoice {0} {1} ", 
                    invoice.InvoiceDate.ToString("MM/dd/yyyy"), invoice.UniqueId) : string.Empty;
            string description = string.Format("{0}New balance: {1}", invoiceDescription, newBalance.ToString("n2"));
            return NewUserEvent(DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, 
                creatorName, affectedUserId, affectedUserName, _Enums.EventQContext.User, _Enums.EventQVerb.StoreCreditAdjustment,
                oldValue, amountToAdjust.ToString("n2"), description, true);
        }
        /// <summary>
        /// use this when we have a logged in user and can provide the user id
        /// </summary>
        /// <param name="dateToProcess"></param>
        /// <param name="dateProcessed"></param>
        /// <param name="status"></param>
        /// <param name="creatorName"></param>
        /// <param name="affectedUserId"></param>
        /// <param name="affectedUserName"></param>
        /// <param name="context"></param>
        /// <param name="verb"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="description"></param>
        /// <param name="saveToDb"></param>
        /// <returns></returns>
        public static UserEvent NewUserEvent(DateTime dateToProcess, DateTime dateProcessed, _Enums.EventQStatus status,
            string creatorName, Guid affectedUserId, string affectedUserName, _Enums.EventQContext context, _Enums.EventQVerb verb, 
            string oldValue, string newValue, string description, bool saveToDb)
        {
            EventQ eventQ = EventQ.LogEvent(dateToProcess, dateProcessed, status, creatorName, affectedUserId, affectedUserName, 
                context, verb, oldValue, newValue, description);

            //log to eventQ as processed
            UserEvent evt = new UserEvent();
            evt.TEventQId = eventQ.Id;
            evt.UserId = affectedUserId;
            evt.DtStamp = DateTime.Now;

            if(saveToDb)
                evt.Save();

            return evt;
        }
    }
}
