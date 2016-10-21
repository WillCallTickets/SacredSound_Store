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
    /// Controller class for Search
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SearchController
    {
        // Preload our schema..
        Search thisSchemaLoad = new Search();
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
        public SearchCollection FetchAll()
        {
            SearchCollection coll = new SearchCollection();
            Query qry = new Query(Search.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SearchCollection FetchByID(object Id)
        {
            SearchCollection coll = new SearchCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SearchCollection FetchByQuery(Query qry)
        {
            SearchCollection coll = new SearchCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Search.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Search.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid ApplicationId,DateTime DtStamp,string VcContext,string Terms,int IResults,string EmailAddress,string IpAddress)
	    {
		    Search item = new Search();
		    
            item.ApplicationId = ApplicationId;
            
            item.DtStamp = DtStamp;
            
            item.VcContext = VcContext;
            
            item.Terms = Terms;
            
            item.IResults = IResults;
            
            item.EmailAddress = EmailAddress;
            
            item.IpAddress = IpAddress;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid ApplicationId,DateTime DtStamp,string VcContext,string Terms,int IResults,string EmailAddress,string IpAddress)
	    {
		    Search item = new Search();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.ApplicationId = ApplicationId;
				
			item.DtStamp = DtStamp;
				
			item.VcContext = VcContext;
				
			item.Terms = Terms;
				
			item.IResults = IResults;
				
			item.EmailAddress = EmailAddress;
				
			item.IpAddress = IpAddress;
				
	        item.Save(UserName);
	    }
    }
}
