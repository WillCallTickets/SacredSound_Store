using System;
using System.ServiceProcess;

using Jobs;
using Wcss;

namespace SvcMail
{
    public partial class SvcMail : ServiceBase
    {
        private JobHandler jobHandler;
        private int MailThreads = _Config.svc_MaxThreads;

        public SvcMail()
        {
            InitializeComponent();
            this.ServiceName = "MailService";
        }

        protected override void OnStart(string[] args)
        {
            if (Wcss._Config._ErrorsToDebugger)
                System.Diagnostics.Debug.WriteLine("Starting Mail Service...");

            jobHandler = new JobHandler();

            //_Error.LogException(new Exception("Starting MailService...#of threads: " + MailThreads.ToString()));

            for (int i = 0; i < 1; i++)
                jobHandler.AddJob(new MailJob());
        }

        protected override void OnStop()
        {
            jobHandler.StopJobs();

            jobHandler.WaitJobs();

            if (Wcss._Config._ErrorsToDebugger)
                System.Diagnostics.Debug.WriteLine("Exiting Mail Service...");
        }
    }
}
