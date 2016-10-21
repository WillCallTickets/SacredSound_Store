using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class MerchItemTemplate : WillCallWeb.BaseControl
    {
        public string SectionTitle { get; set; }

        private MerchCollection _merchCollection = new MerchCollection();
        public MerchCollection merchCollection
        {
            get
            {
                return _merchCollection;
            }
            set
            {
                _merchCollection = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {   
            if (!IsPostBack)
                rptItems.DataBind();
        }
        protected void rptItems_DataBinding(object sender, EventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            if (merchCollection.Count > 0)
            {
                litNoItems.Visible = false;
                rpt.Visible = true;
                rpt.DataSource = merchCollection;
            }
            else
            { 
                rpt.Visible = false;
                litNoItems.Visible = true;
                litNoItems.Text = string.Format("<div class=\"noneavailable\">Sorry, there are no {0} available at this time. Check back soon.</div>",
                    SectionTitle);
            }
        }

        protected void rptItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ListItemType lit = (ListItemType)e.Item.ItemType;

            if (lit != ListItemType.Footer && lit != ListItemType.Header && lit != ListItemType.Pager && lit != ListItemType.Separator)
            {
                // i++;
                Literal picture = (Literal)e.Item.FindControl("LiteralPicture");
                Merch entity = (Merch)e.Item.DataItem;

                if (picture != null && entity != null)
                {
                    ItemImageCollection coll = new ItemImageCollection();
                    coll.AddRange(_Lookits.MerchImages.GetList().FindAll(
                        delegate(ItemImage match) { return (match.TMerchId == entity.Id && match.IsItemImage); }));
                    if (coll.Count > 1)
                        coll.Sort("IDisplayOrder", true);

                    foreach (ItemImage image in coll)
                    {
                        picture.Text += string.Format("<a href=\"/Store/ChooseMerch.aspx?mite={0}\" title=\"Buy\">", entity.Id);
                        picture.Text += string.Format("<img src=\"{0}\" border=\"0\" class=\"{1}\" {2} /></a>",
                            (_Config._AlwaysUseLargeThumbsForDetail) ? System.Web.HttpUtility.HtmlEncode(image.Thumbnail_Large) : System.Web.HttpUtility.HtmlEncode(image.Thumbnail_Small),
                            image.ThumbClass,
                            (image.IsPortrait) ? string.Format("height=\"{0}\"", _Config._MerchThumbSizeSm) : string.Format("width=\"{0}\"", _Config._MerchThumbSizeSm));
                    }
                }

                Literal hires = (Literal)e.Item.FindControl("LiteralHiRes");
                if (hires != null && entity != null)
                {
                    ItemImageCollection coll = new ItemImageCollection();
                    coll.AddRange(_Lookits.MerchImages.GetList().FindAll(
                        delegate(ItemImage match) { return (match.TMerchId == entity.Id && match.IsDetailImage); }));

                    if (coll.Count > 0)
                    {
                        string conText = "Hi-Res";
                        string status = string.Format("onMouseOver=\" window.status=&#39;{0}&#39;; return true\" onMouseOut=\"window.status=&#39; &#39;; return true\"", conText);
                        hires.Text = string.Format("<a href=\"javascript:imagePopup(&#39;{0}&#39;,&#39;{1}&#39;);\" {2} class=\"btnhi_res\" title=\"{3}\" alt=\"{3}\" border=\"0\">{3}</a>",
                            entity.Id, _Config._MerchThumbSizeMax, status, conText);
                    }
                }

                if (entity != null && entity.UseSalePrice)
                {
                    Literal saleItem = (Literal)e.Item.FindControl("litSaleItem");
                    if (saleItem != null)
                        saleItem.Text = string.Format("<span class=\"onsalenow\">On Sale Now! Save {0}</span>", entity.SalePriceSavings);
                }
            }
        }
}
}