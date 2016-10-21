using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Wcss;

namespace Wcss.QueryRow
{
    [Serializable]
    public partial class ShopifyExportRow
    {
        /* These first two vars are not shown in CSV */
        public int Sorter                           { get; set; } //0-parent, 1 - image, 2 - inventory
        public int SorterId                         { get; set; } //parentId or inventoryId

        public string Handle                        { get; set; }
	    public string Title	                        { get; set; }
	    public string Body	                        { get; set; }
	    public string Vendor                        { get; set; }
	    public string Type	                        { get; set; }
	    public string Tags                          { get; set; }
	    public string Published                     { get; set; }
        public string Option1_Name                  { get; set; }
	    public string Option1_Value                 { get; set; }
	    public string Option2_Name                  { get; set; }
	    public string Option2_Value                 { get; set; }
	    public string Option3_Name                  { get; set; }
	    public string Option3_Value                 { get; set; }
	    public string Variant_SKU                   { get; set; }
	    public string Variant_Grams                 { get; set; }
	    public string Variant_Inventory_Tracker     { get; set; }
	    public string Variant_Inventory_Quantity    { get; set; }
	    public string Variant_Inventory_Policy      { get; set; }
	    public string Variant_Fulfillment_Service   { get; set; }
	    public string Variant_Price                 { get; set; }
	    public string Variant_Compare_At_Price      { get; set; }
	    public string Variant_Requires_Shipping     { get; set; }
	    public string Variant_Taxable               { get; set; }
	    public string Variant_Barcode               { get; set; }
	    public string Image_Src                     { get; set; }
	    public string Image_Alt_Text                { get; set; }

        private void SetHandle(Merch m)
        {
            Merch parent = (m.IsParent) ? m : m.ParentMerchRecord;
            
            Handle = LaunderHandle(parent.Name).ToLower();
        }

        private string LaunderHandle (string s)
        {
            string handle = s.Replace("&amp;", "and").Replace("+", "-and-").Replace("&", "-and-").Replace("#", "").Replace("'", "").Replace("\"", "");
            //convert whitespace to dash
            handle = Regex.Replace(handle, @"\s+", "-");
            //convert any non alpha to dash
            handle = Regex.Replace(handle, @"[^a-zA-Z0-9]", "-");
            //Regex regex = new Regex(@"[^a-zA-Z0-9]");

            //remove any mult dashes
            handle = Regex.Replace(handle, @"\-+", "-");
            //remove leading and trailing dashes
            handle = handle.Trim(new char[] { '-' });

            return handle;
        }

        private void SetType(Merch m)
        {
            Merch parent = (m.IsParent) ? m : m.ParentMerchRecord;
            MerchJoinCatCollection coll = new MerchJoinCatCollection();
            coll.AddRange(parent.MerchJoinCats);
            if(coll.Count > 1)
                coll.Sort("IDisplayOrder", true);
            Type = coll[0].MerchCategorieName;
        }

        private void SetTags(Merch m)
        {
            Merch parent = (m.IsParent) ? m : null;
            List<string> list = new List<string>();

            if (parent != null)
            {
                MerchJoinCatCollection coll = new MerchJoinCatCollection();
                coll.AddRange(parent.MerchJoinCats);
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);


                foreach (MerchJoinCat mjc in coll)
                {
                    if (!list.Contains(mjc.MerchCategorieRecord.MerchDivisionRecord.Name))
                        list.Add(mjc.MerchCategorieRecord.MerchDivisionRecord.Name);
                    if (!list.Contains(mjc.MerchCategorieName))
                        list.Add(mjc.MerchCategorieName);
                }

                //if (m.IsChild && m.Style != null && m.Style.Trim().Length > 0 && (!list.Contains(m.Style.Trim())))
                //    list.Add(m.Style.Trim());

                ////not necessary to include color and size
            }

            Tags = string.Join(",", list.ToArray());
        }

