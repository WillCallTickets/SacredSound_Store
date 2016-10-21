using System;
using System.Xml.Serialization;

using System.IO;

namespace Wcss
{
    public partial class AspnetUser
    {   
        public static AspnetUser GetUserByUserName(string userName)
        {
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand("SET NOCOUNT ON; SELECT * FROM [Aspnet_Users] WHERE [ApplicationId] = @appId AND [LoweredUserName] = @userName ",
                SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
            cmd.Parameters.Add("@userName", userName.ToLower());

            AspnetUserCollection coll = new AspnetUserCollection();
            coll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));

            AspnetUser usr = (coll.Count > 0) ? coll[0] : null;

            return usr;
        }
        
    }
}
