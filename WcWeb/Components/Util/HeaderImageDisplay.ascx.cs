using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Wcss;

namespace WillCallWeb.Components.Util
{
    public partial class HeaderImageDisplay : WillCallWeb.BaseControl
    {
        protected List<Triplet> _imgList = new List<Triplet>();
        protected string _promotext = string.Empty;
        protected string _startUrl = string.Empty;
        protected string _endUrl = string.Empty;
        protected string _textUrl = string.Empty;
        protected string _bannerFilePath = string.Empty;
        protected string _context = "";
        protected int _itemId = 0;

        protected _Enums.HeaderImageContext imageContext = _Enums.HeaderImageContext.All;
        protected void SetPageContext()
        {
            string pageName = this.Page.ToString().ToLower();
            if (pageName.StartsWith("asp."))
                pageName = pageName.Substring(4);
            if (pageName.EndsWith("_aspx"))
                pageName = pageName.Substring(0, (pageName.Length - 5));

            switch (pageName)
            {
                case "store_about":
                    imageContext = _Enums.HeaderImageContext.About;
                    break;

                case "store_cart_edit":
                    imageContext = _Enums.HeaderImageContext.Cart;
                    break;

                case "store_checkout":
                case "store_shipping":
                    imageContext = _Enums.HeaderImageContext.Checkout;
                    break;

                case "store_choosemerch":
                    imageContext = _Enums.HeaderImageContext.Merch;
                    break;

                case "store_chooseticket":
                    imageContext = _Enums.HeaderImageContext.Show;
                    break;

                case "store_confirmation":
                    imageContext = _Enums.HeaderImageContext.Confirm;
                    break;

                case "error":
                case "faq":
                case "contact":                
                case "contactsuccess":                
                case "charitableorgs":
                    imageContext = _Enums.HeaderImageContext.Aux;
                    break;                
                
                case "default":
                case "editprofile":
                case "accessdenied":
                case "accountupdate":
                case "webuser_default":
                case "unsubscribe":
                case "register":
                case "mailerconfirm":
                case "mailermanage":
                case "passwordrecovery":
                case "passwordrecoverysuccess": 
                imageContext = _Enums.HeaderImageContext.Account;
                    break;

                case "store_index":
                case "index2":
                case "store_maintenance":
                case "store_printconfirm":
                case "store_processingorder":
                case "store_processingshipping":
                case "contactprocessing":
                    break;
            }
        }
        protected HeaderImageCollection coll = new HeaderImageCollection();
        protected void SetCollectionByContext()
        {
            coll.Clear();

            if (imageContext != _Enums.HeaderImageContext.All)
            {
                List<HeaderImage> list = new List<HeaderImage>();

                switch (imageContext)
                {
                    case _Enums.HeaderImageContext.About:
                        list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match) {
                            return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) && 
                                (match.HasHeaderImageContext(_Enums.HeaderImageContext.About) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All)) 
                                ); }));
                        break;
                    case _Enums.HeaderImageContext.Account:
                        list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                        {
                            return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) &&
                                (match.HasHeaderImageContext(_Enums.HeaderImageContext.Account) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All))
                                );
                        }));
                        break;
                    case _Enums.HeaderImageContext.Aux:
                        list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                        {
                            return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) &&
                                (match.HasHeaderImageContext(_Enums.HeaderImageContext.Aux) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All))
                                );
                        }));
                        break;
                    case _Enums.HeaderImageContext.Cart:
                        list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                        {
                            return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) &&
                                (match.HasHeaderImageContext(_Enums.HeaderImageContext.Cart) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All))
                                );
                        }));
                        break;
                    case _Enums.HeaderImageContext.Checkout:
                        list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                        {
                            return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) &&
                                (match.HasHeaderImageContext(_Enums.HeaderImageContext.Checkout) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All))
                                );
                        }));
                        break;
                    case _Enums.HeaderImageContext.Confirm:
                        list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                        {
                            return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) &&
                                (match.HasHeaderImageContext(_Enums.HeaderImageContext.Confirm) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All))
                                );
                        }));
                        break;
                    case _Enums.HeaderImageContext.Merch:
                        if (Globals.MerchId > 0)
                            list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                        {
                            return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) && match.TMerchId == Globals.MerchId);
                        }));

                        if (list.Count == 0)
                        {
                            list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                            {
                                return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) &&
                                    (match.HasHeaderImageContext(_Enums.HeaderImageContext.Merch) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All))
                                    );
                            }));
                        }
                        break;
                    case _Enums.HeaderImageContext.Show:
                        if (Globals.ShowId > 0)
                            list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                            {
                                return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) && match.TShowId == Globals.ShowId);
                            }));

                        if (list.Count == 0)
                        {
                            list.AddRange(_Lookits.HeaderImages.GetList().FindAll(delegate(HeaderImage match)
                            {
                                return (match.IsCurrentlyRunning(Ctx.MarketingProgramKey) &&
                                    (match.HasHeaderImageContext(_Enums.HeaderImageContext.Show) || match.HasHeaderImageContext(_Enums.HeaderImageContext.All))
                                    );
                            }));
                        }
                        break;
                }

                if (list.Count > 0)
                {
                    //Set exclusivity
                    if (list.FindIndex(delegate(HeaderImage match) { return (match.IsExclusive); }) != -1)
                        list.RemoveAll(delegate(HeaderImage match) { return (!match.IsExclusive); });

                    //ordering - priority then idisplayorder
                    if (list.Count > 0)
                    {
                        if (_Config._HeaderImages_IgnoreOrder)
                        {
                            var randomized = list.OrderBy(a => Guid.NewGuid());

                            coll.AddRange(randomized);
                        }
                        else
                        {
                            var sortedColl =
                                from item in list
                                select item;

                            coll.AddRange(sortedColl.OrderBy(x => x.IsDisplayPriority)
                                .ThenBy(x => x.DisplayOrder));
                        }
                    }
                }
            }
        }
        protected System.Text.StringBuilder sb = new System.Text.StringBuilder();
        protected void RenderCollection()
        {
            int count = coll.Count;

            if (count > 0)
            {
                sb.Length = 0;
                sb.AppendLine("<div id=\"cycle-container\">");
                sb.AppendLine("<div id=\"cycle-wrapper\">");

                foreach (HeaderImage ent in coll)
                    RenderIndividualHeaderImage(ent, count);

                sb.AppendLine("</div>");
                sb.AppendLine("</div>");

                litControl.Text = sb.ToString();
            }
            else
                this.Controls.Clear();
        }
        protected void RenderIndividualHeaderImage(HeaderImage ent, int count)
        {
            //override the default css of display:none
            sb.AppendFormat("<div{0}>", (count == 1) ? " style=\"display:inline;\"" : string.Empty);

            string navigateUrl = ent.NavigateUrl;
            bool hasNavigateUrl = (navigateUrl != null && navigateUrl.Trim().Length > 0);
            if (hasNavigateUrl)
                sb.AppendLine(string.Format("<a href=\"{0}\">", navigateUrl));

            sb.AppendLine(string.Format("<img src=\"{0}\" alt=\"{1}\" rel=\"{2}\"/>",
                ent.VirtualFilePath, ent.DisplayText, ent.TimeoutMsec.ToString()));

            if (hasNavigateUrl)
                sb.AppendLine("</a>");

            //do not leave a space between end of img and the next div
            //doing so will create a spacing/padding issue
            sb.AppendLine("</div>");
        }

        protected override void CreateChildControls()
        {

            if (_Config._Site_Entity_Name.ToLower() != "sts9")
            {
                //Set Context of this page
                SetPageContext();

                SetCollectionByContext();

            }
             
            RenderCollection();

        }
}
}