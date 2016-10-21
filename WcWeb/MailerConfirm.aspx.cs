using System;
using System.Web.UI.WebControls;
using System.Net.Mail;

using Wcss;

namespace WillCallWeb
{
    public partial class MailerConfirm : BasePage
   {
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
      protected void Page_Load(object sender, EventArgs e) 
      {
          if (!IsPostBack)
          {
              //get the userid
              string req = Request.QueryString["pqd"];

              if (req != null)
              {
                  AspnetUser user = null;

                  //get the user from the id 
                  try
                  {
                      user = new AspnetUser("UserId", req.ToString());
                  }
                  catch (Exception ex)
                  {
                      _Error.LogException(ex);
                      base.Redirect("/Store/ChooseTicket.aspx");//should be a page conatining an emailer signup control
                  }

                  //if the user is null - redirect - log error
                  if (user == null || user.UserId == Guid.Empty)
                      base.Redirect("/Store/ChooseTicket.aspx");//should be a page conatining an emailer signup control

                  //if the user is not null
                  string result = string.Empty;

                  DateTime now = DateTime.Now;

                  //get the subs for 
                  SubscriptionUserCollection coll = new SubscriptionUserCollection().Where("UserId", user.UserId.ToString()).Load();

                  //subscribe to the default subscription
                  SubscriptionUser defaultsub = coll.GetList().Find(delegate(SubscriptionUser match) { return match.SubscriptionRecord.IsDefault; });

                  //if not there create
                  if (defaultsub == null)
                  {
                      defaultsub = new SubscriptionUser();
                      defaultsub.DtStamp = DateTime.Now;
                      defaultsub.IsHtmlFormat = true;
                      defaultsub.IsSubscribed = false;//this will be subscribed in a moment - see below
                      defaultsub.LastActionDate = DateTime.Now;
                      Subscription defaultNewsletter = _Lookits.Subscriptions.GetList().Find(delegate(Subscription match) { return match.IsDefault; });
                      defaultsub.TSubscriptionId = (defaultNewsletter != null) ? defaultNewsletter.Id : 0;
                      defaultsub.UserId = user.UserId;
                  }

                  //update the sub and save and record event
                  if (!defaultsub.IsSubscribed)
                  {
                      defaultsub.IsSubscribed = true;
                      defaultsub.Save();

                      //record events
                      string subInfo = string.Format("{0}~{1}~ Page: {2}", defaultsub.SubscriptionRecord.Id, defaultsub.SubscriptionRecord.Name, this.Page.ToString());

                      UserEvent.NewUserEvent(user.LoweredUserName, now, now, _Enums.EventQStatus.Success, user.LoweredUserName, _Enums.EventQContext.User,
                          _Enums.EventQVerb.SubscriptionUpdate, "Not Subscribed", "Subscribed", subInfo, true);
                  }

                  lblResult.Text = string.Format("{0} is subscribed to our newsletter.", user.LoweredUserName);
              }
          }
      }
}
}
