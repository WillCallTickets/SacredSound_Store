using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using WillCallWeb;
using Wcss;

public partial class Store_ChooseMerch : WillCallWeb.BasePage, IPostBackEventHandler
{
    void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
    {
        string[] args = eventArgument.Split('~');
        string command = args[0];
        int idx = (args.Length > 1 && Utils.Validation.IsInteger((string)args[1])) ? int.Parse(args[1]) : 0;
        string result = string.Empty;

        switch (command.ToLower())
        {
            case "age18cancel":
                string g = "l";
                break;
        }
    }

    protected override void OnPreInit(EventArgs e)
    {
        QualifySsl(false);
        base.OnPreInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //avoid double clicks! always reset
        Ctx.OrderProcessingVariables = null;

        bool userIsAdmin = HttpContext.Current.User.IsInRole("Administrator");

        //always allow gift certificates
        if((!(Globals.MerchItem != null && Globals.MerchItem.IsGiftCertificateDelivery)) && (!_Config._Sales_Merch_Active) && (!userIsAdmin))
        {
            Literal lit = new Literal();
            lit.Text = string.Format("<div class=\"comingsoon\">{0}</div>", _Config._Message_MerchComingSoon);

            Content.Controls.Add(lit);
        }
        else if (Globals.MerCat != null)
        {
            //redirect if internal only
            if (Globals.MerCat.IsInternalOnly || Globals.MerCat.MerchDivisionRecord.IsInternalOnly)
            {
                _Error.LogException(new Exception(string.Format("{0}: NewSession?: {1} - invalid merch category - Redirected. ", DateTime.Now.ToString(),
                    (Ctx != null && Ctx.Session != null) ? Ctx.Session.IsNewSession.ToString() : "no session")));
                base.Redirect("Store/ChooseMerch.aspx");
            }

            MerchJoinCatCollection coll = new MerchJoinCatCollection();
            coll.AddRange(Globals.MerCat.MerchJoinCatRecords().GetList().FindAll(
                delegate(MerchJoinCat entity) { return (entity.MerchCategorieRecord.Id == Globals.MerCat.Id && 
                    (!entity.MerchCategorieRecord.IsInternalOnly) && (!entity.MerchRecord.IsInternalOnly)); }));

            if (coll.Count > 1)
                coll.Sort("IDisplayOrder", true);

            MerchCollection merchColl = new MerchCollection();
            foreach (MerchJoinCat cat in coll)
            {
                //verify that it has been published tosale merch
                int idx = Ctx.SaleMerch.GetList().FindIndex(
                    delegate(Merch match) { return (match.Id == cat.TMerchId && (!match.IsInternalOnly)); });

                if (idx != -1)
                    merchColl.Add(cat.MerchRecord);
            }

            WillCallWeb.Controls.MerchItemTemplate inst = (WillCallWeb.Controls.MerchItemTemplate)LoadControl(@"..\controls\MerchItemTemplate.ascx");
            inst.SectionTitle = Globals.MerCat.Name;
            inst.merchCollection.CopyFrom(merchColl);
            Content.Controls.Add(inst);
        }
        else if (Globals.MerchItem != null && Globals.MerchItem.IsParent)
        {
            //determine how to display
            string merch = "/Listing_Merch";
            Content.Controls.Add(LoadControl(string.Format(@"../controls{0}.ascx", merch)));
        }
        else
        {
            bool itemsDisplayed = false;

            //give collections to listings
            if (Ctx.FeaturedMerchListing.Count > 0)
            {
                WillCallWeb.Controls.MerchItemTemplate inst = (WillCallWeb.Controls.MerchItemTemplate)LoadControl(@"..\controls\MerchItemTemplate.ascx");
                inst.SectionTitle = "Featured Items";
                inst.merchCollection = new MerchCollection();
                inst.merchCollection.AddRange(
                    (_Config._Items_FeaturedToDisplay == 0) ? 
                    Ctx.FeaturedMerchListing : Ctx.FeaturedMerchListing.Take(_Config._Items_FeaturedToDisplay));
                Content.Controls.Add(inst);

                itemsDisplayed = true;
            }

            if (Ctx.Merch_BestSellers.Count > 0)
            {
                WillCallWeb.Controls.MerchItemTemplate inst = (WillCallWeb.Controls.MerchItemTemplate)LoadControl(@"..\controls\MerchItemTemplate.ascx");
                inst.SectionTitle = "Best Sellers";
                inst.merchCollection = Ctx.Merch_BestSellers;
                Content.Controls.Add(inst);

                itemsDisplayed = true;
            }

            if (Ctx.Merch_GoingFast.Count > 0)
            {
                WillCallWeb.Controls.MerchItemTemplate inst = (WillCallWeb.Controls.MerchItemTemplate)LoadControl(@"..\controls\MerchItemTemplate.ascx");
                inst.SectionTitle = "Going Fast";
                inst.merchCollection = Ctx.Merch_GoingFast;
                Content.Controls.Add(inst);

                itemsDisplayed = true;
            }

            //we should display something if we haven't already
            if (!itemsDisplayed)
            {
                if (Ctx.SaleMerch.Count > 0)
                {
                    MerchCollection coll = new MerchCollection();
                    
                    System.Collections.Generic.List<Merch> list = new System.Collections.Generic.List<Merch>();
					list.AddRange(Ctx.SaleMerch.GetList().FindAll(delegate(Merch match) { return (match.IsParent && match.IsDisplayable); }));

					var sortedColl =
						from listItem in list
						select listItem;

					coll.AddRange(sortedColl.OrderBy(x => x.Id).Take(50));

                    if (coll.Count > 0)
                    {
                        WillCallWeb.Controls.MerchItemTemplate inst = (WillCallWeb.Controls.MerchItemTemplate)LoadControl(@"..\controls\MerchItemTemplate.ascx");
                        inst.SectionTitle = "Recent Items";
                        inst.merchCollection = coll;
                        Content.Controls.Add(inst);
                    }
                }
            }
        }

        string titleText = string.Empty;

        if (Globals.MerchItem != null)
            titleText = string.Format(" - {0}", Globals.MerchItem.DisplayNameWithAttribs);

        this.Page.Title = string.Format("{0}{1}", _Config._PageTitle_Header, titleText);

    }
}
