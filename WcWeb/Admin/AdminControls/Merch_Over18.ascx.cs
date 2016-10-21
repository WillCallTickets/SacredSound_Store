using System;
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Linq;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{ 
    public partial class Merch_Over18 : BaseControl
    {
        private System.Text.StringBuilder sb = new System.Text.StringBuilder();   
        private MerchCollection _basecoll = null;
                
        //note this method includes only list items
        protected MerchCollection BaseColl
        {
            get
            {
                if (_basecoll == null)
                {
                    _basecoll = new MerchCollection();
                    sb.Length = 0; 
                    string configVal = _Config._Merch_Requires_18Over_Ack_List.ValueX;

                    if (configVal != null && configVal.Trim().Length > 0)
                    {
                        sb.Append("SELECT * FROM [Merch] m WHERE m.[ApplicationId] = @appId AND m.[tParentListing] IS NULL ");
                        sb.AppendFormat("AND m.[Id] IN ({0}) ORDER BY m.[Name] ASC ", configVal);

                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);
                        _basecoll.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd));
                    }
                }

                return _basecoll;
            }
        }

        private List<int> _theList = null;
        public List<int> TheList
        {
            get
            {
                if (_theList == null)
                {
                    _theList = new List<int>();

                    string configVal = _Config._Merch_Requires_18Over_Ack_List.ValueX;

                    if (configVal != null && configVal.Trim().Length > 0)
                        _theList.AddRange(_Config._Merch_Requires_18Over_Ack_List.ValueX.Split(',').Select<string, int>(int.Parse).ToList());
                }

                return _theList;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void ddlParentList_DataBound(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            grid.DataSource = BaseColl;

            string[] keyNames = { "Id" };
            grid.DataKeyNames = keyNames;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView grid = (GridView)sender;
            DataControlRowType typ = e.Row.RowType;
            DataControlRowState state = e.Row.RowState;

            if (typ == DataControlRowType.DataRow)
            {
                Merch entity = (Merch)e.Row.DataItem;

                LinkButton button = (LinkButton)e.Row.FindControl("btnDelete");
                if (button != null && entity != null)
                    button.OnClientClick = string.Format("return confirm('Are you sure you want to delete {0}?')",
                        Utils.ParseHelper.ParseJsAlert(entity.Name));
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridView grid = (GridView)sender;
            int idx = (int)grid.DataKeys[e.RowIndex]["Id"];

            try
            {
                TheList.Remove(idx);

                SaveListChanges();
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);

                CustomValidator val = (CustomValidator)grid.Rows[e.RowIndex].FindControl("RowValidator");
                if (val != null)
                {
                    val.IsValid = false;
                    val.ErrorMessage = ex.Message;
                }
            }
        }
        
        protected void btnAddSelection_Click(object sender, EventArgs e)
        {
            //Button btn = (Button)sender;
            ListItem sel = ddlParentList.SelectedItem;
            if (sel != null && sel.Value != "0")
            {
                int newAdd = int.Parse(sel.Value);
                //add item to list if it does not exist
                if (!TheList.Contains(newAdd))
                {
                    TheList.Add(newAdd);

                    SaveListChanges();
                }
            }
        }

        protected void SaveListChanges()
        {
            TheList.Sort();
            _Config._Merch_Requires_18Over_Ack_List.ValueX = string.Join(",", TheList.Select(i => i.ToString()).ToArray());
            _Config._Merch_Requires_18Over_Ack_List.Save();
            _Lookits.RefreshLookup(_Enums.LookupTableNames.SiteConfigs.ToString());
            _basecoll = null;

            ddlParentList.DataBind();//calls binding on grid view
        }

        protected void SqlParentList_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
        {
            e.Command.Parameters["@appId"].Value = Wcss._Config.APPLICATION_ID;

            //note this method does not include list items
            string configVal = _Config._Merch_Requires_18Over_Ack_List.ValueX;

            sb.Length = 0;
            sb.Append("SELECT ' Select a merch item' as [ParentName], 0 as [ParentId] UNION ");
            sb.Append("SELECT CASE WHEN m.[mPrice] IS NULL THEN m.[Name] ");
            sb.Append("ELSE m.[Name] + ' (' + CONVERT(varchar, m.[mPrice]) + ')' END as [ParentName], ");
            sb.Append("m.[Id] as [ParentId] ");
            sb.Append("FROM Merch m WHERE m.[ApplicationId] = @appId AND m.[tParentListing] IS NULL ");
            sb.AppendFormat("{0} ORDER BY [ParentName] ASC ",
                (configVal != null && configVal.Trim().Length > 0) ? string.Format("AND m.[Id] NOT IN ({0})", configVal) : string.Empty);

            e.Command.CommandText = sb.ToString();
        }
}
}