using System;
using System.Text;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.ErrorViewer
{
    public partial class Listing : WillCallWeb.BasePage
    {
        StringBuilder sb = new StringBuilder();
        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(string.Empty, "ErrorLog");

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
            {
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#errorlisting", true);
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {   
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.Rows.Count > 0 && grid.SelectedIndex == -1)
                grid.SelectedIndex = 0;

            btnArchive.Visible = grid.Rows.Count > 0;
        }

        protected void SqlDataSource1_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@ApplicationName"].Value = Wcss._Config.APPLICATION_NAME;
        }

        protected void btnArchive_Click(object sender, EventArgs e)
        {
            sb.Length = 0;//INSERT LogArchive(Id, [Date], [Source], [Message], [Form], [Querystring], [TargetSite], [StackTrace], [Referrer], [IpAddress], [Email], [ApplicationName]) ");
            sb.Append("CREATE TABLE #tmpLog(Idx int); INSERT #tmpLog(Idx) ");
            sb.Append("SELECT l.[Id] as 'Idx' FROM [Log] l WHERE l.[ApplicationName] = @appName; ");

            sb.Append("INSERT [LogArchive] ");
            sb.Append("SELECT l.* FROM [Log] l, [#tmpLog] tl ");
            sb.Append("WHERE tl.[Idx] NOT IN (SELECT [Id] FROM [LogArchive]) AND l.[Id] = tl.[Idx]; ");

            sb.Append("DELETE FROM [Log] WHERE [Id] IN (SELECT [Idx] FROM #tmpLog); ");

            sb.Append("DROP TABLE #tmpLog; ");

            //cmd.Provider.con
            string name = cmd.ProviderName;
            cmd.CommandSql = sb.ToString();
            cmd.Parameters.Add("@appName", _Config.APPLICATION_NAME);

            try
            {
                SubSonic.DataService.ExecuteQuery(cmd);

                //GridView1.DataBind();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }
    }
}