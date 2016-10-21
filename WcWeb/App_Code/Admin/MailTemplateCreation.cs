using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;

using Wcss;

namespace WillCallWeb.Admin
{
    /// <summary>
    /// Summary description for MailTemplateCreation
    /// </summary>
    public class MailerTemplateCreation
    {
        private Mailer _mailer = null;
        public Mailer MailerObject { get { return _mailer; } set { _mailer = value;  } }

        //#region Section Names
        //public enum TemplateSectionName
        //{
        //    title = 0,
        //    announcement,
        //    highlight,
        //    mainlisting,
        //    specialinterest,
        //    secondarylisting,            
        //    custom1
        //}
        //#endregion

        #region TagNames

        public static readonly string replacementTag = "{0}";//
        public const string tagRepeat_Start = "<REPEAT>";//
        public const string tagRepeat_End = "</REPEAT>";//
        public const string tagTitle = "<TITLE>";//
        public const string tagContent = "<CONTENT>";//
        public const string tagDate = "<DATE>";//
        public const string tagHeader = "<HEADER>";//
        public const string tagHeadliner = "<HEADLINER>";//
        public const string tagImageHeightMax = "<IMAGEHEIGHTMAX>";//        
        public const string tagOpener = "<OPENER>";//
        public const string tagPromoter = "<PROMOTER>";//
        public const string tagShowDisplayUrl = "<SHOWDISPLAYURL>"; //
        public const string tagShowId = "<SHOWID>";//
        public const string tagShowTitle = "<SHOWTITLE>";//
        public const string tagStatus = "<STATUS>"; 
        public const string tagTimes = "<TIMES>";//
        public const string tagVenue = "<VENUE>";//
        public const string tagAges = "<AGES>";//
        public const string tagPricing = "<PRICING>";//
        public const string tagTextStart = "<TEXT_";//

        #endregion

        public StringBuilder _textVersion;
        public StringBuilder _htmlVersion;

        public MailerTemplateCreation() { }

        public MailerTemplateCreation(Mailer mailer) 
        {
            //Exit if no mailer
            if (mailer == null)
                return;

            _mailer = new Mailer();
            _mailer.CopyFrom(mailer);

            _textVersion = new StringBuilder();
            _htmlVersion = new StringBuilder();
        }

        public void GenerateMailer()
        {
            if (MailerObject != null)
            {
                MailerTemplate mailerTemplate = MailerObject.MailerTemplateRecord;
                //get the style
                string _style = mailerTemplate.Style;
                //get the header
                string _header = mailerTemplate.Header;

                _htmlVersion.Append(_header);

                //text version ignores footer, header and style - but we stick in a line or 2 for better readability
                _textVersion.AppendLine();
                _textVersion.AppendLine();
                _textVersion.AppendLine(this.MailerObject.Subject);
                _textVersion.AppendLine();
                _textVersion.AppendLine();

                MailerContentCollection coll = new MailerContentCollection();
                coll.AddRange(this.MailerObject.MailerContentRecords());
                if (coll.Count > 1)
                    coll.GetList().Sort(new Utils.Reflector.CompareEntities<MailerContent>(Utils.Reflector.Direction.Ascending, "DisplayOrder"));

                foreach (MailerContent mContent in coll)
                {
                    CreateMailerSection(mContent);
                }

                //get the footer - ignore in text?
                string _footer = mailerTemplate.Footer;
                _htmlVersion.Append(_footer);

            }
        }

