using System;
using System.Xml.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace Wcss
{
    public partial class Inventory
    {
        #region Properties

        [XmlAttribute("ParentContext")]
        public _Enums.ItemContextCode ParentContext
        {
            get { return (_Enums.ItemContextCode)Enum.Parse(typeof(_Enums.ItemContextCode), this.VcParentContext, true); }
            set { this.VcParentContext = value.ToString(); }
        }
        /// <summary>
        /// refers to the date that the user is allowed to signup a request. Maxdate = not available
        /// </summary>
        [XmlAttribute("ParentInventoryId")]
        public int ParentId
        {
            get { return this.IParentInventoryId; }
            set { this.IParentInventoryId = value; }
        }
        /// <summary>
        /// refers to the matching saleitem
        /// </summary>
        [XmlAttribute("SaleItemId")]
        public Guid? SaleItemId
        {
            get { return this.GSaleItemId; }
            set { this.GSaleItemId = value; }
        }
        /// <summary>
        /// refers to the date that the user is allowed to signup a request. Maxdate = neverBeenFulfilled
        /// </summary>
        [XmlAttribute("DateSold")]
        public DateTime DateSold
        {
            get { return (!this.DtSold.HasValue) ? DateTime.MaxValue : this.DtSold.Value; }
            set { this.DtSold = value; }
        }
        /// <summary>
        /// refers to the date that the user is allowed to signup a request. Maxdate = neverBeenFulfilled
        /// </summary>
        [XmlAttribute("DateRedeemed")]
        public DateTime DateRedeemed
        {
            get { return (!this.DtRedeemed.HasValue) ? DateTime.MaxValue : this.DtSold.Value; }
            set { this.DtRedeemed = value; }
        }

        //dtstamp
        //code
        //description
        //InvoiceItemRecord
        //varchar(15) ipredeemed
        #endregion

        #region Methods

        /// <summary>
        /// This will set the inventory for the ticket in question as well as it's linked tickets
        /// </summary>
        /// <param name="showTicket"></param>
        /// <param name="currentAllotment"></param>
        /// <param name="adjustment"></param>
        /// <param name="userName"></param>
        /// <param name="context"></param>
        public static void AdjustInventoryHistory(ShowTicket showTicket, int currentAllotment, int adjustment, string userName, _Enums.HistoryInventoryContext context)
        {
            Inventory.AdjustInventoryHistory(
                showTicket.Id,
                currentAllotment,
                adjustment,
                System.Web.HttpContext.Current.Profile.UserName,
                _Enums.HistoryInventoryContext.Allotment
                );

            foreach (ShowTicketPackageLink link in showTicket.ShowTicketPackageLinkRecords())
            {
                Inventory.AdjustInventoryHistory(
                    link.LinkedShowTicketId,
                    currentAllotment,
                    adjustment,
                    System.Web.HttpContext.Current.Profile.UserName,
                    _Enums.HistoryInventoryContext.Allotment
                    );
            }
        }
        private static void AdjustInventoryHistory(int showTicketId, int currentAllotment, int adjustment, string userName, _Enums.HistoryInventoryContext context)
        {
            HistoryInventory hist = new HistoryInventory();
            hist.DtStamp = DateTime.Now;
            System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(userName);
            hist.UserId = (Guid)mem.ProviderUserKey;
            hist.TShowTicketId = showTicketId;
            hist.DateAdjusted = DateTime.Now;
            hist.CurrentlyAllotted = currentAllotment;
            hist.Adjustment = adjustment;
            hist.Context = context;

            hist.Save();
        }
        public static void AdjustInventoryHistory(Merch child, int currentAllotment, int adjustment, string userName, _Enums.HistoryInventoryContext context)
        {
            HistoryInventory hist = new HistoryInventory();
            System.Web.Security.MembershipUser mem = System.Web.Security.Membership.GetUser(userName);
            hist.UserId = (Guid)mem.ProviderUserKey;
            hist.DtStamp = DateTime.Now;
            hist.TMerchId = child.Id;
            hist.DateAdjusted = DateTime.Now;
            hist.CurrentlyAllotted = currentAllotment;
            hist.Context = context;

            if(context == _Enums.HistoryInventoryContext.Damage) 
                adjustment = (-1) * Math.Abs(adjustment);

            hist.Adjustment = adjustment;            
            hist.Save();
        }
        
        public static int GetNumberOfUsedCodes(_Enums.ItemContextCode context, int productId)
        {
            string sql = "SELECT COUNT(*) FROM [Inventory] WHERE [vcParentContext] = @context AND [iParentInventoryId] = @productId AND ([gSaleItemId] IS NOT NULL OR [tInvoiceItemId] IS NOT NULL); RETURN ";
            _DatabaseCommandHelper cmd = new _DatabaseCommandHelper(sql);
            cmd.AddCmdParameter("productId", productId, DbType.Int32);
            cmd.AddCmdParameter("context", context.ToString(), DbType.String);

            return cmd.PerformQuery("GetNumberOfUsedCodes");
        }
        public static int GetNumberOfAvailableCodes(_Enums.ItemContextCode context, int productId)
        {
            string sql = "SELECT COUNT(*) FROM [Inventory] WHERE [vcParentContext] = @context AND [iParentInventoryId] = @productId AND [gSaleItemId] IS NULL AND [tInvoiceItemId] IS NULL; RETURN ";
            _DatabaseCommandHelper cmd = new _DatabaseCommandHelper(sql);
            cmd.AddCmdParameter("productId", productId, DbType.Int32);
            cmd.AddCmdParameter("context", context.ToString(), DbType.String);

            return cmd.PerformQuery("GetNumberOfAvailableCodes");
        }
        public static int DeleteUnusedCodes(_Enums.ItemContextCode context, int productId)
        {
            string sql = "DELETE FROM [Inventory] WHERE [vcParentContext] = @context AND [iParentInventoryId] = @productId AND [gSaleItemId] IS NULL AND [tInvoiceItemId] IS NULL; SELECT @@ROWCOUNT; RETURN ";
            _DatabaseCommandHelper cmd = new _DatabaseCommandHelper(sql);
            cmd.AddCmdParameter("productId", productId, DbType.Int32);
            cmd.AddCmdParameter("context", context.ToString(), DbType.String);

            return cmd.PerformQuery("DeleteUnusedFromInventory");
        }

        public static string ReissueItemCode(int itemIdx)
        {
            if (itemIdx > 0)
            {
                InvoiceItem item = InvoiceItem.FetchByID(itemIdx);

                if (item != null && item.IsDeliverableByCode && (!item.IsGiftCertificateDelivery))
                {
                    string deliveryConst = (item.IsDownloadDelivery) ? InvoiceItem.DownloadCodeDeliveryConstant :
                        (item.IsActivationCodeDelivery) ? InvoiceItem.ActivationCodeDeliveryConstant : null;

                    if (item.IsActivationCodeDelivery)
                    {
                        string sql = "SELECT COUNT(*) FROM [Inventory] WHERE [iParentInventoryId] = @itemIdx AND [tInvoiceItemId] IS NULL ";
                        _DatabaseCommandHelper cmd = new _DatabaseCommandHelper(sql);
                        cmd.AddCmdParameter("itemIdx", item.TMerchId, DbType.Int32);
                        int availables = cmd.PerformQuery("ReissueItemCodeAvailabilityCheck");

                        if (availables <= 0)
                            return null;
                    }

                    object codeUsed = SPs.TxProcessInventoryCode(item.Id, _Enums.ItemContextCode.m.ToString(), item.TMerchId,
                        item.InvoiceRecord.InvoiceDate,
                        Utils.ParseHelper.InterleavedString(Guid.NewGuid().ToString(), item.Id.ToString()),
                        deliveryConst, InvoiceItem.ActivationCodeDeliveryConstant, true).ExecuteScalar();

                    if (codeUsed != null)
                        return codeUsed.ToString();
                }
            }

            return null;
        }

        //AssignCodeToSaleItem
        //GetAvailableCode_AssignToInvoiceItem()
        //this will happen at the end of the sale
        //when tickets are handled - this will  have to change
        //find an available item and assign invoiceitemid, record dtsold - return the code
        //- available = null tinvoiceitemid
        /// <summary>
        /// returns a unique code without any parameter leader. ie: code-only!
        /// </summary>
        /// <returns>string</returns>
        public static string CreateDeliveryCodeForInvoiceItem(InvoiceItem item, Merch merch, bool useOriginalDateOfSale)
        {
            string deliveryConst = (merch.IsDownloadDelivery) ? InvoiceItem.DownloadCodeDeliveryConstant :
                (merch.IsActivationCodeDelivery) ? InvoiceItem.ActivationCodeDeliveryConstant :
                (merch.IsGiftCertificateDelivery) ? InvoiceItem.GiftCertificateDeliveryConstant : null;

            if (deliveryConst != null)
            {
                string defaultcode = (merch.IsGiftCertificateDelivery) ? 
                    item.Guid.ToString() :
                    Utils.ParseHelper.InterleavedString(item.Guid.ToString(), item.Id.ToString());

                object codeUsed = SPs.TxProcessInventoryCode(item.Id, _Enums.ItemContextCode.m.ToString(), merch.Id,
                    (useOriginalDateOfSale) ? item.InvoiceRecord.InvoiceDate : DateTime.Now,
                    defaultcode,
                    deliveryConst, InvoiceItem.ActivationCodeDeliveryConstant, true).ExecuteScalar();

                if (codeUsed != null)
                {
                    if (deliveryConst == InvoiceItem.ActivationCodeDeliveryConstant && codeUsed.ToString() == defaultcode)
                    {
                        //send a message to an admin - inventory tracker - warn that this is out of inventory
                        //in theory it should rarely, if ever be triggered this far down in the order flow
                        //this is a just in case
                        string userName = string.Empty;

                        try
                        {
                            userName = System.Web.HttpContext.Current.User.Identity.Name;
                        }
                        catch (Exception) { }

                        EventQ.CreateInventoryNotification(userName, "soldout",
                            //tags not necessary for this notification
                            Utils.ParseHelper.StripHtmlTags(merch.DisplayNameWithAttribs),
                            _Enums.InvoiceItemContext.merch, merch.Id,
                            _Config._Inventory_LowMerch_Threshold);
                    }

                    return codeUsed.ToString();
                }
            }

            return null;
        }

        public static int BulkEnterCodes(_Enums.ItemContextCode context, int parentId, List<string> codes)
        {
            InventoryHeaderCollection coll = new InventoryHeaderCollection();
            int num = -1;

            foreach (string s in codes)                
                coll.Add(new InventoryHeader(context.ToString(), parentId, s));


            using (var conn = new SqlConnection(_Config.DSN))
            {
                conn.Open();
                using (var cmd = new SqlCommand("dbo.tx_BulkInsert_InventoryCodes", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var inventoryParam = cmd.Parameters.AddWithValue("@InventoryHeaders", coll);

                    inventoryParam.SqlDbType = SqlDbType.Structured;

                    try
                    {
                        num = (int)cmd.ExecuteNonQuery();
                    }
                    catch (SqlException sex)
                    {
                        _Error.LogException(sex);
                        throw sex;
                    }
                    catch (Exception ex)
                    {
                        _Error.LogException(ex);
                        throw ex;
                    }
                }
                conn.Close();
            }

            return num;
        }

        
        //RedeemCode()
        //record userid - redeemer, dtredeemed, ipaddress




        //BulkEnterCodes
        //load from a csv file
        //constructor should dtstamp, parentcontext, parentid, generatecode and given a code
        //record inventory history
        //load from a simple list/textarea control



        //CreateNewCode
        //create based on a seed



        #endregion
    }

    public class InventoryHeader
    {   
        public string vcParentContext { get; set; }
        public int iParentInventoryId { get; set; }
        public string Code { get; set; }

        //public InventoryHeader(_Enums.ItemContextCode parentContext, int parentId, string code)
        //{
        //    vcParentContext = parentContext.ToString();
        //    iParentInventoryId = parentId;
        //    Code = code;
        //}

        public InventoryHeader(string parentContextChar, int parentId, string code)
        {
            vcParentContext = parentContextChar;
            iParentInventoryId = parentId;
            Code = code;
        }
    }

    public class InventoryHeaderCollection : List<InventoryHeader>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
           // var fds = new SqlDataRecord(new SqlMetaData("vcParentContext", SqlDbType.VarChar));

            var sdr = new SqlDataRecord(
                
                new SqlMetaData("vcParentContext", SqlDbType.VarChar, 1), 
                new SqlMetaData("iParentInventoryId", SqlDbType.Int),
                new SqlMetaData("Code", SqlDbType.VarChar, 25)
                );

            foreach (InventoryHeader ih in this)
            {
                sdr.SetString(0, ih.vcParentContext);
                sdr.SetInt32(1, ih.iParentInventoryId);
                sdr.SetString(2, ih.Code);

                yield return sdr;
            }
        }
    }

}
