using System;

namespace Wcss
{
    public partial class EventQ
    {
        public static string CheckMailerSignupEventFrequency(Guid userId, string ip)
        {
            string result = null;

            //set vars
            int userLimit = 4;//TODO: put into config
            DateTime userDate = DateTime.Now.AddHours(-12);//TODO: put into config

            int ipLimit = 12;//TODO: put into config
            DateTime ipDate = DateTime.Now.AddHours(-12);//TODO: put into config

            _Enums.EventQVerb verb = _Enums.EventQVerb.Mailer_SignupAwaitConfirm;

            SubSonic.QueryCommand cmdUser =
                       new SubSonic.QueryCommand("SELECT COUNT(eq.[Id]) FROM [EventQ] eq WHERE eq.[UserId] = @userId AND eq.[dtStamp] > @userDate AND eq.[Context] = @context AND eq.[Verb] = @verb; ", 
                           SubSonic.DataService.Provider.Name);
            cmdUser.Parameters.Add("@userId", userId.ToString());
            cmdUser.Parameters.Add("@context", _Enums.EventQContext.User.ToString());
            cmdUser.Parameters.Add("@verb", verb.ToString());
            cmdUser.Parameters.Add("@userDate", userDate, System.Data.DbType.DateTime);
            int pastUsers = (int)SubSonic.DataService.ExecuteScalar(cmdUser);

            if (pastUsers >= userLimit)
                result = "Please wait 12 hours before attempting another request.";


            if (result == null)
            {
                SubSonic.QueryCommand cmdIp =
                    new SubSonic.QueryCommand("SELECT COUNT(eq.[Id]) FROM [EventQ] eq WHERE eq.[Ip] = @ip AND eq.[dtStamp] > @ipDate AND eq.[Context] = @context AND eq.[Verb] = @verb; ", 
                        SubSonic.DataService.Provider.Name);
                cmdIp.Parameters.Add("@ip", ip);
                cmdIp.Parameters.Add("@context", _Enums.EventQContext.User.ToString());
                cmdIp.Parameters.Add("@verb", verb);
                cmdIp.Parameters.Add("@ipDate", ipDate, System.Data.DbType.DateTime);
                int pastIps = (int)SubSonic.DataService.ExecuteScalar(cmdIp);

                if (pastIps >= ipLimit)
                    result = "Please wait 12 hours before attempting another request.";

            }

            return result;
        }

        /// <summary>
        /// notifies admin of mailer issues - also badmail services. this requires processing to actually send the mail
        /// </summary>
        public static EventQ CreateMailerNotification(DateTime dateToProcess, string creatorName, string affectedUserName, _Enums.EventQVerb verb, string oldValue, 
            string newValue, string description)
        {
            Guid userId = Guid.Empty;
            AspnetUser usr = null;

            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            eventQ.DateToProcess = dateToProcess;
            eventQ.CreatorName = creatorName;

            if (affectedUserName != null)
            {
                eventQ.UserName = affectedUserName;

                //config is handled by module
                usr = AspnetUser.GetUserByUserName(affectedUserName);

                if (usr != null)
                {
                    userId = usr.UserId;
                    eventQ.UserId = userId;
                }
                else
                {
                    eventQ.UserId = Guid.Empty;
                    //throw new Exception("User specified does not match application.");
                }
            }

            eventQ.Context = _Enums.EventQContext.Mailer.ToString();
            eventQ.Verb = verb.ToString();

            if(oldValue != null)
                eventQ.OldValue = oldValue;
            if(newValue != null)
                eventQ.NewValue = newValue;

            if(description != null)
                eventQ.Description = (description.Length >= 2000) ? description.Substring(0, 1999).Trim() : description.Trim();

            eventQ.DtStamp = DateTime.Now;
            eventQ.AttemptsRemaining = 3;
            eventQ.Ip = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Request.UserHostAddress : "127.0.0.1";

            eventQ.Save();

            if (usr != null)
            {
                UserEvent evt = new UserEvent();
                evt.TEventQId = eventQ.Id;
                evt.UserId = usr.UserId;
                evt.DtStamp = DateTime.Now;
                evt.Save();
            }

            return eventQ;
        }

