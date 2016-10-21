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
    /// Controller class for Tune
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TuneController
    {
        // Preload our schema..
        Tune thisSchemaLoad = new Tune();
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
        public TuneCollection FetchAll()
        {
            TuneCollection coll = new TuneCollection();
            Query qry = new Query(Tune.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TuneCollection FetchByID(object Id)
        {
            TuneCollection coll = new TuneCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TuneCollection FetchByQuery(Query qry)
        {
            TuneCollection coll = new TuneCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Tune.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Tune.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TActId,string FileName,string DisplayText,string Description,bool BActive,int IDisplayOrder,DateTime DtStamp,Guid ApplicationId)
	    {
		    Tune item = new Tune();
		    
            item.TActId = TActId;
            
            item.FileName = FileName;
            
            item.DisplayText = DisplayText;
            
            item.Description = Description;
            
            item.BActive = BActive;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TActId,string FileName,string DisplayText,string Description,bool BActive,int IDisplayOrder,DateTime DtStamp,Guid ApplicationId)
	    {
		    Tune item = new Tune();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TActId = TActId;
				
			item.FileName = FileName;
				
			item.DisplayText = DisplayText;
				
			item.Description = Description;
				
			item.BActive = BActive;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
