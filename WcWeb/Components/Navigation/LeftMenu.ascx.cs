using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

using Wcss;

//<%@OutputCache Duration="20" VaryByParam="none" %> 

namespace WillCallWeb.Components.Navigation
{
    public partial class LeftMenu : WillCallWeb.BaseControl
    {
        private bool isAdmin = false;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.Now;
        private string selectedMerchCategory = string.Empty;
        ShowDateCollection dates = new ShowDateCollection();

        protected void Page_Load(object sender, EventArgs e) {}

        protected void InitMenuItems()
        {
            isAdmin = this.Page.MasterPageFile.ToLower().IndexOf("Admin") != -1;
            if (Globals.MerCat != null)
                selectedMerchCategory = Globals.MerCat.Name;

            dates = Ctx.OrderedDisplayable_ShowDates;
            if (dates.Count > 0)
            {
                startDate = dates[0].DateOfShow;
                endDate = dates[dates.Count - 1].DateOfShow;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if(Trace.IsEnabled)
                Trace.Write("left menu start");

            InitMenuItems();

            writer.WriteLine();
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "MenuSection");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //ticket section
            if (_Config._Sales_Tickets_Active && dates.Count > 0)
            {
                RenderMenu(writer, "tickets");
            }
            
            //merch section
            if (_Config._Sales_Merch_Active)
            {   
                //draw merch elements
                RenderMenu(writer, "merch");
            }
            
            writer.RenderEndTag();

            if(Trace.IsEnabled)
                Trace.Write("left menu end");
        }

