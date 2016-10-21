using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.Services;
using System.Linq;

using Wcss;
using WillCallWeb;
using WillCallWeb.StoreObjects;

public partial class Store_AgeVerification18 : WillCallWeb.BasePage
{
    [WebMethod]
    public static object SetComplianceDate18(string userName, string profileDob, string month, string day, string year)
    {
        return SaleItem_Services.SetComplianceDate18(new WebContext(), userName, profileDob, month, day, year);
    }  
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlMonth.DataBind();
            ddlDay.DataBind();
            ddlYear.DataBind();
        }

        DateTime dob = DateTime.MaxValue;
        string userName = (this.Profile != null && (!this.Profile.IsAnonymous)) ? this.Profile.UserName : string.Empty;

        hidUserName.Value = userName;
        hidProfileDob.Value = (DateTime.TryParse((userName.Trim().Length > 0) ? this.Profile.DateOfBirth : string.Empty, out dob)) 
            ? dob.ToString("MM/dd/yyyy") : string.Empty;
    }
    
    protected void ddl_DataBinding(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        if (ddl != null)
        {
            List<ListItem> list = new List<ListItem>();

            switch (ddl.ID)
            {
                case "ddlDay":
                    for (int i = 1; i <= 31; i++)
                        list.Add(new ListItem(i.ToString()));
                    break;
                case "ddlMonth":
                    for (int i = 1; i <= 12; i++)
                        list.Add(new ListItem(i.ToString()));
                    break;
                //already set up for year - resort high to low
                default:
                    int max = DateTime.Now.Year;
                    int min = max - 120;
                    for (int i = max; i >= min; i--)
                        list.Add(new ListItem(i.ToString()));
                    break;
            }
            
            ddl.DataSource = list;
        }
    }   
}