        private void CreateMailerSection(MailerContent mailerContent)
        {
            if (mailerContent.IsActive)
            {
                //TemplateSectionName _sectionName = (TemplateSectionName)Enum.Parse(typeof(TemplateSectionName), 
                //    mailerContent.MailerTemplateContentRecord.Name, true);

                //init template sections
                string _templateStart = GetTemplatePart("start", mailerContent);
                string _repeatableTemplate = GetTemplatePart("repeat", mailerContent);
                string _templateMiddle = string.Empty;
                string _templateEnd = GetTemplatePart("end", mailerContent);

                //title is only in start template
                if (_templateStart.Trim().Length > 0)
                {
                    _templateStart = ReplaceTitleElement(_templateStart, mailerContent);
                    _templateStart = ReplaceTextElements(_templateStart, mailerContent);
                }

                //content will only reside in start or end templates
                if (_repeatableTemplate.TrimEnd().Length > 0)
                {
                    _templateMiddle = LoopRepeatableContent(_repeatableTemplate, _templateMiddle, mailerContent);
                }

                if (_templateEnd.Trim().Length > 0)
                    _templateEnd = ReplaceContentElement(ReplaceTextElements(_templateEnd, mailerContent), mailerContent);
                else if (_templateStart.Trim().Length > 0)
                    _templateStart = ReplaceContentElement(_templateStart, mailerContent);

                _htmlVersion.Append(_templateStart);
                _htmlVersion.Append(_templateMiddle);
                _htmlVersion.Append(_templateEnd);
            }
        }
        private string ReplaceTextElements(string template, MailerContent mailerContent)
        {
            string templateCopy = String.Copy(template);

            //.find the list of substitutes that begin with <TEXT_
            List<MailerTemplateSubstitution> list = new List<MailerTemplateSubstitution>();
            list.AddRange(mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords()
                .GetList().FindAll(delegate(MailerTemplateSubstitution match) { return (match.TagName.StartsWith(tagTextStart) ); } ));

            if (list.Count > 0)
            {
                foreach (MailerTemplateSubstitution sub in list)
                {
                    if (templateCopy.IndexOf(sub.TagName) != -1)
                    {
                        //place the value into the text version
                        _textVersion.AppendLine(sub.TagValue);
                        _textVersion.AppendLine();

                        //remove the tag from the template - we are done with it
                        templateCopy = templateCopy.Replace(sub.TagName, string.Empty);
                    }
                }
            }

            return templateCopy;
        }

        private string LoopRepeatableContent(string template, string allContent, MailerContent mailerContent)
        {
            int i = 0;

            string separator = mailerContent.MailerTemplateContentRecord.SeparatorTemplate;
            string flags = mailerContent.MailerTemplateContentRecord.VcFlags;
            if (separator != null && separator.Trim().Length == 0)
                separator = null;
            ShowEventCollection showEvents = new ShowEventCollection();
            showEvents.AddRange(mailerContent.ShowEventRecords.GetList().FindAll(delegate(ShowEvent match) { return (match.IsActive); } ));

            //if we have a simple show list
            if (flags != null && flags == SimpleShow.flagString)
            {
                List<SimpleShow> list = SimpleShow.ShowList(mailerContent.VcContent);
                foreach (SimpleShow simp in list)
                {
                    allContent += ReplaceRepeatElement(template, mailerContent, simp);

                    if (i++ < (list.Count - 1)) 
                    {
                        _textVersion.AppendLine();

                        if(separator != null && separator.TrimEnd().Length > 0)
                            allContent += separator;
                    }
                }
            }
            else if (showEvents.Count > 0)
            {
                showEvents.Sort("IOrdinal", true);
                int showCount = showEvents.Count;

                foreach(ShowEvent se in showEvents)
                {
                    allContent += ReplaceRepeatElement(template, mailerContent, se);

                    if (i++ < (showCount - 1))
                    {
                        _textVersion.AppendLine();

                        if (separator != null && separator.TrimEnd().Length > 0)
                            allContent += separator;
                    }
                }
            }

            return allContent;
        }
        
