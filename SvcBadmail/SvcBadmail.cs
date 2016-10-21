using System;
//using System.Diagnostics;

using Jobs;

namespace SvcBadmail
{
	public class ServiceMain : System.ServiceProcess.ServiceBase
	{
		private JobHandler jobHandler;
		//private int Threads = 4;//_Config.svc_MaxThreads;

		public ServiceMain()
		{
            this.ServiceName = "BadmailService";
		}

		//Use this for testing
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    ////_Error.LogException(new Exception("find the dir"));
        //    try
        //    {
        //        if (Wcss._Config._ErrorsToDebugger)
        //        {
        //            string g = "l";
        //        }

        //        BadmailJob badmailJob = new BadmailJob();
        //        badmailJob.DoJob();
        //        badmailJob.CleanUp();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
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
                System.Diagnostics.Debug.Write(ex);
                Wcss._Error.LogException(ex);
                throw;
            }
        }

		protected override void OnStart(string[] args)
		{
            if (Wcss._Config._ErrorsToDebugger)
                System.Diagnostics.Debug.WriteLine("Starting Badmail Service...");
			
			jobHandler = new JobHandler();
		
            for (int i = 0; i < 1; i++)
                jobHandler.AddJob(new BadmailJob());
		}
 
		protected override void OnStop()
		{
			jobHandler.StopJobs();

			jobHandler.WaitJobs();

            if (Wcss._Config._ErrorsToDebugger)
                System.Diagnostics.Debug.WriteLine("Exiting Badmail Service...");
		}

	}
}
