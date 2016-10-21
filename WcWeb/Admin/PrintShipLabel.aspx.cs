using System;
using System.Data;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin
{
    public partial class PrintShipLabel : WillCallWeb.BasePage
    {
        private _Enums.ProductContext shipCtx = _Enums.ProductContext.all;
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string req = Request["ctx"];

            if (req != null)
                shipCtx = (_Enums.ProductContext)Enum.Parse(typeof(_Enums.ProductContext), req, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCols.DataBind();
                ddlRows.DataBind();
                dlLabel.DataBind();
            }
        }

        #region Rows and Columns

        protected void ddlOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            dlLabel.DataBind();
        }

        protected void ddlRows_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.Items.Count == 0)
            {
                int i = 0;
                while (i < 75)
                {
                    i += 10;
                    ddl.Items.Add(i.ToString());
                }
            }       
        }
        protected void ddlRows_SelectedIndexChanged(object sender, EventArgs e)
        {
            dlLabel.DataBind();
        }
        
        protected void ddlCols_DataBinding(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            if (ddl.Items.Count == 0)
                for (int i = 1; i < 10; i++)
                    ddl.Items.Add(i.ToString());
        }
        protected void ddlCols_SelectedIndexChanged(object sender, EventArgs e)
        {
            dlLabel.DataBind();
        }

        #endregion

        #region repeater Label Grid

        protected void dlLabel_DataBinding(object sender, EventArgs e)
        {
            DataList dl = (DataList)sender;
            SubSonic.QueryCommand qry = new SubSonic.QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            int cols = int.Parse(ddlCols.SelectedValue);
            int rows = int.Parse(ddlRows.SelectedValue);
            int max = cols * rows;

            sb.AppendFormat("SELECT TOP {0} ", max);
            sb.Append("ISNULL([CompanyName],'') as 'CompanyName', [FirstName] + ' ' + [LastName] as 'Name', [Address1], ISNULL([Address2],'') as 'Address2', [City], ");
            sb.Append("[StateProvince] as 'State', [PostalCode] as 'Zip', [Country], [Id], [ReferenceNumber], [vcCarrier], [ShipMethod] ");
            sb.Append("FROM [InvoiceShipment] ship WHERE ship.[bLabelPrinted] = 0 AND ");
            sb.AppendFormat("[vcContext] = '{0}' ORDER BY [Id] {1} ", shipCtx.ToString(), ddlOrder.SelectedValue);

            DataSet ds = Utils.DataHelper.ExecuteQuery(sb, _Config.DSN);

            dl.DataSource = ds;
            dl.RepeatColumns = cols;
        }
        protected void dlLabel_ItemDataBound(object sender, DataListItemEventArgs e) {}

        protected void dlLabel_ItemCommand(object source, DataListCommandEventArgs e) {}

        #endregion
}
}