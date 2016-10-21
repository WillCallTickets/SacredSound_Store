using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TestApp
{

    #region Testing Sandbox

    public class MultiThreadTestApp_StringConcat
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //display should be handled like an application singleton
            EntityDisplay display = new EntityDisplay();

            //create multiple threads here
            //some loop to construct threads

            //tests for consistency

            string ewok = display.BuildSomeString;
            string wookie = display.IsThisThreadSafe;
            string banta = display.PrivateProperty;
        }
    }

    #endregion

    #region Entity, Repository and Display

    public class Entity
    {
        public int Idx { get; set; }
        public string Name { get; set; }
        public string PreDisplay { get; set; }
        public string AdditionalDisplay { get; set; }
        public string PostDisplay { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class EntityRepository
    {
        public List<Entity> listing;

        public EntityRepository()
        {
            listing = new List<Entity>()
            {
                new Entity { Idx = 1, Name = "Darth Vader", PreDisplay = "Lord", AdditionalDisplay = "wears a cape", PostDisplay = "breathes heavily", DisplayOrder = 0 },
                new Entity { Idx = 2, Name = "Luke Skywalker", PreDisplay = null, AdditionalDisplay = "is in training", DisplayOrder = 2 },
                new Entity { Idx = 3, Name = "Han Solo", AdditionalDisplay = "captain of the milenium falcon", PostDisplay = "makes the kil(something) run in ...", DisplayOrder = 3 },
                new Entity { Idx = 4, Name = "Leia", PreDisplay = "Princess", PostDisplay = "has great hair", DisplayOrder = 1 }
            };
        }
    }

    #endregion

    #region Display Instance

    public class EntityDisplay
    {
        EntityRepository repo = new EntityRepository();
        
        /// <summary>
        /// Would changing this to static make it thread safe?
        /// </summary>
        StringBuilder sb = new StringBuilder();


        /// <summary>
        /// NOT THREAD SAFE!!!!
        /// </summary>
        public string BuildSomeString
        {
            get
            {
                sb.Length = 0;

                List<Entity> list = new List<Entity>();
                list.AddRange(this.repo.listing.FindAll(
                    delegate(Entity ent) { return (ent.DisplayOrder < 3); }
                    ));
                    
                //contrived sort - display order will never be equal during testing
                if (list.Count > 1)
                    list.Sort(delegate(Entity x, Entity y) { return (x.DisplayOrder > y.DisplayOrder) ? 1 : 0; });

                if(list.Count > 0)
                {
                    foreach(Entity e in list)
                    {
                        if(e.PreDisplay != null && e.PreDisplay.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"pre\">{0}</span> ", e.PreDisplay.Trim());

                        sb.AppendFormat("<span class=\"name\">{0}</span> ", e.Name.Trim());

                        if (e.AdditionalDisplay != null && e.AdditionalDisplay.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"additional\">{0}</span> ", e.AdditionalDisplay.Trim());

                        if (e.PostDisplay != null && e.PostDisplay.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"post\">{0}</span> ", e.PostDisplay.Trim());

                        //add a delimiter
                        sb.Append(Environment.NewLine);
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string IsThisThreadSafe
        {
            get
            {
                string str = string.Empty;

                List<Entity> list = new List<Entity>();
                list.AddRange(this.repo.listing.FindAll(
                    delegate(Entity ent) { return (ent.DisplayOrder < 3); }
                    ));

                //contrived sort - display order will never be equal during testing
                if (list.Count > 1)
                    list.Sort(delegate(Entity x, Entity y) { return (x.DisplayOrder > y.DisplayOrder) ? 1 : 0; });

                if (list.Count > 0)
                {
                    foreach (Entity e in list)
                    {
                        if (e.PreDisplay != null && e.PreDisplay.Trim().Length > 0)
                            str += String.Format("<span class=\"pre\">{0}</span> ", e.PreDisplay.Trim());

                        str += String.Format("<span class=\"name\">{0}</span> ", e.Name.Trim());
                          
                        if (e.AdditionalDisplay != null && e.AdditionalDisplay.Trim().Length > 0)
                            str += String.Format("<span class=\"additional\">{0}</span> ", e.AdditionalDisplay.Trim());

                        if (e.PostDisplay != null && e.PostDisplay.Trim().Length > 0)
                            str += String.Format("<span class=\"post\">{0}</span> ", e.PostDisplay.Trim());

                        //add a delimiter
                        str += Environment.NewLine;
                    }
                }

                return str;
            }
        }


        /// <summary>
        /// Are private vars thread safe via this method?
        /// </summary>
        private string _privateProperty = null;

        public string PrivateProperty
        {
            get
            {
                if (_privateProperty == null)
                {
                    _privateProperty = string.Empty;

                    List<Entity> list = new List<Entity>();
                    list.AddRange(this.repo.listing.FindAll(
                        delegate(Entity ent) { return (ent.DisplayOrder < 3); }
                        ));

                    //contrived sort - display order will never be equal during testing
                    if (list.Count > 1)
                        list.Sort(delegate(Entity x, Entity y) { return (x.DisplayOrder > y.DisplayOrder) ? 1 : 0; });

                    if (list.Count > 0)
                    {
                        foreach (Entity e in list)
                        {
                            if (e.PreDisplay != null && e.PreDisplay.Trim().Length > 0)
                                _privateProperty += String.Format("<span class=\"pre\">{0}</span> ", e.PreDisplay.Trim());

                            _privateProperty += String.Format("<span class=\"name\">{0}</span> ", e.Name.Trim());

                            if (e.AdditionalDisplay != null && e.AdditionalDisplay.Trim().Length > 0)
                                _privateProperty += String.Format("<span class=\"additional\">{0}</span> ", e.AdditionalDisplay.Trim());

                            if (e.PostDisplay != null && e.PostDisplay.Trim().Length > 0)
                                _privateProperty += String.Format("<span class=\"post\">{0}</span> ", e.PostDisplay.Trim());

                            //add a delimiter
                            _privateProperty += Environment.NewLine;
                        }
                    }
                }

                return _privateProperty;
            }
        }
    }
    #endregion


    #region The original offending code

    /*
     * 
     * public string wc_CartHeadliner
        {
            get
            {
                sb.Length = 0;

                JShowActCollection coll = new JShowActCollection();
                coll.AddRange(this.JShowActRecords().GetList().FindAll(
                    delegate(JShowAct entity) { return (entity.TopBilling_Effective); }));
                if (coll.Count > 1)
                    coll.Sort("IDisplayOrder", true);

                //always display co headlines
                if (coll.Count > 0)
                {
                    foreach (JShowAct ent in coll)
                    {
                        if (ent.PreText != null && ent.PreText.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"pretext\">{0}</span> ", ent.PreText.Trim());

                        sb.AppendFormat("<span class=\"name\">{0}</span> ", ent.ActRecord.Name_Displayable);

                        if (ent.ActText != null && ent.ActText.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"extra\">{0}</span> ", ent.ActText.Trim());

                        if (ent.Featuring != null && ent.Featuring.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"featuring\">{0}</span> ", ent.Featuring.Trim());

                        if (ent.PostText != null && ent.PostText.Trim().Length > 0)
                            sb.AppendFormat("<span class=\"posttext\">{0}</span>", ent.PostText.Trim());

                        sb.Append("~");
                    }

                    Utils.ParseHelper.ConvertTildesToCommasAndAmpersands(sb);
                }

                return sb.ToString(); //This is line 393
             }
        }
      
     * Error information
     * mscorlib	
     * Index was out of range. Must be non-negative and less than the size of the collection.  
     * Parameter name: chunkLength	http://sts9store.com/store/chooseticket.aspx		
     * Stack Trace:
     * System.Text.StringBuilder_ToString	at System.Text.StringBuilder.ToString()     
     * at Wcss.ShowDate.get_wc_CartHeadliner() in d:\source\Sts9_2015\Wcss\Overridden\ShowDate.cs:line 393     
     * at WillCallWeb.Controls.Listing_Ticket.ProcessDates(Object sender, RepeaterItemEventArgs e)     
     * at System.Web.UI.WebControls.Repeater.CreateItem(Int32 itemIndex, ListItemType itemType, Boolean dataBind, Object dataItem)     
     * at System.Web.UI.WebControls.Repeater.CreateControlHierarchy(Boolean useDataSource)     
     * at System.Web.UI.WebControls.Repeater.OnDataBinding(EventArgs e)     
     * at WillCallWeb.Controls.Listing_Ticket.rptTickets_ItemDataBound(Object sender, RepeaterItemEventArgs e)     
     * at System.Web.UI.WebControls.Repeater.CreateItem(Int32 itemIndex, ListItemType itemType, Boolean dataBind, Object dataItem)     
     * at System.Web.UI.WebControls.Repeater.CreateControlHierarchy(Boolean useDataSource)     
     * at System.Web.UI.WebControls.Repeater.OnDataBinding(EventArgs e)     
     * at WillCallWeb.Controls.Listing_Ticket.BindTicketListing()     
     * at WillCallWeb.Controls.Listing_Ticket.Page_Load(Object sender, EventArgs e)     
     * at System.Web.UI.Control.LoadRecursive()     
     * at System.Web.UI.Control.LoadRecursive()     
     * at System.Web.UI.Control.LoadRecursive()     
     * at System.Web.UI.Control.LoadRecursive()     
     * at System.Web.UI.Control.LoadRecursive()     
     * at System.Web.UI.Control.LoadRecursive()     
     * at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)	     
     * 
     */

    #endregion

}