        /// <summary>
        /// We must rely on the elements here - no freebies - OT FOR WAGATAIL OR ANYTHING WITH A SIMPLESHOW
        /// an element will only be replaced if it has a substitution element
        /// </summary>
        /// <param name="template"></param>
        /// <param name="mailerContent"></param>
        /// <returns></returns>
        private string ReplaceRepeatElement(string template, MailerContent mailerContent, object showObject)
        {
            if(showObject == null)
                return string.Empty;

            string templateCopy = String.Copy(template);

            foreach (MailerTemplateSubstitution sub in mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords())
            {
                string tagName = sub.TagName;

                //do not do TITLE and CONTENT in here
                if (tagName != tagTitle && tagName != tagContent)
                {
                    //TODO: make this an input
                    //bool displayDatesAsRange = false;
                    string replacement = string.Empty;
                    string tagValue = sub.TagValue;
                    string showValue = string.Empty;

                    //special case
                    if (tagName == tagImageHeightMax)
                    {
                        //if the image height is greater than 120px - then set to 120px
                        try
                        {
                            int max = int.Parse(tagValue);

                            System.Web.UI.Pair p = Utils.ImageTools.GetDimensions(System.Web.HttpContext.Current.Server.MapPath(((Show)showObject).ImageManager.Thumbnail_Small));
                            //int _imgWidth = (int)p.First;
                            int _imgHeight = (int)p.Second;
                            if (_imgHeight > max)
                                replacement = string.Format(" height=\"{0}\" ", max.ToString());
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        SimpleShow simpleShow = (showObject.GetType().ToString() == "Wcss.SimpleShow") ? (SimpleShow)showObject : null;
                        ShowEvent showEvent = (showObject.GetType().ToString() == "Wcss.ShowEvent") ? (ShowEvent)showObject : null;

                        if (showEvent != null)
                        {
                            //handle dates separately
                            if (tagName == tagDate && tagValue.IndexOf(SimpleShow.flagSeparator) != -1)
                            {
                                //get tagValue pieces
                                string[] pieces = tagValue.Split(SimpleShow.flagSeparator.ToCharArray());
                                string dte = pieces[0];
                                string sta = pieces[1];

                                string configured = showEvent.DateString.Trim();

                                //if there is a status in the string - replace
                                if (showEvent.DateString.IndexOf(MailerContent.tagDateStatusStart) != -1)
                                {
                                    string[] seps = { replacementTag };
                                    string[] statiiParts = sta.Split(seps, StringSplitOptions.RemoveEmptyEntries);
                                    string start = statiiParts[0];
                                    string end = statiiParts[1];

                                    configured = configured.Replace(MailerContent.tagDateStatusStart, start)
                                        .Replace(MailerContent.tagDateStatusEnd, end);
                                }

                                if (configured != null && configured.Trim().Length > 0)
                                {
                                    showValue = configured;//Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(configured, true);
                                    replacement = showValue;// tagValue.Replace(replacementTag, showValue);
                                }
                            }
                            else
                            {
                                showValue = GetShowValueFromTag(tagName, showObject);

                                if (showValue != null && showValue.TrimEnd().Length > 0)
                                {
                                    //remove any formatting 
                                    //showValue = showValue, true);
                                    replacement = tagValue.Replace(replacementTag, showValue);
                                }
                            }
                        }
                        else//it is a simple show
                        {  
                            showValue = GetShowValueFromTag(tagName, showObject);

                            if (showValue != null && showValue.TrimEnd().Length > 0)
                            {
                                //remove any formatting 
                                //showValue = showValue;

                                //if we are the opener tag and are in the upcoming shows section
                                //we need to remove special guest - this is going to bite us sometimes though
                                //on 2 day runs, etc where we have diff acts each day
                                if (tagName == tagOpener && mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.showlinear)
                                    showValue = ShowDisplay.RemoveSpecialGuest(showValue);

                                replacement = tagValue.Replace(replacementTag, showValue);
                            }
                        }

                        //TEXT VERSION
                        if (tagName != tagShowId && tagName != tagShowDisplayUrl && showValue != null && showValue.TrimEnd().Length > 0)
                        {
                            //some sections should not write to new line for every value
                            if (mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.show)
                                _textVersion.AppendLine(FormatForText(showValue));
                            else if (mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.showlinear ||
                                mailerContent.MailerTemplateContentRecord.TemplateAsset == MailerTemplateContent.ContentAsset.simple)
                                _textVersion.Append(string.Format("{0} ", FormatForText(showValue)));
                        }
                    }

                    templateCopy = templateCopy.Replace(tagName, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(replacement,true));
                }
            }

            return templateCopy;
        }
        
        private string FormatForText(string format)
        {
            return Utils.ParseHelper.StripHtmlTags(format).Replace(@"\r\n", Environment.NewLine).Replace(@"\t", string.Empty)
                .Replace("&#39;", "'").Replace("&amp;", "&").Replace("&ndash;", "-").Replace("&nbsp;", "-").Trim();
        }
        private string ReplaceContentElement(string template, MailerContent mailerContent)
        {
            string copy = String.Copy(template);

            string _content = (mailerContent.VcContent != null && mailerContent.VcContent.Trim().Length > 0) ? mailerContent.VcContent.Trim() : string.Empty;
            string flags = mailerContent.MailerTemplateContentRecord.VcFlags;

            if(flags == null || flags.Trim().Length == 0)
            {
                _textVersion.AppendLine(FormatForText(_content));
                _textVersion.AppendLine();
            }

            MailerTemplateSubstitution subContent = mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords().GetList()
                .Find(delegate(MailerTemplateSubstitution match) { return (match.TagName == tagContent); });

            //if no tag then dont replace
            //content should be encoded already
            if (subContent != null && subContent.TagValue.Trim().Length > 0)
            {   
                string tagValue = subContent.TagValue.Trim();
                string replacement = tagValue.Replace(replacementTag, _content);
                _content = replacement;
            }

            return copy.Replace(tagContent, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(_content));
        }
        private string ReplaceTitleElement(string template, MailerContent mailerContent)
        {
            string copy = String.Copy(template);

            string _title = (mailerContent.Title != null && mailerContent.Title.Trim().Length > 0) ? mailerContent.Title.Trim() : string.Empty;
            _textVersion.AppendLine(FormatForText(_title));
            _textVersion.AppendLine();

            MailerTemplateSubstitution subTitle = mailerContent.MailerTemplateContentRecord.MailerTemplateSubstitutionRecords().GetList()
                .Find(delegate(MailerTemplateSubstitution match) { return (match.TagName == tagTitle); });

            //if no tag then dont replace
            if (subTitle != null && subTitle.TagValue.Trim().Length > 0)
            {   
                string tagValue = subTitle.TagValue.Trim();
                string replacement = tagValue.Replace(replacementTag, System.Web.HttpUtility.HtmlEncode(_title));
                _title = replacement;
            }

            return copy.Replace(tagTitle, Utils.ParseHelper.EscCommonSequencesInHtml_AvoidInnerHtml(_title));
        
        }
        private string GetTemplatePart(string context, MailerContent mailerContent)
        {
            string template = mailerContent.MailerTemplateContentRecord.Template;
            int rptStart = template.IndexOf(tagRepeat_Start);
            string[] seps = {tagRepeat_Start, tagRepeat_End};

            switch (context.ToLower())
            {
                case "start":
                    if (rptStart > -1)
                    {
                        string[] parts = template.Split(seps, StringSplitOptions.None); 
                        return parts[0];
                    }
                    return template;
                    break;
                case "repeat":
                    if (rptStart > -1)
                    {
                        string[] parts = template.Split(seps, StringSplitOptions.None); 
                        return parts[1];
                    }
                    break;
                case "end":
                    if (rptStart > -1)
                    {
                        string[] parts = template.Split(seps, StringSplitOptions.None); 
                        return parts[2];
                    }
                    break;
            }

            return string.Empty;
        }
        private string GetShowValueFromTag(string tagName, object showObject)
        {
            Show show = (showObject.GetType().ToString() == "Wcss.Show") ? (Show)showObject : null;
            SimpleShow simple = (showObject.GetType().ToString() == "Wcss.SimpleShow") ? (SimpleShow)showObject : null;
            ShowEvent showEvent = (showObject.GetType().ToString() == "Wcss.ShowEvent") ? (ShowEvent)showObject : null;

            bool useShow = show != null;
            bool useShowEvent = showEvent != null;

            string showValue = string.Empty;

            if(showObject != null)
            {
            //ignore title and content tags here
                switch (tagName)
                {
                    case tagAges:
                        showValue = (useShow) ? show.Display.Mailer_Ages_NoMarkup.Trim() : (useShowEvent) ? showEvent.Ages : null;
                        break;
                    case tagDate:
                        showValue = (useShow) ? show.Display.Date_Markup_3Day_NoTime_ListAll.Trim() : (useShowEvent) ? showEvent.DateString : simple.DATE;
                        break;
                    case tagHeader:
                        showValue = (useShow) ? show.TopText_Derived.Trim() : (useShowEvent) ? showEvent.Header : null;
                        break;
                    case tagHeadliner:
                        showValue = (useShow) ? show.Display.Headliners_NoMarkup_Verbose_NoLinks : (useShowEvent) ? showEvent.Headliner : simple.HEADLINER;
                        break;
                    case tagOpener:
                        showValue = (useShow) ? show.Display.Openers_NoMarkup_Verbose_NoLinks.Trim() : (useShowEvent) ? showEvent.Opener : simple.OPENER;
                        break;
                    case tagPromoter:
                        showValue = (useShow) ? show.Display.Promoters_NoMarkup_NoLinks : (useShowEvent) ? showEvent.Promoter : null;
                        break;
                    case tagShowDisplayUrl:
                        showValue = (useShow && show.ImageManager != null) ? show.ImageManager.Thumbnail_Small : (useShowEvent) ? showEvent.ImageUrl : null;
                        if (showValue != null)
                            showValue = showValue.Insert(0, string.Format("http://{0}{1}", _Config._DomainName, (showValue.StartsWith("/")) ? string.Empty : "/"));
                        break;
                    case tagShowId:
                        showValue = (useShow) ? show.Id.ToString() : 
                            (useShowEvent && showEvent.ParentId > 0 && showEvent.ParentType == ShowEvent.ParentTypes.Show) ? showEvent.TParentId.ToString() : null;
                        break;
                    case tagShowTitle:
                        showValue = (useShow) ? (show.ShowTitle != null && show.ShowTitle.Trim().Length > 0) ? show.ShowTitle.Trim() : null : null;
                        break;
                    case tagStatus:
                        showValue = (useShow) ? show.StatusText : (useShowEvent) ? showEvent.Status : simple.STATUS;
                        break;
                    case tagTimes:
                        showValue = (useShow) ? show.Display.Times_NoMarkup_ShowTimeOnly : (useShowEvent) ? showEvent.Times : null;
                        break;
                    case tagPricing:
                        showValue = (useShow) ? show.Display.Mailer_Pricing_NoMarkup : (useShowEvent) ? showEvent.Pricing : null;
                        break;
                    case tagVenue:
                        showValue = (useShow) ? show.Display.Venue_NoMarkup_NoLinks_NoAddress_NoLeadIn : (useShowEvent) ? showEvent.Venue : simple.VENUE;
                        break;
                }
            }

            return showValue;
        }
    }
}

