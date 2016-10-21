using System;
using System.Collections.Generic;
using System.Data;

namespace Wcss.QueryRow
{
    [Serializable]
    public partial class ItemBillShipsRow
    {
        public int InvoiceId { get; set; }
        public int QtyPurchased { get; set; }
        public string PurchaserEmail { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingPhone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string ShipMessage { get; set; }

        public ItemBillShipsRow(IDataReader dr)
        {
            InvoiceId = (int)dr.GetValue(dr.GetOrdinal("InvoiceId"));
            QtyPurchased = (int)dr.GetValue(dr.GetOrdinal("QtyPurchased"));
            PurchaserEmail = dr.GetValue(dr.GetOrdinal("PurchaserEmail")).ToString();
            BillingFirstName = dr.GetValue(dr.GetOrdinal("BillingFirstName")).ToString();
            BillingLastName = dr.GetValue(dr.GetOrdinal("BillingLastName")).ToString();
            BillingPhone = dr.GetValue(dr.GetOrdinal("BillingPhone")).ToString();
            FirstName = dr.GetValue(dr.GetOrdinal("FirstName")).ToString();
            LastName = dr.GetValue(dr.GetOrdinal("LastName")).ToString();
            Address1 = dr.GetValue(dr.GetOrdinal("Address1")).ToString();
            Address2 = dr.GetValue(dr.GetOrdinal("Address2")).ToString();
            City = dr.GetValue(dr.GetOrdinal("City")).ToString();
            StateProvince = dr.GetValue(dr.GetOrdinal("StateProvince")).ToString();
            PostalCode = dr.GetValue(dr.GetOrdinal("PostalCode")).ToString();
            Country = dr.GetValue(dr.GetOrdinal("Country")).ToString();
            Phone = dr.GetValue(dr.GetOrdinal("Phone")).ToString();
            ShipMessage = dr.GetValue(dr.GetOrdinal("ShipMessage")).ToString();
        }
    }

    public class ItemBillShips
    {
        public int ItemId { get; set; }
        public bool IsExclusive { get; set; }
        public int MinQty { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public _Enums.InventoryCheck_Context Context { get; set; }
        public List<ItemBillShipsRow> Listing = new List<ItemBillShipsRow>();
        public int startRowIndex { get; set; }
        public int maximumRows { get; set; }
                
        /// <summary>
        /// This will report for parents or a single child
        /// </summary>
        /// <param name="m"></param>
        public ItemBillShips(Merch m)
        {
            ItemId = m.Id;
            IsExclusive = true;
            MinQty = 1;
            Context = _Enums.InventoryCheck_Context.Ticket;
            DtStart = DateTime.Parse("1/1/2005 12AM");
            DtEnd = DateTime.Now.AddDays(7);
            startRowIndex = 1;
            maximumRows = 100000;

            RunBillShips();
        }
        /// <summary>
        /// This will report for tickets - packages report as a linked entity - one reports for all
        /// </summary>
        /// <param name="m"></param>
        //public ItemBillShips(ShowTicket m)
        //{
        //    ItemId = m.Id;
        //    IsExclusive = true;
        //    MinQty = 1;
        //    Context = _Enums.InventoryCheck_Context.Ticket;
        //    DtStart = DateTime.Parse("1/1/2005 12AM");
        //    DtEnd = DateTime.Now.AddDays(7);
        //    startRowIndex = 1;
        //    maximumRows = 100000;

        //    RunBillShips();
        //}
        ///// <summary>
        ///// This will report for a salepromotion - decides merch or ticket
        ///// </summary>
        ///// <param name="m"></param>
        //public ItemBillShips(SalePromotion m)
        //{
        //    ItemId = m.Id;
        //    IsExclusive = true;
        //    MinQty = 1;
        //    Context = (m.IsMerchPromotion) ? _Enums.InventoryCheck_Context.MerchPromo : _Enums.InventoryCheck_Context.TicketPromo;
        //    DtStart = DateTime.Parse("1/1/2005 12AM");
        //    DtEnd = DateTime.Now.AddDays(7);
        //    startRowIndex = 1;
        //    maximumRows = 100000;

        //    RunBillShips();
        //}

        private void RunBillShips()
        {
            switch(Context)
            {
                case _Enums.InventoryCheck_Context.Merch:
                    Listing = GetItemBillShipsRow_Merch(ItemId, IsExclusive, MinQty, DtStart, DtEnd, startRowIndex, maximumRows);
                    break;
            }
        }

        public static void GetItemBillShips_CSVReport(int itemId, bool isExclusive, int minQty, DateTime startDate, DateTime endDate,
            string fileAttachmentName, string pageToAccommodateDownload)
        {
            List<ItemBillShipsRow> list = GetItemBillShipsRow_Merch(itemId, isExclusive, minQty, startDate, endDate, 1, 100000);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //write header
            sb.AppendFormat("InvoiceId,QtyPurchased,PurchaserEmail,BillingFirstName,BillingLastName,BillingPhone,FirstName,LastName,");
            sb.AppendFormat("Address1,Address2,City,StateProvince,PostalCode,Country,Phone,ShipMessage{0}", Environment.NewLine);

            foreach (ItemBillShipsRow rw in list)
                ProcessRowPerFormat(sb, rw);

            Utils.FileLoader.CSV_WriteToContextForDownload(sb, fileAttachmentName, pageToAccommodateDownload);
        }

        private static void ProcessRowPerFormat(System.Text.StringBuilder sb, ItemBillShipsRow row)
        {
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\"{16}",
            row.InvoiceId.ToString(),
            row.QtyPurchased.ToString(),            
            row.PurchaserEmail.Replace(',', ' '),
            row.BillingFirstName.Replace(',', ' '),
            row.BillingLastName.Replace(',', ' '),
            //4^
            row.BillingPhone.Replace(',', ' '),
            row.FirstName.Replace(',', ' '),
            row.LastName.Replace(',', ' '),
            row.Address1.Replace(',', ' '),
            row.Address2.Replace(',', ' '),            
            //9^
            row.City.Replace(',', ' '),
            row.StateProvince.Replace(',', ' '),
            row.PostalCode.Replace(',', ' '),
            row.Country.Replace(',', ' '),            
            row.Phone.Replace(',', ' '),
            //14^
            row.ShipMessage.Replace(',', ' '),
            Environment.NewLine);
        }

        public static List<ItemBillShipsRow> GetItemBillShipsRow_Merch(int itemId, bool isExclusive, int minQty, DateTime dtStart, DateTime dtEnd, int startRowIndex, int maximumRows)
        {
            dtStart = dtStart.Date;
            dtEnd = dtEnd.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            List<ItemBillShipsRow> list = new List<ItemBillShipsRow>();

            using (IDataReader dr = SPs.TxGetBillShipsOfMerchItem(_Config.APPLICATION_ID, itemId, isExclusive, minQty, startRowIndex, maximumRows,
                dtStart, dtEnd).GetReader())
            {
                while (dr.Read())
                {
                    ItemBillShipsRow rw = new ItemBillShipsRow(dr);

                    list.Add(rw);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetItemBillShipsRow_Count(int itemId, bool isExclusive, int minQty, DateTime dtStart, DateTime dtEnd)
        {
            dtStart = dtStart.Date;
            dtEnd = dtEnd.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Note that invoice can be listed in both categories - ndepends on items in invoice
            int count = 0;

            using (IDataReader dr = SPs.TxGetBillShipsOfMerchItemCount(_Config.APPLICATION_ID, itemId, isExclusive, minQty, dtStart, dtEnd).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }
    }
}