using System;
using System.ServiceProcess;

namespace SvcMail
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// reinstate for deployment
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SvcMail() 
            };
            ServiceBase.Run(ServicesToRun);
        }

        ////Use this for testing
        //[STAThread]
        //static void Main(string[] args)
        //{
        //    //Wcss._Error.LogException(new Exception("find the dir"));
        //    MailJob mailJob = new MailJob();
        //    mailJob.DoJob();
        //    mailJob.CleanUp();
        //}

    }
}
