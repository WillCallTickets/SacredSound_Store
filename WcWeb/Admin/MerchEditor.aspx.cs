using System;
using System.Web.Services;
using System.Collections.Generic;
using System.Linq;

namespace WillCallWeb.Admin
{
    public partial class MerchEditor : WillCallWeb.BasePage
    {
        [WebMethod]
        public static object UpdateOrder_fet(string str)
        {
            Wcss.SiteConfig config = Wcss._ContextBase.GetFeatureOrderConfig();
            if (config.ValueX != str)
            {
                config.ValueX = str;
                config.Save();
                Wcss._Lookits.RefreshLookup(Wcss._Enums.LookupTableNames.SiteConfigs.ToString());
            }

            return "true";
        }

        //[WebMethod]
        //public static object Update_MerchOver18(string str)
        //{
        //    List<int> ints = new List<int>(str.Split(',').Select(int.Parse));

        //    if (! ints.SequenceEqual(Wcss._Config._Merch_Requires_18Over_Ack_List))
        //    {
        //        Wcss._Config._Merch_Requires_18Over_Ack_List = ints;
        //        Wcss._Lookits.RefreshLookup(Wcss._Enums.LookupTableNames.SiteConfigs.ToString());
        //    }

        //    return "true";
        //}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //set opacity for nav events
            if (this.HasControls() && this.UpdatePanel1.Visible)
                Atx.RegisterJQueryScript_BlockUI_AjaxMethod(this.UpdatePanel1, "#mercheditor", true);
        }
        protected override void OnPreInit(EventArgs e)
        {
            QualifySsl(true);
            base.OnPreInit(e);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string req = Request.QueryString["merchitem"];
            if (req != null && Utils.Validation.IsInteger(req))
            {
                int idx = int.Parse(req);

                if (idx > 0)
                {
                    Atx.SetCurrentMerchRecord(idx);

                    if (Atx.CurrentMerchRecord.IsChild)
                        Atx.SetCurrentMerchRecord(Atx.CurrentMerchRecord.TParentListing.Value);
                }
                else if (idx == 0 && Atx.CurrentMerchRecord != null)
                    Atx.SetCurrentMerchRecord(0);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {   
            SetPageControl();
        }

        private void SetPageControl()
        {
            //SET UP PAGE BASED UPON QS
            string controlToLoad = "MerchPicker";
            string selectorTitle = "Merch";
            string req = Request.QueryString["p"];

            if (req != null && req.Trim().Length > 0)
                controlToLoad = req;

            //toggles the upper context merch menu
            this.Menu_MerchSelection1.EditorPage = "itemedit";

            switch (controlToLoad.ToLower())
            {
                case "legacypicker":
                    this.Menu_MerchSelection1.Visible = false;
                    controlToLoad = "Merch_Picker";
                    selectorTitle = "Merch";
                    break;
                case "merchpicker":
                case "itemedit":
                    controlToLoad = "Merch_Item";
                    selectorTitle = "Merch";
                    break;
                case "postp":
                    controlToLoad = "Merch_PostPurchase";
                    selectorTitle = "Post Purchase Text";
                    break;
                case "itemcopy":
                    controlToLoad = "Merch_Copier";
                    selectorTitle = "Copy";
                    break;
                case "images":
                    controlToLoad = "Merch_ImageUpload";
                    selectorTitle = "Images";
                    break;
                case "bundle":
                    controlToLoad = "Merch_Bundle";
                    selectorTitle = "Bundle";
                    break;
                case "fetord":
                    controlToLoad = "Merch_OrderFeatured";
                    selectorTitle = "Order Featured Items";
                    this.Menu_MerchSelection1.Visible = false;
                    break;
                case "merchover18":
                    controlToLoad = "Merch_Over18";
                    selectorTitle = "Over 18 Items";
                    this.Menu_MerchSelection1.Visible = false;
                    break;
                //case "altedit":
                //    controlToLoad = "Merchandise_Item";
                //    selectorTitle = "Merchandise Editor";
                //    this.Menu_MerchSelection1.EditorPage = "altedit";
                //    break;
                //case "inventory":
                //    controlToLoad = "Merch_Inventory";
                //    selectorTitle = "Inventory";
                //    break;
            }

            this.Menu_MerchSelection1.Title = selectorTitle;
            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}