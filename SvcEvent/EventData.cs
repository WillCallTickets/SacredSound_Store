using System;
using System.Data;
using System.Collections;

using Wcss;

namespace SvcEvent
{
    public class EventData
    {
        public EventData(Guid Job_Guid) 
        {
            this.batchGuid = Job_Guid;
        }

        private Guid batchGuid = Guid.Empty;
        private EventQCollection events = null;
        private IEnumerator enumerator = null;
        private int safeWaitInterval = ((_Config.svc_JobIntervalMilliSeconds * _Config.svc_BatchRetrievalSize) / 1000) + 60;

        private void GetNextBatch()
        {
            //clean up data - missed rows will be caught in the cleanup
            if (events != null)
                events = null;
            
            using (IDataReader rdr = SPs.TxJobGetBatchEventData(_Config.APPLICATION_ID, batchGuid,
                _Config.svc_BatchRetrievalSize, safeWaitInterval, _Config.svc_ArchiveAfterDays).GetReader())
            {
                events = new EventQCollection();
                events.LoadAndCloseReader(rdr);
            }

            this.enumerator = events.GetEnumerator();
        }
        public EventQ GetNextEvent()
        {
            if (this.enumerator != null && this.enumerator.MoveNext())
            {
                if (_Config._ErrorsToDebugger)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}...PROCESS EXISTING ROW", DateTime.Now.ToString()));
                    if (_Config.svc_UseSqlDebug)
                    {
                        string sql = "SELECT 'PROCESS EXISTING ROW'";
                        SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                        SubSonic.DataService.ExecuteQuery(cmd);
                    }
                }

                return (EventQ)this.enumerator.Current;
            }
            else
            {
                if (_Config._ErrorsToDebugger)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("{0}...PAUSE BETWEEN BATCH", DateTime.Now.ToString()));
                    if (_Config.svc_UseSqlDebug)
                    {
                        string sql1 = "SELECT 'PAUSE BETWEEN BATCH'";
                        SubSonic.QueryCommand cmd1 = new SubSonic.QueryCommand(sql1, SubSonic.DataService.Provider.Name);
                        SubSonic.DataService.ExecuteQuery(cmd1);
                    }
                }

                 System.Threading.Thread.Sleep(_Config.svc_PauseBetweenBatches * 1000);

                 if (_Config._ErrorsToDebugger)
                 {
                     System.Diagnostics.Debug.WriteLine(string.Format("{0}...GET NEXT BATCH", DateTime.Now.ToString()));
                     if (_Config.svc_UseSqlDebug)
                     {
                         string sql = "SELECT 'GET NEXT BATCH'";
                         SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                         SubSonic.DataService.ExecuteQuery(cmd);
                     }
                 }

                GetNextBatch();

                if (this.enumerator.MoveNext())
                {
                    return (EventQ)this.enumerator.Current;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
