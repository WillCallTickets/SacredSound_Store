using System;
using System.Text;
using System.Xml.Serialization;

namespace Wcss
{
    public partial class ShowDateDisplay
    {
        private ShowDate _entity = null;
        private ShowDate Entity { get { return _entity; } }

        public ShowDateDisplay(ShowDate entity)
        {
            _entity = entity;
        }

        #region Wrapped Strings

        private string _listing = null;
        public string Listing
        {
            get
            {
                if (_listing == null)
                {
                    string title = string.Format("{0} {1}", Entity.Display.Date_NoMarkup_StatusNotFirstNoMarkup_NoTime,
                        Entity.ShowRecord.ShowNamePart);

                    _listing = title;
                }

                return _listing;
            }
        }

        private string _billing = null;
        /// <summary>
        /// decides whether or not to use the billing text - formats a name for the show to display
        /// </summary>
        public string Billing
        {
            get
            {
                //if(_billing == null || _billing.Trim().Length == 0)
                if (_billing == null)
                    _billing = string.Format("{0} {1}", this.Date_NoMarkup_StatusNotFirstNoMarkup_NoTime,
                        (Entity.Billing != null && Entity.Billing.Trim().Length > 0) ?
                        Entity.Billing.Trim().ToUpper() : Entity.Display.Heads_NoFeatures);

                return _billing;
            }
        }

        private string _date_Markup_NoStatus_NoTime = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string Date_Markup_NoStatus_NoTime
        {
            get
            {
                if (_date_Markup_NoStatus_NoTime == null)
                {
                    _date_Markup_NoStatus_NoTime = this.fmDateFormatted(true, false, true, true, 3, false);
                }

                return _date_Markup_NoStatus_NoTime;
            }
        }
        private string _date_NoMarkup_StatusNotFirstNoMarkup_NoTime = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// fmDateFormatted(false, true, false, false, 0, false)
        /// </summary>
        public string Date_NoMarkup_StatusNotFirstNoMarkup_NoTime
        {
            get
            {
                if (_date_NoMarkup_StatusNotFirstNoMarkup_NoTime == null)
                {
                    _date_NoMarkup_StatusNotFirstNoMarkup_NoTime = this.fmDateFormatted(false, true, false, false, 0, false);
                }

                return _date_NoMarkup_StatusNotFirstNoMarkup_NoTime;
            }
        }

        private string _date_AllOptions = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string Date_AllOptions
        {
            get
            {
                if (_date_AllOptions == null)
                {
                    _date_AllOptions = this.fmDateFormatted(true, true, true, true, 3, true);
                }

                return _date_AllOptions;
            }
        }
        private string _heads_NoMarkup_NoLinks_Brief = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string Heads_NoMarkup_NoLinks_Brief
        {
            get
            {
                if (_heads_NoMarkup_NoLinks_Brief == null)
                {
                    _heads_NoMarkup_NoLinks_Brief = this.fmHeadliners(false, false, false);
                }

                return _heads_NoMarkup_NoLinks_Brief;
            }
        }
        private string _allActs_Markup_NoVerbose_NoLinks = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string AllActs_Markup_NoVerbose_NoLinks
        {
            get
            {
                if (_allActs_Markup_NoVerbose_NoLinks == null)
                {
                    _allActs_Markup_NoVerbose_NoLinks = this.fmAllActs(true, false, false);
                }

                return _allActs_Markup_NoVerbose_NoLinks;
            }
        }
        private string _ages_True = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string Ages_True
        {
            get
            {
                if (_ages_True == null)
                {
                    _ages_True = this.fmAges(true);
                }

                return _ages_True;
            }
        }
        private string _heads_NoFeatures = null;
        private string _heads_NoFeatures_Verbose = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string Heads_NoFeatures
        {
            get
            {
                if (_heads_NoFeatures == null)
                {
                    _heads_NoFeatures = this.fmHeadliners(false, false, false);
                }

                return _heads_NoFeatures;
            }
        }
        public string Heads_NoFeatures_Verbose
        {
            get
            {
                if (_heads_NoFeatures_Verbose == null)
                {
                    _heads_NoFeatures_Verbose = this.fmHeadliners(false, false, true);
                }

                return _heads_NoFeatures_Verbose;
            }
        }
        private string _openes_NoFeatures = null;
        private string _openes_NoFeatures_Verbose = null;
        /// <summary>
        /// Displays the date formatted for display next event control
        /// </summary>
        public string Openers_NoFeatures
        {
            get
            {
                if (_openes_NoFeatures == null)
                {
                    _openes_NoFeatures = this.fmOpeners(false, false, false);
                }

                return _openes_NoFeatures;
            }
        }
        public string Openers_NoFeatures_Verbose
        {
            get
            {
                if (_openes_NoFeatures_Verbose == null)
                {
                    _openes_NoFeatures_Verbose = this.fmOpeners(false, false, true);
                }

                return _openes_NoFeatures_Verbose;
            }
        }

