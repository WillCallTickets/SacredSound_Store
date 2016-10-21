using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Xml.Serialization;
using System.Web.UI;
using System.Data;
using SubSonic;

namespace Wcss
{
    #region MailerContent

    public partial class MailerContent
    {
        public const string tagDateStatusStart = "<STAT>"; 
        public const string tagDateStatusEnd = "</STAT>"; 


        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.MailerTemplateContentRecord.DisplayOrder; }
        }
        [XmlAttribute("Title")]
        public string Title
        {
            get
            {
                if (VcTitle == null)
                    return this.MailerTemplateContentRecord.Title;

                return this.VcTitle.Trim();
            }
            set
            {
                if (value == null || value == this.MailerTemplateContentRecord.Title)
                    this.VcTitle = null;
                else 
                    this.VcTitle = value.Trim();
            }
        }
        [XmlAttribute("IsActive")]
        public bool IsActive
        {
            get
            {   
                return this.BActive;
            }
            set
            {
                this.BActive = value;
            }
        }
        //public void PopulateShowList(int maxItems, bool useImage)
        //{
        //    PopulateShowList(maxItems, useImage, false);
        //}
        public void PopulateShowList(int maxItems, bool useImage, bool deleteExistingEvents, string dateFormat)
        {
            if (deleteExistingEvents)
            {
                while(this.ShowEventRecords.Count > 0)
                    this.ShowEventRecords.DeleteFromCollection(this.ShowEventRecords[this.ShowEventRecords.Count-1].Id);
            }

            QueryCommand cmd = new QueryCommand(string.Empty, SubSonic.DataService.Provider.Name);
            cmd.CommandSql = string.Format("SELECT TOP {0} s.[Id] FROM [Show] s WHERE s.[Name] > @date ORDER BY s.[Name] ",
                    (maxItems > 0) ? maxItems.ToString() : 1000.ToString());
            cmd.Parameters.Add("@date", DateTime.Now.AddDays(-3).ToString("yyyy/MM/dd 12AM"), DbType.String);

            DataSet ds;

            ds = SubSonic.DataService.GetDataSet(cmd);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ShowEventCollection coll = new ShowEventCollection();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Show s = Show.FetchByID((int)row.ItemArray[0]);
                    if (s != null)
                    {
                        string date = MailerContent.ConfigureDate(s, false, dateFormat);
                        string opener = ShowDisplay.RemoveSpecialGuest(s.Display.Openers_NoMarkup_Verbose_NoLinks);

                        string venue = s.VenueRecord.Name;
                        if(venue.Equals(_Config._Default_VenueName, StringComparison.OrdinalIgnoreCase))
                            venue = null;

                        string imageUrl = (useImage && s.ImageManager != null) ? s.ImageManager.Thumbnail_Small : null;
                        
                        this.ShowEventRecords.AddToCollection(this.Id, ShowEvent.OwnerTypes.MailerContent, s.Id, ShowEvent.ParentTypes.Show, false,
                            date, s.StatusText, s.ShowTitle, s.Display.Promoters_NoMarkup_NoLinks, s.TopText_Derived, s.Display.Headliners_NoMarkup_Verbose_NoLinks, 
                            opener, venue, s.Display.Times_NoMarkup_ShowTimeOnly, s.Display.Mailer_Ages_NoMarkup, s.Display.Mailer_Pricing_NoMarkup, null, imageUrl);
                    }
                }
            }
        }
        private static string ConfigureDate(Show s, bool displayDatesAsRange, string dateFormat)
        {
            string line = string.Empty;

            //get date pieces
            //then when contstructing string - add in replacements
            ShowDateCollection coll = new ShowDateCollection();
            coll.AddRange(s.ShowDateRecords());

            if (coll.Count > 0)
            {
                coll.Sort("DtDateOfShow", true);

                List<Pair> dateList = new List<Pair>();

                //construct pairs
                for (int i = 0; i < coll.Count; i++)
                {
                    ShowDate sd = coll[i];
                    string dateOfShow = sd.DateOfShow.ToString(dateFormat);
                    string status = (sd.ShowStatusRecord != null) ? (sd.ShowStatusRecord.Name.ToLower() != "onsale") ? sd.ShowStatusRecord.Name : string.Empty : string.Empty;
                    if (status.ToLower() == "soldout")
                        status = "Sold Out!";

                    //if we are the first date - throw it in
                    if (i == 0)
                        dateList.Add(new Pair(dateOfShow, status));
                    else
                    {
                        //if same status as the last entry - then add date to list
                        string currentStatus = dateList[dateList.Count - 1].Second.ToString();
                        if (status == currentStatus)
                        {
                            //if the date is not already in there....
                            string residing = dateList[dateList.Count - 1].First.ToString();
                            if (residing.IndexOf(dateOfShow) == -1)
                                dateList[dateList.Count - 1].First += string.Format(", {0}", dateOfShow);
                        }
                        else
                            dateList.Add(new Pair(dateOfShow, status));
                    }
                }

                //if we are in rangemode and there is one entry (mult status not allowed)
                //we will only be working with one listing here
                //string textline = string.Empty;
                string range = string.Empty;

                if (displayDatesAsRange && dateList.Count == 1)
                {
                    string[] dateArray = dateList[0].First.ToString().Split(',');
                    if (dateArray.Length > 1)
                    {
                        string firstDate = dateArray[0];
                        string lastDate = dateArray[dateArray.Length - 1];

                        range = string.Format("{0} - {1}", firstDate.Trim(), lastDate.Trim());
                    }
                    else
                        range = dateArray[0].Replace(",", string.Empty);

                    string showStatus = dateList[0].Second.ToString();
                    if (showStatus != null && showStatus.Trim().Length > 0)
                        showStatus = string.Format("{0}{1}{2}", tagDateStatusStart, showStatus, tagDateStatusEnd);

                    line = string.Format("{0} {1}", range, showStatus).Trim();
                }
                else
                {
                    for (int j = 0; j < dateList.Count; j++)
                    {
                        range = dateList[j].First.ToString();
                        string showStatus = dateList[j].Second.ToString();

                        if (showStatus != null && showStatus.Trim().Length > 0)
                            showStatus = string.Format("{0}{1}{2}", tagDateStatusStart, showStatus, tagDateStatusEnd);

                        line = string.Format("{0} {1}", range, showStatus).Trim();
                    }

                    //remove last spaces and commas
                    //replace the last comma with an ampersand
                    char[] trimChars = { ' ', ',' };

                    line = line.TrimEnd(trimChars);
                    int lastComma = line.LastIndexOf(',');
                    if (lastComma != -1)
                        line = line.Remove(lastComma, 1).Insert(lastComma, " &");
                }
            }

            return line;
        }
    }
    #endregion

    #region MailerTemplateContent
    
    public partial class MailerTemplateContent
    {
        [XmlAttribute("DisplayOrder")]
        public int DisplayOrder
        {
            get { return this.IDisplayOrder; }
            set { this.IDisplayOrder = value; }
        }
        [XmlAttribute("MaxListItems")]
        public int MaxListItems
        {
            get { return this.IMaxListItems; }
            set { this.IMaxListItems = value; }
        }
        [XmlAttribute("MaxSelections")]
        public int MaxSelections
        {
            get { return this.IMaxSelections; }
            set { this.IMaxSelections = value; }
        }
        /// <summary>
        /// a list of name value pairs
        /// </summary>
        [XmlAttribute("Flags")]
        public List<string> Flags
        {
            get
            {
                List<string> list = new List<string>();
                if (this.VcFlags != null && this.VcFlags.Trim().Length > 0)
                    list.AddRange(this.VcFlags.Split(Utils.Constants.Separator));

                return list;
            }
            set
            {
                if (value.Count == 0)
                    this.VcFlags = null;
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    foreach (string s in value)
                        sb.AppendFormat("{0}{1}", s, Utils.Constants.Separator.ToString());

                    this.VcFlags = sb.ToString().TrimEnd(Utils.Constants.Separator);
                }
            }
        }

        /// <summary>
        /// show - uses showevent, showlinear - displays the info in one line, editor - use the html editor for content, simple - uses the simple show construct, 
        /// combo - not yet used, but will combine a show listing with an html editor
        /// </summary>
        public enum ContentAsset
        {
            show,
            showlinear,
            editor,
            simple
            //,            combo
        }
        /// <summary>
        /// a list of name value pairs
        /// </summary>
        [XmlAttribute("TemplateAsset")]
        public ContentAsset TemplateAsset
        {
            get
            {
                //List<ContentAsset> list = new List<ContentAsset>();
                //if (this.VcTemplateAsset != null && this.VcTemplateAsset.Trim().Length > 0)
                //{
                //    string[] arr = this.VcTemplateAsset.Split(Utils.Constants.Separator);
                //    foreach(string s in arr)
                //        return (ContentAsset)Enum.Parse(typeof(ContentAsset), s, true));
                //}

                //return list;
                return (ContentAsset)Enum.Parse(typeof(ContentAsset), this.VcTemplateAsset, true);
            }
            set
            {
                this.VcTemplateAsset = value.ToString();

                //if (value.Count == 0)
                //    this.VcTemplateAsset = null;
                //else
                //{
                //    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //    foreach (ContentAsset s in value)
                //        sb.AppendFormat("{0}{1}", s.ToString(), Utils.Constants.Separator.ToString());

                //    this.VcTemplateAsset = sb.ToString().TrimEnd(Utils.Constants.Separator);
                //}
            }
        }

        //TagNameListing
        //private string _tagNameListing = null;
        [XmlAttribute("TagNameListing")]
        public string TagNameListing
        {
            get
            {
                string listing = string.Empty;
                foreach(MailerTemplateSubstitution sub in this.MailerTemplateSubstitutionRecords())
                    listing += string.Format("{0}, ", sub.TagName.Replace("<",string.Empty).Replace(">",string.Empty));

                return listing.Trim().TrimEnd(',');
            }
        }
        //TagNameList
        [XmlAttribute("TagNameList")]
        public List<string> TagNameList
        {
            get
            {
                List<string> list = new List<string>();
                foreach(MailerTemplateSubstitution sub in this.MailerTemplateSubstitutionRecords())
                    list.Add(sub.TagName.Replace("<",string.Empty).Replace(">",string.Empty));

                return list;
            }
        }
    }
    public partial class MailerTemplateContentCollection : Utils._Collection.IOrderable<MailerTemplateContent>
    {
        /// <summary>
        /// Adds a MailerTemplateContent to the collection
        /// </summary>
        /// <param name="tMailerTemplateId"></param>
        /// <param name="name"></param>
        /// <param name="title"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public MailerTemplateContent AddToCollection(int tMailerTemplateId, string name, string title, string template)
        {
            System.Collections.Generic.List<System.Web.UI.Pair> args = new System.Collections.Generic.List<System.Web.UI.Pair>();
            args.Add(new System.Web.UI.Pair("DtStamp", DateTime.Now));
            args.Add(new System.Web.UI.Pair("TMailerTemplateId", tMailerTemplateId));
            args.Add(new System.Web.UI.Pair("Name", name));
            args.Add(new System.Web.UI.Pair("Title", title));
            args.Add(new System.Web.UI.Pair("Template", template));
            
            return AddToCollection(args);
        }

        public MailerTemplateContent AddToCollection(System.Collections.Generic.List<System.Web.UI.Pair> args)
        {
            return Utils._Collection.AddItemToOrderedCollection(this.GetList(), args);
        }

        /// <summary>
        /// Delete a MailerTemplateContent from the collection by ID
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public bool DeleteFromCollection(int idx)
        {
            return Utils._Collection.DeleteFromOrderedCollection(this.GetList(), idx);
        }

        /// <summary>
        /// Reorder a MailerTemplateContent by ID and direction
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public MailerTemplateContent ReorderItem(int idx, string direction)
        {
            return Utils._Collection.ReorderOrderedCollection(this.GetList(), idx, direction);
        }
    }

    #endregion
}
