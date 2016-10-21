using System;
using System.IO;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Prom_BannerOrder : BaseControl
    {
        #region New paging

        protected void GridView_DataBound(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            if (grid.SelectedIndex == -1 && grid.Rows.Count > 0)
                grid.SelectedIndex = 0;
        }

        #endregion

        #region Page Overhead

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GridView1.DataBind();
            }
        }

        #endregion

        #region Grid

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = (GridView)sender;
            string cmd = e.CommandName.ToLower();

            switch (cmd)
            {
                case "up":
                case "down":
                    int ordHold = int.Parse(e.CommandArgument.ToString());
                    int ordSwap = (cmd == "up") ? ordHold - 1 : ordHold + 1;

                    int idxHold = (int)grid.DataKeys[ordHold]["Id"];
                    int idxSwap = (int)grid.DataKeys[ordSwap]["Id"];

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("DECLARE @holdOrder int SELECT @holdOrder = [iDisplayOrder] FROM [SalePromotion] WHERE [Id] = @idxHold ");
                    sb.Append("DECLARE @swapOrder int SELECT @swapOrder = [iDisplayOrder] FROM [SalePromotion] WHERE [Id] = @idxSwap ");
                    sb.Append("UPDATE [SalePromotion] SET [iDisplayOrder] = @swapOrder WHERE [Id] = @idxHold ");
                    sb.Append("UPDATE [SalePromotion] SET [iDisplayOrder] = @holdOrder WHERE [Id] = @idxSwap ");

                    SubSonic.QueryCommand command = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                    command.Parameters.Add("@idxHold", idxHold, DbType.Int32);
                    command.Parameters.Add("@idxSwap", idxSwap, DbType.Int32);

                    SubSonic.DataService.ExecuteQuery(command);

                    GridView1.DataBind();

                    break;
            }
        }
        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;

            LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
            LinkButton down = (LinkButton)e.Row.FindControl("btnDown");

            if (up != null && down != null)
            {
                up.CommandArgument = e.Row.DataItemIndex.ToString();
                down.CommandArgument = e.Row.DataItemIndex.ToString();
            }
        }
        protected int _rowCounter = 0;
        protected void GridView_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;
            _rowCounter = grid.PageSize * grid.PageIndex;
        }        
        protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //display row number
                _rowCounter += 1;
                Literal rowCounter = (Literal)e.Row.FindControl("LiteralRowCounter");
                if (rowCounter != null)
                    rowCounter.Text = _rowCounter.ToString();

                LinkButton up = (LinkButton)e.Row.FindControl("btnUp");
                LinkButton down = (LinkButton)e.Row.FindControl("btnDown");
                
                if (up != null && down != null)
                {
                    up.Enabled = (e.Row.RowIndex > 0);

                    int rowCount = _selectedRows;// grid.DataKeys.Count;

                    down.Enabled = (e.Row.RowIndex < rowCount - 1);
                }

                Literal litNaming = (Literal)e.Row.FindControl("litNaming");
                Literal litImage = (Literal)e.Row.FindControl("litImage");
                Literal litDates = (Literal)e.Row.FindControl("litDates");

                if (litNaming != null && litImage != null && litDates != null)
                {
                    //SalePromotion entity = (SalePromotion)e.Row.DataItem;

                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    DataRow row = drv.Row;

                    //name display additional
                    string name = row.ItemArray.GetValue(row.Table.Columns.IndexOf("Name")).ToString();
                    string display = row.ItemArray.GetValue(row.Table.Columns.IndexOf("DisplayText")).ToString();
                    string additional = row.ItemArray.GetValue(row.Table.Columns.IndexOf("AdditionalText")).ToString();

                    string txt = string.Format("{0}{1}{2}", 
                        (name != null && name.Trim().Length > 0) ? string.Format("<div>{0}</div>", name) : string.Empty,
                        (display != null && display.Trim().Length > 0) ? string.Format("<div>{0}</div>", display) : string.Empty,
                        (additional != null && additional.Trim().Length > 0) ? string.Format("<div>{0}</div>", additional) : string.Empty);
                    
                    litNaming.Text = txt.Trim();

                    string imageUrl = row.ItemArray.GetValue(row.Table.Columns.IndexOf("BannerUrl")).ToString();

                    string image = string.Empty;
                    if (imageUrl != null && imageUrl.Trim().Length > 0)
                    {
                        //get the image - dimensions
                        //display the image only so big - max height : 50 max width: 200
                        //so two versions of the image
                        string mappedPath = Server.MapPath(string.Format("{0}{1}", Wcss.SalePromotion.Banner_VirtualDirectory, imageUrl));

                        try
                        {
                            Pair p = Utils.ImageTools.GetDimensions(mappedPath);

                            //if over max width//if over max height
                            int width = (int)p.First;
                            int height = (int)p.Second;

                            imageUrl = string.Format("{0}{1}", Wcss.SalePromotion.Banner_VirtualDirectory, imageUrl);

                            if (height > 50)
                                image = string.Format("<img src=\"{0}\" border=\"0\" height=\"45\" />", imageUrl);
                            else
                                image = string.Format("<img src=\"{0}\" border=\"0\" width=\"180\" />", imageUrl);
                        }
                        catch (Exception ex)
                        {
                            Wcss._Error.LogException(ex);
                        }
                    }

                    litImage.Text = (image.Trim().Length > 0) ? image : string.Empty;

                    DateTime startDate = DateTime.MinValue;
                    DateTime endDate = DateTime.MaxValue;
                    object go = row.ItemArray.GetValue(row.Table.Columns.IndexOf("dtStartDate"));
                    object stop = row.ItemArray.GetValue(row.Table.Columns.IndexOf("dtEndDate"));

                    if (go != null && go.ToString().Trim().Length > 0)
                        startDate = (DateTime)go;
                    if (stop != null && stop.ToString().Trim().Length > 0)
                        endDate = (DateTime)stop;

                    string start = (startDate <= DateTime.MinValue) ? string.Empty : startDate.ToString("MM/dd/yyyy hh:mmtt");
                    string end = (endDate >= DateTime.MaxValue) ? string.Empty : endDate.ToString("MM/dd/yyyy hh:mmtt");
                    litDates.Text = string.Format("{0} {2} {1}", (start.Trim().Length > 0) ? string.Format("<div>start {0}</div>", start) : string.Empty, 
                        (end.Trim().Length > 0) ? string.Format("<div>end {0}</div>", end) : string.Empty,
                        (start.Trim().Length > 0 && start.Trim().Length > 0) ? "-" : string.Empty).Trim();
                }
            }
        }
        #endregion

        protected void SqlList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;
        }
        int _selectedRows = 0;
        protected void SqlList_Selected(object sender, SqlDataSourceStatusEventArgs e)
        {
            _selectedRows = e.AffectedRows;
        }
}
}