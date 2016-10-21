using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Wcss;

/// <summary>
/// Summary description for Statics
/// </summary>
public class Statics
{
    public Statics()
    {   
    }
    
    ////Collection determines the activeness
    ///// <summary>
    ///// This helps us determine what is displayable in the moment - determines validity of announcedate. Note
    ///// that this is for show and showDates - tickets need to be checked within context of mp key
    ///// </summary>
    ///// <param name="coll"></param>
    ///// <returns></returns>
    //public static ShowDateCollection OrderedDisplayable_ShowDates(ShowDateCollection coll)
    //{
    //    ShowDateCollection returnColl = new ShowDateCollection();

    //    if(coll == null)
    //    {
    //    }

    //    //get default venue
    //    //order by venue, then date
    //    List<ShowDate> list = new List<ShowDate>();
    //    list.AddRange(coll.GetList().FindAll(delegate(ShowDate match) { return (match.IsActive && match.ShowRecord.IsDisplayable); }));

    //    if (_Config._Site_Entity_Mode == _Enums.SiteEntityMode.Venue)
    //    {
    //        var sortedColl =
    //            from listItem in list
    //            select listItem;

    //        returnColl.AddRange(sortedColl.OrderBy(x => x.DateOfShow)
    //            .ThenBy(x => (x.ShowRecord.VenueRecord.Name_Displayable.ToLower() == _Config._Default_VenueName.ToLower()) ? string.Empty : x.ShowRecord.VenueRecord.NameRoot));
    //    }
    //    else
    //    {
    //        var sortedColl =
    //            from listItem in list
    //            orderby listItem.ShowRecord.VenueRecord.Name_Displayable, listItem.DateOfShow
    //            select listItem;

    //        returnColl.AddRange(sortedColl);
    //    }

    //    return returnColl;
    //}
}
