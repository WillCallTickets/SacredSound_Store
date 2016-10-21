using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Diagnostics;

using Wcss;

namespace WillCallWeb
{

    public partial class Testing_ConcurrencyTest : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        private WebContext sessionContext;
        private string depFile = string.Empty;

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            lblConditions.Text = string.Empty;
            //Ctx.Publishio();
            sessionContext = Ctx;
            depFile = _Config._ShowDependencyFile;

            StartTest();
        }

        public void StartTest()
        {
            for (int i = 0; i < 10; i++)
            {
                new Thread(HydrateCacheObject).Start();
            }
        }


        private void HydrateCacheObject()
        {   
            try
            {
                string file = _Config._ShowDependencyFile;

                WebContext threadContext;

                threadContext = sessionContext;

                ShowDateCollection coll = new ShowDateCollection();
                coll.AddRange(threadContext.SaleShowDates_All);

                foreach (var item in coll)
                {
                    var laxyObject1 = item.ShowTicketRecords()[0].Allotment;

                    var laxyObject3 = item.ShowTicketRecords()[0].Sold;
                    //string g = "if";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                lblConditions.Text += string.Format("{0}<br/>", ex.Message);
            }
        }

       
    }
}
