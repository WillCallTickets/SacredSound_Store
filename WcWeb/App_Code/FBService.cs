using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;

using Wcss;

namespace WillCallWeb
{
    /// <summary>
    /// Summary description for FBService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class FBService : System.Web.Services.WebService
    {
        //private WillCallWeb.WebContext _ctx;
        //protected WillCallWeb.WebContext Ctx { get { return _ctx; } }

        public FBService()
        {
            //_ctx = new WillCallWeb.WebContext();
        }

        [WebMethod]
        public bool FB_Like(int entityId, string entityLink)
        {
            return FB_Liking(_Enums.FB_Api.FB_Like, entityId, entityLink);
        }
        [WebMethod]
        public bool FB_Unlike(int entityId, string entityLink)
        {
            return FB_Liking(_Enums.FB_Api.FB_UnLike, entityId, entityLink);
        }
        [WebMethod]
        private static bool FB_Liking(_Enums.FB_Api api, int entityId, string entityLink)
        {
            StringBuilder sb = new StringBuilder();
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", Wcss._Config.APPLICATION_ID, DbType.Guid);

            switch (api)
            {
                case _Enums.FB_Api.FB_Like:
                case _Enums.FB_Api.FB_UnLike:
                    cmd.Parameters.Add("@apiFunction", api.ToString());
                    cmd.Parameters.Add("@apiAggregate", _Enums.FB_Api.Likes.ToString());
                    //cmd.Parameters.Add("@idx", int.Parse(entityId), DbType.Int32);
                    cmd.Parameters.Add("@idx", entityId, DbType.Int32);
                    cmd.Parameters.Add("@url", entityLink);

                    //individual api totals
                    sb.AppendLine("DECLARE	@rowsAffected int; ");
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine("UPDATE	FB_Stat ");
                    sb.AppendLine("SET		Total = Total + 1, dtModified = getDate()  ");
                    sb.AppendLine("WHERE	ApplicationId = @appId AND Url = @url AND ApiFunction = @apiFunction; ");
                    sb.AppendLine();
                    sb.AppendLine("SELECT @rowsAffected = @@ROWCOUNT; ");
                    sb.AppendLine();
                    sb.AppendLine("IF (@rowsAffected = 0) BEGIN  ");
                    sb.AppendLine("    INSERT FB_Stat (ApplicationId, EntityId, Url, ApiFunction, Total, dtModified ) ");
                    sb.AppendLine("    VALUES (@appId, @idx, @url, @apiFunction, 1, getDate() ) ");
                    sb.AppendLine("END ");
                    sb.AppendLine();
                    sb.AppendLine();

                    //aggregate
                    sb.AppendLine("UPDATE	FB_Stat ");
                    sb.AppendFormat("SET	Total = Total {0} 1, dtModified = getDate()  ",
                        (api == _Enums.FB_Api.FB_Like) ? "+" : "-");
                    sb.AppendLine();
                    sb.AppendLine("WHERE	ApplicationId = @appId AND Url = @url AND ApiFunction = @apiAggregate; ");
                    sb.AppendLine();

                    //only create new rows for like
                    if (api == _Enums.FB_Api.FB_Like)
                    {
                        sb.AppendLine("SELECT @rowsAffected = @@ROWCOUNT; ");
                        sb.AppendLine();
                        sb.AppendLine("IF (@rowsAffected = 0) BEGIN  ");
                        sb.AppendLine("    INSERT FB_Stat (ApplicationId, EntityId, Url, ApiFunction, Total, dtModified ) ");
                        sb.AppendLine("    VALUES (@appId, @idx, @url, @apiAggregate, 1, getDate() ) ");
                        sb.AppendLine("END ");
                    }


                    break;
            }

            try
            {
                cmd.CommandSql = sb.ToString();
                SubSonic.DataService.ExecuteQuery(cmd);
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                Wcss._Error.LogException(sex);
                return false;
            }
            catch (Exception ex)
            {
                Wcss._Error.LogException(ex);
                return false;
            }

            return true;
        }
    }

}
