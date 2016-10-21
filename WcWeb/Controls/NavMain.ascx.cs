using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Controls
{
    public partial class NavMain : WillCallWeb.BaseControl
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        List<Triplet> list = new List<Triplet>();

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            sb.Length = 0;
            list.Clear();

            this.GatherMenuItems();

            foreach(Triplet t in list)
            {
                string text = t.First.ToString();

                sb.AppendFormat("<td>");
                
                sb.AppendFormat("{3}{4}<a href=\"{0}\" title=\"{1}\" >{2}</a></td>", 
                    t.Third.ToString(), t.Second.ToString(), text,
                    Utils.Constants.NewLine, Utils.Constants.Tabs(2));
            }

            litControlContent.Text = sb.ToString();
        }

        private List<Triplet> GatherMenuItems()
        {
            //link text - tooltip - link
            if (_Config.APPLICATION_NAME.ToLower() == "taos")
                list.Add(new Triplet("Tix &amp; Camping", "see tickets on sale", "/Store/ChooseTicket.aspx"));
            else
                list.Add(new Triplet("Tickets", "see tickets on sale", "/Store/ChooseTicket.aspx"));

            if(_Config.APPLICATION_NAME.ToLower() == "sts9")
                list.Add(new Triplet("Merchandise", "see merchandise on sale", "/Store/ChooseMerch.aspx"));
            else
                list.Add(new Triplet("Merch", "see merchandise on sale", "/Store/ChooseMerch.aspx"));

            list.Add(new Triplet("Cart", "edit shopping cart", "/Store/Cart_Edit.aspx"));
            if (this.Page.User.IsInRole("WebUser"))
                list.Add(new Triplet("Account", "view account details &amp; purchase history", "/EditProfile.aspx"));
            if (_Config._FAQ_Page_On)
                list.Add(new Triplet("FAQ", "frequently asked questions", "/Faq.aspx"));
            list.Add(new Triplet("Contact", "contact customer support", "/Contact.aspx"));
            if (_Config._CharityListing_On)
                list.Add(new Triplet("Goodwill", "learn more about causes important to us", "/CharitableOrgs.aspx"));
            
            //add any additional links per application
            list.AddRange(_Config._Navigation_AdditionalLinks);

            if (this.IsAuthdAdminUser)
                list.Add(new Triplet("Admin", "administration", "/Admin/Default.aspx"));
           
            return list;
        }
}
}