using System;
using System.Web.Security;

namespace Wcss
{
    public partial class Customer 
    {
        /// <summary>
        /// The isAuthenticated parameter is more of a reminder to validate the user. The operation wil not take place unless isAuthenticated is true;
        /// Depending on the context of the change - you may need to logout and login to enable new membership in session.
        /// We do not simply change the membership - we need to make sure this can be done as a transaction
        /// </summary>
        /// <param name="isAuthenticated"></param>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static bool ChangeUserName(bool isAuthenticated, string creatorName, string oldName, string newName)
        {
            if (isAuthenticated)
            {
                try
                {
                    //determine if the new name already exists
                    //MembershipUser exists = Membership.GetUser(newName);

                    string qry1 = string.Format("SELECT a.* FROM Aspnet_Users a WHERE a.[UserName] = @userName");
                    SubSonic.QueryCommand cmd2 = new SubSonic.QueryCommand(qry1, SubSonic.DataService.Provider.Name);
                    cmd2.Parameters.Add("@userName", newName);

                    AspnetUserCollection existColl = new AspnetUserCollection();
                    existColl.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd2));

                    //if (exists != null)
                    if(existColl.Count > 0)
                    {
                        //log an event
                        string reason = "This user name already exists.";
                        EventQ.CreateChangeUserNameEvent(DateTime.Now, DateTime.Now, creatorName, oldName, newName, _Enums.EventQStatus.Failed, reason);

                        throw new Exception(reason);
                    }

                    //now get the current membership
                    //MembershipUser mem = Membership.GetUser(oldName);

                    string qry2 = string.Format("SELECT a.* FROM Aspnet_Users a WHERE a.[UserName] = @userName");
                    SubSonic.QueryCommand cmd1 = new SubSonic.QueryCommand(qry2, SubSonic.DataService.Provider.Name);
                    cmd1.Parameters.Add("@userName", oldName);

                    AspnetUserCollection aColl = new AspnetUserCollection();
                    aColl.LoadAndCloseReader(SubSonic.DataService.GetReader(cmd1));

                    //if (mem != null)
                    if (aColl.Count > 0)
                    {
                        AspnetUser usr = aColl[0];

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append("BEGIN TRANSACTION ");
                        //sb.Append("UPDATE Aspnet_Profile SET [UserId] = @newUserId WHERE [ApplicationId] = @appId AND [UserId] = @userId; ");
                        sb.Append("UPDATE Aspnet_Membership SET [Email] = @newUserName, [LoweredEmail] = @newUserName WHERE [ApplicationId] = @appId AND [UserId] = @userId; ");
                        sb.Append("UPDATE Aspnet_Users SET [UserName] = @newUserName, [LoweredUserName] = @newUserName WHERE [ApplicationId] = @appId AND [UserId] = @userId; ");
                        sb.Append("COMMIT TRANSACTION ");
                        sb.Append("INSERT User_PreviousEmail([UserId], [EmailAddress], [dtStamp]) ");
                        sb.Append("VALUES (@userId, @oldUserName, @now); ");

                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sb.ToString(), SubSonic.DataService.Provider.Name);
                        cmd.Parameters.Add("@newUserName", newName.ToLower());//we know its lower - but make it apparently clear
                        cmd.Parameters.Add("@oldUserName", oldName.ToLower());
                        cmd.Parameters.Add("@appId", _Config.APPLICATION_ID.ToString());
                        cmd.Parameters.Add("@userId", usr.UserId.ToString());
                        //cmd.Parameters.Add("@userId", acoll[0]..ProviderUserKey.ToString());
                        cmd.Parameters.Add("@now", DateTime.Now, System.Data.DbType.DateTime);

                        int affRows = SubSonic.DataService.ExecuteQuery(cmd);

                        if (affRows > 0)
                        {
                            //log an event
                            EventQ.CreateChangeUserNameEvent(DateTime.Now, DateTime.Now, creatorName, oldName, newName, _Enums.EventQStatus.Success, null);

                            //send an email
                            MailQueue.SendUserChangeEmail(oldName, newName);

                            //do we need to login again?
                            return true;
                        }
                        else
                            throw new Exception(string.Format("Sql Membership could not be completed for oldName: {0} newName {1} oldUserId: {2}",
                                oldName, newName, usr.UserId.ToString()));//mem.ProviderUserKey.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return false;
        }