 #region OLD SHOW
                            ////TODO: make this work for all two-part tagValues
                            //if ((sectionName == "highlight" || sectionName == "mainlisting" || sectionName == "secondarylisting") && 
                            //    show != null && tagName == tagDate && tagValue.IndexOf(SimpleShow.flagSeparator) != -1)
                            //{
                            //    #region
                            //    //get tagValue pieces
                            //    string[] pieces = tagValue.Split(SimpleShow.flagSeparator.ToCharArray());
                            //    string dte = pieces[0];
                            //    string sta = pieces[1];

                            //    //get date pieces
                            //    //then when constructing string - add in replacements
                            //    ShowDateCollection coll = new ShowDateCollection();
                            //    coll.AddRange(show.ShowDateRecords());

                            //    if (coll.Count > 0)
                            //    {
                            //        coll.Sort("DtDateOfShow", true);

                            //        List<Pair> dateList = new List<Pair>();

                            //        //construct pairs
                            //        for (int i = 0; i < coll.Count; i++)
                            //        {
                            //            ShowDate sd = coll[i];
                            //            string dateOfShow = sd.DateOfShow.ToString("MM/dd");
                            //            string status = (sd.ShowStatusRecord != null) ? (sd.ShowStatusRecord.Name.ToLower() != "onsale") ? sd.ShowStatusRecord.Name : string.Empty : string.Empty;
                            //            if (status.ToLower() == "soldout")
                            //                status = "Sold Out!";

