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
    /// Controller class for ShowTicketDosTicket
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ShowTicketDosTicketController
    {
        // Preload our schema..
        ShowTicketDosTicket thisSchemaLoad = new ShowTicketDosTicket();
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
        public ShowTicketDosTicketCollection FetchAll()
        {
            ShowTicketDosTicketCollection coll = new ShowTicketDosTicketCollection();
            Query qry = new Query(ShowTicketDosTicket.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowTicketDosTicketCollection FetchByID(object Id)
        {
            ShowTicketDosTicketCollection coll = new ShowTicketDosTicketCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ShowTicketDosTicketCollection FetchByQuery(Query qry)
        {
            ShowTicketDosTicketCollection coll = new ShowTicketDosTicketCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ShowTicketDosTicket.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ShowTicketDosTicket.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,int ParentId,int DosId)
	    {
		    ShowTicketDosTicket item = new ShowTicketDosTicket();
		    
            item.DtStamp = DtStamp;
            
            item.ParentId = ParentId;
            
            item.DosId = DosId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,int ParentId,int DosId)
	    {
		    ShowTicketDosTicket item = new ShowTicketDosTicket();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ParentId = ParentId;
				
			item.DosId = DosId;
				
	        item.Save(UserName);
	    }
    }
}
