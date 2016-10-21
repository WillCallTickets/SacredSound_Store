using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace Wcss
{
    /// <summary>
    /// Controller class for ShowDate
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShowDateController
    {
        // Preload our schema..
        ShowDate thisSchemaLoad = new ShowDate();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public ShowDateCollection FetchAll()
        {
            ShowDateCollection coll = new ShowDateCollection();
            Query qry = new Query(ShowDate.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowDateCollection FetchByID(object Id)
        {
            ShowDateCollection coll = new ShowDateCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowDateCollection FetchByQuery(Query qry)
        {
            ShowDateCollection coll = new ShowDateCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ShowDate.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ShowDate.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtDateOfShow,string ShowTime,bool BLateNightShow,int TShowId,bool BActive,string ShowDateTitle,string StatusText,string PricingText,string TicketUrl,string DisplayNotes,int TAgeId,int TStatusId,string Billing,bool BAutoBilling,bool BPrivateShow,bool BUseFbRsvp,string FbRsvpUrl,DateTime DtStamp)
	    {
		    ShowDate item = new ShowDate();
		    
            item.DtDateOfShow = DtDateOfShow;
            
            item.ShowTime = ShowTime;
            
            item.BLateNightShow = BLateNightShow;
            
            item.TShowId = TShowId;
            
            item.BActive = BActive;
            
            item.ShowDateTitle = ShowDateTitle;
            
            item.StatusText = StatusText;
            
            item.PricingText = PricingText;
            
            item.TicketUrl = TicketUrl;
            
            item.DisplayNotes = DisplayNotes;
            
            item.TAgeId = TAgeId;
            
            item.TStatusId = TStatusId;
            
            item.Billing = Billing;
            
            item.BAutoBilling = BAutoBilling;
            
            item.BPrivateShow = BPrivateShow;
            
            item.BUseFbRsvp = BUseFbRsvp;
            
            item.FbRsvpUrl = FbRsvpUrl;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtDateOfShow,string ShowTime,bool BLateNightShow,int TShowId,bool BActive,string ShowDateTitle,string StatusText,string PricingText,string TicketUrl,string DisplayNotes,int TAgeId,int TStatusId,string Billing,bool BAutoBilling,bool BPrivateShow,bool BUseFbRsvp,string FbRsvpUrl,DateTime DtStamp)
	    {
		    ShowDate item = new ShowDate();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtDateOfShow = DtDateOfShow;
				
			item.ShowTime = ShowTime;
				
			item.BLateNightShow = BLateNightShow;
				
			item.TShowId = TShowId;
				
			item.BActive = BActive;
				
			item.ShowDateTitle = ShowDateTitle;
				
			item.StatusText = StatusText;
				
			item.PricingText = PricingText;
				
			item.TicketUrl = TicketUrl;
				
			item.DisplayNotes = DisplayNotes;
				
			item.TAgeId = TAgeId;
				
			item.TStatusId = TStatusId;
				
			item.Billing = Billing;
				
			item.BAutoBilling = BAutoBilling;
				
			item.BPrivateShow = BPrivateShow;
				
			item.BUseFbRsvp = BUseFbRsvp;
				
			item.FbRsvpUrl = FbRsvpUrl;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
