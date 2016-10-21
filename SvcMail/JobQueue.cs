using System;
using System.Collections;
using System.Threading;

namespace Jobs
{
	/// <summary>
	/// 
	/// </summary>
	public class JobQueue
	{
		private Queue jobs = Queue.Synchronized( new Queue() );
		private ManualResetEvent ExitEvent = new ManualResetEvent(false);
		private ManualResetEvent ThreadsCompleteEvent = new ManualResetEvent(false);
		private AutoResetEvent JobsWaitingEvent = new AutoResetEvent( false );
		private long NumActiveThreads = 0;

        public void WorkerLoop()
        {
            Interlocked.Increment(ref NumActiveThreads);

            while (!ExitEvent.WaitOne(100, true))
            {
                try
                {
                    if (jobs.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("{0}...WORKER PROCESS THREAD JOB COUNT", DateTime.Now.ToString()));
                        if (Wcss._Config.svc_UseSqlDebug)
                        {
                            string sql = "SELECT 'WORKER PROCESS THREAD JOB COUNT'";
                            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                            SubSonic.DataService.ExecuteQuery(cmd);
                        }

                        Job job = (Job)jobs.Dequeue();
                        if (job != null) job.DoJob();
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("{0}...WORKER PROCESS THREAD WAIT FOR NEXT JOB", DateTime.Now.ToString()));
                        if (Wcss._Config.svc_UseSqlDebug)
                        {
                            string sql = "SELECT 'WORKER PROCESS THREAD WAIT FOR NEXT JOB'";
                            SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                            SubSonic.DataService.ExecuteQuery(cmd);
                        }

                        //if no jobs present wait a while
                        JobsWaitingEvent.WaitOne(10000, true);
                    }
                }
                catch (Exception) { }
            }

            System.Diagnostics.Debug.WriteLine(string.Format("{0}...WORKER THREAD EXITING", DateTime.Now.ToString()));
            if (Wcss._Config.svc_UseSqlDebug)
            {
                string sql = "SELECT 'WORKER THREAD EXITING'";
                SubSonic.QueryCommand cmd = new SubSonic.QueryCommand(sql, SubSonic.DataService.Provider.Name);
                SubSonic.DataService.ExecuteQuery(cmd);
            }

            long ThreadsLeft = Interlocked.Decrement(ref NumActiveThreads);
            if (ThreadsLeft == 0) ThreadsCompleteEvent.Set();
        }

		public void AddJob( Job job)
		{
			jobs.Enqueue(job);
			JobsWaitingEvent.Set();
		}

		public JobQueue(int numThreads)
		{
			for (int i = 0; i < numThreads; i++)
			{
				Thread t = new Thread( new ThreadStart( WorkerLoop));
				t.Start();
			}
		}

		public bool StopJobs( int TimeoutMs)
		{
			ExitEvent.Set();
			return ThreadsCompleteEvent.WaitOne( TimeoutMs, true);
		}
	}
}