                            //            //if we are the first date - throw it in
                            //            if (i == 0)
                            //                dateList.Add(new Pair(dateOfShow, status));
                            //            else
                            //            {
                            //                //if same status as the last entry - then add date to list
                            //                string currentStatus = dateList[dateList.Count - 1].Second.ToString();
                            //                if (status == currentStatus)
                            //                {
                            //                    //if the date is not already in there....
                            //                    string residing = dateList[dateList.Count - 1].First.ToString();
                            //                    if (residing.IndexOf(dateOfShow) == -1)
                            //                        dateList[dateList.Count - 1].First += string.Format(", {0}", dateOfShow);
                            //                }
                            //                else
                            //                    dateList.Add(new Pair(dateOfShow, status));
                            //            }
                            //        }

                            //        //if we are in rangemode and there is one entry (mult status not allowed)
                            //        //we will only be working with one listing here
                            //        string textline = string.Empty;
                            //        string range = string.Empty;

                            //        if (displayDatesAsRange && dateList.Count == 1)
                            //        {
                            //            string[] dateArray = dateList[0].First.ToString().Split(',');
                            //            if (dateArray.Length > 1)
                            //            {
                            //                string firstDate = dateArray[0];
                            //                string lastDate = dateArray[dateArray.Length - 1];

                            //                range = string.Format("{0} - {1}", firstDate.Trim(), lastDate.Trim());
                            //            }
                            //            else
                            //                range = dateArray[0].Replace(",", string.Empty);

