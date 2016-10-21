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
    /// Controller class for Lottery
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class LotteryController
    {
        // Preload our schema..
        Lottery thisSchemaLoad = new Lottery();
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
        public LotteryCollection FetchAll()
        {
            LotteryCollection coll = new LotteryCollection();
            Query qry = new Query(Lottery.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public LotteryCollection FetchByID(object Id)
        {
            LotteryCollection coll = new LotteryCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public LotteryCollection FetchByQuery(Query qry)
        {
            LotteryCollection coll = new LotteryCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (Lottery.Delete(Id) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (Lottery.Destroy(Id) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(DateTime? DtStamp,int TShowTicketId,int TShowDateId,int TShowId,bool BActiveSignup,DateTime? DtSignupStart,DateTime? DtSignupEnd,string Name,string Description,string DisplayText,string Writeup,bool BActiveFulfillment,DateTime? DtFulfillStart,DateTime? DtFulfillEnd,int IEstablishQty)
	    {
		    Lottery item = new Lottery();
		    
            item.DtStamp = DtStamp;
            
            item.TShowTicketId = TShowTicketId;
            
            item.TShowDateId = TShowDateId;
            
            item.TShowId = TShowId;
            
            item.BActiveSignup = BActiveSignup;
            
            item.DtSignupStart = DtSignupStart;
            
            item.DtSignupEnd = DtSignupEnd;
            
            item.Name = Name;
            
            item.Description = Description;
            
            item.DisplayText = DisplayText;
            
            item.Writeup = Writeup;
            
            item.BActiveFulfillment = BActiveFulfillment;
            
            item.DtFulfillStart = DtFulfillStart;
            
            item.DtFulfillEnd = DtFulfillEnd;
            
            item.IEstablishQty = IEstablishQty;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,DateTime? DtStamp,int TShowTicketId,int TShowDateId,int TShowId,bool BActiveSignup,DateTime? DtSignupStart,DateTime? DtSignupEnd,string Name,string Description,string DisplayText,string Writeup,bool BActiveFulfillment,DateTime? DtFulfillStart,DateTime? DtFulfillEnd,int IEstablishQty)
	    {
		    Lottery item = new Lottery();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.DtStamp = DtStamp;
				
			item.TShowTicketId = TShowTicketId;
				
			item.TShowDateId = TShowDateId;
				
			item.TShowId = TShowId;
				
			item.BActiveSignup = BActiveSignup;
				
			item.DtSignupStart = DtSignupStart;
				
			item.DtSignupEnd = DtSignupEnd;
				
			item.Name = Name;
				
			item.Description = Description;
				
			item.DisplayText = DisplayText;
				
			item.Writeup = Writeup;
				
			item.BActiveFulfillment = BActiveFulfillment;
				
			item.DtFulfillStart = DtFulfillStart;
				
			item.DtFulfillEnd = DtFulfillEnd;
				
			item.IEstablishQty = IEstablishQty;
				
	        item.Save(UserName);
	    }
    }
}
