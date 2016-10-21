using System;
using System.Threading;

namespace Jobs
{
    abstract public class Job
    {
        public Guid JobId = Guid.NewGuid();

        abstract public bool DoJob();

        abstract public void CleanUp();
    }

	public class JobHandler
	{
		private long NumActiveThreads = 0;
		private ManualResetEvent ExitEvent = new ManualResetEvent(false);
		public ManualResetEvent LastThreadEvent = new ManualResetEvent(false);
		private int LoopInterval = 10000;//10 secs

		public JobHandler()
		{
			//LoopInterval = _Config.svc_JobIntervalMilliSeconds;
		}

		public void JobCallback( Object state )
		{
			System.Threading.Interlocked.Increment( ref NumActiveThreads );

			do
			{
                System.Diagnostics.Debug.WriteLine(string.Format("{0}...DO JOB", DateTime.Now.ToString()));
                //if (_Config.svc_UseSqlDebug)
                //{
                //    string sql = "SELECT 'DO JOB'";
                //    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql);
                //    SubSonic.DataService.ExecuteQuery(cmd);
                //}

				bool bContinue = ((Job)state).DoJob();
				if (!bContinue) break;
			} 
			while ( !ExitEvent.WaitOne( LoopInterval, false) );

			((Job)state).CleanUp();

			long Count = System.Threading.Interlocked.Decrement( ref NumActiveThreads );

            if (Wcss._Config._ErrorsToDebugger)
                System.Diagnostics.Debug.WriteLine("Thread " + Count.ToString() + " " + ((Job)state).JobId.ToString() + " quitting");
			
            if (Count == 0) 
                LastThreadEvent.Set();
		}

		public void AddJob( Job job )
		{
            if (Wcss._Config._ErrorsToDebugger)
                System.Diagnostics.Debug.WriteLine(string.Format("{0}...ADD JOB", DateTime.Now.ToString()));
            //if (_Config.svc_UseSqlDebug)
            //{
            //    string sql = "SELECT 'ADD JOB'";
            //    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql);
            //    SubSonic.DataService.ExecuteQuery(cmd);
            //}

			ThreadPool.QueueUserWorkItem( new WaitCallback( JobCallback), job);
		}

		public long ActiveThreadCount
		{
			get
			{
				return NumActiveThreads;
			}
		}

		public void WaitJobs()
		{
			LastThreadEvent.WaitOne( 100, false);
		}

		public void StopJobs()
		{
            System.Diagnostics.Debug.WriteLine(string.Format("{0}...STOP JOB", DateTime.Now.ToString()));
            //if (_Config.svc_UseSqlDebug)
            //{
            //    string sql = "SELECT 'STOP JOB'";
            //    SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql);
            //    SubSonic.DataService.ExecuteQuery(cmd);
            //}

			ExitEvent.Set();
		}
	}
}