        //EventQ.CreateAdminNotification(DateTime.Now, "ClearCart", username, _Enums.EventQVerb.CartCleared,
        //            "Error", (postSale_IncrementSales) ? "Post Sale" : "Not Post Sale", retVal);
        public static EventQ CreateAdminNotification(DateTime dateToProcess, string processGoneBad, string affectedUserName, 
            _Enums.EventQVerb verb, string oldValue, string newValue, string description)
        {
            Guid userId = Guid.Empty;
            AspnetUser usr = null;

            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            eventQ.DateToProcess = dateToProcess;
            eventQ.CreatorName = processGoneBad;

            if (affectedUserName != null)
            {
                eventQ.UserName = affectedUserName;

                //config is handled by module
                usr = AspnetUser.GetUserByUserName(affectedUserName);

                if (usr != null)
                {
                    userId = usr.UserId;
                    eventQ.UserId = userId;
                }
                else
                {
                    eventQ.UserId = Guid.Empty;
                    //throw new Exception("User specified does not match application.");
                }
            }

            eventQ.Context = _Enums.EventQContext.AdminNotification.ToString();
            eventQ.Verb = verb.ToString();

            if (oldValue != null)
                eventQ.OldValue = oldValue;
            if (newValue != null)
                eventQ.NewValue = newValue;

            if (description != null)
                eventQ.Description = (description.Length >= 2000) ? description.Substring(0, 1999).Trim() : description.Trim();

            eventQ.DtStamp = DateTime.Now;
            eventQ.AttemptsRemaining = 3;
            eventQ.Ip = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Request.UserHostAddress : "127.0.0.1";

            eventQ.Save();

            if (usr != null)
            {
                UserEvent evt = new UserEvent();
                evt.TEventQId = eventQ.Id;
                evt.UserId = usr.UserId;
                evt.DtStamp = DateTime.Now;
                evt.Save();
            }

            return eventQ;
        }

        /// <summary>
        /// this may or may not require processing. If used for logging, provide a dateProcessed
        /// </summary>
        public static EventQ CreateTicketRefundEvent(DateTime dateToProcess, DateTime? dateProcessed, string creatorName, 
            string affectedUserName, Invoice invoice, int showTicketId, bool refundProcessing, bool refundTicketShipping, 
            bool refundServiceFees, string reason)
        {
            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            eventQ.DateToProcess = dateToProcess;
            if (dateProcessed.HasValue)
                eventQ.DateProcessed = dateProcessed;

            AspnetUser usr = AspnetUser.GetUserByUserName(creatorName);

            Guid creatorId = (usr != null) ? usr.UserId : Guid.Empty;

            eventQ.CreatorId = creatorId;
            eventQ.CreatorName = creatorName;
            eventQ.UserId = invoice.UserId;
            eventQ.UserName = affectedUserName;
            eventQ.Context = _Enums.EventQContext.ShowTicket.ToString();
            eventQ.Verb = _Enums.EventQVerb.Refund.ToString();
            
            eventQ.OldValue = string.Format("{0}~{1}~{2}~{3}~{4}", showTicketId, invoice.Id, 
                (refundProcessing) ? 1 : 0, (refundTicketShipping) ? 1 : 0, (refundServiceFees) ? 1 : 0);
            eventQ.NewValue = string.Format("tktid~invid~prc~shp~svc");

            if(reason != null)
                eventQ.Description = (reason.Length >= 2000) ? reason.Substring(0, 1999).Trim() : reason.Trim();
            eventQ.DtStamp = DateTime.Now;

            eventQ.Ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            eventQ.Save();

            // create an invoiceEvent as well
            InvoiceEvent ie = new InvoiceEvent();
            ie.TInvoiceId = invoice.Id;
            ie.TEventQId = eventQ.Id;
            ie.DtStamp = DateTime.Now;

            ie.Save();

            return eventQ;
        }

        /// <summary>
        /// this may or may not require processing. If used for logging, provide a dateProcessed
        /// </summary>
        public static EventQ CreateChangeUserNameEvent(DateTime dateToProcess, DateTime? dateProcessed, string creatorName, string oldUserName,
            string newUserName, _Enums.EventQStatus status, string reason)
        {
            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            eventQ.DateToProcess = dateToProcess;
            if (dateProcessed.HasValue)
                eventQ.DateProcessed = dateProcessed;

            //simply gets who did the operation
            AspnetUser usr = AspnetUser.GetUserByUserName(creatorName);
            
            Guid creatorId = (usr != null) ? usr.UserId : Guid.Empty;

            eventQ.CreatorId = creatorId;
            eventQ.CreatorName = creatorName;

            //at this point we have already updated the user
            string affectedUserName = (status == _Enums.EventQStatus.Success) ? newUserName.ToLower() : oldUserName.ToLower();
            AspnetUser affected = AspnetUser.GetUserByUserName(affectedUserName);

            if (affected == null)
                throw new Exception(string.Format("{0} does not exist in this application.", affectedUserName));

            Guid affectedUserId = (affected != null) ? affected.UserId : Guid.Empty;
            
            eventQ.UserId = affectedUserId;
            eventQ.UserName = oldUserName;//this should point to old address - in case of failure - also leaves a more proper trail
            
            eventQ.Context = _Enums.EventQContext.User.ToString();
            eventQ.Verb = _Enums.EventQVerb.ChangeUserName.ToString();

            eventQ.OldValue = oldUserName;
            eventQ.NewValue = newUserName;

            if (reason != null)
                eventQ.Description = (reason.Length >= 2000) ? reason.Substring(0, 1999).Trim() : reason.Trim();

            eventQ.Status = status.ToString();
            eventQ.DtStamp = DateTime.Now;
            eventQ.Ip = System.Web.HttpContext.Current.Request.UserHostAddress;

            eventQ.Save();

            UserEvent ue = new UserEvent();
            ue.DtStamp = DateTime.Now;
            ue.UserId = affectedUserId;
            ue.TEventQId = eventQ.Id;;

            ue.Save();

            return eventQ;
        }

