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
    /// Controller class for ShowEvent
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShowEventController
    {
        // Preload our schema..
        ShowEvent thisSchemaLoad = new ShowEvent();
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
        public ShowEventCollection FetchAll()
        {
            ShowEventCollection coll = new ShowEventCollection();
            Query qry = new Query(ShowEvent.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowEventCollection FetchByID(object Id)
        {
            ShowEventCollection coll = new ShowEventCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowEventCollection FetchByQuery(Query qry)
        {
            ShowEventCollection coll = new ShowEventCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ShowEvent.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ShowEvent.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int TOwnerId,string VcOwnerType,int TParentId,string VcParentType,bool BActive,int IOrdinal,string DateString,string Status,string ShowTitle,string Promoter,string Header,string Headliner,string Opener,string Venue,string Times,string Ages,string Pricing,string Url,string ImageUrl)
	    {
		    ShowEvent item = new ShowEvent();
		    
            item.DtStamp = DtStamp;
            
            item.TOwnerId = TOwnerId;
            
            item.VcOwnerType = VcOwnerType;
            
            item.TParentId = TParentId;
            
            item.VcParentType = VcParentType;
            
            item.BActive = BActive;
            
            item.IOrdinal = IOrdinal;
            
            item.DateString = DateString;
            
            item.Status = Status;
            
            item.ShowTitle = ShowTitle;
            
            item.Promoter = Promoter;
            
            item.Header = Header;
            
            item.Headliner = Headliner;
            
            item.Opener = Opener;
            
            item.Venue = Venue;
            
            item.Times = Times;
            
            item.Ages = Ages;
            
            item.Pricing = Pricing;
            
            item.Url = Url;
            
            item.ImageUrl = ImageUrl;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int TOwnerId,string VcOwnerType,int TParentId,string VcParentType,bool BActive,int IOrdinal,string DateString,string Status,string ShowTitle,string Promoter,string Header,string Headliner,string Opener,string Venue,string Times,string Ages,string Pricing,string Url,string ImageUrl)
	    {
		    ShowEvent item = new ShowEvent();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TOwnerId = TOwnerId;
				
			item.VcOwnerType = VcOwnerType;
				
			item.TParentId = TParentId;
				
			item.VcParentType = VcParentType;
				
			item.BActive = BActive;
				
			item.IOrdinal = IOrdinal;
				
			item.DateString = DateString;
				
			item.Status = Status;
				
			item.ShowTitle = ShowTitle;
				
			item.Promoter = Promoter;
				
			item.Header = Header;
				
			item.Headliner = Headliner;
				
			item.Opener = Opener;
				
			item.Venue = Venue;
				
			item.Times = Times;
				
			item.Ages = Ages;
				
			item.Pricing = Pricing;
				
			item.Url = Url;
				
			item.ImageUrl = ImageUrl;
				
	        item.Save(UserName);
	    }
    }
}
