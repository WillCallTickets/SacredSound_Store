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
    /// Controller class for FaqCategorie
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class FaqCategorieController
    {
        // Preload our schema..
        FaqCategorie thisSchemaLoad = new FaqCategorie();
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
        public FaqCategorieCollection FetchAll()
        {
            FaqCategorieCollection coll = new FaqCategorieCollection();
            Query qry = new Query(FaqCategorie.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public FaqCategorieCollection FetchByID(object Id)
        {
            FaqCategorieCollection coll = new FaqCategorieCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public FaqCategorieCollection FetchByQuery(Query qry)
        {
            FaqCategorieCollection coll = new FaqCategorieCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (FaqCategorie.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (FaqCategorie.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,bool BActive,string DisplayText,string Description,int IDisplayOrder,string Name,Guid ApplicationId)
	    {
		    FaqCategorie item = new FaqCategorie();
		    
            item.DtStamp = DtStamp;
            
            item.BActive = BActive;
            
            item.DisplayText = DisplayText;
            
            item.Description = Description;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.Name = Name;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(DateTime DtStamp,bool BActive,string DisplayText,string Description,int IDisplayOrder,int Id,string Name,Guid ApplicationId)
	    {
		    FaqCategorie item = new FaqCategorie();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.DtStamp = DtStamp;
				
			item.BActive = BActive;
				
			item.DisplayText = DisplayText;
				
			item.Description = Description;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.Id = Id;
				
			item.Name = Name;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
