using System;
using System.Data;
using System.Xml.Serialization;

using SubSonic;

namespace Wcss
{
    public partial class PendingOperation
    {
        [XmlAttribute("Context")]
        public _Enums.PendingOpContext Context
        {
            get { return (_Enums.PendingOpContext)Enum.Parse(typeof(_Enums.PendingOpContext), this.VcContext, true); }
            set { this.VcContext = value.ToString(); }
        }

        /// <summary>
        /// Makes us wait 1 minutes in between any sort of redemptions
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public static bool PendingExists_StoreCredit(string userName, int invoiceId)
        {
            //insert unless there is another pending op for this user
            string sql = "IF EXISTS (SELECT * FROM [PendingOperation] WHERE [ApplicationId] = @appId AND [vcContext] = @context ";
            sql += "AND [dtValidUntil] > (getDate()) AND [UserName] = @userName) BEGIN SELECT 0 RETURN END ";
            sql += "INSERT [PendingOperation] ([dtValidUntil], [ApplicationId], [IdentifierId], [vcContext], [UserName]) ";
            sql += "VALUES (@validUntilDate, @appId, @identifierId, @context, @userName); SELECT SCOPE_IDENTITY() RETURN; ";

            SubSonic.QueryCommand cmd = new QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.AddParameter("@appId", _Config.APPLICATION_ID, DbType.Guid);
            cmd.Parameters.Add("@validUntilDate", DateTime.Now.AddMinutes(1), DbType.DateTime);
            cmd.Parameters.Add("@context", _Enums.PendingOpContext.storecreditredemption.ToString(), DbType.String);
            cmd.Parameters.Add("@userName", userName, DbType.String);
            cmd.Parameters.Add("@identifierId", invoiceId, DbType.Int32);

            object retVal = SubSonic.DataService.ExecuteScalar(cmd);

            int ret = int.Parse(retVal.ToString());

            //note that we are returning "IF EXISTS" which in this case is zero
            return (ret != 0) ? false : true;
        }

        public static int DeleteOperation(int identifierId)
        {
            string sql = "DELETE FROM [PendingOperation] WHERE [ApplicationId] = @appId AND [IdentifierId] = @identifierId ";
            SubSonic.QueryCommand cmd = new QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.AddParameter("@appId", _Config.APPLICATION_ID, DbType.Guid);
            cmd.Parameters.Add("@identifierId", identifierId, DbType.Int32);

            return SubSonic.DataService.ExecuteQuery(cmd);
        }
    }
}
