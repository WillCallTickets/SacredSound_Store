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
    /// Controller class for ActivationWindow
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ActivationWindowController
    {
        // Preload our schema..
        ActivationWindow thisSchemaLoad = new ActivationWindow();
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
        public ActivationWindowCollection FetchAll()
        {
            ActivationWindowCollection coll = new ActivationWindowCollection();
            Query qry = new Query(ActivationWindow.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ActivationWindowCollection FetchByID(object Id)
        {
            ActivationWindowCollection coll = new ActivationWindowCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ActivationWindowCollection FetchByQuery(Query qry)
        {
            ActivationWindowCollection coll = new ActivationWindowCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ActivationWindow.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ActivationWindow.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime DtStamp,Guid ApplicationId,string VcContext,int TParentId,bool BUseCode,string Code,DateTime? DtCodeStart,DateTime? DtCodeEnd,DateTime? DtPublicStart,DateTime? DtPublicEnd)
	    {
		    ActivationWindow item = new ActivationWindow();
		    
            item.DtStamp = DtStamp;
            
            item.ApplicationId = ApplicationId;
            
            item.VcContext = VcContext;
            
            item.TParentId = TParentId;
            
            item.BUseCode = BUseCode;
            
            item.Code = Code;
            
            item.DtCodeStart = DtCodeStart;
            
            item.DtCodeEnd = DtCodeEnd;
            
            item.DtPublicStart = DtPublicStart;
            
            item.DtPublicEnd = DtPublicEnd;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime DtStamp,Guid ApplicationId,string VcContext,int TParentId,bool BUseCode,string Code,DateTime? DtCodeStart,DateTime? DtCodeEnd,DateTime? DtPublicStart,DateTime? DtPublicEnd)
	    {
		    ActivationWindow item = new ActivationWindow();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ApplicationId = ApplicationId;
				
			item.VcContext = VcContext;
				
			item.TParentId = TParentId;
				
			item.BUseCode = BUseCode;
				
			item.Code = Code;
				
			item.DtCodeStart = DtCodeStart;
				
			item.DtCodeEnd = DtCodeEnd;
				
			item.DtPublicStart = DtPublicStart;
				
			item.DtPublicEnd = DtPublicEnd;
				
	        item.Save(UserName);
	    }
    }
}
