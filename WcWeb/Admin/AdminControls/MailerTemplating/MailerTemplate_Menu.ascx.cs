using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Wcss;

namespace WillCallWeb.Admin.AdminControls.MailerTemplating
{
    [ToolboxData("<{0}:MailerTemplate_Menu runat=\"Server\" Title=\"\" ></{0}:MailerTemplate_Menu>")]
    public partial class MailerTemplate_Menu : BaseControl
    {
        private string _title = string.Empty;
        public string Title { get { return _title; } set { _title = value; } }
        protected override void LoadControlState(object savedState)
        {
            object[] ctlState = (object[])savedState;
            base.LoadControlState(ctlState[0]);
            this._title = ctlState[1].ToString();
        }
        protected override object SaveControlState()
        {
            object[] ctlState = new object[2];
            ctlState[0] = base.SaveControlState();
            ctlState[1] = this._title;
            return ctlState;
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnSelect - always enabled
            btnContent.Enabled = Atx.CurrentMailer != null;
            btnGenerate.Enabled = Atx.CurrentMailer != null; 
            //btnUpload.Enabled - always enabled

            btnTptEdit.Enabled = Atx.CurrentMailerTemplate != null;
            btnTptContainer.Enabled = Atx.CurrentMailerTemplate != null;
            btnTptSubstitution.Enabled = Atx.CurrentMailerTemplate != null;
        }

        protected void btnNav_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            string cmd = btn.CommandName.ToLower();
            
            switch (cmd)
            {
                case "select":
                    base.Redirect("/Admin/Mailers.aspx?p=select");
                    break;
                case "content":
                    base.Redirect("/Admin/Mailers.aspx?p=mlredit");
                    break;
                case "generate":
                    base.Redirect("/Admin/Mailers.aspx?p=mlrgenerate");
                    break;
                case "upload":
                    base.Redirect("/Admin/Mailers.aspx?p=mlrupload");
                    break;
                case "tptedit":
                    base.Redirect("/Admin/Mailers.aspx?p=tpledit");
                    break;
                case "tptcontainer":
                    base.Redirect("/Admin/Mailers.aspx?p=tplparts");
                    break;
                case "tptsubstitution":
                    base.Redirect("/Admin/Mailers.aspx?p=tplsubs");
                    break;
            }
        }
    }
}