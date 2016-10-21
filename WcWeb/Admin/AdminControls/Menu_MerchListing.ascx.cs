using System;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class Menu_MerchListing : BaseControl
    {
        public void BindAll()
        {
            CreateChildControls();
        }
        
        protected override void CreateChildControls()
        {
            base.Controls.Clear();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int depth = 0;

            //setup promotion items
            MerchCollection promotions = new MerchCollection();
                        
            sb.AppendFormat("{1}<span class=\"menu\">{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

            MerchDivisionCollection divs = new MerchDivisionCollection();
            divs.AddRange(_Lookits.MerchDivisions);
            if (divs.Count > 1)
                divs.Sort("IDisplayOrder", true);

            foreach (MerchDivision div in divs)
            {
                //if we have categories within this division
                if (div.MerchCategorieRecords().Count > 0)
                {
                    //show breakdown - division >> category
                    sb.AppendFormat("{1}<ul class=\"header\">{2}{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++), div.Name);

                    MerchCategorieCollection categs = new MerchCategorieCollection();
                    categs.AddRange(div.MerchCategorieRecords());
                    if (categs.Count > 1)
                        categs.Sort("IDisplayOrder", true);

                    foreach (MerchCategorie cat in categs)
                    {
                        sb.AppendFormat("{1}<li class=\"subheader\">{2}</li>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++), cat.Name);

                        //if we have items within categories
                        if (cat.MerchJoinCatRecords().Count > 0)
                        {
                            sb.AppendFormat("{1}<ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

                            MerchCollection parents = new MerchCollection();
                            foreach (MerchJoinCat joiner in cat.MerchJoinCatRecords())
                                parents.Add(joiner.MerchRecord);

                            if (parents.Count > 1)
                                parents.Sort("Name", true);

                            foreach (Merch parent in parents)
                            {
                                if (Atx.AdminMerchListingContext == 0 || (Atx.AdminMerchListingContext == 1 && parent.IsActive))
                                {
                                    if (parent.IsPromotionalItem)
                                        promotions.Add(parent);

                                    FillListItem(sb, depth, parent);
                                }
                            }

                            sb.AppendFormat("{1}</ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
                        }
                    }

                    sb.AppendFormat("{1}</ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
                }
            }
            
            //insert featured list
            if (Ctx.FeaturedMerchListing.Count > 0)
            {
                System.Text.StringBuilder lst = new System.Text.StringBuilder();

                lst.AppendFormat("{1}<ul class=\"header\">{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));

                lst.AppendFormat("{1}Featured Items{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

                lst.AppendFormat("{1}<ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
                foreach (Merch m in Ctx.FeaturedMerchListing)
                    FillListItem(lst, depth, m);
                lst.AppendFormat("{1}</ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));

                lst.AppendFormat("{1}</ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));

                sb.Insert(0, lst);
            }
            //insert promotion list
            if (promotions.Count > 0)
            {
                System.Text.StringBuilder prom = new System.Text.StringBuilder();

                prom.AppendFormat("{1}<ul class=\"header\">{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));

                prom.AppendFormat("{1}Promotion Items{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

                prom.AppendFormat("{1}<ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
                foreach (Merch m in promotions)
                    FillListItem(prom, depth, m);
                prom.AppendFormat("{1}</ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));

                prom.AppendFormat("{1}</ul>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));

                sb.Insert(0, prom);
            }

            sb.AppendFormat("{1}</span>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));

            Literal menu = new Literal();
            menu.Text = sb.ToString();
            this.Controls.Add(menu);
        }

        private void FillListItem(System.Text.StringBuilder builder, int depth, Merch merchRecord)
        {
            bool isInventoryLink = (this.Page.ToString() == "ASP.admin_orders_aspx");

            builder.AppendFormat("{1}<li>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth++));

            ItemImageCollection imgColl = new ItemImageCollection();
            imgColl.AddRange(merchRecord.ItemImageRecords().GetList().FindAll(delegate(ItemImage match) { return (match.IsItemImage); }));

            if ((!isInventoryLink) && imgColl.Count > 0)
            {
                if (imgColl.Count > 1)
                    imgColl.Sort("IDisplayOrder", true);
                ItemImage img = imgColl[0];

                builder.AppendFormat("<a href=\"/Admin/MerchEditor.aspx?p=Images&merchitem={2}\"><img src=\"{3}\" alt=\"\" /></a>",
                    Utils.Constants.NewLine, Utils.Constants.Tabs(depth--),
                    merchRecord.Id, img.Thumbnail_Small);
            }
            else
                builder.Append("&#141;&nbsp;");

            builder.AppendFormat("{1}<a href=\"/Admin/{2}{3}\">{4}</a>",
                Utils.Constants.NewLine, Utils.Constants.Tabs(depth--), 
                (isInventoryLink) ? string.Format("Order.aspx?p=merch&merchitem=") :
                string.Format("MerchEditor.aspx?p=ItemEdit&merchitem="),
                merchRecord.Id, merchRecord.DisplayName);

            builder.AppendFormat("{1}</li>{0}", Utils.Constants.NewLine, Utils.Constants.Tabs(depth--));
        }
}
}