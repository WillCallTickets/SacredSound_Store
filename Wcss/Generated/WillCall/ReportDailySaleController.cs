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
    /// Controller class for Report_DailySales
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class ReportDailySaleController
    {
        // Preload our schema..
        ReportDailySale thisSchemaLoad = new ReportDailySale();
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
        public ReportDailySaleCollection FetchAll()
        {
            ReportDailySaleCollection coll = new ReportDailySaleCollection();
            Query qry = new Query(ReportDailySale.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ReportDailySaleCollection FetchByID(object Id)
        {
            ReportDailySaleCollection coll = new ReportDailySaleCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public ReportDailySaleCollection FetchByQuery(Query qry)
        {
            ReportDailySaleCollection coll = new ReportDailySaleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (ReportDailySale.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (ReportDailySale.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime? DtStamp,DateTime ReportDate,string VcContext,int ItemId,string Description,string MiniDesc,int Alloted,int Sold,int TotalSold,int Available,Guid ApplicationId)
	    {
		    ReportDailySale item = new ReportDailySale();
		    
            item.DtStamp = DtStamp;
            
            item.ReportDate = ReportDate;
            
            item.VcContext = VcContext;
            
            item.ItemId = ItemId;
            
            item.Description = Description;
            
            item.MiniDesc = MiniDesc;
            
            item.Alloted = Alloted;
            
            item.Sold = Sold;
            
            item.TotalSold = TotalSold;
            
            item.Available = Available;
            
            item.ApplicationId = ApplicationId;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? DtStamp,DateTime ReportDate,string VcContext,int ItemId,string Description,string MiniDesc,int Alloted,int Sold,int TotalSold,int Available,Guid ApplicationId)
	    {
		    ReportDailySale item = new ReportDailySale();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.ReportDate = ReportDate;
				
			item.VcContext = VcContext;
				
			item.ItemId = ItemId;
				
			item.Description = Description;
				
			item.MiniDesc = MiniDesc;
				
			item.Alloted = Alloted;
				
			item.Sold = Sold;
				
			item.TotalSold = TotalSold;
				
			item.Available = Available;
				
			item.ApplicationId = ApplicationId;
				
	        item.Save(UserName);
	    }
    }
}
