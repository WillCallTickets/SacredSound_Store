using System;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Wcss;

namespace WillCallWeb.Admin.AdminControls
{
    public partial class CustomerPicker : BaseControl
    {   
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnAdmins_Click(object sender, EventArgs e)
        {
            List<ListItem> admins = new List<ListItem>();
            List<ListItem> list = new List<ListItem>();
            string sql = "SELECT DISTINCT u.UserName ";
            sql += ", LTRIM(ISNULL(dbo.fn_GetProfileValue(u.userId, 'FirstName'), '') + ' ' + ISNULL(dbo.fn_GetProfileValue(u.userId, 'LastName'), '')) as 'Name' ";
            sql += "FROM aspnet_users u, aspnet_usersinroles ur, aspnet_roles r ";
            sql += "WHERE u.userid = ur.userid and ur.roleid = r.roleid and r.rolename <> 'webuser' ORDER BY u.UserName";
            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);

            using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
            {
                while (dr.Read())
                {   
                    admins.Add(new ListItem(
                        string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()), 
                        dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                }
            }

            if(admins.Count > 0)
            {
                foreach(ListItem li in admins)
                {
                    string[] inRoles = Roles.GetRolesForUser(li.Value);
                    string desc = string.Format("{0} - {1}", li.Text, string.Join(", ", inRoles));
                    list.Add(new ListItem(desc, li.Value));
                }
            }

            BindResults("Admins", list);
        }

        private void BindResults(string criteria, List<ListItem> list)
        {
            lblCriteria.Text = string.Format("Search results for: ... {0} ...", criteria);
            rptResults.DataSource = list;
            
            rptResults.DataBind();
        }

        protected void rptResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rpt = (Repeater)sender;

            ListItem li = (ListItem)e.Item.DataItem;
            Literal item = (Literal)e.Item.FindControl("litItem");

            if (li != null && li.Text != null && li.Text.Trim().Length > 0 && item != null)
            {
                if (li.Value != null && li.Value.Trim().Length > 0)
                {
                    if(li.Value.ToLower().StartsWith("p=view&inv="))
                        item.Text = string.Format("<a {0}>{1}</a>", string.Format("href=\"/Admin/Orders.aspx?{0}\"", li.Value), li.Text);
                    else
                        item.Text = string.Format("<a {0}>{1}</a>", string.Format("href=\"/Admin/EditUser.aspx?username={0}\"", li.Value), li.Text);
                }
                else
                    item.Text = string.Format("<div class=\"{0}\">{1}</div>", "criteria", li.Text);
            }
        }

        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<ListItem> list = new List<ListItem>();        

            //decide what to search for
            string email = txtEmail.Text.Trim();
            string number = txtInvoice.Text.Trim();
            string lastname = txtLastName.Text.Trim();
            string custId = txtCustId.Text.Trim();
            string lastfour = txtLastFour.Text.Trim();
            string month = ddlBdMonth.SelectedValue;

            string criteria = string.Empty;

            if(email.Length > 0)
            {
                criteria = string.Format("Email address: {0}", email);

                string search = string.Format("{0}%", email);

                using (IDataReader dr = SPs.TxCustomerSearchLikeUserName(_Config.APPLICATION_NAME, email).GetReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()),
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }

