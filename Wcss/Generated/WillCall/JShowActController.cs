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
    /// Controller class for JShowAct
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class JShowActController
    {
        // Preload our schema..
        JShowAct thisSchemaLoad = new JShowAct();
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
        public JShowActCollection FetchAll()
        {
            JShowActCollection coll = new JShowActCollection();
            Query qry = new Query(JShowAct.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public JShowActCollection FetchByID(object Id)
        {
            JShowActCollection coll = new JShowActCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public JShowActCollection FetchByQuery(Query qry)
        {
            JShowActCollection coll = new JShowActCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (JShowAct.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (JShowAct.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int TActId,int TShowDateId,string PreText,string ActText,string Featuring,string PostText,int IDisplayOrder,bool? BTopBilling,DateTime DtStamp)
	    {
		    JShowAct item = new JShowAct();
		    
            item.TActId = TActId;
            
            item.TShowDateId = TShowDateId;
            
            item.PreText = PreText;
            
            item.ActText = ActText;
            
            item.Featuring = Featuring;
            
            item.PostText = PostText;
            
            item.IDisplayOrder = IDisplayOrder;
            
            item.BTopBilling = BTopBilling;
            
            item.DtStamp = DtStamp;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int TActId,int TShowDateId,string PreText,string ActText,string Featuring,string PostText,int IDisplayOrder,bool? BTopBilling,DateTime DtStamp)
	    {
		    JShowAct item = new JShowAct();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.TActId = TActId;
				
			item.TShowDateId = TShowDateId;
				
			item.PreText = PreText;
				
			item.ActText = ActText;
				
			item.Featuring = Featuring;
				
			item.PostText = PostText;
				
			item.IDisplayOrder = IDisplayOrder;
				
			item.BTopBilling = BTopBilling;
				
			item.DtStamp = DtStamp;
				
	        item.Save(UserName);
	    }
    }
}
