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
    /// Controller class for ShowTicketPackageLink
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShowTicketPackageLinkController
    {
        // Preload our schema..
        ShowTicketPackageLink thisSchemaLoad = new ShowTicketPackageLink();
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
        public ShowTicketPackageLinkCollection FetchAll()
        {
            ShowTicketPackageLinkCollection coll = new ShowTicketPackageLinkCollection();
            Query qry = new Query(ShowTicketPackageLink.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowTicketPackageLinkCollection FetchByID(object Id)
        {
            ShowTicketPackageLinkCollection coll = new ShowTicketPackageLinkCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowTicketPackageLinkCollection FetchByQuery(Query qry)
        {
            ShowTicketPackageLinkCollection coll = new ShowTicketPackageLinkCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ShowTicketPackageLink.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ShowTicketPackageLink.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(Guid? GroupIdentifier,int ParentShowTicketId,int LinkedShowTicketId,DateTime? DtStamp)
	    {
		    ShowTicketPackageLink item = new ShowTicketPackageLink();
		    
            item.GroupIdentifier = GroupIdentifier;
            
            item.ParentShowTicketId = ParentShowTicketId;
            
            item.LinkedShowTicketId = LinkedShowTicketId;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,Guid? GroupIdentifier,int ParentShowTicketId,int LinkedShowTicketId,DateTime? DtStamp)
	    {
		    ShowTicketPackageLink item = new ShowTicketPackageLink();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.GroupIdentifier = GroupIdentifier;
				
			item.ParentShowTicketId = ParentShowTicketId;
				
			item.LinkedShowTicketId = LinkedShowTicketId;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
