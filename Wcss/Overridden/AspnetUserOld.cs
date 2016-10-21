using System;
using System.Xml.Serialization;

using System.IO;

namespace Wcss
{
    public partial class AspnetUsersOld
    {
        public bool ProfileHasBeenUpdated
        {
            get { return this.DateUpdated < DateTime.Now; }
        }
        [XmlAttribute("DateUpdated")]
        public DateTime DateUpdated
        {
            get { return (this.DtUpdated.HasValue) ? this.DtUpdated.Value : DateTime.MaxValue; }
            set { this.DtUpdated = value; }
        }
        [XmlAttribute("Password")]
        public string Password
        {
            get { return Utils.ObsCrypt.Decrypt(this.OldPass, this.UserName.ToLower());  }
        }

        public static AspnetUsersOld GetOldUser(string userName)
        {
            AspnetUsersOldCollection coll = new AspnetUsersOldCollection();
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(string.Format("Select * from aspnet_users_old where username = '{0}'", userName), 
                SubSonic.DataService.Provider.Name)));

            if (coll.Count > 0)
                return coll[0];

            return null;
        }

    }
}