        private void SetOptions(Merch m)
        {
            Merch child = (m.IsChild) ? m : null;
            List<string> names = new List<string>();
            List<string> values = new List<string>();

            if (child != null)
            {
                if (child.Style != null && child.Style.Trim().Length > 0)
                {
                    names.Add("Style");
                    values.Add(child.Style.Trim());
                }
                if (child.Color != null && child.Color.Trim().Length > 0)
                {
                    names.Add("Color");
                    values.Add(child.Color.Trim());
                }
                if (child.Size != null && child.Size.Trim().Length > 0)
                {
                    names.Add("Size");
                    values.Add(child.Size.Trim());
                }
            }

            Option1_Name = (names.Count > 0) ? names[0] : string.Empty;
            Option2_Name = (names.Count > 1) ? names[1] : string.Empty;
            Option3_Name = (names.Count > 2) ? names[2] : string.Empty;
            Option1_Value = (values.Count > 0) ? values[0] : string.Empty;
            Option2_Value = (values.Count > 1) ? values[1] : string.Empty;
            Option3_Value = (values.Count > 2) ? values[2] : string.Empty;
        }

        private void SetImage(Merch m, bool includeImages)
        {
            Merch parent = (m.IsParent) ? m : null;
            string image = string.Empty;
            string imageAlt = string.Empty;

            if (includeImages && parent != null)
            {
                ItemImageCollection coll = new ItemImageCollection();
                coll.AddRange(parent.ItemImageRecords());//.GetList().FindAll(delegate(ItemImage match) { return match.IsItemImage; }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                if (coll.Count > 0)
                {
                    image = SaveImage(coll[0]);                    
                }
            }

            Image_Src = image;
            Image_Alt_Text = imageAlt;
        }

