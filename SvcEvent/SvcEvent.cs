using System;
using System.Collections;
using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.IO;
using System.Threading;
using System.Reflection;

using Jobs;
using Wcss;

namespace SvcEvent
{
	public class ServiceMain : System.ServiceProcess.ServiceBase
	{
		private JobHandler jobHandler;
		private int Threads = _Config.svc_MaxThreads;

		public ServiceMain()
		{
            this.ServiceName = "StaticEventService";
		}

        //Use this for testing
        //Note that this will only process one row (wont do batch) and may leave the others in a batch hanging
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    //_Error.LogException(new Exception("find the dir"));
        //    EventJob eventJob = new EventJob();
        //    eventJob.DoJob();
        //    eventJob.CleanUp();
        //}


		//Reinstate for deployment
        static void Main(string[] args)
        {
            try
            {
                System.ServiceProcess.ServiceBase.Run(new ServiceMain());
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                throw ex;
            }
        }
		

		protected override void OnStart(string[] args)
		{
            if(_Config._ErrorsToDebugger)
                Debug.WriteLine("Starting Static Event Service...");
			
			jobHandler = new JobHandler();

            //_Error.LogException(new Exception("Starting StaticEventService...#of threads: " + Threads.ToString()));
		
            for (int i = 0; i < 1; i++)
				jobHandler.AddJob( new EventJob() );
		}
 
		protected override void OnStop()
		{
			jobHandler.StopJobs();

			jobHandler.WaitJobs();

            if (_Config._ErrorsToDebugger)
                Debug.WriteLine("Exiting Static Event Service...");
		}

	}
}