        #endregion

        #region acts
        /// <summary>
        /// needs to be internal as showdisplay class needs to call this dynamically
        /// </summary>
        /// <param name="includeMarkup"></param>
        /// <param name="includeLinks"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        internal string fmHeadliners(bool includeMarkup, bool includeLinks, bool verbose)
        {
            return fmActs(includeMarkup, includeLinks, verbose, true);
        }
        /// <summary>
        /// needs to be internal as showdisplay class needs to call this dynamically
        /// </summary>
        /// <param name="includeMarkup"></param>
        /// <param name="includeLinks"></param>
        /// <param name="verbose"></param>
        /// <returns></returns>
        internal string fmOpeners(bool includeMarkup, bool includeLinks, bool verbose)
        {
            return fmActs(includeMarkup, includeLinks, verbose, false);
        }
        private string fmActs(bool includeMarkup, bool includeLinks, bool verbose, bool headliners)
        {
            StringBuilder fm = new StringBuilder();

            bool blankTarget = true;

            JShowActCollection coll = new JShowActCollection();

            coll.AddRange(Entity.JShowActRecords().GetList()
                .FindAll(delegate(JShowAct match) { return ((headliners) ? match.TopBilling_Effective : (!match.TopBilling_Effective)); }));

            if (coll.Count > 0)
            {
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                if (includeMarkup)
                    fm.AppendFormat("<span class=\"{0}\">", (headliners) ? "head" : "open");

                foreach (JShowAct j in coll)
                {
                    if (verbose && j.PreText != null && j.PreText.Trim().Length > 0)
                    {
                        if (includeMarkup)
                            fm.AppendFormat("<span class=\"pretext\">{0}</span> ", j.PreText.Trim());
                        else
                            fm.AppendFormat("{0} ", (j.PreText).Trim());
                    }

                    bool includeLink = (includeLinks && j.ActRecord.Website != null && j.ActRecord.Website.Trim().Length > 0);
                    if (includeLink)
                        fm.AppendFormat("<a href=\"{0}\"{1}>", j.ActRecord.Website_Configured, (blankTarget) ? string.Format(" target=\"_blank\"") : string.Empty);

                    if (includeMarkup)
                        fm.Append("<span class=\"name\">");

                    fm.AppendFormat("{0}", (j.ActRecord.Name_Displayable));

                    if (j.ActText != null && j.ActText.Trim().Length > 0)
                        fm.AppendFormat(" {0}", (j.ActText).Trim());

                    if (includeMarkup)
                        fm.Append("</span>");

                    if (includeLink)
                        fm.Append("</a>");

                    if (verbose)
                    {
                        bool features = (j.Featuring != null && j.Featuring.Trim().Length > 0);
                        if (features)
                        {
                            if (includeMarkup)
                                fm.AppendFormat(" <span class=\"featuring\">{0}</span>", (j.Featuring).Trim());
                            else
                                fm.AppendFormat(" {0}", (j.Featuring).Trim());
                        }
                    }

                    if (j.PostText != null && j.PostText.Trim().Length > 0)
                    {
                        if (includeMarkup)
                            fm.AppendFormat(" <span class=\"posttext\">{0}</span>", (j.PostText).Trim());
                        else
                            fm.AppendFormat(" {0}", (j.PostText).Trim());
                    }

                    fm.Append("~");
                }

                if (this.Entity.IsAutoBilling)
                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(fm);
                else
                    fm.Replace("~", " ");

                if (includeMarkup)
                    fm.Append("</span>");
            }

            return System.Text.RegularExpressions.Regex.Replace(fm.ToString(),@"\s+", " ");
        }
        #endregion