        public ShopifyExportRow(Merch m, bool includeImages)
        {
            bool isParent = m.IsParent;
            Merch inventory = m;
            if (isParent)
            {
                MerchCollection cl = new MerchCollection();
                cl.AddRange(m.AvailableInventory);
                if (cl.Count > 1)
                    cl.Sort("Id", true);

                try
                {
                    inventory = cl[0];
                }
                catch (Exception ex)
                {
                    _Error.LogException(ex);
                }
            }

            Sorter = (isParent) ? 0 : 2;
            SorterId = inventory.Id;
            SetHandle(m);
            Title = (isParent) ? m.Name.Replace(@"""", string.Empty) : string.Empty;

            //need to update relative image links
            Body = (isParent && m.Description_Derived != null) ? m.Description_Derived
                .Replace("\"", "'")
                .Replace("src='/WillCallResources/", "src='http://sts9store.com/WillCallResources/") : string.Empty;
            Vendor = string.Empty;//(isParent) ? "Sts9Store" : string.Empty
            SetType(m);
            SetTags(m);
            Published = "TRUE";
            
            SetOptions(inventory);
            Variant_SKU = inventory.Id.ToString();

            Variant_Grams = (inventory.Weight * 28).ToString();
            Variant_Inventory_Tracker = "shopify";
            Variant_Inventory_Quantity = inventory.Available.ToString();
            Variant_Inventory_Policy = "deny";
            Variant_Fulfillment_Service = "manual";
            Variant_Price = inventory.Price.ToString();
            Variant_Compare_At_Price = string.Empty;

            string delivery = inventory.DeliveryType.ToString();
            Variant_Requires_Shipping = (delivery.ToLower() == _Enums.DeliveryType.parcel.ToString().ToLower()) ? "TRUE" : "FALSE";
            Variant_Taxable = "FALSE";
            Variant_Barcode = string.Empty;
            
            SetImage(m, includeImages);
        }

        public ShopifyExportRow(ItemImage ii)
        {
            Merch parent = ii.MerchRecord;
            MerchCollection cl = new MerchCollection();
            cl.AddRange(parent.AvailableInventory);
            if (cl.Count > 1)
                cl.Sort("Id", true);

            Merch inventory = cl[0];

            Sorter = 1;
            SorterId = inventory.Id;
            SetHandle(parent);
            Title = string.Empty;
            Body = string.Empty;
            Vendor = string.Empty;
            Type = string.Empty;
            Tags = string.Empty;
            Published = string.Empty;//"TRUE";**
            SetOptions(parent);//parent doesn't write out options - method assigns blanks for images

            //TODO: do we need sku for image?
            Variant_SKU = string.Empty;// inventory.Id.ToString();
            Variant_Grams = string.Empty;
            Variant_Inventory_Tracker = string.Empty;
            Variant_Inventory_Quantity = string.Empty;// "0";**
            //Variant_Inventory_Policy = string.Empty;
            Variant_Inventory_Policy = string.Empty;//"deny";**
            Variant_Fulfillment_Service = string.Empty;//"manual";**
            Variant_Price = string.Empty;
            Variant_Compare_At_Price = string.Empty;

            Variant_Requires_Shipping = string.Empty;
            Variant_Taxable = string.Empty;
            Variant_Barcode = string.Empty;

            Image_Src = SaveImage(ii);
            Image_Alt_Text = string.Empty;
        }

        private string SaveImage(ItemImage ii)
        {
            string imageUrl = ii.ImageManager._OriginalUrl;
            //if (imageUrl.IndexOf(@"\WillCall") != -1)
            //{
            //    string extension = Path.GetExtension(imageUrl);
            //    string filename = Path.GetFileNameWithoutExtension(imageUrl);

            //    try { 
            //        ii.ImageName = string.Format("{0}{1}", filename, extension);
            //        ii._imageManager = null;

            //        ii.ImageManager.CreateAllThumbs();
            //        ii.Save();
            //    }
            //    catch (Exception ex)
            //    {
            //        _Error.LogException(ex);
            //    }

            //}
            //else 
            if (imageUrl.IndexOf(" ") != -1)
            {   
                string extension = Path.GetExtension(imageUrl); 
                string filename = Path.GetFileNameWithoutExtension(imageUrl);             
                string newFile = string.Format("{0}wct", Utils.ParseHelper.RemoveSpaces(filename));
                string basePath = Path.GetDirectoryName(imageUrl);

                try
                {
                    //if the new file exists - we should point the imagemaneger to it
                    string newPathAndFile = string.Format("{0}\\{1}{2}", basePath, newFile, extension);
                    string newMapped = System.Web.HttpContext.Current.Server.MapPath(newPathAndFile);

                    if(File.Exists(newMapped))
                    {
                        ii.ImageManager.DeleteThumbnails();
                        ii.ImageName = string.Format("{0}{1}", newFile, extension);
                        //ii.ImageManager._OriginalUrl = ii.ImageName;
                        ii._imageManager = null;

                        ii.ImageManager.CreateAllThumbs();
                        ii.Save();
                    }
                    else
                    {
                        ii.ChangeImageName(string.Format("{0}{1}", newFile, extension));                        
                    }
                }
                catch (Exception ex)
                {
                    _Error.LogToFile(string.Format("{0} - {1}", ex.Message, ii.ImageName), string.Format("{0}_FAILURES", DateTime.Now.ToString("yyyyMMdd")));
                }
            }

            return string.Format("http://{0}{1}", _Config._DomainName, ii.ImageManager._OriginalUrl);
        }
        

        public override string ToString()
        {
            return this.Handle;
        }
    }
    

    public partial class ShopifyExport
    {
        public bool _includeImages = false;
        public List<ShopifyExportRow> exportList;

        public ShopifyExport(bool includeImages) {

            _includeImages = includeImages;
            InitMerchExportList();
        }

        private void InitMerchExportList()
        {            
            //init
            List<ShopifyExportRow> list = new List<ShopifyExportRow>();

            MerchCollection coll = new MerchCollection();
            coll.Where(Merch.Columns.ApplicationId, _Config.APPLICATION_ID);
            coll.Where("tParentListing", null);
            coll.Where("bActive", true);
            coll.Where("vcDeliveryType", SubSonic.Comparison.Equals, _Enums.DeliveryType.parcel);
            coll.Where("bInternalOnly", SubSonic.Comparison.Equals, 0);
            //coll.Where("vcDeliveryType", SubSonic.Comparison.NotEquals, _Enums.DeliveryType.giftcertificate);
            //coll.Where("vcDeliveryType", SubSonic.Comparison.NotEquals, _Enums.DeliveryType.download);
            //coll.Where("vcDeliveryType", SubSonic.Comparison.NotEquals, _Enums.DeliveryType.activationcode);
            coll.Load();
            
            //keep for testing
            //MerchCollection avail = new MerchCollection();
            //avail.AddRange(coll.GetList().FindAll(delegate(Merch match)
            //{
            //    return match.Available > 0;
            //}));
            //MerchCollection diff = new MerchCollection();
            //diff.AddRange(coll.Except(avail));

            //filter list - keep only those with inventory
            coll.GetList().RemoveAll(delegate (Merch match) {
                
                return match.AvailableInventory.Count == 0;
                });

            foreach (Merch parent in coll)
            {
                list.Add(new ShopifyExportRow(parent, _includeImages));

                if (_includeImages)
                {
                    //add images - dont add first - already added in parent row
                    ItemImageCollection imgs = new ItemImageCollection();
                    imgs.AddRange(parent.ItemImageRecords());//.GetList().FindAll(delegate(ItemImage match) { return match.IsItemImage; }));
                    if (imgs.Count > 1)
                    {
                        imgs.Sort("IDisplayOrder", true);

                        int first = 0;
                        foreach (ItemImage ii in imgs)
                        {
                            if (first++ > 0)
                                list.Add(new ShopifyExportRow(ii));
                        }
                    }
                }

                //add inventory
                int skipFirst = 0;
                MerchCollection cl = new MerchCollection();
                cl.AddRange(parent.AvailableInventory);
                if (cl.Count > 1)
                    cl.Sort("Id", true);
                foreach (Merch inventory in cl)
                    if(skipFirst++ > 0)
                        list.Add(new ShopifyExportRow(inventory, _includeImages));
            }

            //now we have the data for our exportable list - and it is sorted
            exportList = new List<ShopifyExportRow>();
            
            exportList.AddRange(list);//.Take(46));//20 ok45ok -46 bad
        }

        public void GetCSVReport_ShopifyMerchExport(string fileAttachmentName, string pageToAccommodateDownload)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //write header
            sb.AppendFormat("Handle,Title,Body (HTML),Vendor,Type,Tags,Published,");
            sb.AppendFormat("Option1 Name,Option1 Value,Option2 Name,Option2 Value,Option3 Name,Option3 Value,");
            sb.AppendFormat("Variant SKU,Variant Grams,Variant Inventory Tracker,Variant Inventory Quantity,Variant Inventory Policy,Variant Fulfillment Service,");
            sb.AppendFormat("Variant Price,Variant Compare At Price,Variant Requires Shipping,Variant Taxable,Variant Barcode,");
            sb.AppendFormat("Image Src,Image Alt Text{0}", Environment.NewLine);

            foreach (ShopifyExportRow rw in exportList)
                ProcessRowPerFormat(sb, rw);

            Utils.FileLoader.CSV_WriteToContextForDownload(sb, fileAttachmentName, pageToAccommodateDownload);
        }

        private static void ProcessRowPerFormat(System.Text.StringBuilder sb, ShopifyExportRow row)
        {
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",",            
            row.Handle,
            row.Title,
            row.Body,
            row.Vendor,
            row.Type);
                //4^
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",",
            row.Tags,
            row.Published,
            row.Option1_Name,
            row.Option1_Value,
            row.Option2_Name);
                //9^
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",",
            row.Option2_Value,
            row.Option3_Name,
            row.Option3_Value,
            row.Variant_SKU,
            row.Variant_Grams);
                //14^
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",",
            row.Variant_Inventory_Tracker,
            row.Variant_Inventory_Quantity,
            row.Variant_Inventory_Policy,
            row.Variant_Fulfillment_Service,
            row.Variant_Price);
            //19^
            sb.AppendFormat("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",",
            row.Variant_Compare_At_Price,
            row.Variant_Requires_Shipping,
            row.Variant_Taxable,
            row.Variant_Barcode,
            row.Image_Src

            //next line does not work
            //string.Format("'{0}'", row.Image_Src)


            //System.Web.HttpUtility.UrlEncode(row.Image_Src)
            
            );
            //24^
            sb.AppendFormat("\"{0}\",",
            row.Image_Alt_Text);

            sb.AppendFormat("{0}",
                Environment.NewLine);
        }
    }
}
