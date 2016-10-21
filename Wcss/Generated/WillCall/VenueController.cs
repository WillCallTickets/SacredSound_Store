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
    /// Controller class for Venue
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class VenueController
    {
        // Preload our schema..
        Venue thisSchemaLoad = new Venue();
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
        public VenueCollection FetchAll()
        {
            VenueCollection coll = new VenueCollection();
            Query qry = new Query(Venue.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public VenueCollection FetchByID(object Id)
        {
            VenueCollection coll = new VenueCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public VenueCollection FetchByQuery(Query qry)
        {
            VenueCollection coll = new VenueCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Venue.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Venue.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(string Name,string NameRoot,string DisplayName,int? ICapacity,string PictureUrl,int IPicWidth,int IPicHeight,string WebsiteUrl,string ShortAddress,string Address,string City,string State,string ZipCode,string Country,string Latitude,string Longitude,string BoxOfficePhone,string BoxOfficePhoneExt,string BoxOfficeNotes,string MainPhone,string MainPhoneExt,string Notes,DateTime DtStamp,Guid ApplicationId)
	    {
		    Venue item = new Venue();
		    
            item.Name = Name;
            
            item.NameRoot = NameRoot;
            
            item.DisplayName = DisplayName;
            
            item.ICapacity = ICapacity;
            
            item.PictureUrl = PictureUrl;
            
            item.IPicWidth = IPicWidth;
            
            item.IPicHeight = IPicHeight;
            
            item.WebsiteUrl = WebsiteUrl;
            
            item.ShortAddress = ShortAddress;
            
            item.Address = Address;
            
            item.City = City;
            
            item.State = State;
            
            item.ZipCode = ZipCode;
            
            item.Country = Country;
            
            item.Latitude = Latitude;
            
            item.Longitude = Longitude;
            
            item.BoxOfficePhone = BoxOfficePhone;
            
            item.BoxOfficePhoneExt = BoxOfficePhoneExt;
            
            item.BoxOfficeNotes = BoxOfficeNotes;
            
            item.MainPhone = MainPhone;
            
            item.MainPhoneExt = MainPhoneExt;
            
            item.Notes = Notes;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,string Name,string NameRoot,string DisplayName,int? ICapacity,string PictureUrl,int IPicWidth,int IPicHeight,string WebsiteUrl,string ShortAddress,string Address,string City,string State,string ZipCode,string Country,string Latitude,string Longitude,string BoxOfficePhone,string BoxOfficePhoneExt,string BoxOfficeNotes,string MainPhone,string MainPhoneExt,string Notes,DateTime DtStamp,Guid ApplicationId)
	    {
		    Venue item = new Venue();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.Name = Name;
				
			item.NameRoot = NameRoot;
				
			item.DisplayName = DisplayName;
				
			item.ICapacity = ICapacity;
				
			item.PictureUrl = PictureUrl;
				
			item.IPicWidth = IPicWidth;
				
			item.IPicHeight = IPicHeight;
				
			item.WebsiteUrl = WebsiteUrl;
				
			item.ShortAddress = ShortAddress;
				
			item.Address = Address;
				
			item.City = City;
				
			item.State = State;
				
			item.ZipCode = ZipCode;
				
			item.Country = Country;
				
			item.Latitude = Latitude;
				
			item.Longitude = Longitude;
				
			item.BoxOfficePhone = BoxOfficePhone;
				
			item.BoxOfficePhoneExt = BoxOfficePhoneExt;
				
			item.BoxOfficeNotes = BoxOfficeNotes;
				
			item.MainPhone = MainPhone;
				
			item.MainPhoneExt = MainPhoneExt;
				
			item.Notes = Notes;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