        #region Ages
        internal string fmAges(bool includeMarkup)
        {
            if (includeMarkup)
                return string.Format("<span class=\"ages\">{0}</span>", (Entity.AgesString));
            else
                return (Entity.AgesString);
        }
        #endregion

        #region Dates

        private string fmDateFormatted(bool includeMarkup)
        {
            return fmDateFormatted(includeMarkup, false, false, true, 3, true);
        }
        private string fmDateFormatted(bool includeMarkup, bool includeStatus, bool statusFirst, bool includeStatusMarkup, int dayLength, bool includeTime)
        {
            StringBuilder fm = new StringBuilder();
            DateTime eDate = Entity.DateOfShow;

            if (includeMarkup)
                fm.AppendFormat("<span class=\"datesection\">");

            //attach a status if status is called for and if different than on sale
            if (includeStatus && statusFirst && Entity.StatusName.Length > 0 && Entity.StatusName != _Enums.ShowDateStatus.OnSale.ToString())
            {
                //always include markup for status
                if (includeStatusMarkup)
                    fm.AppendFormat("<span class=\"datestatus\">");

                string status = Entity.StatusName;
                if (status == _Enums.ShowDateStatus.SoldOut.ToString())
                    status = "Sold Out";

                fm.AppendFormat(" {0}! ", (status));

                if (includeStatusMarkup)
                    fm.AppendFormat("</span>");
            }

            //if dayLength is zero - dont show the day name
            string date = string.Format("{0}{1}",
                (dayLength > 0) ? string.Format("{0} ", eDate.DayOfWeek.ToString().Substring(0, dayLength)) : string.Empty,
                (includeTime) ? eDate.ToString("MM/dd hh:mmtt") : eDate.ToString("MM/dd"));

            if (includeMarkup)
                fm.AppendFormat("<span class=\"date\">{0}</span>", date);
            else
                fm.Append(date);

            //attach a status if status is called for and if different than on sale
            if (includeStatus && (!statusFirst) && Entity.StatusName.Length > 0 && Entity.StatusName != _Enums.ShowDateStatus.OnSale.ToString())
            {
                //always include markup for status
                if (includeStatusMarkup)
                    fm.AppendFormat("<span class=\"datestatus\">");

                string status = Entity.StatusName;
                if (status == _Enums.ShowDateStatus.SoldOut.ToString())
                    status = "Sold Out";

                fm.AppendFormat(" {0}! ", (status));

                if (includeStatusMarkup)
                    fm.AppendFormat("</span>");
            }

            if (includeMarkup)//end the date section
                fm.AppendFormat("</span>");

            return fm.ToString();
        }
        #endregion

        #region Display All

        private string fmAll(bool includeMarkup, bool verboseHeads, bool includeLinksHeads, bool verboseOpens, bool includeLinksOpens, int dayLength, bool includeTime)
        {
            //only allow the call to a date format with status from a direct call?
            return string.Format("{0} {1} {2}", this.fmDateFormatted(false, true, includeMarkup, true, dayLength, includeTime), this.fmAges(includeMarkup),
                fmAllActs(includeMarkup, verboseHeads, includeLinksHeads, verboseOpens, includeLinksOpens));
        }
        private string fmAllActs(bool includeMarkup, bool verbose, bool includeLinks)
        {
            return fmAllActs(includeMarkup, verbose, includeLinks, verbose, includeLinks);
        }
        private string fmAllActs(bool includeMarkup, bool verboseHeads, bool includeLinksHeads, bool verboseOpens, bool includeLinksOpens)
        {
            string openers = this.fmOpeners(includeMarkup, verboseOpens, includeLinksOpens);
            return string.Format("{0}{1}{2}{3}", (includeMarkup) ? "<div class=\"actlist\">" : string.Empty,
                this.fmHeadliners(includeMarkup, verboseHeads, includeLinksHeads),
                (openers.Trim().Length > 0) ? string.Format("{0}{1}", (this.Entity.IsAutoBilling) ? " with " : " ", openers) : string.Empty,
                (includeMarkup) ? "</div>" : string.Empty);
        }
        #endregion
    }
}
