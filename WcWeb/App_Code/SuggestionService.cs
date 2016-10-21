using System;
using System.Web.Services;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
//using System.Web.Script.Services;[ScriptMethod(UseHttpGet=true, ResponseFormat = ResponseFormat.Json)]


using Wcss;

namespace WillCallWeb.Admin
{
    /// <summary>
    /// Summary description for SuggestionService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class SuggestionService : System.Web.Services.WebService
    {
        private WillCallWeb.WebContext _ctx;
        protected WillCallWeb.WebContext Ctx { get { return _ctx; } }

        public SuggestionService()
        {
            _ctx = new WillCallWeb.WebContext();
        }

        private string _actSql = null;
        private string _promoterSql = null;
        private string _venueSql = null;
        private string _charitableorgSql = null;

        [WebMethod]        
        public string AutocompleteMerchSelection(string partialName)
        {
            List<ListItem> list = new List<ListItem>();

            string sql = string.Format("SET NOCOUNT ON; SELECT DISTINCT [Id], [Name] FROM [Merch] WHERE [tParentListing] IS NULL AND [Name] LIKE '%' + @searchText + '%' ORDER BY [Name] ");

            _DatabaseCommandHelper helper = new _DatabaseCommandHelper(sql);
            helper.AddCmdParameter("searchText", partialName, System.Data.DbType.String);
            using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(helper.Cmd))
            {
                while (dr.Read())
                    list.Add(new ListItem(dr.GetValue(dr.GetOrdinal("Name")).ToString(), dr.GetValue(dr.GetOrdinal("Id")).ToString()));

                dr.Close();
            }

            var oSerializer = new JavaScriptSerializer();
            string sJson = oSerializer.Serialize(list);

            return sJson;
        }

        [WebMethod]
        public string AutocompleteMerchInventory(string parentId)
        {
            List<ListItem> list = new List<ListItem>();
            list.Add(new ListItem("[All Inventory]", parentId.ToString()));

            string sql = string.Format("SET NOCOUNT ON; SELECT DISTINCT [Id], [Style], [Color], [Size], [iAllotment] FROM [Merch] WHERE [tParentListing] IS NOT NULL AND [tParentListing] = @parentId ORDER BY  [Style], [Color], [Size]");

            _DatabaseCommandHelper helper = new _DatabaseCommandHelper(sql);
            helper.AddCmdParameter("parentId", int.Parse(parentId), System.Data.DbType.Int32);
            using (System.Data.IDataReader dr = SubSonic.DataService.GetReader(helper.Cmd))
            {
                while (dr.Read())
                    list.Add(new ListItem(
                        dr.GetValue(dr.GetOrdinal("Style")).ToString() + 
                        dr.GetValue(dr.GetOrdinal("Color")).ToString() + 
                        dr.GetValue(dr.GetOrdinal("Size")).ToString(),
                //" [allotment: " + dr.GetValue(dr.GetOrdinal("iAllotment")).ToString() + "]", 
                        
                        dr.GetValue(dr.GetOrdinal("Id")).ToString()));

                dr.Close();

                //if there is only one option - simplify for user experience
                if (list.Count == 2)
                    list.RemoveAt(1);
            }

            var oSerializer = new JavaScriptSerializer();
            string sJson = oSerializer.Serialize(list);

            return sJson;
        }

        protected string GetSuggestionSql(string context)
        {
          switch (context.ToLower())
            {
                case "act":
                    if (_actSql != null)
                        return _actSql;
                    break;
                case "promoter":
                    if (_promoterSql != null)
                        return _promoterSql;
                    break;
                case "venue":
                    if (_venueSql != null)
                        return _venueSql;
                    break;
                case "charitableorg":
                    context = "CharitableOrg";
                    if (_charitableorgSql != null)
                        return _charitableorgSql;
                    break;
            }

            StringBuilder sb = new StringBuilder();

            //sb.Append("SELECT Suggestion, Id FROM (	 SELECT	Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum ");
            sb.AppendFormat("FROM	[{0}] a WHERE a.[ApplicationId] = @appId AND CASE @SearchLike WHEN 0 THEN ", context);
            sb.Append("CASE WHEN (@Hint = 't' OR @Hint = 'th' OR @Hint = 'the' OR @Hint = 'the ') THEN ");
			sb.Append("CASE WHEN (a.[Name] >= @Hint) OR (a.[NameRoot] >= @Hint) THEN 1 ELSE 0 END ");
            sb.Append("ELSE CASE WHEN (a.[NameRoot] >= @Hint) THEN 1 ELSE 0 END END ELSE ");
            sb.Append("CASE WHEN (a.[Name] LIKE @Hint + '%') OR (a.[NameRoot] LIKE @Hint + '%') THEN 1 ELSE 0 END END = 1 ) Suggestions ");
            sb.Append("WHERE Suggestions.RowNum BETWEEN 0 AND @ResultSize ");

            switch (context.ToLower())
            {
                case "act":
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _actSql = sb.ToString();
                    break;
                case "promoter":
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _promoterSql = sb.ToString();
                    break;
                case "venue":
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) + CASE WHEN a.[City] IS NOT NULL THEN ' - ' + a.[City] ELSE '' END + CASE WHEN a.[City] IS NOT NULL AND a.[State] IS NOT NULL THEN ', ' + a.[State] ELSE '' END as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _venueSql = sb.ToString();
                    break;
                case "charitableorg":
                    sb.Insert(0, string.Format("SELECT Suggestion, Id FROM (SELECT Distinct(a.[Name]) as 'Suggestion', a.[Id] as 'Id', ROW_NUMBER() OVER (ORDER BY a.[NameRoot] ASC) AS RowNum "));
                    _charitableorgSql = sb.ToString();
                    break;
            }

            return sb.ToString();
        }

        [WebMethod]
        public List<string> Name_Suggestions(string prefixText, int count, string contextKey)
        {
            List<string> suggestions = new List<string>();
            bool useLikeSearchMethod = true;
            string sql = sql = GetSuggestionSql(contextKey);

            switch (contextKey.ToLower()) 
            {
                case "act":
                    useLikeSearchMethod = Ctx.SearchLike_Act;
                    break;
                case "promoter":
                    useLikeSearchMethod = Ctx.SearchLike_Promoter;
                    break;
                case "venue":
                    useLikeSearchMethod = Ctx.SearchLike_Venue;
                    break;
                case "goodorg":
                    useLikeSearchMethod = Ctx.SearchLike_GoodOrg;
                    break;
                case "charitableorg":
                    useLikeSearchMethod = Ctx.SearchLike_CharitableOrg;
                    break;
            }

            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
            cmd.Parameters.Add("@appId", Wcss._Config.APPLICATION_ID, DbType.Guid);
            cmd.Parameters.Add("@Hint", prefixText);
            cmd.Parameters.Add("@ResultSize", count, System.Data.DbType.Int32);
            cmd.Parameters.Add("@SearchLike", (useLikeSearchMethod) ? 1 : 0, System.Data.DbType.Boolean);

            try
            {
                using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
                {
                    while (dr.Read())
                        suggestions.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(
                            dr.GetValue(dr.GetOrdinal("Suggestion")).ToString(), dr.GetValue(dr.GetOrdinal("Id")).ToString()));

                    dr.Close();
                }

                return suggestions;
            }
            catch (System.Data.SqlClient.SqlException sex)
            {
                Wcss._Error.LogException(sex);
            }
            catch (Exception ex)
            {
                Wcss._Error.LogException(ex);
            }

            //return an empty result on error?
            return new List<string>();
        }

    }
}