        protected void RenderMenu(HtmlTextWriter writer, string context)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "amenu");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, context);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
        
            if(context == "tickets")
            {
                writer.WriteLine(string.Format("<h4><a href=\"/Store/ChooseTicket.aspx\">{0}</a></h4>", _Config._CartTitle_Tickets));

                RenderTicketMenu(writer);
            }
            else
            {
                writer.WriteLine(string.Format("<h4><a href=\"/Store/ChooseMerch.aspx\">{0}</a></h4>", _Config._CartTitle_Merch));

                RenderMerchMenu(writer);
            }
       
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected void RenderTicketMenu(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "MenuTickets");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            string displayMethod = _Config._EventMenu_ListingType.ToLower();

            if (displayMethod == "show" || displayMethod == "shows")
            {
                ShowCollection coll = new ShowCollection();
                foreach (ShowDate sd in dates)
                {
                    int exists = coll.GetList().FindIndex(delegate(Show match) { return match.Id == sd.ShowRecord.Id; });
                    if(exists == -1 && sd.ShowRecord.AnnounceDate < DateTime.Now)//the show must be announced!
                        coll.Add(sd.ShowRecord);
                }        

                //sort by showname
                coll.Sort("Name", true);

                foreach (Show s in coll)
                {
                    string billing = (s.ActBilling != null && s.ActBilling.Trim().Length > 0) ?
                        s.ActBilling.Trim().ToUpper() : s.Display.AllActs_NoMarkup_NoVerbose_NoLinks;

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "sandwich");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseTicket.aspx?sid={0}", s.Id.ToString()));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "datelisting");
                    writer.RenderBeginTag("div");
                    writer.Write(s.Display.ShowDateListing_Simple);
                    writer.RenderEndTag();

                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "infolisting");
                    writer.RenderBeginTag("div");
                    writer.WriteEncodedText(billing);
                    writer.RenderEndTag();//end infolisting

                    writer.RenderEndTag();//a
                    writer.RenderEndTag();//li
                }
            }
            else if (displayMethod == "showdate" || displayMethod == "showdates" || displayMethod == "date" || displayMethod == "dates")
            {
                foreach (ShowDate sd in dates)
                {
                    if (sd.ShowRecord.AnnounceDate < DateTime.Now)//the show must be announced!
                    {
                        string billing = (sd.Billing != null && sd.Billing.Trim().Length > 0) ?
                            sd.Billing.Trim().ToUpper() : null;
                        string title = string.Format("{0} {1}", sd.Display.Date_NoMarkup_StatusNotFirstNoMarkup_NoTime,
                            (billing != null) ? billing : sd.Display.Heads_NoFeatures);

                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseTicket.aspx?sid={0}", sd.TShowId));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);

                        string status = sd.StatusName;
                        if (status != _Enums.ShowDateStatus.OnSale.ToString())
                        {
                            if (status == _Enums.ShowDateStatus.SoldOut.ToString())
                                status = "Sold Out";

                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "datestatus");
                            writer.RenderBeginTag("div");
                            writer.Write(status);
                            writer.RenderEndTag();
                        }

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "datelisting");
                        writer.RenderBeginTag("span");
                        writer.Write(sd.DateOfShow.ToString("MM/dd"));
                        writer.RenderEndTag();//.RenderEndTag("span");

                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "infolisting");
                        writer.RenderBeginTag("span");

                        writer.WriteEncodedText((billing != null) ? billing : sd.Display.Heads_NoFeatures);


                        //FEATURE GOFAST
                        //if (status == _Enums.ShowDateStatus.OnSale.ToString() && (!sd.ShowRecord.IsSoldOut))
                        //{
                        //    //if we are using this feature - we have a threshhold and text
                        //    if (_Config._Inventory_Tickets_GoingFast_Threshhold > 0 && _Config._Inventory_Tickets_GoingFast_Text.Trim().Length > 0)
                        //    {
                        //        int available = 10000;
                        //        //get the relevant inventory
                        //        //if not found then ignore 
                        //        if (Ctx.ShowDateInventory.TryGetValue(sd.Id, out available))
                        //        {
                        //            //dont encourage when available is zero!!!
                        //            if (available > 0 && available < _Config._Inventory_Tickets_GoingFast_Threshhold)
                        //            {
                        //                writer.AddAttribute(HtmlTextWriterAttribute.Class, "tktgofast");
                        //                writer.RenderBeginTag("span");
                        //                writer.Write(_Config._Inventory_Tickets_GoingFast_Text.Trim());
                        //                writer.RenderEndTag();
                        //            }
                        //        }
                        //    }
                        //}

                        writer.RenderEndTag();//end infolisting

                        writer.RenderEndTag();//a
                        writer.RenderEndTag();//li
                    }
                }
            }
            else //if we will be listing by month....
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseTicket.aspx?mo={0}",
                    Utils.Constants._MinDate.ToShortDateString()));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.WriteLine("List All Shows");
                writer.RenderEndTag();//a
                writer.RenderEndTag();//li

                System.Collections.ArrayList list = GetMonthList(startDate, endDate);

                int count = list.Count;

				for(int i=0;i<count;i++)
				{
					ListItem li = (ListItem)list[i];
                    int idx = 0;

                    //note the backwards logic here due to the wording of WITHOUT
                    if (! _Config._DisplayMenu_MonthsWithoutShows)
                    {
                        //only include months with shows in that month
                        //value 2010_06_1
                        string[] parts = li.Value.Split('_');
                        int yr = int.Parse(parts[0]);
                        int mo = int.Parse(parts[1]);
                        idx = dates.GetList().FindIndex(delegate(ShowDate match) { return (match.DateOfShow.Year == yr && match.DateOfShow.Month == mo); });
                    }

                    if (idx != -1)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseTicket.aspx?mo={0}", li.Value));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.WriteLine(li.Text);
                        writer.RenderEndTag();//a
                        writer.RenderEndTag();//li
                    }
				}
			}

            writer.RenderEndTag();//end UL tag
        }
        protected void RenderMerchMenu(HtmlTextWriter writer)
        {
            if (Ctx.SaleMerch.Count > 0)
            {
                RenderMerch_UL(writer);
            }
            else
                writer.WriteLine(string.Format("<div class=\"comingsoon\">{0}</div>", _Config._Message_MerchComingSoon));
        }
        protected void RenderMerch_UL(HtmlTextWriter writer)
        {
            #region Featured

            //add featured items
            MerchCollection Featured = new MerchCollection();
            Featured.CopyFrom(Ctx.FeaturedMerchListing);

            if (Featured.Count > 0)
            {   
                if (_Config._Items_FeaturedToDisplay > 0 && Featured.Count > _Config._Items_FeaturedToDisplay)
                    Featured.GetList().RemoveRange(_Config._Items_FeaturedToDisplay, Featured.Count - _Config._Items_FeaturedToDisplay);

                writer.AddAttribute(HtmlTextWriterAttribute.Id, "FeaturedMerch");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "merchdivision");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.WriteLine("Featured Items");
                writer.RenderEndTag();//li

                //inner UL
                //writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (Merch merch in Featured)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseMerch.aspx?mite={0}", merch.Id));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.WriteLine(merch.DisplayName);
                    writer.RenderEndTag();//a
                    writer.RenderEndTag();//li
                }
                //writer.RenderEndTag();//end inner UL tag

                writer.RenderEndTag();//end UL tag
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Id, "MenuMerch");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            #endregion

            //foreach division - list the available categories
            MerchDivisionCollection mdColl = new MerchDivisionCollection();
            mdColl.AddRange(_Lookits.MerchDivisions.GetList().FindAll(delegate (MerchDivision match) { return (!match.IsInternalOnly); } ));

            foreach (MerchDivision div in mdColl)
			{
                bool addDownloadLink = (_Config._MerchMenu_DownloadsLink != null &&
                    _Config._MerchMenu_DownloadsDivision != null && 
                    _Config._MerchMenu_DownloadsDivision.ToLower() == div.Name.ToLower());

                if (div.HasDisplayableItems(Ctx.SaleMerch) || addDownloadLink)
                {
                    if (!_Config._MerchMenu_HideDivisionHeader)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "merchdivision");
                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        writer.WriteLine(div.Name);
                        writer.RenderEndTag();//li
                    }
                
                    MerchCategorieCollection categories = new MerchCategorieCollection();
                    categories.AddRange(_Lookits.MerchCategories.GetList().FindAll(
                        delegate(MerchCategorie match) { return ((!match.IsInternalOnly) && match.MerchDivisionRecord.Id == div.Id); }));
                    if(categories.Count > 1)
                        categories.Sort("IDisplayOrder", true);

                    if (categories.Count > 0 || addDownloadLink)
                    {
                        //writer.RenderBeginTag(HtmlTextWriterTag.Ul);

                        foreach (MerchCategorie cat in categories)
                        {
                            //if there is only one selection - then go directly to that page
                            MerchJoinCatCollection catColl = new MerchJoinCatCollection();
                            catColl.AddRange(cat.MerchJoinCatRecords().GetList()
                                .FindAll(delegate(MerchJoinCat match) { return ((!match.MerchRecord.IsInternalOnly) && match.MerchRecord.IsActive); } ));

                            
                            if (cat.Name != "NULL" && catColl.Count > 0)
                            {
                                if (_Config._MerchMenu_DisplayOnlyItemInCategorieAsCategorie && catColl.Count == 1)
                                {
                                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseMerch.aspx?cat={0}", cat.Id));
                                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                                    writer.WriteLine(cat.Name);
                                    writer.RenderEndTag();//a
                                    writer.RenderEndTag();//li

                                    //Merch theOnlyMerch = new Merch();
                                    //theOnlyMerch.CopyFrom(catColl[0].MerchRecord);

                                    //writer.RenderBeginTag(HtmlTextWriterTag.Li);
                                    //writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseMerch.aspx?mite={0}",
                                    //    theOnlyMerch.Id));
                                    //writer.RenderBeginTag(HtmlTextWriterTag.A);
                                    //writer.WriteLine(theOnlyMerch.DisplayName);
                                    //writer.RenderEndTag();//a
                                    //writer.RenderEndTag();//li
                                }
                                else if (catColl.Count > 1)
                                {
                                    if (!_Config._MerchMenu_HideCategorieHeader)
                                    {
                                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseMerch.aspx?cat={0}", cat.Id));
                                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                                        writer.WriteLine(cat.Name);
                                        writer.RenderEndTag();//a
                                        writer.RenderEndTag();//li
                                    }

                                    if (_Config._MerchMenu_DisplayAllCategoryProducts)
                                    {
                                        foreach (MerchJoinCat mjc in catColl)
                                        {
                                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "catproduct");
                                            writer.RenderBeginTag(HtmlTextWriterTag.Li);
                                            writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseMerch.aspx?mite={0}", mjc.TMerchId));
                                            writer.RenderBeginTag(HtmlTextWriterTag.A);

                                            if (_Config._MerchMenu_DisplayProductThumbnail)
                                            {
                                                //get the default image for the merch
                                                ItemImageCollection coll = new ItemImageCollection();
                                                coll.AddRange(_Lookits.MerchImages.GetList().FindAll(
                                                    delegate(ItemImage match) { return (match.TMerchId == mjc.TMerchId && match.IsItemImage); }));

                                                //if not found then use a spacer image
                                                if (coll.Count == 0)
                                                    writer.WriteLine("<span class=\"imgcont\"><img alt=\"\" src=\"/Images/spacer.gif\" /></span>");
                                                else
                                                {
                                                    if (coll.Count > 1)
                                                        coll.Sort("IDisplayOrder", true);

                                                    writer.WriteLine(string.Format("<span class=\"imgcont\"><img alt=\"\" src=\"{0}\"{1} /></span>", coll[0].Thumbnail_Small, 
                                                        (coll[0].IsPortrait) ? " class=\"portrait\"" : string.Empty ));
                                                }

                                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "catproductname");
                                                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                                                writer.WriteLine(mjc.MerchRecordName);
                                                writer.RenderEndTag();//span
                                            }
                                            else
                                                writer.WriteLine(mjc.MerchRecordName);

                                            writer.RenderEndTag();//a
                                            writer.RenderEndTag();//li
                                        }
                                    }
                                }
                                else
                                {
                                    Merch theOnlyMerch = new Merch();
                                    theOnlyMerch.CopyFrom(catColl[0].MerchRecord);

                                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/Store/ChooseMerch.aspx?mite={0}",
                                        theOnlyMerch.Id));
                                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                                    writer.WriteLine(theOnlyMerch.DisplayName);
                                    writer.RenderEndTag();//a
                                    writer.RenderEndTag();//li
                                }
                            }
                        }

                        //add a downloads link
                        if (addDownloadLink)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Li);
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, 
                                Utils.ParseHelper.FormatUrlFromString(_Config._MerchMenu_DownloadsLink, true, false));
                            writer.AddAttribute(HtmlTextWriterAttribute.Target, "_blank");
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.WriteLine(_Config._MerchMenu_DownloadsLinkText);
                            writer.RenderEndTag();//a
                            writer.RenderEndTag();//li
                        }

                        //writer.RenderEndTag();//end UL tag
                    }
                }
			}

            writer.RenderEndTag();//end UL tag
		
        }
        private static System.Collections.ArrayList GetMonthList(DateTime startDate, DateTime endDate)
        {
            //month name is based on %12
            //month value is determined by an int that indicates how many months from now - so 15 is next year
            System.Collections.ArrayList months = new System.Collections.ArrayList();

            DateTime lastEntry = endDate.AddDays(-endDate.Day).AddDays(startDate.Day);

            int i = 0;
            while (startDate.AddMonths(i).Date <= lastEntry.Date)
            {
                DateTime monthToDraw = startDate.AddMonths(i);

                //int actualMonth = monthToDraw.Month;
                string monthText = monthToDraw.ToString("MMMM &#39;yy");

                ListItem li = new ListItem(monthText, monthToDraw.ToString("yyyy_MM_1"));
                months.Add(li);
                i++;
            }

            return months;
        }
    }
}