                            //            string showStatus = dateList[0].Second.ToString();

                            //            //text version
                            //            textline = string.Format("{0} {1}", range, showStatus).Trim();

                            //            //do date and status replacement dte, sta
                            //            showValue = string.Format("{0}{1}", dte.Replace(replacementTag, range),
                            //                (showStatus.Trim().Length > 0) ? sta.Replace(replacementTag, showStatus) : string.Empty).Trim();
                            //        }
                            //        else
                            //        {
                            //            for (int j = 0; j < dateList.Count; j++)
                            //            {
                            //                range = dateList[j].First.ToString();
                            //                string showStatus = dateList[j].Second.ToString();

                            //                textline += string.Format("{0} {1}, ", range, showStatus).Trim();

                            //                //do date and status replacement dte, sta
                            //                showValue += string.Format("{0}{1}, ", dte.Replace(replacementTag, range),
                            //                    (showStatus.Trim().Length > 0) ? sta.Replace(replacementTag, showStatus) : string.Empty).Trim();
                            //            }

                            //            //remove last spaces and commas
                            //            //replace the last comma with an ampersand
                            //            char[] trimChars = { ' ', ',' };

                            //            textline = textline.TrimEnd(trimChars);
                            //            int lastComma = textline.LastIndexOf(',');
                            //            if (lastComma != -1)
                            //                textline = textline.Remove(lastComma, 1).Insert(lastComma, " &");

                            //            showValue = showValue.TrimEnd(trimChars);
                            //            lastComma = showValue.LastIndexOf(',');
                            //            if (lastComma != -1)
                            //                showValue = showValue.Remove(lastComma, 1).Insert(lastComma, " &amp;");
                            //        }

                            //        if (sectionName == "highlight" || sectionName == "mainlisting")
                            //            _textVersion.AppendLine(FormatForText(textline));
                            //        else if (sectionName == "secondarylisting" || sectionName.IndexOf("custom") != -1)
                            //        {
                            //            if (mailerContent.ShowEventRecords.Count == 0)
                            //                _textVersion.Append(string.Format("{0} ", FormatForText(textline)));
                            //        }

                            //        replacement = showValue;
                            //    }

                            //    //no replacement if no dates
                            //    #endregion
                            //}
                            //else
                            //{
                            #endregion