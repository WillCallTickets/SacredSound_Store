using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class Banner : WillCallWeb.BaseControl
    {
        protected string _promotext = string.Empty;
        protected string _startUrl = string.Empty;
        protected string _endUrl = string.Empty;
        protected string _textUrl = string.Empty;
        protected string _bannerFilePath = string.Empty;
        protected string _context = "";
        protected int _itemId = 0;

        protected override void CreateChildControls()
        {
            //determine context
            if (this.Page.ToString().ToLower().IndexOf("choosemerch_aspx") != -1)
                _context = "merch";
            else if (this.Page.ToString().ToLower().IndexOf("chooseticket_aspx") != -1)
                _context = "ticket";
            else if (this.Page.ToString().ToLower().IndexOf("cart_edit_aspx") != -1)
                _context = "cart";
            else if (this.Page.ToString().ToLower().IndexOf("checkout_aspx") != -1)
                _context = "checkout";
            else if (this.Page.ToString().ToLower().IndexOf("shipping_aspx") != -1)
                _context = "shipping";

            SalePromotionCollection coll = new SalePromotionCollection();
            coll.AddRange(Ctx.Cart.SalePromotions_RunningAndAvailable.GetList().FindAll(delegate(SalePromotion match)
            {
                switch (_context)
                {
                    case "merch":
                        //if it is a merch page and the promotion has matching ids

                        //we also need to match the case of a child item matching
                        if (match.DisplayPromotionAtParentItem)
                        {
                            if(match.RequiredMerchListing.Contains(Globals.MerchId))
                                return true;

                            //if this merchitem contains that child id
                            Merch parent = (Merch)Ctx.SaleMerch.Find(Globals.MerchId);
                            if(parent != null && parent.Id > 0)
                            {
                                foreach (Merch child in parent.ChildMerchRecords_Active )
                                    if (match.RequiredMerchListing.Contains(child.Id))
                                        return true;
                            }
                        }
                        else                        
                            return match.DisplayBannerOnMerch;

                        return false;

                    case "ticket":
                        if (Globals.ShowId > 0)
                        {
                            if (match.DisplayPromotionAtParentItem)
                            {
                                int showIdx = Globals.ShowId;
                                int exists = -1;

                                //we have 2 match cases
                                //if one of the show dates listed is within the show
                                //match on a matching showdateid that matches the current global show id
                                if (match.TRequiredParentShowDateId.HasValue && match.TRequiredParentShowDateId > 0)
                                {
                                    exists = Ctx.OrderedDisplayable_ShowDates.GetList().FindIndex(delegate(ShowDate matcr)
                                    {
                                        return matcr.GetDisplayableTickets(_Enums.VendorTypes.online, Ctx.MarketingProgramKey, true, false, null).Count > 0 &&
                                            match.TRequiredParentShowDateId == matcr.Id && matcr.TShowId == showIdx;
                                    });
                                }

                                //if one of the tickets is within the listed show
                                else if (match.TRequiredParentShowTicketId.HasValue && match.TRequiredParentShowTicketId > 0)
                                    exists = Ctx.SaleTickets.GetList().FindIndex(delegate(ShowTicket matcr)
                                    { return matcr.IsUnlocked(Ctx.MarketingProgramKey, DateTime.Now) && match.TRequiredParentShowTicketId == matcr.Id && matcr.TShowId == showIdx; });

                                if (exists != -1)
                                    return true;
                            }  
                        }
                        else
                            return match.DisplayBannerOnTicketing;

                        return false;
                    case "cart":
                        return match.DisplayBannerOnCart;
                    case "checkout":
                        return match.DisplayBannerOnCheckout;
                    case "shipping":
                        return match.DisplayBannerOnShipping;
                    default:
                        break;
                }

                return false;
            }));
                

            int count = coll.Count;
            if (count > 0)
            {
                //decide which banner to display - get an index within range
                int randomIndex = Utils.Helper.GetRandomInRange(0, count);

                SalePromotion sp = coll[randomIndex];

                if (sp != null)
                {
                    string url = sp.BannerClickUrl;
                    if (url != null && url.Trim().Length > 0)
                        url = Utils.ParseHelper.FormatUrlFromString(url, false).Trim();
                    else
                        url = null;

                    if (sp.HasValidBannerEntry)
                    {
                        _bannerFilePath = sp.Banner_VirtualFilePath;

                        if (url != null)
                        {
                            _startUrl = string.Format("<a title=\"Click for details\" href=\"{0}\">", url);
                            _endUrl = string.Format("</a>");
                        }
                    }
                    else
                    {
                        _promotext = sp.DisplayText;

                        if (url != null)
                            _textUrl = string.Format("<div class=\"textbannerurl\"><a target=\"_blank\" href=\"{0}\">Click here for details...</a></div>", 
                                url);
                    }
                }
            }

            if (_bannerFilePath.Length > 0 || _promotext.Length > 0)
            {
                base.CreateChildControls();
            }
            else
                this.Controls.Clear();
        }
}
}