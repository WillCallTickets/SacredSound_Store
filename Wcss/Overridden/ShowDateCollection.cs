using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wcss
{
    public partial class ShowDateCollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ShowDate GetDisplayableShowDate()
        {
            if (this.Count == 0)
                return null;

            //get shows that pertain to the first day in the collection
            //here we are getting the first date to display
            //if we have shows for today then do those shows
            //else if the collections first date does not occur until after - use the first date in the collection
            DateTime date = _Config.SHOWOFFSETDATE;

            ShowDateCollection coll = new ShowDateCollection();
            coll.AddRange(this.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive && match.ShowRecord.IsDisplayable && match.DateOfShow.Date == date); }));

            //if no results - then we go for the next date
            if (coll.Count == 0)
            {
                coll.AddRange(this.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive && match.ShowRecord.IsDisplayable && match.DateOfShow.Date > date); }));

                if (coll.Count == 0)
                    return null;

                coll.Sort("DtDateOfShow", true);
                date = coll[0].DateOfShow.Date;
                coll.Clear();

                coll.AddRange(this.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive && match.ShowRecord.IsDisplayable && match.DateOfShow.Date == date); }));
            }

            if (coll.Count == 0)
                return null;
            else if (coll.Count == 1)
                return coll[0];
            else if (coll.Count > 1)
            {
                //now we compare by datetosortby
                DateTime now = DateTime.Now;

                coll.SortBy_DateToOrderBy();

                //so if the first date has passed determine the next show to display
                for (int i = 0; i < coll.Count; i++)
                {
                    //return the last show of the day - avoid index errors
                    if(i == (coll.Count - 1))
                        return coll[i];

                    DateTime thisShow = coll[i].DateOfShow_ToSortBy;

                    //if now is less than this show - return this show
                    if (now <= thisShow)
                        return coll[i];

                    DateTime nextShow = coll[i+1].DateOfShow_ToSortBy;
                    if (now < nextShow)
                    {
                        //if now is closer to this show than the next show...
                        //then return this show
                        TimeSpan timeFromLastShow = now.TimeOfDay - thisShow.TimeOfDay;
                        TimeSpan timeUntilNextShow = nextShow.TimeOfDay - now.TimeOfDay;

                        if (timeFromLastShow < timeUntilNextShow)
                            return coll[i];
                        else
                            return coll[i + 1];
                    }
                }
            }

            return null;

            /*obsolete 5/16/2010

            ShowDateCollection showDateColl = new ShowDateCollection();
            showDateColl.AddRange(this.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive && match.ShowRecord.IsDisplayable); }));

            if (showDateColl.Count == 0)
                return null;

            //if (showDateColl.Count > 1)
            //    showDateColl.Sort("DtDateOfShow", true);

            if (showDateColl.Count > 1)
                showDateColl.Sort("DateOfShow_ToSortBy", true);

            //add 24 hours to date - with offset time as well
            //this will give us all shows that are on that same day - and late night too
            DateTime dateInQuestion = showDateColl[0].DateOfShow.Date.AddHours(24 + _Config.DayTurnoverTime);

            ShowDateCollection firstShowDates = new ShowDateCollection();

            if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Venue)
                firstShowDates.AddRange(showDateColl.GetList().FindAll(delegate(ShowDate match)
                {
                    return (match.DtDateOfShow <= dateInQuestion && 
                    match.ShowRecord.VenueRecord.Name.ToLower() == _Config._Default_VenueName.ToLower()); } ));

            if (firstShowDates.Count == 0)
                firstShowDates.AddRange(showDateColl.GetList().FindAll(delegate(ShowDate match) { return (match.DtDateOfShow.Date <= dateInQuestion); }));

            if (firstShowDates.Count == 0)
                return null;

            if (firstShowDates.Count > 1)
                firstShowDates.Sort("DtDateOfShow", true);

            ShowDate firstDate = firstShowDates[0];

            if (firstShowDates.Count > 1)
            {
                //make a new collection with just the matching first day - allow for late night show in this collection
                ShowDateCollection sameDay = new ShowDateCollection();
                sameDay.AddRange(firstShowDates.GetList().FindAll(delegate(ShowDate match)
                {
                    //active and show displayable have already been tested
                    return (match.Id != firstDate.Id && match.DateOfShow.Date == firstDate.DateOfShow.AddHours(-_Config.DayTurnoverTime).Date);
                }));

                if (sameDay.Count > 0)
                {
                    int count = sameDay.Count;

                    //if we have two shows - then it is an easy match
                    if (sameDay.Count > 1)
                        sameDay.Sort("DtDateOfShow", true);
                    DateTime nowDate = DateTime.Now;

                    //note that first date has been set already
                    ShowDate nextDate = null;//sameDay[0];

                    for (int i = 0; i < count; i++)
                    {
                        ShowDate current = sameDay[i];
                        if (current.DtDateOfShow > nowDate)//then we have our range
                        {
                            nextDate = sameDay[i];
                            break;
                        }
                        firstDate = sameDay[i];
                    }

                    if (nextDate != null)
                    {
                        //get the time span between the two - first show's show time and the second's doors
                        TimeSpan diff = nextDate.DtDateOfShow.TimeOfDay - firstDate.DtDateOfShow.TimeOfDay;
                        
                        //split time difference span in half
                        DateTime dateToCompare = DateTime.Parse(string.Format("{0} {1}", firstDate.DtDateOfShow.ToString("MM/dd/yyyy"), firstDate.ShowTime))
                            .Add(TimeSpan.FromTicks(diff.Ticks / 2));

                        //if the nowTime is greater than go with later show, etc
                        if (nowDate > dateToCompare)
                            firstDate = nextDate;
                    }
                    //otherwise - firstdate is already selected, and that is what we return
                }
            }

            return firstDate;
             * 
             * */
        }
    }
}