        public static EventQ CreateInventoryNotification(string userName, string severity, string itemDesc,
            _Enums.InvoiceItemContext context, int idx, int transferred)
        {
            return CreateInventoryNotification(userName, severity, itemDesc, context, idx, transferred, 0);
        }
        public static EventQ CreateInventoryNotification(string userName, string severity, string itemDesc,
            _Enums.InvoiceItemContext context, int idx, int transferred, int pending)
        {
            //changing params here will affect reservation cleanup and eventQ job!!!!
            EventQ q = new EventQ();

            q.DateToProcess = DateTime.Now;
            q.DtStamp = DateTime.Now;
            q.ApplicationId = _Config.APPLICATION_ID;//
            q.AttemptsRemaining = 3;
            q.Context = _Enums.EventQContext.Report.ToString();
            q.Verb = _Enums.EventQVerb.InventoryNotification.ToString();//(severity == "soldout") ? _Enums.EventQVerb.InventorySoldOut.ToString() : _Enums.EventQVerb.InventoryThreshold.ToString();//
            q.OldValue = context.ToString();//these 2 combine to form the subscription name
            
            if(userName != null && userName.Trim().Length > 0)
                q.CreatorName = userName;//            
            q.NewValue = string.Format("{0}", idx.ToString());
            if (severity == "soldout")
                q.NewValue += "~soldout";

            string requestIP = "notify";
            try
            {
                requestIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }

            q.Ip = requestIP;

            string dispAttrib = itemDesc;
            if (dispAttrib.Trim().Length > 200)
                dispAttrib = dispAttrib.Substring(0, 199).Trim();

            if(severity.ToLower() == "dostransfer")
                q.Description = string.Format("{0} - DosTransfer - transferred: {1} - pending: {2} - {3}",
                    DateTime.Now.ToString("MMM dd yyyy h:mmtt"), transferred.ToString(), pending.ToString(), dispAttrib);
            else
                q.Description = string.Format("{0} - {1} - {2} - {3}", DateTime.Now.ToString("MMM dd yyyy h:mmtt"), idx.ToString(),
                    (severity == "soldout") ? "SoldOut" : string.Format("Under {0}", transferred), dispAttrib);

            q.Save();

            return q;
        }

        /// <summary>
        /// Used for logging events to the event queue - where they have already been processed
        /// </summary>
        public static EventQ LogEvent(DateTime dateToProcess, DateTime dateProcessed, _Enums.EventQStatus status, string creatorName, 
            Guid affectedUserId, string affectedUserName, _Enums.EventQContext context, _Enums.EventQVerb verb, string oldValue, string newValue, 
            string description)
        {
            //log to eventQ as processed
            EventQ eventQ = new EventQ();
            eventQ.ApplicationId = _Config.APPLICATION_ID;
            if(dateToProcess != DateTime.MinValue)
                eventQ.DateToProcess = dateToProcess;
            if (dateProcessed != DateTime.MinValue)
                eventQ.DateProcessed = dateProcessed;
            eventQ.Status = status.ToString();

            AspnetUser usr = null;

            if (creatorName != null)
                usr = AspnetUser.GetUserByUserName(creatorName);

            Guid creatorId = (usr != null) ? usr.UserId : Guid.Empty;

            eventQ.CreatorId = creatorId;
            
            if (creatorName != null && creatorName.Trim().Length > 0)
                eventQ.CreatorName = creatorName;
            if (affectedUserId != null && affectedUserId != Guid.Empty)
                eventQ.UserId = affectedUserId;
            if (affectedUserName != null && affectedUserName.Trim().Length > 0)
            {
                eventQ.UserName = affectedUserName;
                if (affectedUserId == Guid.Empty)
                {
                    if (creatorName == affectedUserName)
                        eventQ.UserId = creatorId;
                    else
                    {
                        AspnetUser usr2 = null;
                        usr2 = AspnetUser.GetUserByUserName(affectedUserName);
                        if (usr2 != null)
                            eventQ.UserId = usr2.UserId;
                    }
                }
            }
            
            eventQ.Context = context.ToString();
            eventQ.Verb = verb.ToString();

            if(oldValue != null && oldValue.Trim().Length > 0)
                eventQ.OldValue = oldValue;
            if (newValue != null && newValue.Trim().Length > 0)
                eventQ.NewValue = newValue;
            if (description != null)
                eventQ.Description = (description.Length >= 2000) ? description.Substring(0, 1999).Trim() : description.Trim();

            eventQ.DtStamp = DateTime.Now;
            eventQ.Ip = (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null) ? 
                System.Web.HttpContext.Current.Request.UserHostAddress : "N/A";

            eventQ.Save();

            return eventQ;
        }
    }
}


