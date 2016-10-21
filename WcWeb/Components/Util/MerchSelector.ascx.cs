using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;

using WillCallWeb.StoreObjects;

namespace WillCallWeb.Components.Util
{
    /// <summary>
    /// Note that viewstate may need to be reinstated to have the time portion function properly
    /// </summary>
    [ToolboxData("<{0}:MerchSelector runat=\"Server\" ValidationGroup=\"\" SelectedParentId=\"\" ShowInventory=\"true\" SelectedInventoryId=\"\" Width=\"100%\"></{0}:MerchSelector>")]
    [DefaultPropertyAttribute("SelectedValue")]
    [ValidationProperty("SelectedValue")]
    public partial class MerchSelector : BaseControl
    {
        protected string _valGroup = string.Empty;
        public string ValidationGroup { get { return _valGroup; } set { _valGroup = value; } }
        
        protected string _uniqueId { get { return this.ClientID; } }

        protected bool _showInventory = true;
        public bool ShowInventory { get { return _showInventory; } set { _showInventory = value; } }

        public int SelectedParentId
        {
            get
            {
                return (this.hidSelectedParentId.Value.Trim().Length > 0) ? int.Parse(this.hidSelectedParentId.Value) : 0;
            }
        }
        public int SelectedInventoryId
        {
            get
            {
                return (this.selInventory.Visible && this.selInventory.Value.Length > 0 && this.selInventory.Value != "0") ? 
                    int.Parse(this.selInventory.Value) : 
                    SelectedParentId;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.selInventory.Visible = ShowInventory;
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string script = string.Format("SearchText();");
            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(),
                Guid.NewGuid().ToString(), " ;" + script, true);
        }
}
}

