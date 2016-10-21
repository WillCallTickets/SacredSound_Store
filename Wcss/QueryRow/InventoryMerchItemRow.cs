using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Wcss.QueryRow
{
    [Serializable]
    public partial class InventoryMerchItemRow
    {
        private int _divId;
        private int _catId;
        private int _merchId;
        private int _parentId;
        private string _parentName;
        private string _merchName;
        private string _style;
        private string _color;
        private string _size;
        private bool _isActive;
        private bool _isFeatured;
        private bool _isSoldOut;
        private int _allot;
        private int _dmg;
        private int _pend;
        private int _sold;
        private int _avail;
        private string _vcDeliveryType;
        private decimal _weight;
        private decimal _price;
        //private string _description;

        public int DivId { get { return _divId; } set { _divId = value; } }
        public string DivName { get { return ((MerchDivision)_Lookits.MerchDivisions.Find(DivId)).Name; } }
        public int CatId { get { return _catId; } set { _catId = value; } }
        public string CatName { get { return ((MerchCategorie)_Lookits.MerchCategories.Find(CatId)).Name; } }
        public int ParentId { get { return _parentId; } set { _parentId = value; } }
        public int MerchId { get { return _merchId; } set { _merchId = value; } }
        public string ParentName { get { return _parentName; } set { _parentName = value; } }
        public string MerchName { get { return _merchName; } set { _merchName = value; } }
        public string Style { get { return _style; } set { _style = value; } }
        public string Color { get { return _color; } set { _color = value; } }
        public string Size { get { return _size; } set { _size = value; } }
        public bool IsActive { get { return _isActive; } set { _isActive = value; } }
        public bool IsFeatured { get { return _isFeatured; } set { _isFeatured = value; } }
        public bool IsSoldOut { get { return _isSoldOut; } set { _isSoldOut = value; } }
        public int Allot { get { return _allot; } set { _allot = value; } }
        public int Dmg { get { return _dmg; } set { _dmg = value; } }
        public int Pend { get { return _pend; } set { _pend = value; } }
        public int Sold { get { return _sold; } set { _sold = value; } }
        public int Avail { get { return _avail; } set { _avail = value; } }
        public string VcDeliveryType { get { return _vcDeliveryType; } set { _vcDeliveryType = value; } }
        public _Enums.DeliveryType DeliveryType
        {
            get
            {
                if (this.VcDeliveryType == null) return _Enums.DeliveryType.parcel;
                return (_Enums.DeliveryType)Enum.Parse(typeof(_Enums.DeliveryType), this.VcDeliveryType, true);
            }
        }
        public decimal Weight { get { return _weight; } set { _weight = value; } }
        public decimal Price { get { return _price; } set { _price = value; } }
        //public string Description { get { return _description; } set { _description = value; } }

        public InventoryMerchItemRow(IDataReader dr)
        {
            
            DivId = (int)dr.GetValue(dr.GetOrdinal("DivId"));
            CatId = (int)dr.GetValue(dr.GetOrdinal("CatId"));
            MerchId = (int)dr.GetValue(dr.GetOrdinal("MerchId"));
            object pid = dr.GetValue(dr.GetOrdinal("ParentId"));
            ParentId = (pid == null || pid.ToString().Trim().Length == 0) ? 0 : (int)pid;
            ParentName = dr.GetValue(dr.GetOrdinal("ParentName")).ToString();
            MerchName = dr.GetValue(dr.GetOrdinal("MerchName")).ToString();
            Style = dr.GetValue(dr.GetOrdinal("Style")).ToString();
            Color = dr.GetValue(dr.GetOrdinal("Color")).ToString();
            Size = dr.GetValue(dr.GetOrdinal("Size")).ToString();
            
            object isa = dr.GetValue(dr.GetOrdinal("IsActive"));
            IsActive = (isa == null || isa.ToString().Trim().Length == 0 || isa.ToString() == "1" || bool.Parse(isa.ToString()) == true) ? 
                true : false;

            object isf = dr.GetValue(dr.GetOrdinal("IsFeatured"));
            IsFeatured = (isf == null || isf.ToString().Trim().Length == 0 || isf.ToString() == "0" || bool.Parse(isf.ToString()) == false) ? 
                false : true;

            object iso = dr.GetValue(dr.GetOrdinal("IsSoldOut"));
            IsSoldOut = (iso == null || iso.ToString().Trim().Length == 0 || iso.ToString() == "0" || bool.Parse(iso.ToString()) == false) ? 
                false : true;

            Allot = (int)dr.GetValue(dr.GetOrdinal("Allot"));
            Dmg = (int)dr.GetValue(dr.GetOrdinal("Dmg"));
            Pend = (int)dr.GetValue(dr.GetOrdinal("Pend"));
            Sold = (int)dr.GetValue(dr.GetOrdinal("Sold"));
            Avail  = (int)dr.GetValue(dr.GetOrdinal("Avail"));

            Price = (decimal)dr.GetValue(dr.GetOrdinal("mPrice"));
            VcDeliveryType = dr.GetValue(dr.GetOrdinal("vcDeliveryType")).ToString();
            Weight = (decimal)dr.GetValue(dr.GetOrdinal("mWeight"));
            //Description = dr.GetValue(dr.GetOrdinal("Description")).ToString();
            //    SUBSTRING(ISNULL(m.[Description],''),1,50) as 'Description', 
        }

        public static List<InventoryMerchItemRow> GetMerchParents_ByDivCat(string delivery, int divId, int catId, string activeStatus, 
            int startRowIndex, int maximumRows)
        {
            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<InventoryMerchItemRow> list = new List<InventoryMerchItemRow>();
            
            using (IDataReader dr = SPs.TxGetMerchParentsByDivCat(_Config.APPLICATION_ID, _Config.DeliveryTypeDefault.ToString(), 
                delivery, divId, catId, activeStatus, startRowIndex, maximumRows).GetReader())
            {
                while (dr.Read())
                {
                    InventoryMerchItemRow cpr = new InventoryMerchItemRow(dr);

                    list.Add(cpr);
                }

                dr.Close();
            }

            return list;
        }

        public static int GetMerchParents_ByDivCat_Count(string delivery, int divId, int catId, string activeStatus)
        {
            //Note that invoice can be listed in both categories - depends on items in invoice
            int count = 0;

            using (IDataReader dr = SPs.TxGetMerchParentsByDivCatCount(_Config.APPLICATION_ID, _Config.DeliveryTypeDefault.ToString(), delivery, divId, catId, activeStatus).GetReader())
            {
                while (dr.Read())
                {
                    count = (int)dr.GetValue(0);
                }

                dr.Close();
            }

            return count;
        }


        public static List<InventoryMerchItemRow> GetMerch_CSVReport(string delivery, int divId, int catId, string activeStatus)
        {
            //Note that invoice can be listed in both categories - ndepends on items in invoice
            List<InventoryMerchItemRow> list = new List<InventoryMerchItemRow>();

            using (IDataReader dr = SPs.TxCsvMerchByDivCat(_Config.APPLICATION_ID, _Config.DeliveryTypeDefault.ToString(),
                delivery, divId, catId, activeStatus).GetReader())
            {
                while (dr.Read())
                {
                    InventoryMerchItemRow cpr = new InventoryMerchItemRow(dr);

                    list.Add(cpr);
                }

                dr.Close();
            }

            return list;
        }

        //CSV//
        /// <summary>
        /// Converts a list of merch rows for csv export. Intended for entire batches of shipments.
        /// </summary>
        /// <param name="invoiceShipments"></param>
        /// <param name="invoiceItems"></param>
        /// <param name="fileAttachmentName"></param>
        public static void CSV_ProvideDownload(List<InventoryMerchItemRow> list, string fileAttachmentName, string pageToAccommodateDownload)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //write header
            sb.AppendFormat("Division,Category,Id,ParentId,ParentName,Style,Color,Size,IsActive,IsFeatured,IsSoldOut,Allotment,Damaged,Pending,Sold,Available,DeliveryType,Weight,Price{0}", Environment.NewLine);

            foreach (InventoryMerchItemRow row in list)
                ProcessRowPerFormat(sb, row);

            CSV_WriteToContextForDownload(sb, fileAttachmentName, pageToAccommodateDownload);
        }

        private static void ProcessRowPerFormat(StringBuilder sb, InventoryMerchItemRow row)
        {
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\"{19}",
                
            row.DivName.Replace(',', ' '),
            row.CatName.Replace(',', ' '),
            row.MerchId.ToString(),
            row.ParentId.ToString(),
            row.ParentName.Replace(',', ' '),

            row.Style.Replace(',', ' '),
            row.Color.Replace(',', ' '),
            row.Size.Replace(',', ' '),
            row.IsActive.ToString(),
            row.IsFeatured.ToString(),

            row.IsSoldOut.ToString(),
            row.Allot.ToString(),
            row.Dmg.ToString(),
            row.Pend.ToString(),
            row.Sold.ToString(),

            row.Avail.ToString(),
            row.VcDeliveryType.Replace(',', ' '),
            row.Weight.ToString(),
            row.Price.ToString(),
            Environment.NewLine);
        }

        /*
        private int _divId;
        private int _catId;
        private int _merchId;
        private int _parentId;
        private string _parentName;
        private string _merchName;
        private string _style;
        private string _color;
        private string _size;
        private bool _isActive;
        private bool _isFeatured;
        private bool _isSoldOut;
        private int _allot;
        private int _dmg;
        private int _pend;
        private int _sold;
        private int _avail;
        private string _vcDeliveryType;
        private decimal _weight;
        private decimal _price;
        */

        public static void CSV_WriteToContextForDownload(System.Text.StringBuilder sb, string attachment, string pageToAccommodateDownload)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            context.Response.Clear();
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "application/x-download";//"text/csv";
            context.Response.AddHeader("Content-Disposition", attachment);

            //context.Response.AddHeader("Content-disposition",
            //    string.Format("attachment;filename={0}", attachment));

            try
            {
                context.Response.Write(sb.ToString());
                context.Response.End();//this may thread abort

                return;
            }
            catch (System.Threading.ThreadAbortException)
            {
                //we can safely ignore this error 
                return;
            }
            catch (Exception ex)
            {
                _Error.LogException(ex);
            }
        }


    }
}
