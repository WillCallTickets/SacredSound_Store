using System;
using System.Web.UI.WebControls;

using AjaxControlToolkit;

using Wcss;
using WillCallWeb;

namespace WillCallWeb.Controls
{
    public partial class Faq : WillCallWeb.BaseControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            BindItems();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //ajax control toolkit does not like dynamic data
            //remove post back stipulation and put binding in init
        }
        public void BindItems()
        {
            TabContainer1.DataBind();
        }
        protected void tab_Binding(object sender, EventArgs e)
        {
            TabContainer container = (TabContainer)sender;

            FaqCategorieCollection coll = new FaqCategorieCollection();
            coll.AddRange(_Lookits.FaqCategories.GetList().FindAll(delegate(FaqCategorie match) { return (match.IsActive); }));
            if(coll.Count > 1)
                coll.Sort("IDisplayOrder", true);

            foreach (FaqCategorie cat in coll)
            {                
                TabPanel tab = new TabPanel();
                tab.HeaderText = cat.Display_Preferred;

                FaqItemCollection faks = new FaqItemCollection();
                faks.AddRange(cat.FaqItemRecords().GetList().FindAll(delegate(FaqItem match) { return (match.IsActive); }));

                if (faks.Count > 0)// && aco.SelectedIndex == idx)
                {
                    if (faks.Count > 1)
                        faks.Sort("IDisplayOrder", true);

                    foreach (FaqItem item in faks)
                    {
                        Literal question = new Literal();
                        //question.Text = string.Format("<div class=\"qablock\"><div class=\"question\">Q: {0}</div>", item.Question);
                        question.Text = string.Format("<ul><h6>Q: {0}</h6>", item.Question);
                        tab.Controls.Add(question);

                        Literal answer = new Literal();
                        answer.Text = string.Format("<li>{0}</li></ul>", item.Answer);
                        tab.Controls.Add(answer);
                    }
                }

                container.Tabs.Add(tab);
            }

            if(container.Tabs.Count > 0)
                container.ActiveTab = container.Tabs[0];
        }
    }
}