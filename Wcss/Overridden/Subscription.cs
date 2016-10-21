using System;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class Subscription
    {   
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get { return this.BActive; }
            set { this.BActive = value; }
        }
        [XmlAttribute("IsDefault")]
        public bool IsDefault
        {
            get { return this.BDefault; }
            set { this.BDefault = value; }
        }

        [XmlAttribute("NameAndRecipients")]
        public string NameAndRecipients { get { return string.Format("{0} - {1}", 
            this.Name, this.AspnetRoleRecord.RoleName);  } }

        /// <summary>
        /// create a method to retrieve by param [name] - ignore case 
        /// </summary>
        /// <returns></returns>
        public static Subscription GetSubscriptionByName(string subscriptionName)
        {
            //if we dont get a return - we know we need to create the subscriptions
            SubSonic.QueryCommand subCmd = new SubSonic.QueryCommand("SELECT * FROM [Subscription] sub WHERE [Name] = @subName ", SubSonic.DataService.Provider.Name);
            subCmd.Parameters.Add("@subName", subscriptionName);

            Subscription sub = new Subscription();
            sub.LoadAndCloseReader(SubSonic.DataService.GetReader(subCmd));

            return sub;
        }

        /// <summary>
        /// we do this to ensure necessary subscriptions such as inventory notification subscriptions
        /// </summary>
        /// <returns></returns>
        public static Subscription CreateNewSubscription(string subscriptionName)
        {
            //if we dont get a return - we know we need to create the subscriptions
            SubSonic.QueryCommand subCmd = new SubSonic.QueryCommand("INSERT [Subscription]([RoleId],[bActive],[bDefault],[Name],[Description],[InternalDescription],[dtStamp],[ApplicationId]) ", 
                SubSonic.DataService.Provider.Name);
            subCmd.CommandSql += " SELECT r.[RoleId], 1, 0, @subName, @description, '', (getDate()), @appId ";
            subCmd.CommandSql += " FROM [Aspnet_Roles] r WHERE r.[RoleName] = @roleName ";
            subCmd.CommandSql += " SELECT * FROM [Subscription] WHERE [Id] = SCOPE_IDENTITY() ";
            subCmd.Parameters.Add("@subName", subscriptionName);
            subCmd.Parameters.Add("@roleName", "administrator");
            subCmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
            subCmd.Parameters.Add("@description", "Inventory notification indicates when sales go below threshold as well as when they sell out.");

            Subscription sub = new Subscription();
            sub.LoadAndCloseReader(SubSonic.DataService.GetReader(subCmd));

            return sub;
        }

        public static void RemoveUserFromUnauthorizedSubscriptions(string userName)
        {
            //if the user has a subscription in that role - delete it
            SubscriptionCollection coll = new SubscriptionCollection();
            coll.LoadAndCloseReader(SPs.TxSubscriptionRemoveUserFromUnauthorizedSubs(_Config.APPLICATION_ID, userName).GetReader());

            foreach (Subscription sub in coll)
            {
                UserEvent.NewUserEvent(userName, DateTime.Now, DateTime.Now, _Enums.EventQStatus.Success, userName,
                    _Enums.EventQContext.User, _Enums.EventQVerb.SubscriptionUpdate, "Subscribed", "Not Subscribed", string.Format("{0}~{1}",
                    sub.Id, sub.NameAndRecipients), true);
            }
        }
    }
}
