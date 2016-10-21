using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


namespace Wcss
{
    [Serializable]
    public partial class WorldshipRow
    {
        private string _uniqueId;
        private DateTime _invoiceDate;
        private int _shipItemId;
        private string _lastNameFirst;
        private string _name;
        private string _address1;
        private string _address2;
        private string _zip;
        private string _city;
        private string _country;
        private string _state;
        private string _phone;
        private string _billingName;
        private string _purchaseEmail;
        private string _packingListIds;
        private string _packingListDescription;

        public string UniqueId { get { return _uniqueId; } set { _uniqueId = value; } }
        public DateTime InvoiceDate { get { return _invoiceDate; } set { _invoiceDate = value; } }
        public int ShipItemId { get { return _shipItemId; } set { _shipItemId = value; } }
        public string LastNameFirst { get { return _lastNameFirst; } set { _lastNameFirst = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public string Address1 { get { return _address1; } set { _address1 = value; } }
        public string Address2 { get { return _address2; } set { _address2 = value; } }
        public string Zip { get { return _zip; } set { _zip = value; } }
        public string City { get { return _city; } set { _city = value; } }
        public string Country { get { return _country; } set { _country = value; } }
        public string State { get { return _state; } set { _state = value; } }
        public string Phone { get { return _phone; } set { _phone = value; } }
        public string BillingName { get { return _billingName; } set { _billingName = value; } }
        public string PurchaseEmail { get { return _purchaseEmail; } set { _purchaseEmail = value; } }
        public string PackingListIds { get { return _packingListIds; } set { _packingListIds = value; } }
        public string PackingListDescription { get { return _packingListDescription; } set { _packingListDescription = value; } }

        public WorldshipRow(IDataReader dr)
        {
            UniqueId = dr.GetValue(dr.GetOrdinal("UniqueId")).ToString();
            InvoiceDate = (DateTime)dr.GetValue(dr.GetOrdinal("InvoiceDate"));
            ShipItemId = (int)dr.GetValue(dr.GetOrdinal("ShipItemId"));
            LastNameFirst = Utils.ParseHelper.StringToProperCase(dr.GetValue(dr.GetOrdinal("LastNameFirst")).ToString());
            Name = dr.GetValue(dr.GetOrdinal("Name")).ToString();
            Address1 = dr.GetValue(dr.GetOrdinal("Address1")).ToString();
            Address2 = dr.GetValue(dr.GetOrdinal("Address2")).ToString();
            Zip = dr.GetValue(dr.GetOrdinal("Zip")).ToString();
            City = dr.GetValue(dr.GetOrdinal("City")).ToString();
            Country = dr.GetValue(dr.GetOrdinal("Country")).ToString();

            State = dr.GetValue(dr.GetOrdinal("State")).ToString();
            Phone = dr.GetValue(dr.GetOrdinal("Phone")).ToString();
            BillingName = dr.GetValue(dr.GetOrdinal("BillingName")).ToString();
            PurchaseEmail = dr.GetValue(dr.GetOrdinal("PurchaseEmail")).ToString();
            PackingListIds = dr.GetValue(dr.GetOrdinal("PackingListIds")).ToString();
            PackingListDescription = dr.GetValue(dr.GetOrdinal("PackingListDescription")).ToString();
        }

        public WorldshipRow(Wcss.QueryRow.ShippingInvoiceShipmentRow row, string packingListIds)
        {
            UniqueId = row.UniqueId;
            InvoiceDate = row.InvoiceDate;
            ShipItemId = row.ShipItemId;
            LastNameFirst = Utils.ParseHelper.StringToProperCase(row.LastNameFirst);
            Name = row.FirstNameLastName;
            Address1 = row.Address1;
            Address2 = row.Address2;
            Zip = row.Zip;
            City = row.City;
            Country = row.Country;
            State = row.State;
            Phone = row.Phone;
            BillingName = row.BillingName;
            PurchaseEmail = row.PurchaseEmail;
            PackingListIds = packingListIds;
            PackingListDescription = string.Format("{0} {1}", row.PackingList, row.PackingAdditional).Trim();
        }

        //public WorldshipRow(string uniqueId, DateTime invoiceDate, int shipItemId, string firstNameLastName, string address1, string address2, 
        //    string city, string state, string zip, string country, string phone, string billingName, string purchaseEmail, string packingListIds, string packingList)
        //{
        //    UniqueId = uniqueId;
        //    InvoiceDate = invoiceDate;
        //    ShipItemId = shipItemId;
        //    Name = firstNameLastName;
        //    Address1 = address1;
        //    Address2 = address2;
        //    Zip = zip;
        //    City = city;
        //    Country = country;
        //    State = state;
        //    Phone = phone;
        //    BillingName = billingName;
        //    PurchaseEmail = purchaseEmail;
        //    PackingListIds = packingListIds;
        //    PackingListDescription = packingList;
        //}

        public static List<WorldshipRow> GetWorldshipExportList(string dateIdscsv)
        {
            List<WorldshipRow> list = new List<WorldshipRow>();

            using (IDataReader dr = SPs.TxGetTicketSalesWorldshipExport(dateIdscsv).GetReader())
            {
                while (dr.Read())
                {
                    WorldshipRow tsr = new WorldshipRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            return list;
        }

        public static List<WorldshipRow> GetWorldshipExportList(int batchId, string filter)
        {
            List<WorldshipRow> list = new List<WorldshipRow>();

            using (IDataReader dr = SPs.TxGetTicketSalesWorldshipExportForBatch(batchId, filter).GetReader())
            {
                while (dr.Read())
                {
                    WorldshipRow tsr = new WorldshipRow(dr);

                    list.Add(tsr);
                }

                dr.Close();
            }

            return list;
        }

        
        /// <summary>
        /// Converts a list of worldshiprow rows for csv export. Intended for entire batches of shipments.
        /// </summary>
        /// <param name="invoiceShipments"></param>
        /// <param name="invoiceItems"></param>
        /// <param name="fileAttachmentName"></param>
        public static void CSV_ProvideDownload(List<WorldshipRow> list, string fileAttachmentName, string pageToAccommodateDownload)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //write header
            sb.AppendFormat("InvoiceId,InvoiceDate,ShipItemId,LastNameFirst,Name,Address1,Address2,Zip,City,Country,State,Phone,BillingName,PurchaseEmail,PackingListIds,PackingListDescription{0}", Environment.NewLine);

            foreach (WorldshipRow worldRow in list)
                ProcessWorldshipRowPerFormat(sb, worldRow);

            Utils.FileLoader.CSV_WriteToContextForDownload(sb, fileAttachmentName, pageToAccommodateDownload);
        }
        /// <summary>
        /// Converts a shipping fulfillment to a worldshiprow for csv export. This particular method is designed for page queries - generally not an entire batch
        /// </summary>
        /// <param name="invoiceShipments"></param>
        /// <param name="invoiceItems"></param>
        /// <param name="fileAttachmentName"></param>
        public static void CSV_ProvideDownload(List<Wcss.QueryRow.ShippingInvoiceShipmentRow> invoiceShipments, List<Wcss.QueryRow.ShippingItemRow> invoiceItems,
            string fileAttachmentName, string pageToAccommodateDownload)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //write header
            sb.AppendFormat("InvoiceId,InvoiceDate,ShipItemId,LastNameFirst,Name,Address1,Address2,Zip,City,Country,State,Phone,BillingName,PurchaseEmail,PackingListIds,PackingListDescription{0}", Environment.NewLine);

            foreach (Wcss.QueryRow.ShippingInvoiceShipmentRow row in invoiceShipments)
            {
                //we need to build a list of ticket ids for the report
                System.Text.StringBuilder ss = new System.Text.StringBuilder();
                List<Wcss.QueryRow.ShippingItemRow> items = new List<Wcss.QueryRow.ShippingItemRow>();
                items.AddRange(invoiceItems.FindAll(delegate(Wcss.QueryRow.ShippingItemRow match) { return (match.tInvoiceId == row.InvoiceId); }));
                foreach (Wcss.QueryRow.ShippingItemRow itm in items)
                    ss.AppendFormat("{0}~", itm.tShowTicketId.ToString());

                WorldshipRow worldRow = new WorldshipRow(row, ss.ToString().TrimEnd('~'));

                ProcessWorldshipRowPerFormat(sb, worldRow);
            }

            Utils.FileLoader.CSV_WriteToContextForDownload(sb, fileAttachmentName, pageToAccommodateDownload);
        }

        /// <summary>
        /// Note that InvoiceId is actually unqique id
        /// "InvoiceId,InvoiceDate,ShipItemId,Name,Address1,Address2,Zip,City,Country,State,Phone,BillingName,PurchaseEmail,PackingListIds,PackingListDescription;
        /// </summary>
        /// <param name="sb"></param>
        private static void ProcessWorldshipRowPerFormat(StringBuilder sb, WorldshipRow row)
        {
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\"{15}\"{16}",
            row.UniqueId.Replace(',', ' '),
            row.InvoiceDate.ToString("MM/dd/yyyy hh:mmtt"),
            row.ShipItemId.ToString(),
            row.LastNameFirst.Replace(',', ' '), 
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
}