                //Find previous emails
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SET NOCOUNT ON SELECT DISTINCT u.[UserName] as 'UserName' ");
                sb.Append(", LTRIM(ISNULL(dbo.fn_GetProfileValue(u.userId, 'FirstName'), '') + ' ' + ISNULL(dbo.fn_GetProfileValue(u.userId, 'LastName'), '')) as 'Name' ");
                sb.Append("FROM [User_PreviousEmail] upe, [Aspnet_Users] u WHERE upe.[EmailAddress] LIKE @search AND ");
                sb.Append("upe.[UserId] = u.[UserId] AND u.[ApplicationId] = @appId ORDER BY u.[UserName]; ");
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid); 
                cmd.Parameters.Add("@search", search);

                using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
                {
                    bool init = false;
                    while (dr.Read())
                    {
                        if (!init)
                        {
                            list.Add(new ListItem(string.Empty));
                            list.Add(new ListItem("...related matches...", string.Empty));
                            
                            init = true;
                        }

                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()), 
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));

                    }

                    dr.Close();
                }
                //end find previous


                //Find email subscribers only
                System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                sb2.Append("SELECT DISTINCT u.[UserName] as 'UserName'");
                sb2.Append(", LTRIM(ISNULL(dbo.fn_GetProfileValue(u.userId, 'FirstName'), '') + ' ' + ISNULL(dbo.fn_GetProfileValue(u.userId, 'LastName'), '')) as 'Name' ");
                sb2.Append("FROM [Aspnet_Users] u WHERE u.[ApplicationId] = @appId AND u.[UserName] LIKE @search AND ");
                sb2.Append("u.[UserId] NOT IN (SELECT m.[UserId] FROM [Aspnet_Membership] m WHERE m.[ApplicationId] = @appId) ORDER BY u.[UserName]; ");
                SubSonic.QueryCommand cmd2 = new SubSonic.QueryCommand(sb2.ToString(), SubSonic.DataService.Provider.Name);
                cmd2.Parameters.Add("@appId", _Config.APPLICATION_ID, DbType.Guid);
                cmd2.Parameters.Add("@search", search);

                using (IDataReader dr = SubSonic.DataService.GetReader(cmd2))
                {
                    bool init = false;
                    while (dr.Read())
                    {
                        if (!init)
                        {
                            list.Add(new ListItem(string.Empty));
                            list.Add(new ListItem("...email subscribers...", string.Empty));

                            init = true;
                        }

                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()),
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));

                    }

                    dr.Close();
                }
                //end find subscribers only

                txtEmail.Text = string.Empty;

            }
            else if (number.Length > 0)
            {
                criteria = string.Format("Invoice #: {0}", number);
                //create a proc to search invoice and get emails

                using (IDataReader dr = SPs.TxCustomerSearchInvoiceNumber(_Config.APPLICATION_NAME, number).GetReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()),
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }

                txtInvoice.Text = string.Empty;
            }
            else if (lastname.Length > 0)
            {
                criteria = string.Format("LastName: {0}", lastname);
                using (IDataReader dr = SPs.TxCustomerSearchByProfileParam(_Config.APPLICATION_NAME, "LastName", lastname).GetReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()),
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }
                
                txtLastName.Text = string.Empty;
            }
            else if (lastfour.Length > 0)
            {
                criteria = string.Format("Last Four: {0}", lastfour);

                string search = string.Format("{0}", lastfour);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.Append("CREATE TABLE #tmpInvoiceUsers (UserId uniqueidentifier) ");
                sb.Append("INSERT #tmpInvoiceUsers(UserId) SELECT DISTINCT(i.[UserId]) as 'UserId' FROM [Invoice] i, [InvoiceTransaction] it ");
                sb.Append("WHERE it.[LastFourDigits] = @search AND it.[TInvoiceId] = i.[Id] AND i.[ApplicationId] = @appId ");
                sb.Append("SELECT u.[UserName] "); 
                sb.Append(", LTRIM(ISNULL(dbo.fn_GetProfileValue(u.userId, 'FirstName'), '') + ' ' + ISNULL(dbo.fn_GetProfileValue(u.userId, 'LastName'), '')) as 'Name' ");
                sb.Append("FROM [Aspnet_Users] u, [#tmpInvoiceUsers] i WHERE u.[UserId] = i.[UserId] ORDER BY u.[UserName] ");

                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                cmd.AddParameter("@search", search, DbType.String);

                using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
                {   
                    while (dr.Read())
                    {
                        //list.Add(new ListItem(dr["UserName"].ToString()));
                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()),
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }
                
                txtLastFour.Text = string.Empty;
            }
            else if (custId.Length > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("SELECT u.[UserName] ");
                sb.Append(", LTRIM(ISNULL(dbo.fn_GetProfileValue(u.userId, 'FirstName'), '') + ' ' + ISNULL(dbo.fn_GetProfileValue(u.userId, 'LastName'), '')) as 'Name' ");
                sb.Append("FROM [Aspnet_Users] u WHERE u.[ApplicationId] = @appId AND u.[CustomerId] LIKE @searchId ORDER BY u.[UserName] ");

                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                cmd.Parameters.Add("@appId", _Config.APPLICATION_ID, System.Data.DbType.Guid);
                cmd.AddParameter("@searchId", string.Format("{0}%", custId), DbType.String);
                
                using (IDataReader dr = SubSonic.DataService.GetReader(cmd))
                {
                    while (dr.Read())
                    {
                        //list.Add(new ListItem(dr["UserName"].ToString()));
                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()),
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }

                txtCustId.Text = string.Empty;
            }
            else if (month != "0")
            {
                criteria = string.Format("Birthdays by month : {0}", ddlBdMonth.SelectedItem.Text);

                month = string.Format("{0}/%", month);

                using (IDataReader dr = SPs.TxCustomerSearchLikeProfileParam(_Config.APPLICATION_NAME, "DateOfBirth", month).GetReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new ListItem(
                            string.Format("{0} - {1}", dr.GetValue(dr.GetOrdinal("UserName")).ToString(), dr.GetValue(dr.GetOrdinal("Name")).ToString()),
                            dr.GetValue(dr.GetOrdinal("UserName")).ToString()));
                    }

                    dr.Close();
                }

                ddlBdMonth.SelectedIndex = 0;
            }

            BindResults(criteria, list);
        }
}
}
