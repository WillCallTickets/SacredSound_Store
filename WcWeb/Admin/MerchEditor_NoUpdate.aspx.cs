using System;

namespace WillCallWeb.Admin
{
    public partial class MerchEditor_NoUpdate : WillCallWeb.BasePage
    {
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

            this.Menu_MerchSelection1.EditorPage = "itemedit";

            switch (controlToLoad.ToLower())
            {
                    /*
                case "legacypicker":
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
                     * */
                case "bundle":
                    controlToLoad = "Merch_Bundle";
                    selectorTitle = "Bundle";
                    break;
            }

            this.Menu_MerchSelection1.Title = selectorTitle;
            Content.Controls.Add(LoadControl(string.Format(@"AdminControls\{0}.ascx", controlToLoad)));
        }
    }
}