        public static string MigrateCustomerToMembership()
        {
            //Comment out this line when in use - this is a safety for running accidentally
            return "prepare settings first";

            //string start = _Config_AdminEmailAddress;
            ////string end = "ab";

            //string where = string.Format("EmailAddress = {0}", start);

            //DateTime startTime = DateTime.Now;

            //Query q = new Query("Customer").WHERE(where);

            //System.Data.IDataReader reader = q.ExecuteReader();
            
            //int profilesCreated = 0;
            //int newMembers = 0;
            //int loopCount = 0;

            //try
            //{
            //    while (reader.Read())
            //    {
            //        string email = reader.GetValue(reader.GetOrdinal("EmailAddress")).ToString();
            //        string ptPassword = reader.GetValue(reader.GetOrdinal("ptPassword")).ToString();

            //        //string emailStatusId = reader.GetValue(reader.GetOrdinal("TEmailStatusId")).ToString();
            //        //string statusDate = reader.GetValue(reader.GetOrdinal("dtStatusDate")).ToString();
            //        //string htmlEmail = reader.GetValue(reader.GetOrdinal("bHtmlEmail")).ToString();
            //        //string confirmed = reader.GetValue(reader.GetOrdinal("bConfirmed")).ToString();

            //        string firstName = reader.GetValue(reader.GetOrdinal("FirstName")).ToString();
            //        string lastName = reader.GetValue(reader.GetOrdinal("LastName")).ToString();

            //        string address1 = reader.GetValue(reader.GetOrdinal("Address1")).ToString();
            //        string address2 = reader.GetValue(reader.GetOrdinal("Address2")).ToString();
            //        string city = reader.GetValue(reader.GetOrdinal("City")).ToString();
            //        string state = reader.GetValue(reader.GetOrdinal("State")).ToString();
            //        string zip = reader.GetValue(reader.GetOrdinal("ZipCode")).ToString();
            //        // string country = reader.GetValue(reader.GetOrdinal("Country")).ToString();
            //        string phone = reader.GetValue(reader.GetOrdinal("PhoneNumber")).ToString();

            //        string notes = reader.GetValue(reader.GetOrdinal("Notes")).ToString();
            //        string created = reader.GetValue(reader.GetOrdinal("dtStamp")).ToString();

            //        string oldCustomerId = reader.GetValue(reader.GetOrdinal("Id")).ToString();


            //        MembershipCreateStatus stat = new MembershipCreateStatus();

            //        MembershipUser usr = Membership.GetUser(email);
            //        if (usr == null)
            //        {
            //            usr = Membership.CreateUser(email, ptPassword, email, "new", "pass1", true, out stat);
            //            newMembers++;
            //        }

            //        if (usr != null)
            //        {
            //            usr.Comment = notes;

            //            _Profile prof = (_Profile)ProfileBase.Create(email, true);

            //            prof.FirstName = firstName;
            //            prof.LastName = lastName;
            //            prof.Comments = notes;

            //            prof.Address1 = address1;
            //            prof.Address2 = address2;
            //            prof.City = city;
            //            prof.State = state;
            //            prof.PostalCode = zip;
            //            //prof.Country = "USA";
            //            prof.Phone = phone;

            //            prof.MemberSince = DateTime.Parse(created);
            //            prof.OldCustomerId = int.Parse(oldCustomerId);

            //            prof.Save();

            //            profilesCreated++;
            //        }

            //        loopCount++;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return string.Format("<h1>Error:</h1>{1}{0}{0}{2}", "<BR>", ex.Message, ex.StackTrace);
            //}
            //finally
            //{
            //    reader.Close();
            //}

            //DateTime endTime = DateTime.Now;
            //TimeSpan execution = TimeSpan.FromTicks(endTime.Ticks) - TimeSpan.FromTicks(startTime.Ticks);
            ////DateTime execution = endTime.Subtract(TimeSpan.Parse(startTime.ToString()));

            //return string.Format("Execution Time: {1}{0}Loop Count: {2}{0}Members Created: {3}{0}Profiles Created: {4}{0}{0}{5}",
            //    "<BR>", execution.ToString(), loopCount, newMembers, profilesCreated, q.Inspect());
        }

        
    }
}

