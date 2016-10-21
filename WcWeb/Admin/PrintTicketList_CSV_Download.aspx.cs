using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

using Wcss;

namespace WillCallWeb.Admin
{
    /// <summary>
    /// This page is only to be used against a show date
    /// </summary>
    public partial class PrintTicketList_CSV_Download : WillCallWeb.BasePage
    {
        protected ShowDate ShowDateRecord;
        
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(false);
            base.OnPreInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetInputs();

                GridView1.DataBind();
            }
        }

        #region Set Inputs

        private void SetInputs()
        {
            //dates have precedence
            string date = Request.QueryString["date"];

            if (date != null && Utils.Validation.IsInteger(date))
            {
                ShowDateRecord = new ShowDate(int.Parse(date));
                
                if (ShowDateRecord != null && ShowDateRecord.ShowRecord.ApplicationId != _Config.APPLICATION_ID)
                    ShowDateRecord = null;
            }
            else
            {
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = "The event date was not specified.";
                return;
            }

            if (ShowDateRecord == null)
            {
                string errorMsg = string.Format("The event specified ({0}) does not match the application ({1}).", date, _Config.APPLICATION_ID);

                _Error.LogException(new Exception(errorMsg));

                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = errorMsg;
                return;
            }
        }

        #endregion

        StringBuilder sb = new StringBuilder();

        private void ListSales()
        {
            sb.Length = 0;

            sb.AppendFormat("InvoiceId,InvoiceDate,ShipItemId,Name,Address1,Address2,Zip,City,Country,State,Phone,BillingName,PurchaseEmail,PackingListIds,PackingListDescription{0}", Environment.NewLine);

            List<WorldshipRow> list = WorldshipRow.GetWorldshipExportList(ShowDateRecord.Id.ToString());

            foreach (WorldshipRow row in list)
            {
                //sb.Append("ShipItemId,Name,Address1,Address2,Zip,City,State,Phone,BillingName,PurchaseEmail,PackingListIds,PackingListDescription\r\n");
               sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\"{15}",
                    row.UniqueId.Replace(',', ' '),
                    row.InvoiceDate.ToString("MM/dd/yyyy hh:mmtt"),
                    row.ShipItemId.ToString(),
                    row.Name.Replace(',', ' '),
                    row.Address1.Replace(',', ' '),
                    row.Address2.Replace(',', ' '),
                    row.Zip.Replace(',', ' '),
                    row.City.Replace(',', ' '),
                    row.Country.Replace(',', ' '),
                    row.State.Replace(',', ' '),
                    row.Phone.Replace(',', ' '),
                    row.BillingName.Replace(',', ' '),
                    row.PurchaseEmail.Replace(',', ' '),
                    row.PackingListIds.Replace(',', ' '),
                    row.PackingListDescription.Replace(',', ' '),
                    Environment.NewLine);
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            ExportCSV(ShowDateRecord);
        }
        public void ExportCSV(ShowDate _showDateRecord)
        {
            sb.Length = 0;

            string date = Request.QueryString["date"];
           
            if (date != null && Utils.Validation.IsInteger(date))
            {
                ShowDateRecord = new ShowDate(int.Parse(date));
                
                if (ShowDateRecord != null && ShowDateRecord.ShowRecord.ApplicationId != _Config.APPLICATION_ID)
                    ShowDateRecord = null;
            }
            else
            {
                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = "The event date was not specified.";
                return;
            }

            if (ShowDateRecord == null)
            {
                string errorMsg = string.Format("The event specified ({0}) does not match the application ({1}).", date, _Config.APPLICATION_ID);

                _Error.LogException(new Exception(errorMsg));

                CustomValidation.IsValid = false;
                CustomValidation.ErrorMessage = errorMsg;
                return;
            }

            string attachment = string.Format("attachment; filename=TicketList_{0}.csv", ShowDateRecord.DateOfShow.ToString("MM_dd_yyyy_hhmmtt"));

            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.ContentType = "application/x-download";//"text/csv";
            context.Response.AddHeader("Content-Disposition", attachment);

            //TODO: write header
            sb.AppendFormat("InvoiceId,InvoiceDate,ShipItemId,Name,Address1,Address2,Zip,City,Country,State,Phone,BillingName,PurchaseEmail,PackingListIds,PackingListDescription{0}", Environment.NewLine);

            List<WorldshipRow> list = WorldshipRow.GetWorldshipExportList(ShowDateRecord.Id.ToString());

            foreach (WorldshipRow row in list)
            {
                //sb.Append("InvoiceId,InvoiceDate,ShipItemId,Name,Address1,Address2,Zip,City,Country,State,Phone,BillingName,PurchaseEmail,PackingListIds,PackingListDescription\r\n");
                sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\"{15}",
                    row.UniqueId.Replace(',', ' '),
                    row.InvoiceDate.ToString("MM/dd/yyyy hh:mmtt"),
                    row.ShipItemId.ToString(),
                    row.Name.Replace(',', ' '),
                    row.Address1.Replace(',', ' '),
                    row.Address2.Replace(',', ' '),
                    row.Zip.Replace(',', ' '),
                    row.City.Replace(',', ' '),
                    row.Country.Replace(',', ' '),
                    row.State.Replace(',', ' '),
                    row.Phone.Replace(',', ' '),
                    row.BillingName.Replace(',', ' '),
                    row.PurchaseEmail.Replace(',', ' '),
                    row.PackingListIds.Replace(',', ' '),
                    row.PackingListDescription.Replace(',', ' '),
                    Environment.NewLine);
            }

            context.Response.Write(sb.ToString());
            context.Response.End();
        }
        protected void GridView1_DataBinding(object sender, EventArgs e)
        {
            GridView grid = (GridView)sender;

            List<WorldshipRow> list = WorldshipRow.GetWorldshipExportList(ShowDateRecord.Id.ToString());

            grid.DataSource = list;
        }
}
}