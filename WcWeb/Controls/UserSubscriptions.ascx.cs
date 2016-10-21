using System;
using System.Web.UI.WebControls;
using System.Web.UI;

using Wcss;

namespace WillCallWeb.Controls
{
   public partial class UserSubscriptions : WillCallWeb.BaseControl
   {
       private static readonly object UpdatedEventKey = new object();

       public delegate void UpdatedEventHandler(object sender, EventArgs e);

       public event UpdatedEventHandler Updated
       {
           add { Events.AddHandler(UpdatedEventKey, value); }
           remove { Events.RemoveHandler(UpdatedEventKey, value); }
       }
       public virtual void OnUpdated(EventArgs e)
       {
           UpdatedEventHandler handler = (UpdatedEventHandler)Events[UpdatedEventKey];

           if (handler != null)
               handler(this, e);
       }


       private string _userName = "";
       public string UserName
       {
           get { return _userName; }
           set { _userName = value; }
       }
       private bool _allowAdminSubscriptions = false;
       public bool AllowAdminSubscriptions
       {
           get { return _allowAdminSubscriptions; }
           set { _allowAdminSubscriptions = value; }
       }

      protected void Page_Init(object sender, EventArgs e)
      {
         this.Page.RegisterRequiresControlState(this);
      }

      protected override void LoadControlState(object savedState)
      {
         object[] ctlState = (object[])savedState;
         base.LoadControlState(ctlState[0]);
         this.UserName = (string)ctlState[1];
         this.AllowAdminSubscriptions = (bool)ctlState[2];
      }

      protected override object SaveControlState()
      {
         object[] ctlState = new object[3];
         ctlState[0] = base.SaveControlState();
         ctlState[1] = this.UserName;
         ctlState[2] = this.AllowAdminSubscriptions;
         return ctlState;
      }
      protected void chkSubs_DataBinding(object sender, EventArgs e)
      {
          // fill the CheckBoxList with all the subscriptions that are available for the customers roles
          SubscriptionCollection coll = new SubscriptionCollection();

          if (base.IsAuthdAdminUser)
          {   
              coll.LoadAndCloseReader(SPs.TxSubscriptionGetSubsForUser(_Config.APPLICATION_ID, this.UserName).GetReader());
          }
          else if (_Config._SubscriptionsActive)
          {
              //always return webuser subscriptions
              //handles anonymous and new signups
              coll.AddRange(_Lookits.Subscriptions.GetList().FindAll(delegate(Subscription match)
              {
                  return (match.IsActive &&
                      match.AspnetRoleRecord.RoleName.ToLower() == "webuser");
              }));
          }

          chkSubs.DataSource = coll;
          chkSubs.DataTextField = "Name";
          chkSubs.DataValueField = "Id";
      }
      protected void chkSubs_DataBound(object sender, EventArgs e)
      {
          //get customer subscriptions  
          AspnetUser usr = AspnetUser.GetUserByUserName(UserName);

          if (usr != null)
          {
              //loop thru all items and set checked or NO
              foreach (ListItem li in chkSubs.Items)
              {
                  SubscriptionUser subUsr = usr.SubscriptionUserRecords().GetList().Find(
                      delegate(SubscriptionUser match) { return (match.TSubscriptionId.ToString() == li.Value && match.IsSubscribed); });
                  li.Selected = (subUsr != null);

                  //make the text more displayable
                  Subscription sub = (Subscription)_Lookits.Subscriptions.Find(int.Parse(li.Value));
                  if (sub != null)
                  {
                      string displayText = string.Format("<div class=\"subscriptionlistitem\"><span class=\"subscriptionname\">{0}</span><div class=\"subscriptiondescription\">{1}</div></div>",
                          sub.Name, sub.Description);
                      li.Text = displayText;
                  }
              }
          }
      }

      protected void Page_Load(object sender, EventArgs e)
      {
          btnUpdate.Visible = (this.Page.ToString().ToLower() == "asp.admin_edituser_aspx");

          lblFeedbackOK.Visible = false;
          
          if (this.UserName.Length == 0)
          {
              UserName = this.Profile.UserName;
          }

          if(! this.IsPostBack)
            chkSubs.DataBind();

          if (chkSubs.Items.Count == 0)
          {
              this.Controls.Clear();
              return;
          }
      }
      protected void btnUpdateSubs_Click(object sender, EventArgs e)
      {
          SaveSubscriptions();
      }
      public void SaveSubscriptions()
      {
           //get customer subscriptions     
          AspnetUser usr = AspnetUser.GetUserByUserName(UserName);
          
          if(usr != null)
          {
              foreach (ListItem li in chkSubs.Items)
              {
                  bool isDirty = false;

                  string creatorName = this.Page.User.Identity.Name;
                  
                  SubscriptionUser sub = usr.SubscriptionUserRecords().GetList()
                      .Find(delegate(SubscriptionUser match) { return match.TSubscriptionId.ToString() == li.Value; } );

                  bool selected = li.Selected;

                  if(selected && sub == null)
                  {
                      //create a new subscriptionUser
                      sub = new SubscriptionUser();
                      sub.UserId = usr.UserId;
                      sub.TSubscriptionId = int.Parse(li.Value);
                      sub.IsSubscribed = true;
                      sub.IsHtmlFormat = true;
                      sub.LastActionDate = DateTime.Now;
                      sub.DtStamp = DateTime.Now;

                      usr.SubscriptionUserRecords().Add(sub);
                      usr.SubscriptionUserRecords().SaveAll();
                      isDirty = true;

                      string subInfo = string.Format("{0}~{1}~ Page: {2}", sub.TSubscriptionId, li.Text, this.Page.ToString());

                      UserEvent.NewUserEvent(UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                        creatorName, _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate,
                        "Created", "Subscribed", subInfo, true);
                  }
                  else if (selected && sub != null && (!sub.IsSubscribed))
                  {
                      sub.IsSubscribed = true;
                      isDirty = true;

                      string subInfo = string.Format("{0}~{1}~ Page: {2}", sub.TSubscriptionId, sub.SubscriptionRecord.Name, this.Page.ToString());

                      UserEvent.NewUserEvent(UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                        creatorName, _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate,
                        "Not Subscribed", "Subscribed", subInfo, true);
                  }
                  else if ((!selected) && sub != null && sub.IsSubscribed)
                  {
                      sub.IsSubscribed = false;
                      isDirty = true;

                      string subInfo = string.Format("{0}~{1}~ Page: {2}", sub.TSubscriptionId, sub.SubscriptionRecord.Name, this.Page.ToString());

                      UserEvent.NewUserEvent(UserName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success,
                        creatorName, _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate,
                        "Subscribed", "Not Subscribed", subInfo, true);
                  }

                  if (isDirty)
                  {
                      sub.DtLastActionDate = DateTime.Now;
                      sub.Save();

                      OnUpdated(new EventArgs());

                      lblFeedbackOK.Visible = true;
                  }
              }
          }
      }

      protected void btnUpdate_Click(object sender, EventArgs e)
      {
          SaveSubscriptions();
      }
}
}