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
namespace Erlg{
    public partial class SPs{
        
        /// <summary>
        /// Creates an object wrapper for the elx_LogError Procedure
        /// </summary>
        public static StoredProcedure ElxLogError(string Source, DateTime? DateX, string Message, string Form, string Querystring, string TargetSite, string StackTrace, string Referrer, string IpAddress, string Email, string ApplicationName, int? EventId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("elx_LogError", DataService.GetInstance("ErrorLog"), "dbo");
        	
            sp.Command.AddParameter("@Source", Source, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Date", DateX, DbType.DateTime, null, null);
        	
            sp.Command.AddParameter("@Message", Message, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Form", Form, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Querystring", Querystring, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@TargetSite", TargetSite, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@StackTrace", StackTrace, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Referrer", Referrer, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@IpAddress", IpAddress, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@ApplicationName", ApplicationName, DbType.AnsiString, null, null);
        	
            sp.Command.AddOutputParameter("@EventId", DbType.Int32, 0, 10);
            
            return sp;
        }
        
    }
    
}
