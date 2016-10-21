using System;
using System.Data;
using System.Collections;

using Wcss;

namespace SvcMail
{
	/// <summary>
	/// Summary description for MailData.
	/// </summary>
    public class MailData
    {
        public MailData(Guid Job_Guid) 
        {
            this.batchGuid = Job_Guid;
        }

        private Guid batchGuid = Guid.Empty;
        private MailQueueCollection mails = null;
        private IEnumerator enumerator = null;
        private int safeWaitInterval = ((_Config.svc_JobIntervalMilliSeconds * _Config.svc_BatchRetrievalSize) / 1000) + 60;

        private void GetNextBatch()
        {
            //clean up data - missed rows will be caught in the cleanup
            if (mails != null)
                mails = null;

            mails = new MailQueueCollection();
            
            using (IDataReader rdr = SPs.TxJobGetBatchMailData(_Config.APPLICATION_ID, batchGuid,
                _Config.svc_BatchRetrievalSize, safeWaitInterval, _Config.svc_ArchiveAfterDays).GetReader())
            {   
                mails.LoadAndCloseReader(rdr);
            }

            this.enumerator = mails.GetEnumerator();
        }
        public MailQueue GetNextMail()
        {
            if (this.enumerator != null && this.enumerator.MoveNext())
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}...PROCESS EXISTING ROW", DateTime.Now.ToString()));
                if (_Config.svc_UseSqlDebug)
                {
                    string sql = "SELECT 'PROCESS EXISTING ROW'";
                    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                    SubSonic.DataService.ExecuteQuery(cmd);
                }

                return (MailQueue)this.enumerator.Current;
            }
            else
            {
                 System.Diagnostics.Debug.WriteLine(string.Format("{0}...PAUSE BETWEEN BATCH", DateTime.Now.ToString()));
                 if (_Config.svc_UseSqlDebug)
                 {
                     string sql1 = "SELECT 'PAUSE BETWEEN BATCH'";
                     SubSonic.QueryCommand cmd1 = new SubSonic.QueryCommand(sql1, SubSonic.DataService.Provider.Name);
                     SubSonic.DataService.ExecuteQuery(cmd1);
                 }

                 System.Threading.Thread.Sleep(_Config.svc_PauseBetweenBatches * 1000);

                 System.Diagnostics.Debug.WriteLine(string.Format("{0}...GET NEXT BATCH", DateTime.Now.ToString()));
                 if (_Config.svc_UseSqlDebug)
                 {
                     string sql = "SELECT 'GET NEXT BATCH'";
                     SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                     SubSonic.DataService.ExecuteQuery(cmd);
                 }

                GetNextBatch();

                if (this.enumerator.MoveNext())
                {
                    return (MailQueue)this.enumerator.Current;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
